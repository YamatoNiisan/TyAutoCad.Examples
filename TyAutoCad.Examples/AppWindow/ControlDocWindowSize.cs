using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.ControlDocWindowSize))]
namespace TyAutoCad.Examples
{
    public class ControlDocWindowSize
    {
        /// <summary>
        /// アクティブなドキュメントウィンドウのサイズを変更
        /// </summary>
        [CommandMethod("ControlDocWindowSize")]
        public void Command()
        {
            // アクティブなドキュメントを取得
            Document doc = Application.DocumentManager.MdiActiveDocument;

            // ドキュメントウィンドウの状態を Normal に設定
            doc.Window.WindowState = Window.State.Normal;

            // ドキュメントウィンドウの位置を設定
            System.Windows.Point location = new System.Windows.Point(0, 0);
            doc.Window.DeviceIndependentLocation = location;

            // ドキュメントウィンドウのサイズを設定
            System.Windows.Size size = new System.Windows.Size(400, 400);
            doc.Window.DeviceIndependentSize = size;
        }
    }
}
