using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.CreatePoint))]
namespace TyAutoCad.Examples
{
    public class CreatePoint
    {
        /// <summary>
        /// 指定した位置に点を作成
        /// </summary>
        [CommandMethod("CreatePoint")]
        public void Command()
        {
            var sw = new Stopwatch();
            sw.Start(); // 計測開始

            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            var ppo = new PromptPointOptions("\n点を指示");
            PromptPointResult ppr = ed.GetPoint(ppo);
            if (ppr.Status != PromptStatus.OK) return;
            Point3d pt = ppr.Value;
            ed.WriteMessage("\n選択された点 : " + pt);

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // モデルスペースを書き込みモードで取得
                    var ms = tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForWrite) as BlockTableRecord;

                    using (var point = new DBPoint())
                    {
                        point.Position = pt;
                        ms.AppendEntity(point);
                        tr.AddNewlyCreatedDBObject(point, true);
                    }

                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error Command\n\t" + ex.Message);
                }
            }

            sw.Stop(); // 計測終了
            TimeSpan span = sw.Elapsed;    //  計測した時間を span に代入
            ed.WriteMessage("\nTime : " + span.TotalMilliseconds);
        }
    }
}
