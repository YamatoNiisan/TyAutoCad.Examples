using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.DefineBlockFromSelectionSet))]
namespace TyAutoCad.Examples
{
    internal static class LayerExtensions
    {
        #region GetLockedLayers
        /// <summary>
        /// ロックされている画層の名前を取得
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        internal static IEnumerable<string> GetLockedLayers(this Database db)
        {
            if (db == null) return Enumerable.Empty<string>();
            var lockedLayers = new List<string>();

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                var layerTable = tr.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                foreach (ObjectId id in layerTable)
                {
                    var layer = tr.GetObject(id, OpenMode.ForRead) as LayerTableRecord;
                    if (layer.IsLocked)
                    {
                        lockedLayers.Add(layer.Name);
                    }
                }
            }
            return lockedLayers;
        }

        internal static IEnumerable<string> GetLockedLayers()
            => Application.DocumentManager.MdiActiveDocument.Database.GetLockedLayers();
        #endregion
    }

    public class DefineBlockFromSelectionSet
    {
        /// <summary>
        /// 選択セットからブロックを定義する
        ///     基点を指示して、図形を選択して、その図形からブロックを定義する
        ///     指示した基点が、ブロック図形内で原点になるように、図形をマトリックス変換する
        /// </summary>
        [CommandMethod("DefineBlockFromSelectionSet")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;
            var ucs = ed.CurrentUserCoordinateSystem;

            ed.WriteMessage("\n--- 選択セットからブロックを定義する ---");

            // ブロックの基点を指示
            PromptPointResult ppr = ed.GetPoint("\nブロックの基点を指示してください");
            if (ppr.Status != PromptStatus.OK) return;
            Point3d basePoint = ppr.Value.TransformBy(ucs);

            // 変換行列を作成
            var mat = Matrix3d.Displacement(basePoint.GetVectorTo(Point3d.Origin));

            // ロックされている画層を取得
            var locked = db.GetLockedLayers();

            // 選択フィルターを設定
            // ロックされている画層にある図形を除外する
            var filter = new SelectionFilter(
                new[]
                {
                    new TypedValue(-4, "<NOT"),
                    new TypedValue(8, string.Join(",", locked)),
                    new TypedValue(-4, "NOT>")
                }
            );

            // ブロック化する図形を選択
            var options = new PromptSelectionOptions();
            options.MessageForAdding = "ブロック化する図形を選択 : ";
            PromptSelectionResult psr = ed.GetSelection(options, filter);

            if (psr.Status != PromptStatus.OK) return;

            // 選択された図形の ObjectId を取得
            ObjectId[] ids = psr.Value.GetObjectIds();

            // ユニークなブロック名を生成
            string blockName = "QB" + DateTime.Now.ToString("yyMMdd_") + Guid.NewGuid().ToString("N").Substring(0, 8);

            // ブロックを定義する
            // 定義したブロックの ObjectId を格納する変数を準備
            ObjectId blockDefId = ObjectId.Null;

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // ブロックテーブルを読込モードで開く
                    var blockTable = tr.GetObject(db.BlockTableId, OpenMode.ForRead, false) as BlockTable;

                    // ブロックの存在確認
                    if (!blockTable.Has(blockName))
                    {
                        // 新しいブロック定義を作成
                        using (var btr = new BlockTableRecord())
                        {
                            // ブロック名をセット
                            btr.Name = blockName;

                            // ブロックの挿入点を原点に設定
                            btr.Origin = Point3d.Origin;

                            // XYZ尺度を均一に設定
                            btr.BlockScaling = BlockScaling.Uniform;

                            // ブロックテーブルにブロック定義を追加して、
                            // その ObjectId を取得
                            blockTable.UpgradeOpen();
                            blockDefId = blockTable.Add(btr);
                            tr.AddNewlyCreatedDBObject(btr, true);
                        }
                    }
                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error DefineBlockFromSelectionSet\n\t" + ex.Message);
                }
            }

            // deepclone を使用して、選択した図形を定義したブロック内に複製
            var objects = new ObjectIdCollection(ids);
            var mapping = new IdMapping();
            db.DeepCloneObjects(objects, blockDefId, mapping, false);

            int attCount = 0;

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                // 変換行列を適用して、図形を指定した基点に移動
                foreach (ObjectId id in objects)
                {
                    // IdMappingから新しいObjectIdを取得
                    ObjectId newId = mapping[id].Value;

                    // 新しいObjectIdが有効かチェック
                    if (newId != ObjectId.Null)
                    {
                        Entity clonedObj = tr.GetObject(newId, OpenMode.ForWrite, false) as Entity;
                        if (clonedObj != null)
                        {
                            clonedObj.TransformBy(mat);
                        }
                    }
                }

                // ブロック挿入
                // 挿入するブロックのブロックテーブルレコードを再度取得（属性を確認設定するため）
                var btr = tr.GetObject(blockDefId, OpenMode.ForRead) as BlockTableRecord;

                // 新しいブロック参照を作成
                using (var blockRef = new BlockReference(basePoint, blockDefId))
                {
                    // モデルスペースを書き込みモードで取得
                    var ms = tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForWrite) as BlockTableRecord;

                    // モデル空間に図形を追加
                    ms.AppendEntity(blockRef);
                    // トランザクションに図形を追加
                    tr.AddNewlyCreatedDBObject(blockRef, true);

                    // ブロックテーブルレコードに属性定義が関連付けられていか確認
                    if (btr.HasAttributeDefinitions)
                    {
                        // ブロックテーブルレコードから属性を追加
                        foreach (ObjectId id in btr)
                        {
                            DBObject obj = tr.GetObject(id, OpenMode.ForRead);

                            // 属性定義の場合
                            if (obj is AttributeDefinition)
                            {
                                // 属性定義にキャスト
                                var attDef = obj as AttributeDefinition;

                                // 固定値が設定されていない事を確認
                                // （固定値が設定されていると、値を設定できない）
                                if (!attDef.Constant)
                                {
                                    // 新しい属性参照を作成してブロックに追加
                                    using (var attRef = new AttributeReference())
                                    {
                                        // 属性参照から属性をコピーする
                                        attRef.SetAttributeFromBlock(attDef, blockRef.BlockTransform);

                                        // 文字の位置を設定
                                        attRef.Position = attDef.Position.TransformBy(blockRef.BlockTransform);

                                        // 既定値を設定
                                        attRef.TextString = attDef.TextString;

                                        blockRef.AttributeCollection.AppendAttribute(attRef);
                                        tr.AddNewlyCreatedDBObject(attRef, true);

                                        attCount++;
                                    }
                                }
                            }
                        }
                    }
                }

                // 元の図形を削除
                foreach (ObjectId id in objects)
                {
                    if (id != ObjectId.Null)
                    {
                        var ent = tr.GetObject(id, OpenMode.ForWrite, false) as Entity;
                        if (ent != null)
                        {
                            // 図形を削除
                            ent.Erase();
                        }
                    }
                }
                tr.Commit();
            }

            ed.WriteMessage("\n--------------------------------------------------");
            ed.WriteMessage("\nブロック化完了");
            ed.WriteMessage("\n--------------------------------------------------");
            ed.WriteMessage($"\nブロック名 : {blockName}");
            ed.WriteMessage($"\n基点    : {basePoint}");
            ed.WriteMessage($"\n図形数  : {objects.Count}");
            ed.WriteMessage($"\n属性数  : {attCount}");
            ed.WriteMessage("\n--------------------------------------------------");
        }
    }
}
