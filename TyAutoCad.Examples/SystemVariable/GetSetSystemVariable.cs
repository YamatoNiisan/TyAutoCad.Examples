using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetSetSystemVariable))]
namespace TyAutoCad.Examples
{
    public class GetSetSystemVariable
    {
        /// <summary>
        /// システム変数を設定および取得する
        /// </summary>
        [CommandMethod("GetSetSystemVariable")]
        public void Command()
        {
            var sw = new Stopwatch();
            sw.Start(); // 計測開始

            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            // システム変数から現在の値を取得します
            int maxSort = System.Convert.ToInt32(Application.GetSystemVariable("MAXSORT"));

            // システム変数を新しい値に設定します
            Application.SetSystemVariable("MAXSORT", 100);

            sw.Stop(); // 計測終了
            TimeSpan span = sw.Elapsed;    //  計測した時間を span に代入
            ed.WriteMessage("\nTime : " + span.TotalMilliseconds);
        }
    }
}
