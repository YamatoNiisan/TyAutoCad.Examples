using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.ControlAppWindowMinMax))]
namespace TyAutoCad.Examples
{
    public class ControlAppWindowMinMax
    {
        /// <summary>
        /// アプリケーションウィンドウを最小化、最大化
        /// </summary>
        [CommandMethod("ControlAppWindowMinMax")]
        public void Command()
        {
            // アプリケーションウィンドウを最小化
            Application.MainWindow.WindowState = Window.State.Minimized;

            System.Windows.Forms.MessageBox.Show("Minimized", "MinMax",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.None,
                        System.Windows.Forms.MessageBoxDefaultButton.Button1,
                        System.Windows.Forms.MessageBoxOptions.ServiceNotification);

            // アプリケーションウィンドウを最大化
            Application.MainWindow.WindowState = Window.State.Maximized;
            System.Windows.Forms.MessageBox.Show("Maximized", "MinMax");
        }
    }
}
