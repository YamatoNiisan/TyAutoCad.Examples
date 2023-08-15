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

[assembly: CommandClass(typeof(TyAutoCad.Examples.Select2PointsToGetRectAnglePoints))]
namespace TyAutoCad.Examples
{
    internal class Select2PointsToGetRectAnglePoints
    {
        /// <summary>
        /// 2点を選択して矩形の4点を取得
        /// </summary>
        [CommandMethod("Select2PointsToGetRectAnglePoints")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- 2点を選択して矩形の4点を取得 ---");

            // 2点を矩形指示 ( GetPoint + GetCorner Method )
            var options = new PromptPointOptions("\n点を指示:");
            PromptPointResult resuit = ed.GetPoint(options);
            if (resuit.Status != PromptStatus.OK) return;
            Point3d p1 = resuit.Value;

            var cornerOptions = new PromptCornerOptions("\nコーナー点を指示:", p1);
            resuit = ed.GetCorner(cornerOptions);
            if (resuit.Status != PromptStatus.OK) return;
            Point3d p2 = resuit.Value;

            // 矩形の4点を計算して取得
            var points = new Point3dCollection();
            points.Add(p1);
            points.Add(new Point3d(p2.X, p1.Y, 0));
            points.Add(p2);
            points.Add(new Point3d(p1.X, p2.Y, 0));

            // 矩形の4点を出力
            int i = 1;
            foreach (Point3d p in points)
            {
                ed.WriteMessage("\nPoint{0} : {1}", i, p);
                i++;
            }

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
