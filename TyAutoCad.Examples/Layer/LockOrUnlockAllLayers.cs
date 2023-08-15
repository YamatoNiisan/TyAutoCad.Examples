using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.LockOrUnlockAllLayers))]
namespace TyAutoCad.Examples
{
    /// <summary>
    /// 全ての画層をロックまたはロック解除する
    /// </summary>
    public static class LockOrUnlockAllLayers
    {
        public static void LockOrUnlockLayers(this Document doc, bool dolock, bool lockZero = false)
        {
            var db = doc.Database;
            var ed = doc.Editor;

            using (var tr = db.TransactionManager.StartTransaction())
            {
                var lt = tr.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                foreach (var ltrId in lt)
                {
                    // 現在のレイヤーまたはレイヤー 0 をロック/ロック解除しようとしないでください。
                    // (depending on whether lockZero == true for the latter)
                    if (ltrId != db.Clayer && (lockZero || ltrId != db.LayerZero))
                    {
                        // レイヤーを書き込み用に開き、ロックまたはロック解除する
                        var ltr = (LayerTableRecord)tr.GetObject(ltrId, OpenMode.ForWrite);
                        ltr.IsLocked = dolock;
                        ltr.IsOff = ltr.IsOff; // This is needed to force a graphics update
                    }
                }
                tr.Commit();
            }
            // These two calls will result in the layer's geometry fading/unfading
            // appropriately
            ed.ApplyCurDwgLayerTableChanges();
            ed.Regen();
        }

        /// <summary>
        /// 全ての画層をロックする
        /// </summary>
        [CommandMethod("LockAllLayers")]
        public static void Command()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            doc.LockOrUnlockLayers(true);
        }

        /// <summary>
        /// 全ての画層をロックまたはロック解除する
        /// </summary>
        [CommandMethod("UnlockAllLayers")]
        public static void Command1()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            doc.LockOrUnlockLayers(false);
        }
    }
}
