using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.TaskDialogShowMarqueeProgressBar))]
namespace TyAutoCad.Examples
{
    public class TaskDialogShowMarqueeProgressBar
    {
        /// <summary>
        /// 処理が終了するまでマーキープログレスバーを表示。
        ///     キャンセルボタンで処理を中止。
        /// </summary>
        [CommandMethod("TaskDialogShowMarqueeProgressBar")]
        public void Command()
        {
            // 終了フラグを設定
            bool isFinished = false;

            // タスクダイアログを作成
            var td = new TaskDialog
            {
                WindowTitle = "TaskDialog ShowMarqueeProgressBar",
                MainIcon = TaskDialogIcon.Shield,
                MainInstruction = "処理が終了するまでマーキープログレスバーを表示",
                ContentText = "処理中です。。。（キャンセルボタンで処理を中止）",
                ShowProgressBar = true,
                AllowDialogCancellation = true, // 右上の ☓ ボタンの表示
                CommonButtons = TaskDialogCommonButtons.Cancel // キャンセルボタンを追加
            };

            // コールバックを設定
            td.Callback =

                delegate (ActiveTaskDialog atd, TaskDialogCallbackArgs args, object sender)
                {
                    // タスクダイアログ作成済ならば 
                    if (args.Notification == TaskDialogNotification.Created)
                    {
                        // プログレスバーをマーキープログレスバーに設定
                        atd.SetMarqueeProgressBar(true);

                        // マーキープログレスバーを開始する
                        // （2番目の引数でスピードを設定）
                        atd.SetProgressBarMarquee(true, 50);

                        // 重たい処理を並列実行
                        Task.Run(() =>
                        {
                            // 重たい処理
                            Thread.Sleep(5000);

                            // 終了フラグ true
                            isFinished = true;

                            // 処理が終了したらキャンセルボタンを押す
                            atd.ClickButton((int)System.Windows.Forms.DialogResult.Cancel);
                        });
                    }

                    // コマンドボタンが押されたときの処理
                    if (args.Notification == TaskDialogNotification.ButtonClicked)
                    {
                        // キャンセルボタンが押されたら終了
                        if (args.ButtonId == (int)System.Windows.Forms.DialogResult.Cancel)
                        {
                            return false;
                        }
                    }
                    return false;
                };

            // タスクダイアログを表示
            td.Show(Application.MainWindow.Handle);

            if (isFinished)
            {
                Application.ShowAlertDialog("処理が終了しました。");
            }
            else
            {
                Application.ShowAlertDialog("キャンセルボタンが押されました。");
            }
        }
    }
}
