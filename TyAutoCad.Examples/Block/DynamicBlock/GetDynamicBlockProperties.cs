using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetDynamicBlockProperties))]
namespace TyAutoCad.Examples
{
    internal class GetDynamicBlockProperties
    {
        /// <summary>
        /// ダイナミックブロックのプロパティを取得する
        ///     選択したダイナミックブロックのプロパティを取得して出力
        /// </summary>
        [CommandMethod("GetDynamicBlockProperties")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- ダイナミックブロックのプロパティを取得する ---");

            var options = new PromptEntityOptions("\nダイナミックブロックを選択");
            options.SetRejectMessage("\nダイナミックブロックを選択してください");
            options.AddAllowedClass(typeof(BlockReference), false);

            string blockName = string.Empty;
            int n = 0;

            do
            {
                PromptEntityResult result = ed.GetEntity(options);

                // トランザクション開始
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    // 選択したブロックを取得
                    var blockRef = tr.GetObject(result.ObjectId, OpenMode.ForRead) as BlockReference;

                    if (blockRef.IsDynamicBlock)
                    {
                        // ダイナミックブロックのプロパティコレクションを取得
                        var properties = blockRef.DynamicBlockReferencePropertyCollection;

                        ed.WriteMessage("\n\n== Dynamic Block Properties ==");
                        // コレクションから各プロパティを取得
                        foreach (DynamicBlockReferenceProperty property in properties)
                        {
                            n++;
                            // プロパティの情報を出力
                            ed.WriteMessage("\n--- Property {0} ---", n);
                            ed.WriteMessage("\n\tプロパティ名       : {0}", property.PropertyName);
                            ed.WriteMessage("\n\tプロパティのタイプ : {0}", property.UnitsType);
                            ed.WriteMessage("\n\tプロパティの説明   : {0}", property.Description);
                            ed.WriteMessage("\n\t設定可能な値       : ");

                            foreach (object value in property.GetAllowedValues())
                            {
                                ed.WriteMessage("\"{0}\" ", value);
                            }

                            ed.WriteMessage("\n\t現在の値           : {0}", property.Value);

                            if (property.ReadOnly)
                            {
                                ed.WriteMessage("\n\t（読取り専用）");
                            }
                            ed.WriteMessage("\n");
                        }

                        // ダイナミックブロックのブロック名を取得
                        var block = tr.GetObject(blockRef.DynamicBlockTableRecord, OpenMode.ForRead) as BlockTableRecord;
                        blockName = block.Name;
                    }
                    else
                    {
                        ed.WriteMessage("\nダイナミックブロックを選択してください。");
                    }
                }
            } while (blockName == string.Empty);

            ed.WriteMessage("\nダイナミックブロック \"{0}\" の {1}個のプロパティ情報を出力しました。", blockName, n);

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
