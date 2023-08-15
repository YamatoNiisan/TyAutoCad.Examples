using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.SelectAndGetNestedEntityObjectId))]
namespace TyAutoCad.Examples
{
    public class SelectAndGetNestedEntityObjectId
    {
        /// <summary>
        /// 選択したネストされた図形の ObjectId を取得
        /// </summary>
        [CommandMethod("SelectAndGetNestedEntityObjectId")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;

            ed.WriteMessage("\n--- 選択したネストされた図形の ObjectId を取得 ---");

            var options = new PromptNestedEntityOptions("\nネストされた図形を選択");
            PromptNestedEntityResult result = ed.GetNestedEntity(options);
            if (result.Status != PromptStatus.OK) return;
            ObjectId objectId = result.ObjectId;
            ed.WriteMessage("\nNested選択した図形のObjectId : {0}", objectId);

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
