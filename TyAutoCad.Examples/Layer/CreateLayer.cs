using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.CreateLayer))]
namespace TyAutoCad.Examples
{
    internal class CreateLayer
    {
        /// <summary>
        /// 新しい画層を作成する
        ///     レイヤーテーブルを取得して、新しい画層を追加します。
        /// </summary>
        [CommandMethod("CreateLayer")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- 新しい画層を作成する ---");

            string layerName = "テスト画層";

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // レイヤーテーブルを書込モードで取得
                    var layerTable = tr.GetObject(db.LayerTableId, OpenMode.ForWrite) as LayerTable;

                    // 画層の存在確認
                    if (layerTable.Has(layerName))
                    {
                        Application.ShowAlertDialog("この画層名はすでに使われています。");
                        return;
                    }

                    // Continuous の Linetype ID を取得
                    var linetypeId = SymbolUtilityServices.GetLinetypeContinuousId(db);

                    // レイヤーテーブルレコードを作成
                    var layer = new LayerTableRecord
                    {
                        Name = layerName,
                        Color = Color.FromColorIndex(ColorMethod.ByLayer, 1),
                        LinetypeObjectId = linetypeId,
                        IsPlottable = true,

                        // 設定しても説明が反映されない（原因不明）
                        //Description = "テスト画層の説明",
                    };

                    layer.Description = "テスト画層の説明";

                    // レイヤーテーブルとトランザクションに追加
                    layerTable.Add(layer);
                    tr.AddNewlyCreatedDBObject(layer, true);
                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error CreateLayer\n\t" + ex.Message);
                }
            }
            ed.WriteMessage("\n\t新しい画層 \"" + layerName + "\" を作成しました。");

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
