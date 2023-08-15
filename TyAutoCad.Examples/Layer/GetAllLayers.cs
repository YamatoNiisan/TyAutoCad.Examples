using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetAllLayers))]
namespace TyAutoCad.Examples
{
    public class GetAllLayers
    {
        /// <summary>
        /// 図面内の全ての画層を取得する
        ///     図面内の全ての画層を取得して、画層名をソートして出力
        /// </summary>
        [CommandMethod("GetAllLayers")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

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

                        ed.WriteMessage("\n画層名 : " + layer.Name);
                    }
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error GetAllLayers\n\t" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 図面内の全ての画層を取得する
        ///     図面内の全ての画層を取得して、画層名をソートして出力
        /// </summary>
        [CommandMethod("GetAllLayersList")]
        public void Command0()
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
            foreach (var item in layers.OrderBy(l => l.Name))
            {
                ed.WriteMessage("\n画層名 : " + item.Name);
            }
        }

        // LINQ を使用
        [CommandMethod("GetAllLayersWithLinq")]
        public void Command1()
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

                    // 画層を取得
                    layers = layerTable.Cast<ObjectId>()
                        .Select(id => tr.GetObject(id, OpenMode.ForRead) as LayerTableRecord)
                        .OrderBy(l => l.Name)
                        .ToList();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error GetAllLayersWithLinq\n\t" + ex.Message);
                }
            }

            // 画層名を出力
            layers.ForEach(l => ed.WriteMessage("\n画層名 : " + l.Name));
        }
    }
}
