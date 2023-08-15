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

[assembly: CommandClass(typeof(TyAutoCad.Examples.Create1MillionLines))]
namespace TyAutoCad.Examples
{
    public class Create1MillionLines
    {
        /// <summary>
        /// 線分を100万本作成
        /// </summary>
        [CommandMethod("Create1MillionLines")]
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
                    // モデルスペースのブロックテーブルレコードを書き込みモードで開く
                    var ms = tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForWrite) as BlockTableRecord;

                    var sp = new Point3d(0, 0, 0);
                    var ep = new Point3d(0, 100, 0);
                    var ap = new Vector3d(1, 0, 0);

                    int n = 1000000;

                    Line line;
                    using (line = new Line(sp, ep))
                    {
                        for (int i = 0; i < n; i++)
                        {
                            ms.AppendEntity(line);
                            tr.AddNewlyCreatedDBObject(line, true);
                            sp += ap;
                            ep += ap;
                            line = new Line(sp, ep);
                        }
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
