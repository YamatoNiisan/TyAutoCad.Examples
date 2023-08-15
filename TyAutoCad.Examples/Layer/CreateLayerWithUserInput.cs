using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.CreateLayerWithUserInput))]
namespace TyAutoCad.Examples
{
    public class CreateLayerWithUserInput
    {
        /// <summary>
        /// ユーザーが入力した名前で新しい画層を作成する
        ///     ユーザーが入力した名前が有効かどうか検証する
        ///     ユーザーが入力した名前が使用済でないか調べる
        /// </summary>
        [CommandMethod("CreateLayerWithUserInput")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            using (var tr = db.TransactionManager.StartTransaction())
            {
                // レイヤーテーブルを読込モードで取得
                var layerTable = tr.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;

                // 入力オプションを設定
                var pso = new PromptStringOptions("\n新しい画層名を入力してください")
                {
                    AllowSpaces = true,
                };

                // 画層名を格納する変数を準備
                string layerName = string.Empty;

                // 使用可能な画層名が入力されるまで続ける
                do
                {
                    // 画層名を入力
                    PromptResult pr = ed.GetString(pso);

                    // 入力キャンセルした場合は終了
                    if (pr.Status != PromptStatus.OK) return;

                    try
                    {
                        // 画層名を検証する
                        SymbolUtilityServices.ValidateSymbolName(pr.StringResult, false);

                        // 画層名が使用されているか調べる
                        if (layerTable.Has(pr.StringResult))
                        {
                            Application.ShowAlertDialog("この画層名はすでに使われています。");
                            continue;
                        }

                        // 画層名をセット
                        layerName = pr.StringResult;
                    }
                    catch
                    {
                        // 画層名が無効の場合は、例外がスローされる
                        Application.ShowAlertDialog("画層名が無効です。");
                    }
                } while (layerName == string.Empty);

                // レイヤーテーブルレコードを作成
                var layer = new LayerTableRecord
                {
                    Name = layerName,
                    Color = Color.FromColorIndex(ColorMethod.ByLayer, 1),
                };

                layerTable.UpgradeOpen();

                // レイヤーテーブルとトランザクションに追加
                layerTable.Add(layer);
                tr.AddNewlyCreatedDBObject(layer, true);
                tr.Commit();

                ed.WriteMessage("\n\t新しい画層 \"" + layerName + "\" を作成しました。");
            }
        }
    }
}
