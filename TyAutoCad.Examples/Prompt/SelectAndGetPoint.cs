using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.SelectAndGetPoint))]
namespace TyAutoCad.Examples
{
    public class SelectAndGetPoint
    {
        /// <summary>
        /// 点を選択して取得
        /// </summary>
        [CommandMethod("SelectAndGetPoint")]
        public void Command()
        {
            // Editor を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;

            ed.WriteMessage("\n--- 点を選択して取得 ---");

            var options = new PromptPointOptions("\n点を指示:");
            PromptPointResult result = ed.GetPoint(options);
            if (result.Status != PromptStatus.OK) return;
            Point3d p = result.Value;
            ed.WriteMessage("\n指示した点の座標:{0}", p);

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
