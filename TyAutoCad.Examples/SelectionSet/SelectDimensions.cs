using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.SelectDimensions))]
namespace TyAutoCad.Examples
{
    public class SelectDimensions
    {
        /// <summary>
        /// 選択セットで寸法を選択する
        /// </summary>
        [CommandMethod("SelectDimensions")]
        public void Command()
        {

            // Document, Editor を取得
            var doc = AcAp.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;

            // TypedValue配列を作成してフィルター基準を定義します
            var typedValues = new TypedValue[1];
            typedValues.SetValue(new TypedValue((int)DxfCode.Start, "DIMENSION"), 0);

            // フィルター基準をSelectionFilterオブジェクトに割り当てます
            var filter = new SelectionFilter(typedValues);

            // GetSelectionメソッド実行
            PromptSelectionResult psr = ed.GetSelection(filter);

            // プロンプトのステータスがOKの場合、オブジェクトが選択されています
            if (psr.Status == PromptStatus.OK)
            {
                SelectionSet ss = psr.Value;
                AcAp.ShowAlertDialog("選択された寸法の数 : " + ss.Count);
            }
            else
            {
                AcAp.ShowAlertDialog("選択された寸法の数 : 0");
            }
        }
    }
}
