using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.SelectAndGetEntityObjectId))]
namespace TyAutoCad.Examples
{
    internal class SelectAndGetEntityObjectId
    {
        /// <summary>
        /// 選択した図形の ObjectId を取得
        /// </summary>
        [CommandMethod("SelectAndGetEntityObjectId")]
        public void Command()
        {
            // Editor を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;

            ed.WriteMessage("\n--- 選択した図形の ObjectId を取得 ---");

            var options = new PromptEntityOptions("\n図形を選択");
            PromptEntityResult result = ed.GetEntity(options);
            if (result.Status != PromptStatus.OK) return;
            ObjectId objectId = result.ObjectId;
            ed.WriteMessage("\n選択した図形のObjectId : {0}", objectId);

             ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
