using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// 直線の方程式クラス
    /// </summary>
    public class LineEquation
    {
        #region Constructors
        /// <summary>
        /// 2点から直線の方程式（一般形）を定義
        /// ax + by + c = 0 (a, b は同時に 0 ではない)
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public LineEquation(Point3d p1, Point3d p2)
        {
            double len = p1.DistanceTo(p2);
            A = (p2.Y - p1.Y) / len;
            B = (p1.X - p2.X) / len;
            C = ((p2.X * p1.Y) - (p1.X * p2.Y)) / len;
        }
        #endregion

        #region Properties
        public double A { get; }
        public double B { get; }
        public double C { get; }

        /// <summary>
        /// 法線ベクトル
        /// </summary>
        public Vector3d Normal => new Vector3d(A, B, 0);
        #endregion
    }
}
