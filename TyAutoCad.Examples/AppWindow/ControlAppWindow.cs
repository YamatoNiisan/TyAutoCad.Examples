using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.ControlAppWindow))]
namespace TyAutoCad.Examples
{
    public class ControlAppWindow
    {
        /// <summary>
        /// アプリケーション ウィンドウの位置とサイズを変更
        ///     ウィンドウをディスプレイの左上コーナーに配置し、サイズを幅 400 ×高さ 400 ピクセルに変更
        /// </summary>
        [CommandMethod("ControlAppWindow")]
        public void Command()
        {
            // アプリケーションウィンドウの位置を設定する
            System.Windows.Point position = new System.Windows.Point(0, 0);
            Application.MainWindow.DeviceIndependentLocation = position;

            // アプリケーションウィンドウのサイズを設定する
            System.Windows.Size size = new System.Windows.Size(400, 400);
            Application.MainWindow.DeviceIndependentSize = size;
        }
    }
}
