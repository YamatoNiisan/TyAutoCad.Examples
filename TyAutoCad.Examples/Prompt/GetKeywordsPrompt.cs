using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetKeywordsPrompt))]
namespace TyAutoCad.Examples
{
    internal class GetKeywordsPrompt
    {
        /// <summary>
        /// キーワードを入力して取得
        /// </summary>
        [CommandMethod("GetKeywordsPrompt")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- キーワードを入力して取得 ---");

            var options = new PromptKeywordOptions("\nキーワード ? ")
            {
                AllowNone = true,
            };
            options.Keywords.Add("Y", "Y", "Y=はい");
            options.Keywords.Add("N", "N", "N=いいえ");
            options.Keywords.Default = "N";
            PromptResult result = ed.GetKeywords(options);
            if (result.Status != PromptStatus.OK) return;
            // "Y=はい" が選択された
            if (result.StringResult == "Y")
            {
                // はいが選択された時のプログラム
                ed.WriteMessage("\nはいが選択されました。");
            }

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
