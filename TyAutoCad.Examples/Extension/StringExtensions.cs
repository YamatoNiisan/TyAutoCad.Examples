using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// string 拡張メソッド
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 文字列内に半角数値文字が含まれているか調べる
        /// </summary>
        /// <param name="s">調べる文字列</param>
        /// <returns></returns>
        private static bool ContainsNumericString(this string s)
        {
            return s.Any(c => int.TryParse(c.ToString(), out int i));
        }

        /// <summary>
        /// 数字文字列の位置を取得 後ろから検索
        /// 
        /// 半角数値文字の存在を確認してから使用
        /// 戻り値タプル型
        /// </summary>
        /// <param name="s"></param>
        /// <returns>タプル型</returns>
        private static (int head, int tail) GetNumericStringPosition(this string s)
        {
            // 文字列の長さを取得する
            int tail = s.Length;
            // 最後の文字を取得
            string last = s.Substring(--tail);
            // 文字列の後ろから検索して半角数字が現れるまで続ける
            while (!int.TryParse(last, out int i))
            {
                last = s.Substring(--tail, 1);
            }
            int head = tail;
            // 数値文字が連続しているか調べる
            while (int.TryParse(last, out int i))
            {
                if (0 >= head)
                {
                    head--;
                    break;
                }
                last = s.Substring(--head, 1);
            }
            head++;
            return (head, tail);
        }

        /// <summary>
        /// 数字文字列を取得 後ろから検索
        /// </summary>
        /// 半角数値文字の存在を確認してから使用
        private static string GetNumericString(this string s)
        {
            // 数字文字列の位置を取得
            var position = s.GetNumericStringPosition();
            return s.Substring(position.head, position.tail - position.head + 1);
        }

        /// <summary>
        /// 連番文字列のリストを作成 
        /// 数値文字が含まれていない場合は後に連番を付加
        /// </summary>
        public static List<string> CreateSerialNumberStrings(this string s, int n)
        {
            // baseName に数値文字が含まれていない場合は後ろに _01 を追加
            if (!s.ContainsNumericString())
            {
                s += "_01";
            }
            // 戻り値の変数を宣言
            var serialNumberStrings = new List<string>();
            // 数値文字の後ろからの位置を取得
            var position = s.GetNumericStringPosition();
            // 数字文字列を取得 後ろから検索
            string last = s.GetNumericString();
            // 数字文字列の連番リストを作成
            List<string> serialNumbers = NumericString.CreateSerialNumbers(last, n);
            // StringBuilder変数を宣言
            StringBuilder sb;
            foreach (var numericString in serialNumbers)
            {
                sb = new StringBuilder(s);
                serialNumberStrings.Add(sb.Replace(last, numericString, position.head, position.tail - position.head + 1).ToString());
            }
            return serialNumberStrings;
        }
    }
}
