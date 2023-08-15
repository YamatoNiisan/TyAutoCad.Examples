using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetAllLinetypes))]
namespace TyAutoCad.Examples
{
    public class GetAllLinetypes
    {
        /// <summary>
        /// 全ての線種を取得してプロパティを出力
        /// </summary>
        [CommandMethod("GetAllLinetypes")]
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
                    // ラインタイプテーブルを読み込みモードで開く
                    var linetypeTable = tr.GetObject(db.LinetypeTableId, OpenMode.ForRead) as LinetypeTable;

                    // LinetypeTableRecordの変数を準備
                    LinetypeTableRecord linetype;

                    // カウンターを準備
                    int count = 0;

                    // LinetypeTableを走査
                    foreach (ObjectId id in linetypeTable)
                    {
                        // 線種を取得
                        linetype = tr.GetObject(id, OpenMode.ForRead, true) as LinetypeTableRecord;

                        ed.WriteMessage("\n\n--- SymbolTableRecord Properties ---");
                        ed.WriteMessage("\n\tIsDependent : " + linetype.IsDependent);
                        ed.WriteMessage("\n\tIsResolved  : " + linetype.IsResolved);
                        ed.WriteMessage("\n\tName        : " + linetype.Name);        // 線種名

                        ed.WriteMessage("\n\n--- LinetypeTableRecord Properties ---");
                        ed.WriteMessage("\n\tAsciiDescription : " + linetype.AsciiDescription);
                        ed.WriteMessage("\n\tComments         : " + linetype.Comments); //説明
                        ed.WriteMessage("\n\tIsScaledToFit    : " + linetype.IsScaledToFit);
                        ed.WriteMessage("\n\tNumDashes        : " + linetype.NumDashes);
                        ed.WriteMessage("\n\tPatternLength    : " + linetype.PatternLength);
                        ed.WriteMessage("\n---------------------------------------------------------------------");

                        count++;
                    }

                    ed.WriteMessage("\n\nNumber of Linetype = " + count);

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
