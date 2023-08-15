using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// PromptNestedEntityResult 拡張メソッド
    /// </summary>
    public static class PromptNestedEntityResultExtensions
    {
        #region IsDimension ##--NOTEST--##
        /// <summary>
        /// GetNestedEntity で選択した図形が寸法かどうか調べる
        /// </summary>
        /// <param name="nestedEntityResult"></param>
        /// <param name="tr"></param>
        /// <returns></returns>
        public static bool IsDimension(this PromptNestedEntityResult nestedEntityResult, Transaction tr)
        {
            // 選択した図形を取得
            var ent = tr.GetObject(nestedEntityResult.ObjectId, OpenMode.ForRead) as Entity;
            if (ent.BlockName.Substring(0, 2) == "*D") return true;

            foreach (var id in nestedEntityResult.GetContainers())
            {
                var container = tr.GetObject(id, OpenMode.ForRead) as Entity;
                if (container.BlockName.Substring(0, 2) == "*D")
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
