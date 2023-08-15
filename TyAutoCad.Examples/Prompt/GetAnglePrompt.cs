using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetAnglePrompt))]
namespace TyAutoCad.Examples
{
    internal class GetAnglePrompt
    {
        /// <summary>
        /// 角度を入力して取得
        /// </summary>
        [CommandMethod("GetAnglePrompt")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;

            ed.WriteMessage("\n--- 角度を入力して取得 ---");

            var options = new PromptAngleOptions("\n角度を入力");
            PromptDoubleResult result = ed.GetAngle(options);
            if (result.Status != PromptStatus.OK) return;
            double r = result.Value;
            ed.WriteMessage("\n入力された角度 : {0}", r);

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
