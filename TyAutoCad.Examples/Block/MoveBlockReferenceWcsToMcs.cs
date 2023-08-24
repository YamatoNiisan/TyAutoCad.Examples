using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;

[assembly: CommandClass(typeof(TyAutoCad.Examples.MoveBlockReferenceWcsToMcs))]

namespace TyAutoCad.Examples
{
    internal class MoveBlockReferenceWcsToMcs
    {
        #region Command
        /// <summary>
        /// 選択したブロック図形をMCSに移動
        /// ブロックエディタで開いたときと同じ位置に移動
        /// </summary>
        [CommandMethod("MoveBlockReferenceWcsToMcs")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = AcAp.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- 選択したブロック図形をMCSに移動 ---");

            // 挿入基点を取得するブロックを選択
            var peo = new PromptEntityOptions("\n移動するブロックを選択してください");
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

                    // MCS -> WCS の変換行列を取得
                    var mat = blockRef.BlockTransform;

                    // 移動する
                    blockRef.UpgradeOpen();
                    blockRef.TransformBy(mat.Inverse());

                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    AcAp.ShowAlertDialog("Error BlockReferenceWcsToMcs\n\t" + ex.Message);
                }
            }

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
        #endregion
    }
}
