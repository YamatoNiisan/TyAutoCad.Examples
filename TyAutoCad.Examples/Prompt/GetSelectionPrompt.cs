using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetSelectionPrompt))]
namespace TyAutoCad.Examples
{
    public class GetSelectionPrompt
    {
        /// <summary>
        /// 図形を範囲選択
        /// </summary>
        [CommandMethod("GetSelectionPrompt")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- 図形を範囲選択 ---");

            var options = new PromptSelectionOptions();
            options.MessageForAdding = "図形を選択";
            PromptSelectionResult result = ed.GetSelection(options);
            if (result.Status != PromptStatus.OK) return;
            SelectionSet ss = result.Value;        // 選択セットを取得
            ObjectId[] ids = ss.GetObjectIds(); // ObjectId の配列を取得

            foreach (ObjectId id in ids)
            {
                ed.WriteMessage("\n選択した図形のObjectId : {0}", id);
            }

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
