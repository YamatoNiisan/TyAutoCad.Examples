using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    public static class CircularArc2dExtensions
    {
        /// <summary>
        /// 円弧の2つの接線の交点を取得する
        /// </summary>
        /// <param name="arc"></param>
        /// <returns></returns>
        public static Point2d GetTangentIntersection(this CircularArc2d arc)
        {
            var angle = arc.Center.GetAngle(arc.StartPoint.GetMidpoint(arc.EndPoint));
            var a = arc.Radius / Math.Cos(arc.EndAngle / 2);
            return arc.Center.GetPolarPoint(angle, a);
        }
    }
}
