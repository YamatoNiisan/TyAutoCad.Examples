using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.ShowProgressMeter))]
namespace TyAutoCad.Examples
{
    public class ShowProgressMeter
    {
        /// <summary>
        /// プログレスメーターを表示する。
        /// </summary>
        [CommandMethod("ShowProgressMeter")]
        public void Command()
        {
            // プログレスメーター作成
            var pm = new ProgressMeter();

            // プログレス開始（メッセージ付）
            pm.Start("ProgressMeter 表示テスト : ");

            // プログレスリミット設定
            pm.SetLimit(100);

            // 重たい処理
            for (int i = 0; i < 100; i++)
            {
                System.Threading.Thread.Sleep(50);
                pm.MeterProgress();
            }

            // プログレス終了
            pm.Stop();
        }
    }
}
