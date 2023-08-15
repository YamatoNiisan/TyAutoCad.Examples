using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// Point2d 拡張メソッド
    /// </summary>
    public static class Point2dExtensions
    {
        #region Convert3d ##--TESTED--##
        /// <summary>
        /// Point3d に変換する
        /// </summary>
        /// <param name="point2d"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Point3d Convert3d(this Point2d point2d, double z = 0.0)
            => new Point3d(point2d.X, point2d.Y, z);

        /// <summary>
        /// 指定された平面に従って、Point3d に変換する
        /// </summary>
        /// <param name="pt">変換する点</param>
        /// <param name="plane">Point2d がある平面</param>
        /// <returns>変換された Point3d</returns>
        public static Point3d Convert3d(this Point2d pt, Plane plane) =>
            pt.Convert3d().TransformBy(Matrix3d.PlaneToWorld(plane));

        /// <summary>
        /// 法線と標高によって定義された平面に従って、Point3d に変換する
        /// </summary>
        /// <param name="pt">変換する点</param>
        /// <param name="normal">Point2d がある平面の法線</param>
        /// <param name="elevation">Point2d がある平面の高度</param>
        /// <returns>変換された Point3d</returns>
        public static Point3d Convert3d(this Point2d pt, Vector3d normal, double elevation) =>
            new Point3d(pt.X, pt.Y, elevation).TransformBy(Matrix3d.PlaneToWorld(normal));
        #endregion

        #region GetAngle ##--NOTEST--##
        /// <summary>
        /// 2点の角度を取得する
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static double GetAngle(this Point2d p, Point2d q) => p.GetVectorTo(q).Angle;

        #endregion

        #region GetMidpoint ##--NOTEST--##
        /// <summary>
        /// 2点間の中点の座標を求める
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point2d GetMidpoint(this Point2d a, Point2d b)
            => new Point2d(
                (a.X + b.X) * 0.5,
                (a.Y + b.Y) * 0.5);
        #endregion

        #region GetPolarPoint ##--TESTED--##
        /// <summary>
        /// 指定された角度と距離だけ離れた点を取得
        /// </summary>
        /// <param name="p"></param>
        /// <param name="angle"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static Point2d GetPolarPoint(this Point2d p, double angle, double distance)
            => new Point2d(
                p.X + distance * Math.Cos(angle),
                p.Y + distance * Math.Sin(angle));
        #endregion

        #region GetRectangleVertices ##--NOTEST--##
        /// <summary>
        /// 対角の頂点と角度から矩形の4頂点を取得
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Point2dCollection GetRectangleVertices(this Point2d p1, Point2d p2, double angle = 0.0)
        {
            if (angle != 0.0)
            {
                p2 = p2.RotateBy(-angle, p1);
            }

            double minX = p1.X;
            double maxX = p2.X;
            if (p1.X > p2.X)
            {
                minX = p2.X;
                maxX = p1.X;
            }

            double minY = p1.Y;
            double maxY = p2.Y;
            if (p1.Y > p2.Y)
            {
                minY = p2.Y;
                maxY = p1.Y;
            }

            var a = new Point2d(minX, minY);
            var b = new Point2d(maxX, minY);
            var c = new Point2d(maxX, maxY);
            var d = new Point2d(minX, maxY);

            if (angle != 0.0)
            {
                a = a.RotateBy(angle, p1);
                b = b.RotateBy(angle, p1);
                c = c.RotateBy(angle, p1);
                d = d.RotateBy(angle, p1);
            }

            return new Point2dCollection(new Point2d[] { a, b, c, d });
        }
        #endregion

        #region IsBetween ##--NOTEST--##
        /// <summary>
        /// 指定した点が 2 点で定義される線分上にあるかどうかを調べる
        /// </summary>
        /// <param name="pt">調べる点</param>
        /// <param name="p1">開始点</param>
        /// <param name="p2">終点</param>
        /// <param name="tol">比較に使用される許容範囲</param>
        /// <returns>点が2点間上にある場合は true 。 それ以外の場合は false。</returns>
        public static bool IsBetween(this Point2d pt, Point2d p1, Point2d p2, Tolerance tol = new Tolerance())
            => p1.GetVectorTo(pt).GetNormal(tol).Equals(pt.GetVectorTo(p2).GetNormal(tol));
        #endregion

        #region IsCollinear ##--TESTED--##
        /// <summary>
        /// 3点が一直線上の点かどうか調べる
        /// ※ この計算は遅い。符号付き面積 == 0 の判定のほうが、約5倍早い。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsCollinear(this Point2d a, Point2d b, Point2d c)
        {
            double ab = a.GetDistanceTo(b).Round(8);
            double bc = b.GetDistanceTo(c).Round(8);
            double ca = c.GetDistanceTo(a).Round(8);
            return (ab + ca == bc) || (ab + bc == ca) || (ca + bc == ab);
        }
        #endregion

        #region IsClockwise ##--NOTEST--##
        /// <summary>
        /// 3点が時計回りに配置されているかどうか調べる
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsClockwise(this Point2d a, Point2d b, Point2d c)
            => (b.X - a.X) * (c.Y - a.Y) < (b.Y - a.Y) * (c.X - a.X);
        #endregion
    }
}
