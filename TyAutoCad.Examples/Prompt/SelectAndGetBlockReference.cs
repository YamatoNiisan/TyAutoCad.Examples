using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.SelectAndGetBlockReference))]
namespace TyAutoCad.Examples
{
    public class SelectAndGetBlockReference
    {
        /// <summary>
        /// ブロックリファレンスを選択して取得
        /// </summary>
        [CommandMethod("SelectAndGetBlockReference")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            // BlockReference図形を選択 ( GetEntity Method )
            var options = new PromptEntityOptions("\nブロックを選択");
            options.SetRejectMessage("ブロックのみを選択してください。");
            options.AddAllowedClass(typeof(BlockReference), true);      //SetRejectMessageの後ろに書かないとエラーになる,抽象クラスは指定できない
            PromptEntityResult result = ed.GetEntity(options);
            if (result.Status != PromptStatus.OK) return;

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // 現在スペースを読込モードで取得
                    var cs = tr.GetObject(db.CurrentSpaceId, OpenMode.ForRead) as BlockTableRecord;

                    // 選択したブロックを取得
                    var blockRef = tr.GetObject(result.ObjectId, OpenMode.ForRead) as BlockReference;

                    ed.WriteMessage("\n選択したブロックの名前:{0}", blockRef.Name);
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error Command\n\t" + ex.Message);
                }
            }
        }
    }
}
