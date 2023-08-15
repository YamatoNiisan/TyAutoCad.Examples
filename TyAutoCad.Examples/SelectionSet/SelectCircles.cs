using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.SelectCircles))]
namespace TyAutoCad.Examples
{
    public class SelectCircles
    {
        /// <summary>
        /// 選択セットに 1 つの条件を指定する
        /// 次のコードは、選択セットに含めるオブジェクトを選択するためのプロンプトをユーザに表示し、
        /// 円を除くすべてのオブジェクトを除外します。
        /// </summary>
        [CommandMethod("SelectCircles")]
        public void Command()
        {
 
            // Document, Editor を取得
            var doc = AcAp.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;

            // TypedValue配列を作成してフィルター基準を定義します
            var typedValues = new TypedValue[1];
            typedValues.SetValue(new TypedValue((int)DxfCode.Start, "CIRCLE"), 0);

            // フィルター基準をSelectionFilterオブジェクトに割り当てます
            var filter = new SelectionFilter(typedValues);

            // GetSelectionメソッド実行
            PromptSelectionResult psr = ed.GetSelection(filter);

            // プロンプトのステータスがOKの場合、オブジェクトが選択されています
            if (psr.Status == PromptStatus.OK)
            {
                SelectionSet ss = psr.Value;
                AcAp.ShowAlertDialog("選択されたオブジェクトの数 : " + ss.Count);
            }
            else
            {
                AcAp.ShowAlertDialog("選択されたオブジェクトの数 : 0");
            }
        }
    }
}
