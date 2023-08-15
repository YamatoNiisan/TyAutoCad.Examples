using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.SelectAndGet2Points))]
namespace TyAutoCad.Examples
{
    public class SelectAndGet2Points
    {
        /// <summary>
        /// 2点を選択して取得（ラバーバンド付）
        /// </summary>
        [CommandMethod("SelectAndGet2Points")]
        public void Command()
        {
            // Editor を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;

            ed.WriteMessage("\n--- 2点を選択して取得（ラバーバンド付） ---");

            PromptPointResult result = ed.GetPoint("\n1点目を指示:");
            if (result.Status != PromptStatus.OK) return;
            Point3d p1 = result.Value;
            var options = new PromptPointOptions("\n2点目を指示:")
            {
                UseBasePoint = true, // ラバーバンドの基点使用
                BasePoint = p1,      // ラバーバンドの基点 
            };
            result = ed.GetPoint(options);
            if (result.Status != PromptStatus.OK) return;
            Point3d p2 = result.Value;

            ed.WriteMessage("\n1点目の座標:{0}", p1);
            ed.WriteMessage("\n2点目の座標:{0}", p2);

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
