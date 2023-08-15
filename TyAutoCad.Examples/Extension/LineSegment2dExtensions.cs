using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// LineSegment2d 拡張メソッド
    /// </summary>
    public static class LineSegment2dExtensions
    {
        /// <summary>
        /// LineSegment2d から Line を作成
        /// </summary>
        /// <param name="lineSegment2D"></param>
        /// <returns></returns>
        public static Line GetDbLine(this LineSegment2d lineSegment2D)
            => new Line(lineSegment2D.StartPoint.Convert3d(), lineSegment2D.EndPoint.Convert3d());

        /// <summary>
        /// 与えられた曲線コレクションの中に交差する曲線が存在するかどうか調べる
        /// </summary>
        /// <param name="lineSegment"></param>
        /// <param name="curves"></param>
        /// <returns></returns>
        public static bool IsIntersect(this LineSegment2d lineSegment, Curve2dCollection curves)
        {
            foreach (Curve2d item in curves)
            {
                Point2d[] p = null;
                switch (item)
                {
                    case CircularArc2d cir:
                        p = cir.IntersectWith(lineSegment);
                        break;
                    case EllipticalArc2d elp:
                        p = elp.IntersectWith(lineSegment);
                        break;
                    case LinearEntity2d line:
                        p = line.IntersectWith(lineSegment);
                        break;
                    default:
                        break;
                }
                if (p != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
