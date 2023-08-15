using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.ChangeColorOfAllEntitiesInBlock))]
namespace TyAutoCad.Examples
{
    public class ChangeColorOfAllEntitiesInBlock
    {
        /// <summary>
        /// モデルスペースの全ての図形の色を変更（ブロック内の図形も）
        /// </summary>
        [CommandMethod("ChangeColorOfAllEntitiesInBlock")]
        public void Command()
        {
            var sw = new Stopwatch();
            sw.Start(); // 計測開始

            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- モデルスペースの全ての図形の色を変更（ブロック内の図形も） ---");

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // モデルスペースを書き込みモードで取得
                    var ms = tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForWrite) as BlockTableRecord;

                    // モデルスペース内の ObjectId を取得
                    var ids = ms.Cast<ObjectId>().ToList();

                    // 処理済のブロック名を格納
                    var blockNames = new List<string>();

                    while (ids.Count > 0)
                    {
                        // Entityリストのコピーを作成
                        var tmpList = new List<ObjectId>(ids); // List の値渡し

                        foreach (ObjectId id in tmpList)
                        {
                            // 図形を取得
                            var e = tr.GetObject(id, OpenMode.ForRead) as Entity;

                            switch (e)
                            {
                                // Curve の場合、色変更
                                case Curve cv:
                                    cv.UpgradeOpen();
                                    cv.ColorIndex = 252;
                                    break;
                                // BlockReferenceの場合
                                case BlockReference br:
                                    // 処理済のブロックは除外
                                    if (blockNames.Contains(br.Name))
                                    {
                                        break;
                                    }

                                    // 処理済のブロックに追加
                                    blockNames.Add(br.Name);

                                    // BlockReference の BlockTableRecord を読み取りモードで開く
                                    var brBtr = tr.GetObject(br.BlockTableRecord, OpenMode.ForRead) as BlockTableRecord;

                                    // BlockTableRecordを走査
                                    foreach (ObjectId ida in brBtr)
                                    {
                                        // Block内の図形のIdを追加
                                        ids.Add(ida);
                                    }
                                    brBtr.Dispose();
                                    break;
                                default:
                                    break;
                            }
                            ids.Remove(id);
                        }
                    }
                    tr.Commit();
                    ed.Regen();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error ChangeColorOfAllEntitiesInBlock\n\t" + ex.Message);
                }
            }

            ed.WriteMessage("\n--- コマンド終了 ---");

            sw.Stop(); // 計測終了
            TimeSpan span = sw.Elapsed;    //  計測した時間を span に代入
            ed.WriteMessage("\nTime : " + span.TotalMilliseconds);
        }
    }
}
