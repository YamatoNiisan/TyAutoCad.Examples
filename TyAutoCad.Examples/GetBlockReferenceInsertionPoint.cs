using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetBlockReferenceInsertionPoint))]
namespace TyAutoCad.Examples
{
    internal class GetBlockReferenceInsertionPoint
    {
        #region Command
        /// <summary>
        /// 選択したブロック図形の挿入基点を取得
        /// WCSとUCSで出力
        /// </summary>
        [CommandMethod("GetBlockReferenceInsertionPoint")]
        public static void Command()
        {
            // Document, Editor, Database, ucs を取得
            var doc = AcAp.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;
            var ucs = ed.CurrentUserCoordinateSystem;

            ed.WriteMessage("\n--- 選択したブロック図形の挿入基点を取得 ---");

            ed.WriteMessage("\n現在のUCS:{0}", ucs);

            // 挿入基点を取得するブロックを選択
            var peo = new PromptEntityOptions("\n挿入基点を取得するブロックを選択してください");
            peo.SetRejectMessage("\nブロック図形のみを選択してください");
            peo.AddAllowedClass(typeof(BlockReference), false);
            PromptEntityResult per = ed.GetEntity(peo);

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // 選択したブロックを取得
                    var blockRef = tr.GetObject(per.ObjectId, OpenMode.ForRead) as BlockReference;

                    // 現在の挿入基点を取得
                    // ここで取得される点はWCS座標
                    Point3d insertionPointWcs = blockRef.Position;

                    // UCS に変換 (変換行列にucsの逆行列を使う)
                    Point3d insertionPointUcs = insertionPointWcs.TransformBy(ucs.Inverse());

                    ed.WriteMessage("\nブロック挿入基点 WCS:{0}", insertionPointWcs);
                    ed.WriteMessage("\nブロック挿入基点 UCS:{0}", insertionPointUcs);
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    AcAp.ShowAlertDialog("Error GetBlockReferenceInsertionPoint\n\t" + ex.Message);
                }
            }

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
        #endregion
    }
}
