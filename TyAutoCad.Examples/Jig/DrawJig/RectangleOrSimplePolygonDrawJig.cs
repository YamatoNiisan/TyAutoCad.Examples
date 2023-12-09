using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.RectangleOrSimplePolygonDrawJig))]
namespace TyAutoCad.Examples
{
    public class RectangleOrSimplePolygonDrawJig
    {
        /// <summary>
        /// 矩形選択または多角形でポリラインを作成
        /// 空Enterで切り替える
        /// </summary>
        [CommandMethod("RectangleOrSimplePolygonDrawJig")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- 矩形選択または多角形でポリラインを作成 ---");

            PromptPointResult ppr;
            var options = new PromptPointOptions("")
            {
                AllowNone = true, // 空 Enter を許可
            };

            bool isRectangle = true;

            // 点が指示されるまで続ける
            do
            {
                if (isRectangle)
                {
                    options.Message = "\n一方のコーナーを指示 または <Enter> で ポリゴン指示";
                }
                else
                {
                    options.Message = "\n一つ目の点を指示 または <Enter> で 矩形指示";
                }

                ppr = ed.GetPoint(options);

                // 空 Enter
                if (ppr.Status == PromptStatus.None)
                {
                    isRectangle = !isRectangle;
                }

            } while (ppr.Status != PromptStatus.OK && ppr.Status != PromptStatus.Cancel);

            if (ppr.Status == PromptStatus.OK)
            {
                ed.WriteMessage("\n点が指示されました。{0}", ppr.Value);

                if (isRectangle)
                {
                    RectangleDrawJig.Execute(ppr.Value);
                }
                else
                {
                    SimplePolygonDrawJig.Execute(ppr.Value);
                }
            }

            if (ppr.Status == PromptStatus.Cancel)
            {
                ed.WriteMessage("\nキャンセルされました。");
            }
        }
    }
}
