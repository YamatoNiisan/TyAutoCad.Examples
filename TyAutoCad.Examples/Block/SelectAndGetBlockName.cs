using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.SelectAndGetBlockName))]
namespace TyAutoCad.Examples
{
    public class SelectAndGetBlockName
    {
        /// <summary>
        /// ブロック図形を選択してブロック名を取得する
        /// 普通のブロックとダイナミックブロックではブロック名の取得の仕方が違う。
        /// </summary>
        [CommandMethod("SelectAndGetBlockName")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- ブロック図形を選択してブロック名を取得する ---");

            // BlockReference のみを選択
            var peo = new PromptEntityOptions("\nブロック図形を選択");
            peo.SetRejectMessage("\nブロック図形のみを選択してください");
            peo.AddAllowedClass(typeof(BlockReference), false);
            PromptEntityResult per = ed.GetEntity(peo);

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // 選択したブロックを取得
                    var blockRef = tr.GetObject(per.ObjectId, OpenMode.ForRead) as BlockReference;

                    BlockTableRecord block = null;
                    // ダイナミックブロックの場合
                    if (blockRef.IsDynamicBlock)
                    {
                        ed.WriteMessage("\n選択した図形は、ダイナミックブロックです。");
                        block = tr.GetObject(blockRef.DynamicBlockTableRecord, OpenMode.ForRead) as BlockTableRecord;
                    }
                    // 普通のブロックの場合
                    else
                    {
                        ed.WriteMessage("\n選択した図形は、普通のブロックです。");
                        block = tr.GetObject(blockRef.BlockTableRecord, OpenMode.ForRead) as BlockTableRecord;

                    }
                    if (block != null)
                    {
                        ed.WriteMessage("\n選択した図形のブロック名は \"{0}\" です。", block.Name);
                    }

                    ed.WriteMessage("\n--- コマンド終了 ---");
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error SelectAndGetBlockName\n\t" + ex.Message);
                }
            }
        }
    }
}
