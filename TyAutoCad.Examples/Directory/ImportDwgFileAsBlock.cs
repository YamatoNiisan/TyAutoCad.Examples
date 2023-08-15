using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.ImportDwgFileAsBlock))]
namespace TyAutoCad.Examples
{
    public class ImportDwgFileAsBlock
    {
        /// <summary>
        /// DWGファイルをブロックとして取り込む
        /// （結果そのDWGファイル内の全てのブロックがインポートされる）
        /// </summary>
        [CommandMethod("ImportDwgFileAsBlock")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var destDb = doc.Database;

            ed.WriteMessage("\n--- DWGファイルをブロックとして取り込む ---");

            // 取り込むDwgファイルのパス
            string fileName = @"C:\Program Files\SSTools\blocks\Screw.dwg";

            // ファイルの存在確認
            if (!File.Exists(fileName))
            {
                ed.WriteMessage("\nファイルが存在しません: {0}", fileName);
                return;
            }

            // ファイル名の末尾が ".dwg" か調べる
            if (fileName.EndsWith(".dwg", StringComparison.InvariantCultureIgnoreCase))
            {
                try
                {
                    // フルパスからファイル名（シンボル）のみ取得
                    string destName = SymbolUtilityServices.GetSymbolNameFromPathName(fileName, "dwg");

                    // 修復されたシンボル名を取得
                    destName = SymbolUtilityServices.RepairSymbolName(destName, false);

                    // DWG をロードするソース データベースを作成
                    using (Database db = new Database(false, true))
                    {
                        // DWGファイルをデータベースに読み込む
                        db.ReadDwgFile(fileName, FileShare.Read, true, "");
                        bool isAnno = db.AnnotativeDwg;

                        ObjectId btrId = destDb.Insert(destName, db, false);

                        if (isAnno)
                        {
                            // 異尺度対応ブロックの場合、結果の BTR を開き、その異尺度対応定義ステータスを設定します
                            using (var tr = destDb.TransactionManager.StartTransaction())
                            {
                                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(btrId, OpenMode.ForWrite);
                                btr.Annotative = AnnotativeStates.True;
                                tr.Commit();
                            }
                        }
                        ed.WriteMessage("\n\"{0}\"をブロックとして取り込みました。", fileName);
                    }
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error ImportDwgFileAsBlock\n\t" + ex.Message);
                }
            }
            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
