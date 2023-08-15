using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.AAA))]
namespace TyAutoCad.Examples
{
    public class SetDynamicBlockProperty
    {
        /// <summary>
        /// 書込み可能な"距離"プロパティを持つ、ダイナミックブロックの値を設定する
        /// </summary>
        [CommandMethod("SetDynamicBlockProperty")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- コマンド開始 ---");

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // ブロック図形を選択
                    var options = new PromptEntityOptions("\nダイナミックブロックを選択: ");
                    options.SetRejectMessage("\nブロック図形ではありません。");
                    options.AddAllowedClass(typeof(BlockReference), false);

                    PromptEntityResult result = ed.GetEntity(options);

                    if (result.Status != PromptStatus.OK) return;

                    // 選択したブロック図形を取得
                    var blockRef = tr.GetObject(result.ObjectId, OpenMode.ForRead) as BlockReference;

                    // ダイナミックブロックかどうか調べる
                    if (!blockRef.IsDynamicBlock)
                    {
                        ed.WriteMessage("\n選択したブロック図形はダイナミックブロックではありません。");
                        return;
                    }

                    // ダイナミックブロックのプロパティコレクションを取得
                    DynamicBlockReferencePropertyCollection properties = blockRef.DynamicBlockReferencePropertyCollection;

                    // 距離プロパティの確認及び、現在の値を取得する
                    double dist = 0;
                    bool exist = false;
                    foreach (DynamicBlockReferenceProperty property in properties)
                    {
                        if (!property.ReadOnly && property.PropertyName == "距離")
                        {
                            dist = (double)property.Value;
                            exist = true;
                        }
                    }

                    if (!exist)
                    {
                        ed.WriteMessage("\n書込み可能な、\"距離\" プロパティは存在しません。");
                        return;
                    }


                    var doubleOptions = new PromptDoubleOptions("\n\"距離\" プロパティに設定する値を入力してください:")
                    {
                        AllowZero = false,     // ゼロ入力     (true:可, false:不可)
                        AllowNegative = false, // 負の値の入力 (true:可, false:不可)
                        AllowNone = false,     // 空Enter     (true:可, false:不可)
                        DefaultValue = dist,
                    };
                    PromptDoubleResult doubleResult = ed.GetDouble(doubleOptions);
                    if (doubleResult.Status != PromptStatus.OK) return;

                    // 距離プロパティに値を設定する
                    foreach (DynamicBlockReferenceProperty property in properties)
                    {
                        if (!property.ReadOnly && property.PropertyName == "距離")
                        {
                            blockRef.UpgradeOpen();
                            property.Value = doubleResult.Value;
                        }
                    }
                    tr.Commit();

                    ed.WriteMessage("\n\"距離\" プロパティの値を \"{0}\" から \"{1}\" に変更しました。", dist, doubleResult.Value);
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error SetDynamicBlockProperty\n\t" + ex.Message);
                }
            }

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
