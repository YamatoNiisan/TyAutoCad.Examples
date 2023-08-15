using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.AAA))]
namespace TyAutoCad.Examples
{
    public class GetAllTextStyles
    {
        /// <summary>
        /// 全てのテキストスタイルを取得
        /// </summary>
        [CommandMethod("GetAllTextStyles")]
        public static void Command()
        {
            var sw = new Stopwatch();
            sw.Start(); // 計測開始

            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            var ed = doc.Editor;
            var db = doc.Database;

            using (var tr = db.TransactionManager.StartOpenCloseTransaction())
            {
                try
                {
                    // テキストスタイルテーブルを読込モードで開く
                    var textStyleTable = tr.GetObject(db.TextStyleTableId, OpenMode.ForRead) as TextStyleTable;

                    // LinetypeTableRecordの変数を準備
                    TextStyleTableRecord textStyle;

                    // カウンターを準備
                    int count = 0;

                    foreach (ObjectId id in textStyleTable)
                    {
                        textStyle = tr.GetObject(id, OpenMode.ForRead) as TextStyleTableRecord;

                        ed.WriteMessage("\n\n--- SymbolTableRecord Properties ---");
                        ed.WriteMessage("\n\tIsDependent : " + textStyle.IsDependent);
                        ed.WriteMessage("\n\tIsResolved  : " + textStyle.IsResolved);
                        ed.WriteMessage("\n\tName        : " + textStyle.Name);        // 線種名

                        ed.WriteMessage("\n\n--- TextStyleTableRecord Properties ---");
                        ed.WriteMessage("\n\tBigFontFileName : " + textStyle.BigFontFileName);
                        ed.WriteMessage("\n\tFileName        : " + textStyle.FileName); //説明
                        ed.WriteMessage("\n\tFlagBits        : " + textStyle.FlagBits);
                        ed.WriteMessage("\n\tFont            : " + textStyle.Font);
                        ed.WriteMessage("\n\tIsShapeFile     : " + textStyle.IsShapeFile);
                        ed.WriteMessage("\n\tIsVertical      : " + textStyle.IsVertical);
                        ed.WriteMessage("\n\tObliquingAngle  : " + textStyle.ObliquingAngle);
                        ed.WriteMessage("\n\tPriorSize       : " + textStyle.PriorSize);
                        ed.WriteMessage("\n\tTextSize        : " + textStyle.TextSize);
                        ed.WriteMessage("\n\tXScale          : " + textStyle.XScale);
                        ed.WriteMessage("\n---------------------------------------------------------------------");

                        count++;
                    }

                    ed.WriteMessage("\n\nNumber of TextStyle = " + count);

                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error GetAllTextStyles Command\n\t" + ex.Message);
                }
            }

            sw.Stop(); // 計測終了
            TimeSpan span = sw.Elapsed;    //  計測した時間を span に代入
            ed.WriteMessage("\nTime : " + span.TotalMilliseconds);
        }
    }
}
