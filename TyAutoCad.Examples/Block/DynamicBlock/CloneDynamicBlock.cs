using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.CloneDynamicBlock))]
namespace TyAutoCad.Examples
{
    public class CloneDynamicBlock
    {
        /// <summary>
        /// ダイナミックブロックのクローンを作成する
        ///     選択したダイナミックブロックのクローンを同じ図面内に作成する
        /// </summary>
        [CommandMethod("CloneDynamicBlock")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- ダイナミックブロックのクローンを作成する ---");

            var peo = new PromptEntityOptions("\nダイナミックブロック図形を選択");
            peo.SetRejectMessage("\nダイナミックブロック図形のみを選択してください");
            peo.AddAllowedClass(typeof(BlockReference), false);

            string newBlockName = string.Empty;

            do
            {
                PromptEntityResult per = ed.GetEntity(peo);

                // トランザクション開始
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    // 選択したブロックを取得
                    var blockRef = tr.GetObject(per.ObjectId, OpenMode.ForRead) as BlockReference;

                    if (blockRef.IsDynamicBlock)
                    {
                        var block = tr.GetObject(blockRef.DynamicBlockTableRecord, OpenMode.ForRead) as BlockTableRecord;
                        newBlockName = block.Name.ToUniqueBlockName();
                        Database cloneDb = db.Wblock(block.ObjectId);
                        using (cloneDb)
                        {
                            db.Insert(newBlockName, cloneDb, true);
                        }
                        tr.Commit();
                    }
                    else
                    {
                        ed.WriteMessage("\nダイナミックブロックを選択してください。");
                    }
                }
            } while (newBlockName == string.Empty);

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
