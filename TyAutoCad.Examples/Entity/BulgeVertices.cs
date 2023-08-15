using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// ポリラインのふくらみと頂点を格納するクラス
    /// </summary>
    public class BulgeVertices
    {
        #region Properties
        /// <summary>
        /// 始点
        /// </summary>
        public Point2d StartPoint { get; set; }

        /// <summary>
        /// 終点
        /// </summary>
        public Point2d EndPoint { get; set; }

        /// <summary>
        /// ふくらみ
        /// </summary>
        public double Bulge { get; set; }
        #endregion
    }
}
