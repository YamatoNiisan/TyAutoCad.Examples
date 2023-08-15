using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetBlockTableRecordNotLayout))]
namespace TyAutoCad.Examples
{
    public class GetBlockTableRecordNotLayout
    {
        #region Command
        /// <summary>
        /// モデルとペーパースペースを除くすべてのブロックを取得
        /// </summary>
        [CommandMethod("GetBlockTableRecordNotLayout")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = AcAp.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- モデルとペーパースペースを除くすべてのブロックを取得 ---");

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // BlockTable を取得
                    var bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead, false) as BlockTable;

                    foreach (ObjectId id in bt)
                    {
                        var btr = tr.GetObject(id, OpenMode.ForRead, false, false) as BlockTableRecord;
                        if (btr.IsLayout) continue;
                        AcAp.DocumentManager.MdiActiveDocument.Editor.WriteMessage("\nBlock name: {0}", btr.Name);
                    }

                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    AcAp.ShowAlertDialog("Error Command\n\t" + ex.Message);
                }
            }

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
        #endregion
    }
}
