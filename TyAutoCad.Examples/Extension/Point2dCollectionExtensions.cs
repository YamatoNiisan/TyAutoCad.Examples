using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// Point2dCollection 拡張メソッド
    /// </summary>
    public static class Point2dCollectionExtensions
    {
        #region GetFirst ##--NOTEST--##
        /// <summary>
        /// コレクションの最初の要素を返す
        /// </summary>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static Point2d GetFirst(this Point2dCollection coll)
        {
            return coll[0];
        }
        #endregion

        #region GetLast ##--NOTEST--##
        /// <summary>
        /// コレクションの最後の要素を返す
        /// </summary>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static Point2d GetLast(this Point2dCollection coll)
        {
            return coll[coll.Count - 1];
        }
        #endregion

        #region GetLines ##--NOTEST--##
        /// <summary>
        /// 各点を結ぶ直線のコレクションを取得
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Line2dCollection GetLines(this Point2dCollection points)
        {
            var lines = new Line2dCollection();
            for (int i = 0; i < points.Count - 1; i++)
            {
                var line = new Line2d(points[i], points[i + 1]);
                lines.Add(line);
            }
            return lines;
        }
        #endregion

        #region GetLineSegments ##--NOTEST--##
        /// <summary>
        /// 各点を結ぶ線分のコレクションを取得
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Curve2dCollection GetLineSegments(this Point2dCollection points)
        {
            var lineSegments = new Curve2dCollection();
            for (int i = 0; i < points.Count - 1; i++)
            {
                var lineSeg = new LineSegment2d(points[i], points[i + 1]);
                lineSegments.Add(lineSeg);
            }
            return lineSegments;
        }
        #endregion

        #region GetTotalLength ##--NOTEST--##
        /// <summary>
        /// 全長を取得
        /// </summary>
        /// <param name="points"></param>
        /// <param name="nextPoint"></param>
        /// <returns></returns>
        public static double GetTotalLength(this Point2dCollection points, Point2d nextPoint)
        {
            double length = 0.0;

            if (points.Count < 1)
            {
                return length;
            }

            if (2 < points.Count)
            {
                for (int i = 1; i < points.Count; i++)
                {
                    length += points[i - 1].GetDistanceTo(points[i]);
                }
            }
            length += points[points.Count - 1].GetDistanceTo(nextPoint);
            return length;
        }
        #endregion

        #region RemoveFirst ##--NOTEST--##
        /// <summary>
        /// 最初の要素を除去したコレクションを返す
        /// </summary>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static Point2dCollection RemoveFirst(this Point2dCollection coll)
        {
            var points = new Point2dCollection();

            for (int i = 1; i < coll.Count; i++)
            {
                points.Add(coll[i]);
            }

            return points;
        }
        #endregion

        #region RemoveLast ##--NOTEST--##
        /// <summary>
        /// 最後の要素を除去したコレクションを返す
        /// </summary>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static Point2dCollection RemoveLast(this Point2dCollection coll)
        {
            var points = new Point2dCollection();

            for (int i = 0; i < coll.Count - 1; i++)
            {
                points.Add(coll[i]);
            }

            return points;
        }
        #endregion

        #region Rotation ##--NOTEST--##
        /// <summary>
        /// 指定回数回転させたコレクションを返す
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Point2dCollection Rotation(this Point2dCollection coll, int n)
        {
            if (n == 0)
            {
                return coll;
            }

            var points = new Point2dCollection();
            var tmp = new Point2dCollection();
            foreach (var item in coll)
            {
                tmp.Add(item);
            }

            // 左回転
            if (n > 0)
            {
                for (int i = 0; i < n; i++)
                {
                    points.Clear();
                    points = tmp.RemoveFirst();
                    points.Add(tmp.GetFirst());
                    tmp.Clear();
                    foreach (Point2d p in points)
                    {
                        tmp.Add(p);
                    }
                }
            }

            // 右回転
            if (n < 0)
            {
                for (int i = 0; i < Math.Abs(n); i++)
                {
                    points.Clear();
                    points = tmp.RemoveLast();
                    points.Insert(0, tmp.GetLast());
                    tmp.Clear();
                    foreach (Point2d p in points)
                    {
                        tmp.Add(p);
                    }
                }
            }
            return points;
        }
        #endregion
    }
}
