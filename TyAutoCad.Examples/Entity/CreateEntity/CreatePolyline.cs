using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.CreatePolyline))]
namespace TyAutoCad.Examples
{
    public class CreatePolyline
    {
        /// <summary>
        /// ポリラインを作成
        /// </summary>
        [CommandMethod("CreatePolyline")]
        public void Command()
        {
            var sw = new Stopwatch();
            sw.Start(); // 計測開始

            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // モデルスペースを書込モードで取得
                    var ms = tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForWrite) as BlockTableRecord;

                    using (var poly = new Polyline())
                    {
                        poly.AddVertexAt(0, new Point2d(2, 4), 0, 0, 0);
                        poly.AddVertexAt(1, new Point2d(4, 2), 0, 0, 0);
                        poly.AddVertexAt(2, new Point2d(6, 4), 0, 0, 0);

                        ms.AppendEntity(poly);
                        tr.AddNewlyCreatedDBObject(poly, true);
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
