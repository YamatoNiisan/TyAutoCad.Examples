using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.ImportBlockToCurrentDwg))]
namespace TyAutoCad.Examples
{
    public class ImportBlockToCurrentDwg
    {
        /// <summary>
        /// ブロック定義を現在の図面に取り込む
        /// すでに存在している場合は取り込まない（上書きはしない）
        /// </summary>
        [CommandMethod("ImportBlockToCurrentDwg")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;
            var sourceDb = new Database(false, true);

            ed.WriteMessage("\n--- ブロック定義を現在の図面に取り込む ---");

            // 取り込むブロックが保存されているDwgファイルのパス
            string fileName = @"C:\Program Files\SSTools\blocks\Screw.dwg";

            // 取り込むブロックの名前
            string blockName = "DrillScrew_M6_Hex";

            // 現在図面のトランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // 現在図面のブロックテーブルの取得
                    var bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                    // 現在図面にすでに存在する場合は終了
                    if (bt.Has(blockName))
                    {
                        ed.WriteMessage("\nブロック \"{0}\" は現在の図面にすでに存在します。", blockName);
                        return;
                    }

                    // ブロックが保存されているファイルの存在確認
                    if (!File.Exists(fileName))
                    {
                        ed.WriteMessage("\nファイルが存在しません: {0}", fileName);
                        return;
                    }

                    // ブロックが保存されている DWG のデータベース（ソースデータベース）を取得
                    sourceDb.ReadDwgFile(fileName, System.IO.FileShare.Read, true, null);

                    // ソースデータベースのトランザクションマネージャーを取得
                    //Autodesk.AutoCAD.DatabaseServices.TransactionManager sourceTm = sourceDb.TransactionManager;

                    // ソースデータベースのトランザクション開始
                    using (Transaction sourceTr = sourceDb.TransactionManager.StartTransaction())
                    {
                        // ソースデータベースのブロックテーブルを取得
                        var sourceBt = sourceTr.GetObject(sourceDb.BlockTableId, OpenMode.ForRead, false) as BlockTable;

                        // ブロックの存在確認
                        if (!sourceBt.Has(blockName))
                        {
                            ed.WriteMessage("\nブロック \"{0}\" は \"{1}\" に存在しません。", blockName, fileName);
                            return;
                        }

                        // ブロック定義の ObjectId を取得してコレクションに格納
                        var blockIds = new ObjectIdCollection();
                        blockIds.Add(sourceBt[blockName]);

                        // 現在のデータベースにインポート
                        var mapping = new IdMapping();
                        sourceDb.WblockCloneObjects(blockIds, db.BlockTableId, mapping, DuplicateRecordCloning.Ignore, false);
                        //sourceDb.WblockCloneObjects(blockIds, db.BlockTableId, mapping, DuplicateRecordCloning.Replace, false); //上書きの場合

                        sourceTr.Commit();
                    }
                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error ImportBlockToCurrentDwg\n\t" + ex.Message);
                }
            }
            sourceDb.Dispose();

            ed.WriteMessage("\nブロック定義 \"{0}\" を現在図面に追加しました。", blockName);

            ed.WriteMessage("\n--- コマンド終了 ---");

        }
    }
}
