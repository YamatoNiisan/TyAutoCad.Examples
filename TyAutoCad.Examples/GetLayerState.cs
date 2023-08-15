using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetLayerState))]
namespace TyAutoCad.Examples
{
    public class GetLayerState
    {
        /// <summary>
        /// 画層の状態を取得して出力
        /// </summary>
        [CommandMethod("GetLayerState")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            // 画層を格納するリストを準備
            var layers = new List<LayerTableRecord>();

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // レイヤーテーブルを読込モードで取得
                    var layerTable = tr.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;

                    foreach (ObjectId id in layerTable)
                    {
                        // 画層を取得
                        var layer = tr.GetObject(id, OpenMode.ForRead) as LayerTableRecord;

                        // リストに追加
                        layers.Add(layer);
                    }
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error GetAllLayers\n\t" + ex.Message);
                }
            }

            // 画層名を出力
            foreach (var l in layers.OrderBy(l => l.Name))
            {
                ed.WriteMessage("\n画層名 : " + l.Name);

                if(l.IsFrozen)
                {
                    ed.WriteMessage("\n\tフリーズ : {0}", "ON");
                }
                else
                {
                    ed.WriteMessage("\n\tフリーズ : {0}", "OFF");
                }
            }
        }
    }
}
