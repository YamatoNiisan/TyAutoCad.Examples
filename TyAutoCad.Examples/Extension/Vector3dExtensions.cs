using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// Vector3d 拡張メソッド
    /// </summary>
    public static class Vector3dExtensions
    {
        /// <summary>
        /// Z値を除去して、Vector2d に変換する
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector2d Convert2d(this Vector3d vector) => new Vector2d(vector.X, vector.Y);

        #region Flatten ##--NOTEST--##
        /// <summary>
        /// ベクトルを WCS XY 平面に投影します。
        /// </summary>
        /// <param name="vector">このメソッドが適用されるインスタンス</param>
        /// <returns>射影されたベクトル</returns>
        public static Vector3d Flatten(this Vector3d vector) => new Vector3d(vector.X, vector.Y, 0.0);
        #endregion

        #region ToUnitVector ##--NOTEST--##
        /// <summary>
        /// このベクトルの単位ベクトルを返す
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3d ToUnitVector(this Vector3d v)
        {
            var dis = Point3d.Origin.DistanceTo(new Point3d(v.X, v.Y, v.Z));
            return new Vector3d(v.X / dis, v.Y / dis, v.Z / dis);
        }
        #endregion

        #region ToUnit ##--NOTEST--##
        /// <summary>
        /// 単位ベクトルを返す
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3d ToUnit(this Vector3d v)
        {
            return new Vector3d(v.X / v.Length, v.Y / v.Length, v.Z / v.Length);
        }
        #endregion
    }
}
