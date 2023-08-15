using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.CreateCircle))]
namespace TyAutoCad.Examples
{
    public class CreateCircle
    {
        /// <summary>
        /// 円を作成
        ///     中心が原点,半径55.5の円をモデル空間に作成
        /// </summary>
        [CommandMethod("CreateCircle")]
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

                    // 中心が原点,半径55.5の円を作成
                    using (var circle = new Circle(Point3d.Origin, Vector3d.ZAxis, 55.5))
                    {
                        // モデルスペースに追加
                        ms.AppendEntity(circle);
                        tr.AddNewlyCreatedDBObject(circle, true);
                    }
                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error CreateCircle\n\t" + ex.Message);
                }
            }
        }
    }
}
