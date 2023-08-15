using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// アセンブリをロードする
[assembly: ExtensionApplication(typeof(TyAutoCad.Examples.HelloWorldApp))]
[assembly: CommandClass(typeof(TyAutoCad.Examples.LoadAssembly))]
namespace TyAutoCad.Examples
{
    public class HelloWorldApp : Autodesk.AutoCAD.Runtime.IExtensionApplication
    {
        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("初期化。。。");
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Terminate()
        {
            Console.WriteLine("終了処理。。。");
        }
    }

    public class LoadAssembly
    {
        /// <summary>
        /// テストコマンド
        /// </summary>
        [CommandMethod("LoadAssembly")]
        public void Command()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("テストコマンド実行。");
        }
    }
}
