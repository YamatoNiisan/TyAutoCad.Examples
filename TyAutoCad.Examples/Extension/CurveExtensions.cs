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
    /// Curve 拡張メソッド
    /// </summary>
    public static class CurveExtensions
    {
        /// <summary>
        /// 曲線上の点かどうか調べる
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool IsPointOnCurve(this Curve curve, Point3d point)
            => IsPointOnCurve(curve, point, Tolerance.Global);

        public static bool IsPointOnCurve(this Curve curve, Point3d point, Tolerance tolerance)
        {
            try
            {
                Point3d pt = curve.GetClosestPointTo(point, false);
                return (pt - point).Length <= tolerance.EqualPoint;
            }
            catch
            { }
            return false;
        }

        /// <summary>
        /// 曲線の長さを取得
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static double GetLength(this Curve curve)
        {
            if (curve == null)
                throw new ArgumentNullException("curve is null");
            if (curve is Ray || curve is Xline)
                return double.PositiveInfinity;
            return
                curve.GetDistanceAtParameter(curve.EndParam) - curve.GetDistanceAtParameter(curve.StartParam);
        }
    }
}
