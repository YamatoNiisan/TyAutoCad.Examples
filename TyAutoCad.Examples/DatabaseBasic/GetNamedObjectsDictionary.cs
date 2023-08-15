using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetNamedObjectsDictionary))]
namespace TyAutoCad.Examples
{
    public class GetNamedObjectsDictionary
    {
        /// <summary>
        /// 名前付きオブジェクト ディクショナリを取得
        /// </summary>
        [CommandMethod("GetNamedObjectsDictionary")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- 名前付きオブジェクト ディクショナリを取得 ---");

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    ////// Named Objects Dictionary //////
                    // Color Dictionary を読込モードで開く
                    var colorDict = tr.GetObject(db.ColorDictionaryId, OpenMode.ForRead) as DBDictionary;

                    // Group Dictionary を読込モードで開く
                    var groupDict = tr.GetObject(db.GroupDictionaryId, OpenMode.ForRead) as DBDictionary;

                    // Image Dictionary ID を取得する
                    ObjectId imageDictId = RasterImageDef.GetImageDictionary(db);
                    // Image Dictionary を読込モードで開く
                    var imageDict = tr.GetObject(imageDictId, OpenMode.ForRead) as DBDictionary;

                    // LayerStateManager を取得
                    LayerStateManager layerStateManager = db.LayerStateManager;

                    // LayerStates Dictionary を読込モードで開く
                    var layerStatesDict = tr.GetObject(layerStateManager.LayerStatesDictionaryId(true), OpenMode.ForRead) as DBDictionary;

                    // Layout Dictionary を読込モードで開く
                    var layoutDict = tr.GetObject(db.LayoutDictionaryId, OpenMode.ForRead) as DBDictionary;
                    // レイアウトマネージャを取得
                    LayoutManager layoutManager = LayoutManager.Current;
                    // 現在のレイアウトを読込モードで開く
                    var currentLayout = tr.GetObject(layoutManager.GetLayoutId(layoutManager.CurrentLayout), OpenMode.ForRead) as Layout;

                    // Material Dictionary を読込モードで開く
                    var materiallDict = tr.GetObject(db.MaterialDictionaryId, OpenMode.ForRead) as DBDictionary;

                    // MLeaderStyle Dictionary を読込モードで開く
                    var mLeaderStyleDict = tr.GetObject(db.MLeaderStyleDictionaryId, OpenMode.ForRead) as DBDictionary;
                    // 現在の MLeaderStyle Id を取得
                    ObjectId mLeaderStyleId = db.MLeaderstyle;

                    // MLineStyle Dictionary を読込モードで開く
                    var mLStyleDict = tr.GetObject(db.MLStyleDictionaryId, OpenMode.ForRead) as DBDictionary;

                    // NamedObjects Dictionary を読込モードで開く
                    var namedObjectsDict = tr.GetObject(db.NamedObjectsDictionaryId, OpenMode.ForRead) as DBDictionary;

                    // PlotSettings Dictionary を読込モードで開く
                    var plotSettingsDict = tr.GetObject(db.PlotSettingsDictionaryId, OpenMode.ForRead) as DBDictionary;

                    // PlotStyleName Dictionary を読込モードで開く
                    var plotStyleNameDict = tr.GetObject(db.PlotStyleNameDictionaryId, OpenMode.ForRead) as DBDictionary;

                    // Scale List Dictionary を読込モードで開く // 不明
                    //var sclDict = tr.GetObject(db.ScaleListDictionaryId, OpenMode.ForRead) as DBDictionary;

                    // TableStyle Dictionary を読込モードで開く
                    var tableStyleDict = tr.GetObject(db.TableStyleDictionaryId, OpenMode.ForRead) as DBDictionary;

                    // Variable Dictionary を読込モードで開く // 不明
                    //var vab = tr.GetObject(db.VariableDictionaryId, OpenMode.ForRead) as DBDictionary;

                    // VisualStyle Dictionary を読込モードで開く
                    var visualStyleDict = tr.GetObject(db.VisualStyleDictionaryId, OpenMode.ForRead) as DBDictionary;
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error Command\n\t" + ex.Message);
                }
            }

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
