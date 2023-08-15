using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.LockLayerSelectedEntity))]
namespace TyAutoCad.Examples
{
    public class LockLayerSelectedEntity
    {
        /// <summary>
        /// 選択した図形の画層をロックする
        /// </summary>
        [CommandMethod("LockLayerSelectedEntity")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            PromptEntityResult per = ed.GetEntity("\n画層をロックする図形を選択");
            if (per.Status != PromptStatus.OK) return;

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // 選択した図形を取得
                    var entity = tr.GetObject(per.ObjectId, OpenMode.ForRead) as Entity;

                    // 画層を取得
                    var layer = tr.GetObject(entity.LayerId, OpenMode.ForWrite) as LayerTableRecord;

                    // 画層をロックする
                    layer.IsLocked = true;

                    tr.Commit();

                    ed.WriteMessage("\n画層 \"{0}\" をロックしました。", layer.Name);
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error LockLayerSelectedEntity\n\t" + ex.Message);
                }
            }
        }
    }
}
