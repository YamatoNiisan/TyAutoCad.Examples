using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.RenameLayer))]
namespace TyAutoCad.Examples
{
    public class RenameLayer
    {
        /// <summary>
        /// 画層名を変更する
        ///     変更する画層名を入力して、次に変更後の画層名を入力します。
        /// </summary>
        [CommandMethod("RenameLayer")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            // 変更前の画層名を格納する変数を準備
            string layerName = string.Empty;

            // 変更後の画層名を格納する変数を準備
            string newLayerName = string.Empty;

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                // レイヤーテーブルを読込モードで取得
                var layerTable = tr.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;

                // 入力オプションを設定
                var pso = new PromptStringOptions("\n変更する画層名を入力してください")
                {
                    AllowSpaces = true, // スペースの入力を許可
                };

                // 存在する画層名が入力されるまで続ける
                do
                {
                    // 変更する画層名を入力
                    PromptResult pr = ed.GetString(pso);

                    // 入力キャンセルした場合は終了
                    if (pr.Status != PromptStatus.OK) return;

                    try
                    {
                        // 画層名を検証する
                        SymbolUtilityServices.ValidateSymbolName(pr.StringResult, false);

                        // 入力画層が存在するか調べる
                        if (!layerTable.Has(pr.StringResult))
                        {
                            var msg = "画層名 \"" + pr.StringResult + "\" はこの図面に存在しません。";
                            ed.WriteMessage("\n" + msg);
                            Application.ShowAlertDialog(msg);
                            continue;
                        }

                        // 画層名をセット
                        layerName = pr.StringResult;
                    }
                    catch
                    {
                        // 画層名が無効の場合は、例外がスローされる
                        var msg = "\"" + pr.StringResult + "\" は画層名に使用できません。";
                        ed.WriteMessage("\n" + msg);
                        Application.ShowAlertDialog(msg);
                    }
                } while (layerName == string.Empty);

                // 入力オプションを再設定
                pso.Message = "\n変更後の画層名を入力してください";

                // 変更可能な画層名が入力されるまで続ける
                do
                {
                    // 新しい画層名を入力
                    PromptResult pr = ed.GetString(pso);

                    // 入力キャンセルした場合は終了
                    if (pr.Status != PromptStatus.OK) return;

                    try
                    {
                        // 画層名を検証する
                        SymbolUtilityServices.ValidateSymbolName(pr.StringResult, false);

                        // 同じ名前かどうか
                        if (layerName == pr.StringResult)
                        {
                            var msg = "\"" + pr.StringResult + "\" は同じ名前です。";
                            ed.WriteMessage("\n" + msg);
                            Application.ShowAlertDialog(msg);
                            continue;
                        }

                        // 入力画層名が存在するか調べる
                        if (layerTable.Has(pr.StringResult))
                        {
                            var msg = "画層名 \"" + pr.StringResult + "\" はすでに使われています。";
                            ed.WriteMessage("\n" + msg);
                            Application.ShowAlertDialog(msg);
                            continue;
                        }

                        // 新しい画層名をセット
                        newLayerName = pr.StringResult;
                    }
                    catch
                    {
                        // 画層名が無効の場合は、例外がスローされる
                        var msg = "\"" + pr.StringResult + "\" は画層名に使用できません。";
                        ed.WriteMessage("\n" + msg);
                        Application.ShowAlertDialog(msg);
                    }
                } while (newLayerName == string.Empty);

                try
                {
                    // 変更する画僧を取得
                    var layer = tr.GetObject(layerTable[layerName], OpenMode.ForWrite) as LayerTableRecord;

                    // 新しい画層名をセット
                    layer.Name = newLayerName;

                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error RenameLayer\n\t" + ex.Message);
                }
            }
            ed.WriteMessage("\n\t画層 \"" + layerName + "\" の名前を \"" + newLayerName + "\" に変更しました。");
        }
    }
}
