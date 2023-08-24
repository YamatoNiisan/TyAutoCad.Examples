using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetPointWcsAndUcs))]
namespace TyAutoCad.Examples
{
    internal class GetPointWcsAndUcs
    {
        #region Command
        /// <summary>
        /// 点を取得してWCSとUCSで出力
        /// </summary>
        [CommandMethod("GetPointWcsAndUcs")]
        public static void Command()
        {
            // Document, Editor, Ucs を取得
            var doc = AcAp.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;
            var ucs = ed.CurrentUserCoordinateSystem;

            ed.WriteMessage("\n--- 点を取得してWCSとUCSで出力 ---");

            ed.WriteMessage("\n現在のUCS:{0}", ucs);

            // GetPointで取得する座標は UCS座標
            PromptPointResult result = ed.GetPoint("\n点を指示:");
            if (result.Status != PromptStatus.OK) return;
            Point3d ucsPoint = result.Value;

            // WCSに変換
            Point3d wcsPoint = ucsPoint.TransformBy(ucs);

            ed.WriteMessage("\nUCS:{0}", ucsPoint);
            ed.WriteMessage("\nWCS:{0}", wcsPoint);

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
        #endregion
    }
}
