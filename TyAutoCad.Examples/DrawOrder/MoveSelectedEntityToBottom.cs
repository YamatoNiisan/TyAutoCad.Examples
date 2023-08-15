using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.MoveSelectedEntityToBottom))]
namespace TyAutoCad.Examples
{
    public class MoveSelectedEntityToBottom
    {
        /// <summary>
        /// 選択した図形を再背面に移動
        /// </summary>
        [CommandMethod("MoveSelectedEntityToBottom")]
        public static void Command()
        {
            // Editor を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            Database db = doc.Database;

            ed.WriteMessage("\n--- 選択した図形の ObjectId を取得 ---");

            var options = new PromptEntityOptions("\n最背面に移動する図形を選択");
            PromptEntityResult result = ed.GetEntity(options);
            if (result.Status != PromptStatus.OK) return;
            ObjectId objectId = result.ObjectId;

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // 選択した図形を取得
                    var ent = tr.GetObject(objectId, OpenMode.ForRead) as Entity;

                    // 図形のブロックテーブルレコードを取得
                    var block = tr.GetObject(ent.BlockId, OpenMode.ForRead) as BlockTableRecord;

                    // ブロックテーブルレコードの DrawOrderTable を取得する
                    var drawOrder = tr.GetObject(block.DrawOrderTableId, OpenMode.ForWrite) as DrawOrderTable;

                    var ids = new ObjectIdCollection
                    {
                        objectId
                    };

                    // 最背面に移動
                    drawOrder.MoveToBottom(ids);
                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error Command\n\t" + ex.Message);
                }
            }
        }
    }
}
