using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.CreatePolylineByPolygonSelection))]
namespace TyAutoCad.Examples
{
    public class CreatePolylineByPolygonSelection
    {
        /// <summary>
        /// ポリゴン選択でポリラインを作成する
        /// ポリラインコマンドを実行する
        /// </summary>
        [CommandMethod("CreatePolylineByPolygonSelection")]
        public void Command()
        {
            var sw = new Stopwatch();
            sw.Start(); // 計測開始

            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            // 3D点を指示 ( GetPoint Method )
            var ppo = new PromptPointOptions("\n1点目を指示");
            PromptPointResult ppr = ed.GetPoint(ppo);
            if (ppr.Status != PromptStatus.OK)
                return;
            Point3d p1 = ppr.Value;

            // ポリラインコマンドを実行する
            doc.SendStringToExecute("._PLINE " + p1.X + "," + p1.Y + " ", true, false, false);

            // このループはエラーになる 2021/12/17確認
            //while (System.Convert.ToInt32(Application.GetSystemVariable("CMDACTIVE")) == 1)
            //{
            //    ppo = new PromptPointOptions("\n次の点を指示")
            //    {
            //        UseBasePoint = true, // ラバーバンドの基点使用
            //        BasePoint = p1,      // ラバーバンドの基点 
            //    };
            //    ppr = ed.GetPoint(ppo);
            //    if (ppr.Status != PromptStatus.OK)
            //        return;
            //    Point3d p2 = ppr.Value;
            //    doc.SendStringToExecute(p2.X + "," + p2.Y + " ", true, false, false);
            //    p1 = p2;
            //}

            //doc.SendStringToExecute(" ", true, false, false);


            sw.Stop(); // 計測終了
            TimeSpan span = sw.Elapsed;    //  計測した時間を span に代入
            ed.WriteMessage("\nTime : " + span.TotalMilliseconds);
        }
    }
}
