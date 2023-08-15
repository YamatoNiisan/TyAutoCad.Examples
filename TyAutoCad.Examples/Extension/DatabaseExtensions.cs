using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.MacroRecorder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// Database 拡張メソッド
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// 全ての寸法スタイルを取得する
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IEnumerable<DimStyleTableRecord> GetAllDimStyles(this Database db)
        {
            using (var tr = db.TransactionManager.StartOpenCloseTransaction())
            {
                // 寸法スタイルテーブルを読込モードで開く
                var dimStyleTable = tr.GetObject(db.DimStyleTableId, OpenMode.ForRead) as DimStyleTable;

                foreach (ObjectId id in dimStyleTable)
                {
                    yield return tr.GetObject(id, OpenMode.ForRead) as DimStyleTableRecord;
                }
            }
        }
    }
}
