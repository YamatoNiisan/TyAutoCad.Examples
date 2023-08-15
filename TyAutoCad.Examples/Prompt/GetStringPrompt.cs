using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetStringPrompt))]
namespace TyAutoCad.Examples
{
    internal class GetStringPrompt
    {
        /// <summary>
        /// 文字列を入力して取得
        /// </summary>
        [CommandMethod("GetStringPrompt")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- 文字列を入力して取得 ---");

            var options = new PromptStringOptions("\n文字列を入力:");
            PromptResult result = ed.GetString(options);
            if (result.Status != PromptStatus.OK) return;
            if (string.IsNullOrEmpty(result.StringResult)) return; // 空文字
            string str = result.StringResult;
            ed.WriteMessage("\n入力された文字列 : {0}", str);

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
