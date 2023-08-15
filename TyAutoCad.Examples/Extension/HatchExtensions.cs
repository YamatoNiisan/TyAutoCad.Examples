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
    /// Hatch 拡張メソッド
    /// </summary>
    public static class HatchExtensions
    {
        #region GetLoopObjects ##--NOTEST--##
        /// <summary>
        /// 境界を構成するオブジェクトのコレクションを取得する
        /// コレクションに含まれる図形は、Polyline 又は Line 又は Arc
        /// </summary>
        /// <param name="hatch"></param>
        /// <returns></returns>
        public static DBObjectCollection GetLoopObjects(this Hatch hatch)
        {
            var objects = new DBObjectCollection();

            for (int i = 0; i < hatch.NumberOfLoops; i++)
            {
                var loop = hatch.GetLoopAt(i);
                if (loop.IsPolyline)
                {
                    var vertexies = loop.Polyline;
                    var poly = new Polyline();
                    for (int j = 0; j < vertexies.Count; j++)
                    {
                        var vertex = vertexies[j].Vertex;
                        var bulge = vertexies[j].Bulge;
                        poly.AddVertexAt(j, vertex, bulge, 0, 0);
                    }
                    objects.Add(poly);
                }
                else
                {
                    foreach (Curve2d cv in loop.Curves)
                    {
                        switch (cv)
                        {
                            case LineSegment2d lineSeg2d:
                                var line = new Line
                                {
                                    StartPoint = lineSeg2d.StartPoint.Convert3d(),
                                    EndPoint = lineSeg2d.EndPoint.Convert3d(),
                                };
                                objects.Add(line);
                                break;
                            case CircularArc2d cirArc2d:
                                if (cirArc2d.StartPoint == cirArc2d.EndPoint)
                                {
                                    var circle = new Circle
                                    {
                                        Center = cirArc2d.Center.Convert3d(),
                                        Radius = cirArc2d.Radius,
                                    };
                                    objects.Add(circle);
                                }
                                else
                                {
                                    var arc = new Arc
                                    {
                                        Center = cirArc2d.Center.Convert3d(),
                                        Radius = cirArc2d.Radius,
                                        StartAngle = cirArc2d.StartAngle,
                                        EndAngle = cirArc2d.EndAngle,
                                    };
                                    objects.Add(arc);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return objects;
        }
        #endregion
    }
}
