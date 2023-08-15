using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetAllDwgFilesFromDirectory))]
namespace TyAutoCad.Examples
{
    public class GetAllDwgFilesFromDirectory
    {
        /// <summary>
        /// 指定フォルダから全てのDWGファイルを取得する
        /// </summary>
        [CommandMethod("GetAllDwgFilesFromDirectory")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- 指定フォルダから全てのDWGファイルを取得する ---");

            // 取得するDwgファイルが保存されているフォルダのパス
            string pathName = @"C:\Program Files\SSTools\blocks";

            // フォルダの存在確認
            if (!Directory.Exists(pathName))
            {
                ed.WriteMessage("\nフォルダが存在しません: {0}", pathName);
                return;
            }

            // フォルダ内の全てのDwgファイルを取得する
            string[] fileNames = Directory.GetFiles(pathName, "*.dwg");

            int i = 1;
            foreach (string fileName in fileNames)
            {
                // ファイル名の末尾が ".dwg" か調べる
                if (fileName.EndsWith(".dwg", StringComparison.InvariantCultureIgnoreCase))
                {
                    ed.WriteMessage("\nBLOCK{0}", i);
                    ed.WriteMessage("\n\tフルパス:{0}", fileName);

                    // フルパスからファイル名（シンボル）のみ取得
                    string destName = SymbolUtilityServices.GetSymbolNameFromPathName(fileName, "dwg");
                    ed.WriteMessage("\n\tファイル名:{0}", destName);

                    // 修復されたシンボル名を取得
                    destName = SymbolUtilityServices.RepairSymbolName(destName, false);
                    ed.WriteMessage("\n\t修復されたファイル名:{0}", destName);

                }
                i++;
            }

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
