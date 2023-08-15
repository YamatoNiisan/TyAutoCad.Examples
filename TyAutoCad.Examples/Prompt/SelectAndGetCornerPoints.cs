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

[assembly: CommandClass(typeof(TyAutoCad.Examples.SelectAndGetCornerPoints))]
namespace TyAutoCad.Examples
{
    public class SelectAndGetCornerPoints
    {
        /// <summary>
        /// 2点を矩形指示して取得
        /// </summary>
        [CommandMethod("SelectAndGetCornerPoints")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;

            ed.WriteMessage("\n--- 2点を矩形指示して取得 ---");

            var options = new PromptPointOptions("\n1点目を指示:");
            PromptPointResult result = ed.GetPoint(options);
            if (result.Status != PromptStatus.OK) return;
            Point3d p1 = result.Value;

            var cornerOptions = new PromptCornerOptions("\nコーナー点を指示:", p1);
            result = ed.GetCorner(cornerOptions);
            if (result.Status != PromptStatus.OK) return;
            Point3d p2 = result.Value;

            ed.WriteMessage("\n1点目 : {0}", p1);
            ed.WriteMessage("\nコーナー点 : {0}", p2);

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
