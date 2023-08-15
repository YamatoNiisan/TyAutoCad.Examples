using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.AAA))]
namespace TyAutoCad.Examples
{
    public class AAA
    {
        /// <summary>
        /// コマンド
        /// </summary>
        [CommandMethod("AAA")]
        public static void Command()
        {

        }
    }
}
