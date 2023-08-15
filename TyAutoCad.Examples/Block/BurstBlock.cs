using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.BurstBlock))]
namespace TyAutoCad.Examples
{
    public class BurstBlock
    {
        #region Command
        /// <summary>
        /// 属性付ブロックをそのままの表示状態で分解
        /// エクスプレスツールのバーストコマンドの実装
        /// </summary>
        [CommandMethod("BurstBlock")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- 属性付ブロックをそのままの表示状態で分解 ---");

            // 図形を選択 ( GetEntity Method )
            var options = new PromptEntityOptions("\nブロック図形を選択");
            options.SetRejectMessage("Only an blockReference.");
            options.AddAllowedClass(typeof(BlockReference), true);
            PromptEntityResult result = ed.GetEntity(options);
            if (result.Status != PromptStatus.OK) return;

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // モデルスペースのブロックテーブルレコードを書き込みモードで開く
                    var ms = tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForWrite) as BlockTableRecord;

                    // 選択したブロックレファレンスを取得
                    var blockRef = tr.GetObject(result.ObjectId, OpenMode.ForRead) as BlockReference;

                    if (blockRef != null)
                    {
                        // DBObjectCollectionを準備
                        var dbObjects = new DBObjectCollection();

                        // ブロックレファレンスのBlockTableRecordを取得
                        var blockDef = tr.GetObject(blockRef.BlockTableRecord, OpenMode.ForRead) as BlockTableRecord;

                        // BlockTableRecordの図形を走査
                        foreach (ObjectId entId in blockDef)
                        {
                            // 属性かどうか調べる
                            if (entId.ObjectClass.Name == "AcDbAttributeDefinition")
                            {
                                // 属性を取得
                                var attDef = tr.GetObject(entId, OpenMode.ForRead) as AttributeDefinition;

                                // Constantフラグが true かつ Invisible(可視性)フラグが false
                                if ((attDef.Constant && !attDef.Invisible))
                                {
                                    // テキストスタイルテーブルを読み込みモードで開く
                                    var textStyleTable = tr.GetObject(db.TextStyleTableId, OpenMode.ForRead) as TextStyleTable;

                                    // 新しい文字を作成
                                    var text = new DBText
                                    {
                                        TextStyleId = textStyleTable[attDef.TextStyleName],
                                        Height = attDef.Height,
                                        TextString = attDef.TextString,
                                        Position = attDef.Position.TransformBy(blockRef.BlockTransform),
                                    };

                                    dbObjects.Add(text);
                                }
                            }
                        }

                        // 非定数および可視属性のテキストを作成します 
                        foreach (ObjectId attRefId in blockRef.AttributeCollection)
                        {
                            var attRef = tr.GetObject(attRefId, OpenMode.ForRead) as AttributeReference;

                            if (attRef.Invisible == false)
                            {
                                // テキストスタイルテーブルを読み込みモードで開く
                                var textStyleTable = tr.GetObject(db.TextStyleTableId, OpenMode.ForRead) as TextStyleTable;

                                var text = new DBText
                                {
                                    TextStyleId = textStyleTable[attRef.TextStyleName],
                                    Height = attRef.Height,
                                    TextString = attRef.TextString,
                                    Position = attRef.Position,
                                };

                                dbObjects.Add(text);
                            }
                        }

                        // Explode メソッドでブロック参照を分解した図形を取得
                        var explodedEntities = new DBObjectCollection();
                        blockRef.Explode(explodedEntities);
                        foreach (Entity ent in explodedEntities)
                        {
                            // 属性は含めない
                            if (!(ent is AttributeDefinition))
                            {
                                dbObjects.Add(ent);
                            }
                        }

                        // エンティティをモデルスペースに追加します 
                        foreach (Entity ent in dbObjects)
                        {
                            ms.AppendEntity(ent);
                            tr.AddNewlyCreatedDBObject(ent, true);
                        }

                        // ブロック参照を消去する
                        blockRef.UpgradeOpen();
                        blockRef.Erase();
                    }
                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error Command\n\t" + ex.Message);
                }
            }

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
        #endregion
    }
}
