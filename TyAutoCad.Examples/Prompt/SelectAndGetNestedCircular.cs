using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.SelectAndGetNestedCircular))]
namespace TyAutoCad.Examples
{
    public class SelectAndGetNestedCircular
    {
        /// <summary>
        /// 円形図形を選択して取得
        /// 円、円弧、ポリラインの円弧
        /// </summary>
        [CommandMethod("SelectAndGetCircular2")]
        public static void Command()
        {

            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;
            var ucs = ed.CurrentUserCoordinateSystem;

            ed.WriteMessage("\n--- 円形図形を選択して取得 ---");

            var dxfNames = new List<string>
            {
                "ARC", "CIRCLE", "LWPOLYLINE"
            };

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    var options = new PromptNestedEntityOptions("\n円または円弧を選択:");
                    PromptNestedEntityResult result;
                    Entity ent;
                    Point3d center = Point3d.Origin;
                    Point3d pointOnCurve = Point3d.Origin;
                    bool flag = true;
                    do
                    {
                        // 図形を選択
                        result = ed.GetNestedEntity(options);

                        if (result.Status != PromptStatus.OK) return;

                        if (!dxfNames.Contains(result.ObjectId.ObjectClass.DxfName))
                        {
                            ed.WriteMessage("\n選択した図形は、円状図形ではありません。");
                            continue;
                        }

                        // 選択した図形を取得
                        ent = tr.GetObject(result.ObjectId, OpenMode.ForRead) as Entity;

                        // WCSに変換されたEntityのコピーをCurveにキャストして取得
                        // (選択された図形の変換マトリックスを渡す)
                        var curve = ent.GetTransformedCopy(result.Transform) as Curve;

                        // 選択した点を WCS に変換
                        var pickedPoint = result.PickedPoint.TransformBy(ucs);

                        // 選択した点に一番近い Curve 上の点を取得 (WCS)
                        pointOnCurve = curve.GetClosestPointTo(pickedPoint, true);

                        if (curve is Polyline poly)
                        {
                            // 要素が線分か円弧か調べる、線分の場合は再選択
                            if (poly.IsLineSegment(pointOnCurve))
                            {
                                ed.WriteMessage("\n選択した図形は、円状図形ではありません。");
                                continue;
                            }

                            // 選択した箇所の要素インデックスを取得
                            int index = poly.GetSegmentIndexAtPoint(pointOnCurve);

                            // ポリラインの円弧要素を取得して円弧を作成
                            var polyArc = Curve.CreateFromGeCurve(poly.GetArcSegmentAt(index)) as Arc;

                            center = polyArc.Center;
                        }

                        if (curve is Arc arc) center = arc.Center;

                        if (curve is Circle circle) center = circle.Center;

                        flag = false;
                    } while (flag);

                    ed.WriteMessage("\n選択した図形の中心(WCS):{0}", center);
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error SelectAndGetNestedCircular\n\t" + ex.Message);
                }
            }
            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
