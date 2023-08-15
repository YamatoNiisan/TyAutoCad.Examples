using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACSMCOMPONENTS24Lib;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetAllOpenSheetSets))]
namespace TyAutoCad.Examples
{
    /// <summary>
    /// シートセットを扱うためには、NuGetで"Digi-Ants.AutoCAD.SheetSetManager"をインストールする
    /// ACSMCOMPONENTS24Lib を使う
    /// </summary>
    public class GetAllOpenSheetSets
    {
        #region Command
        /// <summary>
        /// 開いている全てのシートセットを取得
        /// </summary>
        [CommandMethod("GetAllOpenSheetSets")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = AcAp.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- 開いている全てのシートセットを取得 ---");

            // シートセットマネージャーを取得
            var sheetSetManager = new AcSmSheetSetMgr();

            // シートセットマネージャーデータベース列挙子を取得
            IAcSmEnumDatabase databaseEnum = sheetSetManager.GetDatabaseEnumerator();

            // シートセットオブジェクトを生成
            AcSmSheetSet sheetSet = new AcSmSheetSet();

            // 最初のデータベースを取得
            AcSmDatabase database = databaseEnum.Next();

            if(database == null)
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
        #endregion
    }
}
