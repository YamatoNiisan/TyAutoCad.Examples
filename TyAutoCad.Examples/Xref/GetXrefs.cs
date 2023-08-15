using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetXrefs))]
namespace TyAutoCad.Examples
{
    public class GetXrefs
    {
        /// <summary>
        /// 外部参照を取得
        /// </summary>
        [CommandMethod("GetXrefs")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // 取得した外部参照を格納するコレクションを準備
                    var xrefs = new List<BlockReference>();

                    // モデル空間を読込モードで取得
                    var ms = tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForRead) as BlockTableRecord;

                    foreach (ObjectId id in ms) 
                    {
                        // 図形を取得
                        var ent = tr.GetObject(id, OpenMode.ForRead) as Entity;//図形を読み取りで取得

                        // 外部参照かどうか調べる
                        switch (ent)
                        {
                            case BlockReference br:
                                // BlockReference の BlockTableRecord を取得
                                var brBtr = tr.GetObject(br.BlockTableRecord, OpenMode.ForRead) as BlockTableRecord;
                                // BlockReference の BlockTableRecord が xref かどうか調べる
                                if (brBtr.IsFromExternalReference)
                                {
                                    xrefs.Add(br);
                                }
                                brBtr.Dispose();
                                break;
                            default:
                                break;
                        }
                    }
                    ed.WriteMessage("\n外部参照の数 : {0}", xrefs.Count);

                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error GetXrefs\n\t" + ex.Message);
                }
            }
        }
    }
}
