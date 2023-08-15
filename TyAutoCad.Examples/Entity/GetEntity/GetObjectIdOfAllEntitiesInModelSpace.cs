using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetObjectIdOfAllEntitiesInModelSpace))]
namespace TyAutoCad.Examples
{
    public class GetObjectIdOfAllEntitiesInModelSpace
    {
        /// <summary>
        /// モデル空間の全ての図形の ObjectId を取得する
        ///     モデルスペース内の全てのObjectIdを取得して、図形のDXF名を出力する
        /// </summary>
        [CommandMethod("GetObjectIdOfAllEntitiesInModelSpace")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- モデル空間の全ての図形の ObjectId を取得する ---");

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // モデル空間を読込モードで取得
                    var ms = tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForRead) as BlockTableRecord;

                    int count = 0;

                    // モデル空間から図形の ObjectId を取得する
                    foreach (ObjectId id in ms)
                    {
                        count++;
                        // 図形のDXF名を出力
                        ed.WriteMessage("\n図形のDXF名 : {0}", id.ObjectClass.DxfName);
                    }
                    ed.WriteMessage("\n\n図形の数 : " + count);
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error GetAllObjectIdsInModelSpace\n\t" + ex.Message);
                }
            }

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
