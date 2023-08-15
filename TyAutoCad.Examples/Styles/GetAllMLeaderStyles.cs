using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetAllMLeaderStyles))]
namespace TyAutoCad.Examples
{
    public class GetAllMLeaderStyles
    {
        /// <summary>
        /// 全てのマルチ引出線スタイルを取得
        /// </summary>
        [CommandMethod("GetAllMLeaderStyles")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            var ed = doc.Editor;
            var db = doc.Database;

            using (var tr = db.TransactionManager.StartOpenCloseTransaction())
            {
                try
                {
                    // MLeaderStyle Dictionary を読込モードで開く
                    var mLeaderStyleDict = tr.GetObject(db.MLeaderStyleDictionaryId, OpenMode.ForRead) as DBDictionary;

                    foreach (DBDictionaryEntry dict in mLeaderStyleDict)
                    {
                        // MLineStyle Dictionary を読込モードで開く
                        var mLeaderStyle = tr.GetObject(dict.Value, OpenMode.ForRead) as MLeaderStyle;
                    }
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error GetAllTextStyles Command\n\t" + ex.Message);
                }
            }
        }
    }
}
