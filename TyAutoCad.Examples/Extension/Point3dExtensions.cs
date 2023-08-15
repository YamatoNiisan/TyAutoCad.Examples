using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// Point3d 拡張メソッド
    /// </summary>
    public static class Point3dExtensions
    {
        #region Convert2d ##--TESTED--##
        /// <summary>
        /// Z値を除去して、Point2d に変換する
        /// </summary>
        /// <param name="point3d"></param>
        /// <returns></returns>
        public static Point2d Convert2d(this Point3d point3d) => new Point2d(point3d.X, point3d.Y);
        #endregion

        #region GetAngle ##--NOTEST--##
        /// <summary>
        /// 2点の角度を取得する
        /// Point2d に変換して調べる
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static double GetAngle(this Point3d p, Point3d q)
            => p.Convert2d().GetAngle(q.Convert2d());

        #endregion

        #region GetEquallyDividedPoints ##--NOTEST--##
        /// <summary>
        /// 2点間の等分割点のコレクションを取得(始点,終点含むまない)
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <param name="n">分割数</param>
        /// <returns></returns>
        public static Point3dCollection GetEquallyDividedPoints(this Point3d p, Point3d q, int n)
        {
            Vector3d move = (q - p) / n;
            var points = new Point3dCollection();
            for (int i = 0; i < n - 1; i++)
            {
                p += move;
                points.Add(p);
            }
            return points;
        }
        #endregion

        #region GetMidpoint ##--NOTEST--##
        /// <summary>
        /// 2点間の中点の座標を求める
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point3d GetMidpoint(this Point3d a, Point3d b)
            => new Point3d((a.X + b.X) * 0.5, (a.Y + b.Y) * 0.5, (a.Z + b.Z) * 0.5);
        #endregion

        #region GetPolarPoint ##--NOTEST--##
        /// <summary>
        /// 指定された角度と距離だけ離れた点を取得
        /// </summary>
        /// <param name="p"></param>
        /// <param name="angle"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static Point3d GetPolarPoint(this Point3d p, double angle, double distance)
            => new Point3d(
                p.X + distance * Math.Cos(angle),
                p.Y + distance * Math.Sin(angle),
                p.Z);
        #endregion

        #region IsCollinear ##--TESTED--##
        /// <summary>
        /// 3点が一直線上の点かどうか調べる
        /// Point2d に変換して調べる
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsCollinear(this Point3d a, Point3d b, Point3d c)
            => a.Convert2d().IsCollinear(b.Convert2d(), c.Convert2d());
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
        public static bool IsBetween(this Point3d pt, Point3d p1, Point3d p2, Tolerance tol)
            => p1.GetVectorTo(pt).GetNormal(tol).Equals(pt.GetVectorTo(p2).GetNormal(tol));
        #endregion

        #region IsClockwise ##--NOTEST--##
        /// <summary>
        /// 3点が時計回りに配置されているかどうか調べる
        /// Point2d に変換して調べる
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsClockwise(this Point3d a, Point3d b, Point3d c)
            => a.Convert2d().IsClockwise(b.Convert2d(), c.Convert2d());
        #endregion
    }
}
