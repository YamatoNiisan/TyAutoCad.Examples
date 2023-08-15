using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetTableAndTableRecord))]
namespace TyAutoCad.Examples
{
    public class GetTableAndTableRecord
    {
        /// <summary>
        /// データベースのテーブルとテーブルレコードを取得する
        /// </summary>
        [CommandMethod("GetTableAndTableRecord")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            // 現在のドキュメントをロック
            using (DocumentLock docLock = doc.LockDocument())
            {
                // ここに処理を書く
            }

            ed.WriteMessage("\n--- データベースのテーブルとテーブルレコードを取得する ---");

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    ////// Table //////
                    /// --- BlockTable --- ///
                    // ブロックテーブルを読込モードで開く
                    var blockTable = db.BlockTableId.GetObject(OpenMode.ForRead, false) as BlockTable;
                    var blockTableTr = tr.GetObject(db.BlockTableId, OpenMode.ForRead, false) as BlockTable;

                    // 現在のスペースのブロックテーブルレコードを書込モードで開く
                    var currentSpace = db.CurrentSpaceId.GetObject(OpenMode.ForWrite, false) as BlockTableRecord;
                    var currentSpaceTr = tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite, false) as BlockTableRecord;

                    // モデルスペースのブロックテーブルレコードを書込モードで開く
                    var modelSpace = SymbolUtilityServices.GetBlockModelSpaceId(db).GetObject(OpenMode.ForWrite) as BlockTableRecord;
                    var modelSpaceTr = tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForWrite) as BlockTableRecord;
                    // （他の方法）BlockTableからモデルスペースを取得
                    var modelSpace2 = blockTable[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite, false) as BlockTableRecord;
                    var modelSpace2Tr = tr.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false) as BlockTableRecord;

                    // ペーパースペースのブロックテーブルレコードを読込モードで開く
                    var paperSpace = tr.GetObject(SymbolUtilityServices.GetBlockPaperSpaceId(db), OpenMode.ForRead) as BlockTableRecord;
                    // （他の方法）BlockTableからペーパースペースを取得
                    var paperSpace2 = tr.GetObject(blockTable[BlockTableRecord.PaperSpace], OpenMode.ForWrite, false) as BlockTableRecord;

                    /// --- DrawOrderTable --- ///
                    // モデルスペースの DrawOrder テーブルを書込モードで開く
                    var drawOrderTable = tr.GetObject(modelSpace.DrawOrderTableId, OpenMode.ForWrite) as DrawOrderTable;

                    /// --- DimStyleTable --- ///
                    // 寸法スタイルテーブルを読込モードで開く
                    var dimStyleTable = tr.GetObject(db.DimStyleTableId, OpenMode.ForRead) as DimStyleTable;
                    // 寸法スタイルテーブルレコードを読込モードで開く
                    var dimStyle = tr.GetObject(db.Dimstyle, OpenMode.ForRead) as DimStyleTableRecord;

                    /// --- LinetypeTable --- ///
                    // ラインタイプテーブルを読込モードで開く
                    var linetypeTable = tr.GetObject(db.LinetypeTableId, OpenMode.ForRead) as LinetypeTable;
                    // 現在の線種のラインタイプレコードを書込モードで開く
                    var linetypeTableRecord = tr.GetObject(db.Celtype, OpenMode.ForWrite) as LinetypeTableRecord;
                    // 線種 Continuous のオブジェクトID を取得
                    ObjectId ltContinuousId = SymbolUtilityServices.GetLinetypeContinuousId(db);

                    /// --- LayerTable --- ///
                    // レイヤーテーブルを読込モードで開く
                    var layerTable = tr.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                    SymbolTableEnumerator ste = layerTable.GetEnumerator(); // Enumerator( 列挙子 ) を生成

                    // 指定レイヤのレイヤーテーブルレコードを書込モードで開く
                    string layerName = "LayerName";
                    var layerTableRecord = tr.GetObject(layerTable[layerName], OpenMode.ForWrite) as LayerTableRecord;

                    // LayerStateManager を取得
                    LayerStateManager layerStateManager = db.LayerStateManager;

                    /// --- RegAppTable --- ///
                    // RegApp テーブルを読込モードで開く
                    var regAppTable = tr.GetObject(db.RegAppTableId, OpenMode.ForRead) as RegAppTable;

                    /// --- TextStyleTable --- ///
                    // テキストスタイルテーブルを読込モードで開く
                    var textStyleTable = tr.GetObject(db.TextStyleTableId, OpenMode.ForRead) as TextStyleTable;
                    // 現在のテキストスタイルテーブルレコードを書込モードで開く
                    var textStyleTableRecord = tr.GetObject(db.Textstyle, OpenMode.ForWrite) as TextStyleTableRecord;
                    // 現在のフォント設定を取得する
                    FontDescriptor font = textStyleTableRecord.Font;

                    /// --- UcsTable --- ///
                    // UCS テーブルを読込モードで開く
                    var ucsTable = tr.GetObject(db.UcsTableId, OpenMode.ForRead) as UcsTable;

                    /// --- ViewTable --- ///
                    // View テーブルを読込モードで開く
                    var viewTable = tr.GetObject(db.ViewTableId, OpenMode.ForRead) as ViewTable;

                    /// --- ViewportTable --- ///
                    // Viewport テーブルを読込モードで開く
                    var viewportTable = tr.GetObject(db.ViewportTableId, OpenMode.ForRead) as ViewportTable;
                    // アクティブなViewport テーブルレコードを書き込みモードで開く
                    var viewportTableRecord = tr.GetObject(ed.ActiveViewportId, OpenMode.ForWrite) as ViewportTableRecord;
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
