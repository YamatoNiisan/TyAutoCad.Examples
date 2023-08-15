using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.ChangeColorOfAllEntities))]
namespace TyAutoCad.Examples
{
    public class ChangeColorOfAllEntities
    {
        /// <summary>
        /// モデルスペースの全ての図形の色を変更する
        ///     この例では全て赤に
        /// </summary>
        [CommandMethod("ChangeColorOfAllEntities")]
        public void Command()
        {
            var sw = new Stopwatch();
            sw.Start(); // 計測開始

            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- モデルスペースの全ての図形の色を変更する ---");

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // モデルスペースを書き込みモードで取得
                    var ms = tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForWrite) as BlockTableRecord;

                    // モデルスペースを走査
                    Entity entity;
                    foreach (ObjectId id in ms)
                    {
                        // 図形を取得
                        entity = tr.GetObject(id, OpenMode.ForWrite) as Entity;

                        // 色変更
                        entity.ColorIndex = 1;
                    }

                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error ChangeColorOfAllEntities\n\t" + ex.Message);
                }
            }

            ed.WriteMessage("\n--- コマンド終了 ---");

            sw.Stop(); // 計測終了
            TimeSpan span = sw.Elapsed;    //  計測した時間を span に代入
            ed.WriteMessage("\nTime : " + span.TotalMilliseconds);
        }
    }
}
