using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// Entity 拡張メソッド
    /// </summary>
    public static class EntityExtensions
    {
        #region IsHatchSolid ##--NOTEST--##
        /// <summary>
        /// 図形がハッチングのソリッドかどうか調べる
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public static bool IsHatchSolid(this Entity ent)
            => ent is Hatch hatch && hatch.IsSolidFill;
        #endregion

        #region IsClippedBlock ##--NOTEST--##
        /// <summary>
        /// 図形がクリップされているブロックかどうか調べる
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public static bool IsClippedBlock(this Entity ent)
        {
            if (ent is BlockReference block)
            {
                if (block.IsClipped())
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
