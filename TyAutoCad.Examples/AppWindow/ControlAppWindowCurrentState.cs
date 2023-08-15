using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.ControlAppWindowCurrentState))]
namespace TyAutoCad.Examples
{
    public class ControlAppWindowCurrentState
    {
        /// <summary>
        /// アプリケーションウィンドウの現在の状態を確認
        /// </summary>
        [CommandMethod("ControlAppWindowCurrentState")]
        public void Command()
        {
            Application.ShowAlertDialog(
                "アプリケーションウインドウの状態 : \n " +
                Application.MainWindow.WindowState);
        }
    }
}
