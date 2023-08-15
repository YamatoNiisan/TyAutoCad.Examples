using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.SetCurrentLayer))]
namespace TyAutoCad.Examples
{
    public class SetCurrentLayer
    {
        /// <summary>
        /// 指定画層を現在の画層に設定する
        /// </summary>
        [CommandMethod("SetCurrentLayer")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            string layerName = "テスト画層";

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // レイヤーテーブルを読込モードで取得
                    var layerTable = tr.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;

                    // 画層の存在確認
                    if (layerTable.Has(layerName))
                    {
                        // 現在画層に設定
                        db.Clayer = layerTable[layerName];
                        tr.Commit();
                    }
                    else
                    {
                        Application.ShowAlertDialog("この画層は存在しません。");
                        return;
                    }
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error SetCurrentLayer\n\t" + ex.Message);
                }
            }
            ed.WriteMessage("\n\t\"" + layerName + "\" を現在画層に設定しました。");
        }
    }
}
