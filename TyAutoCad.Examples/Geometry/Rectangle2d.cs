using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// 長方形クラス
    /// </summary>
    public class Rectangle2d : SimplePolygon2d
    {
        #region Constructors
        public Rectangle2d(Point2d p1, Point2d p2, double angle = 0.0)
        {
            Vertices = p1.GetRectangleVertices(p2, angle);
        }

        public Rectangle2d(Point3d p1, Point3d p2, double angle = 0.0)
            : this(p1.Convert2d(), p2.Convert2d(), angle)
        {
        }
        #endregion
    }
}
