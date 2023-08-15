using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.CreateLine))]
namespace TyAutoCad.Examples
{
    public class CreateLine
    {
        /// <summary>
        /// 線分を作成
        ///     始点(0, 0, 0)、終点(75.5, -32.5, 0)の線分をモデル空間に作成
        /// </summary>
        [CommandMethod("CreateLine")]
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
                    // モデルスペースを書込モードで取得
                    var ms = tr.GetObject(
                        SymbolUtilityServices.GetBlockModelSpaceId(db),
                        OpenMode.ForWrite) as BlockTableRecord;

                    // 始点(0, 0, 0)、終点(75.5, -32.5, 0)の線分を作成
                    using (var line = new Line(Point3d.Origin, new Point3d(75.5, -32.5, 0)))
                    {
                        // モデルスペースに追加
                        ms.AppendEntity(line);
                        tr.AddNewlyCreatedDBObject(line, true);
                    }
                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error CreateLine\n\t" + ex.Message);
                }
            }
        }
    }
}
