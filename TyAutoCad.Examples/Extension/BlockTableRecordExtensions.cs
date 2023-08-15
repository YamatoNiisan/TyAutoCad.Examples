using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// BlockTableRecordExtensions の拡張メソッド
    /// </summary>
    public static class BlockTableRecordExtensions
    {
        /// <summary>
        /// ブロックテーブルレコードの図形名とIDのディクショナリを返す
        /// </summary>
        /// <param name="btr"></param>
        /// <param name="EvalOffLayers">非表示画層を含めるかどうか</param>
        /// <param name="EvalFrozenLayers"></param>
        /// <returns></returns>
        public static Dictionary<string, List<ObjectId>> GetEntities(this BlockTableRecord btr, bool EvalOffLayers = true, bool EvalFrozenLayers = true)
        {
            var result = new Dictionary<string, List<ObjectId>>();
            foreach (ObjectId id in btr)
            {
                if (id.IsValid && !id.IsErased && !id.IsNull)
                {
                    var ent = id.GetObject(OpenMode.ForRead) as Entity;
                    if (ent != null)
                    {
                        var ltr = ent.LayerId.GetObject(OpenMode.ForRead) as LayerTableRecord;
                        bool eval = true;
                        if (!EvalOffLayers && ltr.IsOff)
                            eval = false;
                        if (!EvalFrozenLayers && ltr.IsFrozen)
                            eval = false;
                        if (eval)
                        {
                            if (!result.ContainsKey(id.ObjectClass.Name))
                                result.Add(id.ObjectClass.Name, new List<ObjectId>());
                            result[id.ObjectClass.Name].Add(id);
                        }
                    }
                }
            }
            return result;
        }


    }
}
