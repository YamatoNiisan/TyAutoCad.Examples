using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// 数値文字列を扱うクラス
    /// </summary>
    public static class NumericString
    {
        /// <summary>
        /// 数字文字列の連番リストを作成 フォーマットを守る
        /// </summary>
        /// <param name="str"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static List<string> CreateSerialNumbers(string str, int n)
        {
            string f = "D" + str.Length.ToString();
            return Enumerable.Range(int.Parse(str), n).ToList().ConvertAll(x => x.ToString(f));
        }
    }
}
