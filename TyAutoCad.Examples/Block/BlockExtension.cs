using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// ブロックに関する拡張メソッド
    /// </summary>
    public static class BlockExtension
    {
        #region GetBlockName ##--NOTEST--##
        /// <summary>
        /// ブロック名を取得する
        /// </summary>
        /// <param name="blockId"></param>
        /// <returns></returns>
        public static string GetBlockName(this Database db, ObjectId blockId)
        {
            if (db == null) return null;
            //var db = Application.DocumentManager.MdiActiveDocument.Database;
            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                // ブロックを取得
                var blockRef = tr.GetObject(blockId, OpenMode.ForRead) as BlockReference;

                if (blockRef != null)
                {
                    //return GetBlockName(tr, blockRef);

                    var blockDef = tr.GetObject(
                        (blockRef.IsDynamicBlock) ? blockRef.DynamicBlockTableRecord : blockRef.BlockTableRecord,
                        OpenMode.ForRead) as BlockTableRecord;
                    return blockDef.Name;
                }
                return blockRef.Name;
            }
        }

        public static string GetBlockName(ObjectId blockId)
            => Application.DocumentManager.MdiActiveDocument.Database.GetBlockName(blockId);
        #endregion

        #region HasBlock ##--NOTEST--##
        /// <summary>
        /// 現在の図面に指定ブロックが存在するか確認
        /// </summary>
        /// <param name="blockName"></param>
        /// <returns></returns>
        public static bool HasBlock(this Database db, string blockName)
        {
            if (db == null) return false;
            using (var tr = db.TransactionManager.StartTransaction())
            {
                // 現在の図面のブロック テーブルを読取モードで取得
                var bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                return bt.Has(blockName);
            }
        }

        public static bool HasBlock(string blockName)
            => Application.DocumentManager.MdiActiveDocument.Database.HasBlock(blockName);
        #endregion

        #region ToUniqueBlockName ##--NOTEST--##
        /// <summary>
        /// 一意のブロック名に変更
        /// </summary>
        public static string ToUniqueBlockName(this string blockName)
        {
            string newName = blockName;
            string tmp = blockName;
            int n = 0;
            while (HasBlock(newName))
            {
                n++;
                newName = tmp + "_" + n.ToString();
            }
            return newName;
        }
        #endregion
    }
}
