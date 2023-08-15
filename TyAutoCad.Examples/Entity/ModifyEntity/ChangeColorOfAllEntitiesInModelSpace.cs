using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.ChangeColorOfAllEntitiesInModelSpace))]
namespace TyAutoCad.Examples
{
    public class ChangeColorOfAllEntitiesInModelSpace
    {
        /// <summary>
        /// モデル空間のすべての図形の色を変える(252にする)
        /// </summary>
        [CommandMethod("ChangeColorOfAllEntitiesInModelSpace")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- モデル空間のすべての図形の色を変える(252にする) ---");

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // モデルスペースを書込モードで取得
                    var ms = tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForWrite) as BlockTableRecord;

                    // 図形を準備
                    Entity entity = null;

                    // モデル空間から図形を取得する
                    foreach (ObjectId id in ms)
                    {
                        // 図形を取得
                        entity = tr.GetObject(id, OpenMode.ForWrite, true) as Entity;

                        // 色No を 252 に
                        entity.ColorIndex = 252;
                    }
                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error TestCommand00\n\t" + ex.Message);
                }
            }
            ed.WriteMessage("\nコマンド終了");
        }
    }
}
