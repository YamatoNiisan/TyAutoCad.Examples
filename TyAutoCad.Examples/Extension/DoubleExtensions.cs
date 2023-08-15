using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// Double 拡張メソッド
    /// </summary>
    public static class DoubleExtensions
    {
        #region ConvertDegreesToRadians ##--NOTEST--##
        /// <summary>
        /// 度 -> ラジアン 変換
        /// </summary>
        /// <param name="degrees">度</param>
        /// <returns>ラジアン</returns>
        public static double ConvertDegreesToRadians(this double degrees)
        {
            return ((Math.PI / 180) * degrees);
        }
        #endregion

        #region ConvertRadiansToDegrees ##--NOTEST--##
        /// <summary>
        /// ラジアン -> 度 変換
        /// </summary>
        /// <param name="radians">ラジアン</param>
        /// <returns>度</returns>
        public static double ConvertRadiansToDegrees(this double radians)
        {
            return ((180 / Math.PI) * radians);
        }
        #endregion

        #region Round ##--TESTED--##
        /// <summary>
        /// 四捨五入する
        /// </summary>
        /// <param name="val"></param>
        /// <param name="digits">戻り値の小数点以下の桁数</param>
        /// <returns></returns>
        public static double Round(this double val, int digits = 0)
        {
            return Math.Round(val, digits, MidpointRounding.AwayFromZero);
        }
        #endregion
    }
}
