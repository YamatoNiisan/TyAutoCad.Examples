using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.ImportAllBlocksFromDwgFile))]
namespace TyAutoCad.Examples
{
    public class ImportAllBlocksFromDwgFile
    {
        /// <summary>
        /// 指定DWGファイルから全てのブロックを取り込む
        /// </summary>
        [CommandMethod("ImportAllBlocksFromDwgFile")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;
            var sourceDb = new Database(false, true);

            ed.WriteMessage("\n--- 指定DWGファイルから全てのブロックを取り込む ---");

            // 取り込むブロックが保存されているDwgファイルのパス
            string fileName = @"C:\Program Files\SSTools\blocks\Screw.dwg";

            // ファイルの存在確認
            if (!File.Exists(fileName))
            {
                ed.WriteMessage("\nファイルが存在しません: {0}", fileName);
                return;
            }

            try
            {
                // ブロックが保存されている DWG のデータベース（ソースデータベース）を取得
                sourceDb.ReadDwgFile(fileName, System.IO.FileShare.Read, true, "");

                // ブロックの ObjectIdを保存するコレクションを準備
                var blockIds = new ObjectIdCollection();

                // ソースデータベースのトランザクションマネージャーを取得
                Autodesk.AutoCAD.DatabaseServices.TransactionManager tm = sourceDb.TransactionManager;

                // トランザクション開始
                using (Transaction tr = tm.StartTransaction())
                {
                    // ブロックテーブルを取得
                    var bt = tm.GetObject(sourceDb.BlockTableId, OpenMode.ForRead, false) as BlockTable;

                    // ブロックテーブル内のブロックテーブルレコードを取得
                    foreach (ObjectId btrId in bt)
                    {
                        var btr = (BlockTableRecord)tm.GetObject(btrId, OpenMode.ForRead, false) as BlockTableRecord;

                        // 名前付きブロックと非レイアウトブロックのみをコレクションに追加
                        if (!btr.IsAnonymous && !btr.IsLayout)
                            blockIds.Add(btrId);
                        btr.Dispose();
                    }
                }

                // 現在のデータベースにインポート
                var mapping = new IdMapping();
                sourceDb.WblockCloneObjects(blockIds, db.BlockTableId, mapping, DuplicateRecordCloning.Replace, false);

                ed.WriteMessage("\n\"{0}\" からブロック図形{1}個を、現在の図面に取り込みました。", fileName, blockIds.Count);
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                Application.ShowAlertDialog("Error ImportAllBlocksFromDwgFile\n\t" + ex.Message);
            }
            sourceDb.Dispose();

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
