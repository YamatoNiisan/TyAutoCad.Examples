using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetDoublePrompt))]
namespace TyAutoCad.Examples
{
    internal class GetDoublePrompt
    {
        /// <summary>
        /// 実数を入力して取得
        /// </summary>
        [CommandMethod("GetDoublePrompt")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;

            ed.WriteMessage("\n--- 実数を入力して取得 ---");

            var options = new PromptDoubleOptions("\n実数を入力してください")
            {
                AllowZero = false,     // ゼロ入力     (true:可, false:不可)
                AllowNegative = false, // 負の値の入力 (true:可, false:不可)
                AllowNone = false,     // 空Enter     (true:可, false:不可)
            };
            PromptDoubleResult result = ed.GetDouble(options);
            if (result.Status != PromptStatus.OK) return;
            double d = result.Value;
            ed.WriteMessage("\n入力された実数 : {0}", d);

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
