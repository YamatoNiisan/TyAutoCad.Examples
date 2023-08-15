using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    public class GetAllEntitiesInModelSpace
    {
        /// <summary>
        /// モデル空間の全ての図形を取得する
        ///     モデル空間内の全ての図形を取得して、
        ///     図形の種類と図形の数を出力する。
        /// </summary>
        [CommandMethod("GetAllEntitiesInModelSpace")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- モデル空間の全ての図形を取得する ---");

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // モデル空間を読込モードで取得
                    var ms = tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForRead) as BlockTableRecord;

                    int count = 0;
                    Entity entity = null;

                    // モデル空間から図形を取得する
                    foreach (ObjectId id in ms)
                    {
                        count++;
                        // 図形を取得
                        entity = tr.GetObject(id, OpenMode.ForRead, true) as Entity;

                        // 図形の種類を出力
                        ed.WriteMessage("\n図形 : {0}", entity);
                    }
                    ed.WriteMessage("\n\n図形の数 : {0}", count);
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error GetAllEntitiesInModelSpace\n\t" + ex.Message);
                }
            }

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
