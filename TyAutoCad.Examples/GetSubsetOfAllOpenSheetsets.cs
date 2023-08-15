using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;
using ACSMCOMPONENTS24Lib;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetSubsetOfAllOpenSheetsets))]
namespace TyAutoCad.Examples
{
    public class GetSubsetOfAllOpenSheetsets
    {
        /// <summary>
        /// 開いている全てのシートセットのサブセットを取得
        /// </summary>
        [CommandMethod("GetSubsetOfAllOpenSheetsets")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- 開いている全てのシートセットのサブセットを取得 ---");

            // シートセットマネージャーを取得
            var sheetSetManager = new AcSmSheetSetMgr();

            // シートセットマネージャーデータベース列挙子を取得
            IAcSmEnumDatabase databaseEnum = sheetSetManager.GetDatabaseEnumerator();

            // シートセットオブジェクトを生成
            AcSmSheetSet sheetSet = new AcSmSheetSet();

            // 最初のデータベースを取得
            AcSmDatabase database = databaseEnum.Next();

            if (database == null)
            {
                AcAp.ShowAlertDialog("シートセットは開かれていません。");
                ed.WriteMessage("\nシートセットは開かれていません。");
            }

            try
            {
                while (database != null)
                {
                    if (database.GetLockStatus() == AcSmLockStatus.AcSmLockStatus_UnLocked)
                    {
                        // データベースをロック
                        database.LockDb(database);

                        // データベースのシート セットを取得
                        sheetSet = database.GetSheetSet();
                        ed.WriteMessage("\nシートセット名:{0}", sheetSet.GetName());

                        // シートセット列挙子を取得
                        var sheetEnum = sheetSet.GetSheetEnumerator();

                        // 最初のサブセットを取得
                        var subSet = sheetEnum.Next();

                        while (subSet != null)
                        {
                            ed.WriteMessage("\n\t\tサブセット名:{0}", subSet.GetName());

                            // 次のブセットを取得
                            subSet = sheetEnum.Next();
                        }

                        // データベースをロック解除
                        database.UnlockDb(database, true);
                    }
                    // 次のデータベースを取得
                    database = databaseEnum.Next();
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                AcAp.ShowAlertDialog("Error Command\n\t" + ex.Message);
            }
            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
