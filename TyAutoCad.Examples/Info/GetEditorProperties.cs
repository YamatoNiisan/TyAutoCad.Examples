using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetEditorProperties))]
namespace TyAutoCad.Examples
{
    public class GetEditorProperties
    {
        /// <summary>
        /// Editorのプロパティを取得する
        /// </summary>
        [CommandMethod("GetEditorProperties")]
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
                    // Editorのプロパティを出力
                    ed.WriteMessage("\n\n--- Editor Properties ---");
                    ed.WriteMessage("\n\tActiveViewportId                 : " + ed.ActiveViewportId);
                    ed.WriteMessage("\n\tCurrentUserCoordinateSystem      : " + ed.CurrentUserCoordinateSystem);
                    ed.WriteMessage("\n\tCurrentViewportObjectId          : " + ed.CurrentViewportObjectId);
                    ed.WriteMessage("\n\tDocument                         : " + ed.Document);
                    ed.WriteMessage("\n\tIsDragging                       : " + ed.IsDragging);
                    ed.WriteMessage("\n\tIsQuiescent                      : " + ed.IsQuiescent);
                    ed.WriteMessage("\n\tIsQuiescentForTransparentCommand : " + ed.IsQuiescentForTransparentCommand);
                    ed.WriteMessage("\n\tMouseHasMoved                    : " + ed.MouseHasMoved);
                    ed.WriteMessage("\n\tUseCommandLineInterface          : " + ed.UseCommandLineInterface);
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
