using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetDocumentProperties))]
namespace TyAutoCad.Examples
{
    public class GetDocumentProperties
    {
        /// <summary>
        /// Documentのプロパティを取得する
        /// </summary>
        [CommandMethod("GetDocumentProperties")]
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
                    // Documentのプロパティを出力
                    ed.WriteMessage("\n\n--- Document Properties ---");
                    ed.WriteMessage("\n\tCommandInProgress  : " + doc.CommandInProgress);
                    ed.WriteMessage("\n\tDatabase           : " + doc.Database);
                    ed.WriteMessage("\n\tEditor	            : " + doc.Editor);
                    ed.WriteMessage("\n\tFormatForSave      : " + doc.FormatForSave);
                    ed.WriteMessage("\n\tGraphicsManager    : " + doc.GraphicsManager);
                    ed.WriteMessage("\n\tIsActive           : " + doc.IsActive);
                    ed.WriteMessage("\n\tIsNamedDrawing	    : " + doc.IsNamedDrawing);
                    ed.WriteMessage("\n\tIsReadOnly         : " + doc.IsReadOnly);
                    ed.WriteMessage("\n\tName               : " + doc.Name); // 図面ファイル名 Drawing1.dwg
                    ed.WriteMessage("\n\tTransactionManager : " + doc.TransactionManager);
                    ed.WriteMessage("\n\tUserData           : " + doc.UserData);
                    ed.WriteMessage("\n\tWindow	            : " + doc.Window);

                    ed.WriteMessage("\n\n--- DisposableWrapper Properties ---");
                    ed.WriteMessage("\n\tAutoDelete      : " + doc.AutoDelete);
                    ed.WriteMessage("\n\tIsDisposed      : " + doc.IsDisposed);
                    ed.WriteMessage("\n\tUnmanagedObject : " + doc.UnmanagedObject);
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
