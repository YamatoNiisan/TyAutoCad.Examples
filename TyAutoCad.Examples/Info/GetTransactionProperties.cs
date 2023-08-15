using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetTransactionProperties))]
namespace TyAutoCad.Examples
{
    public class GetTransactionProperties
    {
        /// <summary>
        /// トランザクションクラスのプロパティを取得する
        /// </summary>
        [CommandMethod("GetTransactionProperties")]
        public void Command()
        {
            var sw = new Stopwatch();
            sw.Start(); // 計測開始

            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // Transactionのプロパティを出力
                    ed.WriteMessage("\n\n--- Transaction Properties ---");
                    ed.WriteMessage("\n\tNumberOfOpenedObjects : " + tr.NumberOfOpenedObjects);
                    ed.WriteMessage("\n\tTransactionManager    : " + tr.TransactionManager);
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error Command\n\t" + ex.Message);
                }
            }

            sw.Stop(); // 計測終了
            TimeSpan span = sw.Elapsed;    //  計測した時間を span に代入
            ed.WriteMessage("\nTime : " + span.TotalMilliseconds);
        }
    }
}
