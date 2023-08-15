using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using Polyline = Autodesk.AutoCAD.DatabaseServices.Polyline;

namespace TyAutoCad.Examples
{
    public static class PolylineExtensions
    {
        #region AddVertexAt ##--NOTEST--##
        public static void AddVertexAt(this Polyline poly, int index, Vertex2d vertex)
        {
            poly.AddVertexAt(index, vertex.Position.Convert2d(),
                vertex.Bulge, vertex.StartWidth, vertex.EndWidth);
        }
        #endregion

        #region GetArcSegments ##--NOTEST--##
        /// <summary>
        /// すべての円弧要素を取得
        /// </summary>
        /// <param name="polyline"></param>
        /// <returns></returns>
        public static Curve2dCollection GetArcSegments(this Polyline polyline)
        {
            var arcSegments = new Curve2dCollection();

            for (int i = 0; i < polyline.NumberOfVertices; i++)
            {
                if (polyline.GetSegmentType(i) == SegmentType.Arc)
                {
                    arcSegments.Add(polyline.GetArcSegment2dAt(i));
                }
            }

            return arcSegments;
        }
        #endregion

        #region GetBulgingVertexIntersections ##--NOTEST--##
        /// <summary>
        ///  ふくらみを持つ全ての頂点の接線の交点を取得
        /// </summary>
        /// <param name="polyline"></param>
        /// <returns></returns>
        public static Point2dCollection GetBulgingVertexIntersections(this Polyline polyline)
        {
            var points = new Point2dCollection();

            foreach (CircularArc2d arc in polyline.GetArcSegments())
            {
                points.Add(arc.GetTangentIntersection());
            }

            return points;
        }
        #endregion

        #region GetFilletedPolyline ##--NOTEST--##
        /// <summary>
        /// 頂点を指定Rでフィレットしたポリラインを取得
        /// </summary>
        /// <param name="poly"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static Polyline GetFilletedPolyline(this Polyline poly, double r)
        {
            Point2d firstVertex = poly.GetPoint2dAt(0);
            Point2d lastVertex = poly.GetPoint2dAt(poly.NumberOfVertices - 1);

            List<BulgeVertices> BulgeVerticesList = new List<BulgeVertices>();

            for (int i = 1; i < poly.NumberOfVertices - 1; i++)
            {
                var p1 = poly.GetPoint2dAt(i - 1);
                var p2 = poly.GetPoint2dAt(i);
                var p3 = poly.GetPoint2dAt(i + 1);

                var v1 = p2.GetVectorTo(p1);
                var v2 = p2.GetVectorTo(p3);
                var gamma = v1.GetAngleTo(v2);
                var theta = Math.PI - gamma;
                var bulge = Math.Tan(theta / 4);

                if (p1.IsClockwise(p2, p3))
                {
                    bulge *= -1;
                }

                var d = r * Math.Tan(theta / 2);

                var bv = new BulgeVertices
                {
                    StartPoint = p2.GetPolarPoint(v1.Angle, d),
                    EndPoint = p2.GetPolarPoint(v2.Angle, d),
                    Bulge = bulge,
                };

                BulgeVerticesList.Add(bv);
            }

            int j = 1;
            var newPoly = new Polyline();

            newPoly.AddVertexAt(0, firstVertex, 0, 0, 0);

            foreach (var item in BulgeVerticesList)
            {
                newPoly.AddVertexAt(j, item.StartPoint, 0, 0, 0);
                newPoly.SetBulgeAt(j, item.Bulge);
                newPoly.AddVertexAt(j + 1, item.EndPoint, 0, 0, 0);
                j = j + 2;
            }

            newPoly.AddVertexAt(j, lastVertex, 0, 0, 0);
            return newPoly;
        }
        #endregion

        #region GetOffsetPolyline ##--NOTEST--##
        public static Polyline GetOffsetPolyline(this Polyline poly, double offsetDist)
        {
            return poly.GetOffsetCurves(offsetDist)[0] as Polyline;
        }
        #endregion

        #region GetSegmentIndexAtPoint ##--NOTEST--##
        /// <summary>
        /// 指定点にある要素が何番目の要素か返す
        /// </summary>
        /// <param name="polyline"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static int GetSegmentIndexAtPoint(this Polyline polyline, Point3d point)
        {
            return (int)Math.Floor(polyline.GetParameterAtPoint(point));
        }
        #endregion

        #region GetSegmentTypeAtPoint ##--NOTEST--##
        /// <summary>
        /// 指定点にある要素のタイプを返す
        /// </summary>
        /// <param name="polyline"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static SegmentType GetSegmentTypeAtPoint(this Polyline polyline, Point3d point)
        {
            //var a = polyline.GetSegmentIndexAtPoint(point)
            return polyline.GetSegmentType(polyline.GetSegmentIndexAtPoint(point));
        }
        #endregion

        #region GetVertexAt ##--NOTEST--##
        public static Vertex2d GetVertexAt(this Polyline poly, int index)
        {
            return new Vertex2d(poly.GetPoint2dAt(index).Convert3d(),
                poly.GetBulgeAt(index), poly.GetStartWidthAt(index), poly.GetEndWidthAt(index), 0);
        }
        #endregion

        #region GetVertices ##--NOTEST--##
        /// <summary>
        /// 全ての頂点を取得
        /// </summary>
        /// <param name="polyline"></param>
        /// <returns></returns>
        public static Point2dCollection GetVertices(this Polyline polyline)
        {
            var vertices = new Point2dCollection();

            for (int i = 0; i < polyline.NumberOfVertices; i++)
            {
                vertices.Add(polyline.GetPoint2dAt(i));
            }

            return vertices;
        }
        #endregion

        #region IsLineSegment ##--NOTEST--##
        /// <summary>
        /// 指定インデックスの要素が Line かどうか調べる
        /// </summary>
        /// <param name="polyline"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool IsLineSegment(this Polyline polyline, int index)
        {
            return polyline.GetSegmentType(index) == SegmentType.Line;
        }

        /// <summary>
        /// 指定点にある要素が Line かどうか調べる
        /// </summary>
        /// <param name="polyline"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool IsLineSegment(this Polyline polyline, Point3d point)
        {
            return polyline.GetSegmentType(polyline.GetSegmentIndexAtPoint(point)) == SegmentType.Line;
        }
        #endregion


        #region IsPointInside ##--TESTED--##
        /// <summary>
        /// ポリライン内の点かどうか調べる（ポリライン上の点も含む）
        ///     閉じたポリラインに使用
        ///     
        /// AutoCAD Map 3D の MPolygon を使う
        ///     "AcMPolygonMGD.dll" を参照に追加
        /// </summary>
        /// <param name="pline"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool IsPointInside(this Polyline polyline, Point3d point)
        {
            double tolerance = Tolerance.Global.EqualPoint;
            using (var mPolygon = new MPolygon())
            {
                mPolygon.AppendLoopFromBoundary(polyline, true, tolerance);
                return mPolygon.IsPointInsideMPolygon(point, tolerance).Count == 1;
            }
        }
        #endregion

        #region IsPointInside ##--TESTED--##
        /// <summary>
        /// ポリライン上の点かどうか調べる
        /// </summary>
        /// <param name="polyline"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool IsPointOnPolyline(this Polyline polyline, Point3d point)
        {
            bool isOn = false;
            for (int i = 0; i < polyline.NumberOfVertices; i++)
            {
                Curve3d segment = null;

                SegmentType segmentType = polyline.GetSegmentType(i);
                if (segmentType == SegmentType.Arc)
                    segment = polyline.GetArcSegmentAt(i);
                else if (segmentType == SegmentType.Line)
                    segment = polyline.GetLineSegmentAt(i);

                if (segment != null)
                {
                    isOn = segment.IsOn(point);
                    if (isOn)
                        break;
                }
            }
            return isOn;
        }
        #endregion

        #region IsSimplePolygon ##--TESTED--##
        /// <summary>
        /// ポリラインが単純な多角形かどうか調べる
        /// </summary>
        /// <param name="polyline"></param>
        /// <returns></returns>
        public static bool IsSimplePolygon(this Polyline polyline)
        {
            // 全ての頂点を取得
            var vertices = polyline.GetVertices();

            // ポリラインが閉じていない または、
            // 頂点の数が3未満 または、
            // ふくらみ（円弧）が含まれている または、
            // 線分のみで構成されていない 場合は false
            if (!polyline.Closed || vertices.Count < 3 || polyline.HasBulges || !polyline.IsOnlyLines)
            {
                return false;
            }

            // 頂点が3つの場合
            // 3点が一直線上に並んでいる場合は false
            // （三角形を構成できない）
            if (vertices.Count == 3 && vertices[0].IsCollinear(vertices[1], vertices[2]))
            {
                return false;
            }

            // 次の点と手前の点が辺上にないか調べる
            var endPoints = vertices.Rotation(1);
            var nextPoints = vertices.Rotation(2);
            var beforePoints = vertices.Rotation(-1);

            for (int i = 0; i < vertices.Count; i++)
            {
                var startPoint = vertices[i];
                var endPoint = endPoints[i];
                var nextPoint = nextPoints[i];
                var beforePoint = beforePoints[i];

                var dis = startPoint.GetDistanceTo(endPoint);

                var npa = nextPoint.GetDistanceTo(startPoint);
                var npb = nextPoint.GetDistanceTo(endPoint);

                var bpa = beforePoint.GetDistanceTo(startPoint);
                var bpb = beforePoint.GetDistanceTo(endPoint);

                if (dis == npa + npb || dis == bpa + bpa)
                {
                    return false;
                }
            }

            // 隣り合う辺以外の辺とに交点が存在するかどうか調べる
            for (int i = 0; i < vertices.Count; i++)
            {
                var line = new LineSegment2d(vertices[i], endPoints[i]);
                var testSps = vertices.Rotation(2 + i);
                var testEps = vertices.Rotation(3 + i);
                for (int j = 0; j < vertices.Count - 3; j++)
                {
                    var testLine = new LineSegment2d(testSps[j], testEps[j]);
                    var ips = line.IntersectWith(testLine);
                    if (ips == null)
                    {
                        continue;
                    }

                    if (ips.Length > 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion
    }
}
