using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    public static class LayerExtensions
    {
        #region GetLockedLayers
        /// <summary>
        /// ロックされている画層の名前を取得
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetLockedLayers(this Database db)
        {
            if (db == null) return Enumerable.Empty<string>();
            var lockedLayers = new List<string>();

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                var layerTable = tr.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                foreach (ObjectId id in layerTable)
                {
                    var layer = tr.GetObject(id, OpenMode.ForRead) as LayerTableRecord;
                    if (layer.IsLocked)
                    {
                        lockedLayers.Add(layer.Name);
                    }
                }
            }
            return lockedLayers;
        }

        public static IEnumerable<string> GetLockedLayers()
            => Application.DocumentManager.MdiActiveDocument.Database.GetLockedLayers();
        #endregion
    }
}
