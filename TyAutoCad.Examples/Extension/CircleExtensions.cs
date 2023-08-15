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
    /// Circle 拡張メソッド
    /// </summary>
    public static class CircleExtensions
    {
        /// <summary>
        /// 円周上の点かどうか調べる
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool IsOn(this Circle circle, Point3d point)
            => circle.Radius == circle.Center.DistanceTo(point);
 
        /// <summary>
        /// 円の内部の点かどうか調べる
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool Contains(this Circle circle, Point3d point)
            => circle.Radius > circle.Center.DistanceTo(point);

        /// <summary>
        /// 円の外部の点かどうか調べる
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool IsOutside(this Circle circle, Point3d point)
            => circle.Radius < circle.Center.DistanceTo(point);

        /// <summary>
        /// 点から円への接点を求める
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Point3dCollection GetTangentPoints(this Circle circle, Point3d point)
        {
            double r = circle.Radius;
            double x = point.X - circle.Center.X;
            double y = point.Y - circle.Center.Y;

            double z = Math.Pow(x, 2) + Math.Pow(y, 2);
            double w = Math.Sqrt(z - Math.Pow(r, 2));
            double ax = r * ((x * r - y * w) / z) + circle.Center.X;
            double ay = r * ((y * r + x * w) / z) + circle.Center.Y;
            double bx = r * ((x * r + y * w) / z) + circle.Center.X;
            double by = r * ((y * r - x * w) / z) + circle.Center.Y;

            return new Point3dCollection
            {
                new Point3d(ax, ay, 0.0),
                new Point3d(bx, by, 0.0)
            };
        }

        /// <summary>
        /// 点と円の中心を通る直線に平行な円の接点を求める
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Point3dCollection GetParallelTangentPoints(this Circle circle, Point3d point)
        {
            var lineEq = new LineEquation(circle.Center, point);
            return new Point3dCollection
            {
                circle.Center + (lineEq.Normal * circle.Radius),
                point + (lineEq.Normal * circle.Radius),
                circle.Center + (-lineEq.Normal * circle.Radius),
                point + (-lineEq.Normal * circle.Radius),
            };
        }
    }
}
