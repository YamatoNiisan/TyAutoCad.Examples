using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    public static class IListExtensions
    {
        /// <summary>
        /// 配列やリストが空かどうかを返す拡張メソッド
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IList<T> self)
            => self.Count == 0;
    }
}
