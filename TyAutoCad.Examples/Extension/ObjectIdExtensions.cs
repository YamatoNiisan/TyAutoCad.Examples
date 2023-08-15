using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// ObjectId 拡張メソッド
    /// </summary>
    public static class ObjectIdExtensions
    {
        public static bool IsValidEx(this ObjectId id)
        {
            return id.IsValid && !id.IsNull && !id.IsErased;
        }
    }
}
