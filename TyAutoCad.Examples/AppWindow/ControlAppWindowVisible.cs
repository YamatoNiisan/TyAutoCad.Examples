using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.ControlAppWindowVisible))]
namespace TyAutoCad.Examples
{
    public class ControlAppWindowVisible
    {
        /// <summary>
        /// アプリケーションウィンドウを非表示、表示
        /// </summary>
        [CommandMethod("ControlAppWindowVisible")]
        public void Command()
        {
            // アプリケーションウィンドウを非表示
            Application.MainWindow.Visible = false;
            System.Windows.Forms.MessageBox.Show("非表示", "Show/Hide");

            // アプリケーションウィンドウを表示
            Application.MainWindow.Visible = true;
            System.Windows.Forms.MessageBox.Show("表示", "Show/Hide");
        }
    }
}
