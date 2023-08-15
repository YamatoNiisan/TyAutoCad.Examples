using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetLayerSelectedEntity))]
namespace TyAutoCad.Examples
{
    public class GetLayerSelectedEntity
    {
        /// <summary>
        /// 選択した図形の画層を取得する
        ///     図形を選択して、その図形の画層名と画層の色を出力する。
        /// </summary>
        [CommandMethod("GetLayerSelectedEntity")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            PromptEntityResult per = ed.GetEntity("\n図形を選択");
            if (per.Status != PromptStatus.OK) return;

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // 選択した図形を取得
                    var entity = tr.GetObject(per.ObjectId, OpenMode.ForRead) as Entity;

                    // 画層名を出力
                    ed.WriteMessage("\n画層名 : {0}", entity.Layer);

                    // 画層を取得
                    var layer = tr.GetObject(entity.LayerId, OpenMode.ForRead) as LayerTableRecord;

                    // 画層の色を出力
                    ed.WriteMessage("\n画層の色 : {0}", layer.Color);
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error GetLayerSelectedEntity\n\t" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 選択した図形の画層を取得する(LayerTableから画層を取得する)
        ///     図形を選択して、その図形の画層名と画層の色を出力する。
        /// </summary>
        [CommandMethod("GetLayerSelectedEntityFromLayerTable")]
        public void Command1()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            var peo = new PromptEntityOptions("\n図形を選択");
            PromptEntityResult per = ed.GetEntity(peo);
            if (per.Status != PromptStatus.OK)
            {
                return;
            }
            ObjectId objId = per.ObjectId;
            ed.WriteMessage("\n選択した図形のObjectId : " + objId);

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // 選択した図形を取得
                    var entity = tr.GetObject(per.ObjectId, OpenMode.ForRead) as Entity;

                    // 画層名を出力
                    ed.WriteMessage("\n画層名 : {0}", entity.Layer);

                    // レイヤーテーブルを読込モードで取得
                    var layerTable = tr.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;

                    // 画層を取得
                    var layer = tr.GetObject(layerTable[entity.Layer], OpenMode.ForRead) as LayerTableRecord;

                    // 画層の色を出力
                    ed.WriteMessage("\n画層の色 : {0}", layer.Color);
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error GetLayerSelectedEntityFromLayerTable\n\t" + ex.Message);
                }
            }
        }
    }
}
