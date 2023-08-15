using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.DefineBlock))]
namespace TyAutoCad.Examples
{
    public class DefineBlock
    {
        /// <summary>
        /// ブロックを定義する
        ///     新しいブロックを定義し、その定義に円を追加する。
        /// </summary>
        [CommandMethod("DefineBlock")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- ブロックを定義する ---");

            // 作成するブロック名をセット
            string blockName = "testBlock";

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // ブロックテーブルを読込モードで開く
                    var bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead, false)
                        as BlockTable;

                    // ブロックの存在確認
                    if (bt.Has(blockName))
                    {
                        Application.ShowAlertDialog(
                            "このブロック名はすでに使用されています。");
                        return;
                    }

                    // 新規にブロックテーブルレコードを作成
                    using (var btr = new BlockTableRecord())
                    {
                        // ブロック名を設定
                        btr.Name = blockName;

                        // ブロックの挿入ポイントを設定する
                        btr.Origin = new Point3d(0, 0, 0);

                        // 新規に円を作成する
                        using (var cir = new Circle())
                        {
                            cir.Center = new Point3d(0, 0, 0);
                            cir.Radius = 2;

                            // ブロックテーブルレコードに円を追加
                            btr.AppendEntity(cir);

                            bt.UpgradeOpen();

                            // ブロックテーブルとトランザクションに
                            // ブロックテーブルレコードを追加
                            bt.Add(btr);
                            tr.AddNewlyCreatedDBObject(btr, true);
                        }
                    }
                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error DefineBlock\n\t" + ex.Message);
                }
            }

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
