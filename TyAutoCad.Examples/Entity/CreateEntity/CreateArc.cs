using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.CreateArc))]
namespace TyAutoCad.Examples
{
    public class CreateArc
    {
        /// <summary>
        /// 円弧を作成
        ///     中心点(6.25,9.125,0), 半径 6, 
        ///     開始角度 1.117 (64 度), 終了角度 3.5605 (204 度)
        ///     の円弧をモデル空間に作成
        /// </summary>
        [CommandMethod("CreateArc")]
        public void Command()
        {
            // データベースを取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var db = doc.Database;

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // モデルスペースを書き込みモードで取得
                    var ms = tr.GetObject(
                        SymbolUtilityServices.GetBlockModelSpaceId(db),
                        OpenMode.ForWrite) as BlockTableRecord;

                    // 中心点(6.25,9.125,0), 半径 6, 
                    // 開始角度 1.117 (64 度), 終了角度 3.5605 (204 度)
                    // の円弧を作成
                    using (var arc = new Arc(new Point3d(6.25, 9.125, 0), 6, 1.117, 3.5605))
                    {
                        // モデルスペースに追加
                        ms.AppendEntity(arc);
                        tr.AddNewlyCreatedDBObject(arc, true);
                    }
                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error CreateArc\n\t" + ex.Message);
                }
            }
        }
    }
}
