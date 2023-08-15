using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetLockedLayerNames))]
namespace TyAutoCad.Examples
{
    public class GetLockedLayerNames
    {
        /// <summary>
        /// ロックされている画層の名前を取得する
        /// </summary>
        [CommandMethod("GetLockedLayerNames")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            // ロックされている画層の名前を格納するリストを準備
            var locked = new List<string>();

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // レイヤーテーブルを読込モードで取得
                    var layerTable = tr.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;

                    // ロックされている画層を取得
                    foreach (ObjectId id in layerTable)
                    {
                        // 画僧を取得
                        var layer = tr.GetObject(id, OpenMode.ForRead) as LayerTableRecord;

                        // ロックされているかどうか
                        if (layer.IsLocked)
                        {
                            locked.Add(layer.Name);
                        }
                    }
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error GetLockedLayerNames\n\t" + ex.Message);
                }
            }

            if (locked.Count > 0)
            {
                foreach (var item in locked.OrderBy(n => n))
                {
                    ed.WriteMessage("\nロックされている画層 : " + item);
                }
            }
            else
            {
                Application.ShowAlertDialog("ロックされている画層はありません。");
            }
        }
    }
}
