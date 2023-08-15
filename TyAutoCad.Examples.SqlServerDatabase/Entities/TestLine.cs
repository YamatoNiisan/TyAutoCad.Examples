using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples.SqlServerDatabase.Entities
{
    /// <summary>
    /// テスト用 直線クラス
    /// </summary>
    public class TestLine
    {
        #region Properties
        #region Id Property
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        public int Id { get; set; }
        #endregion

        #region Date Property 登録日
        /// <summary>
        /// 更新日
        /// </summary>
        public DateTime Date => DateTime.Now;
        #endregion

        #region StartPointX Property 始点のX座標
        /// <summary>
        /// 始点のX座標
        /// </summary>
        public double StartPointX { get; set; }
        #endregion

        #region StartPointY Property 始点のY座標
        /// <summary>
        /// 始点のY座標
        /// </summary>
        public double StartPointY { get; set; }
        #endregion

        #region EndPointX Property 終点のX座標
        /// <summary>
        /// 終点のX座標
        /// </summary>
        public double EndPointX { get; set; }
        #endregion

        #region EndPointY Property 終点のY座標
        /// <summary>
        /// 終点のY座標
        /// </summary>
        public double EndPointY { get; set; }
        #endregion

        #endregion
    }
}
