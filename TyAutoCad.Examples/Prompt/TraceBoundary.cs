using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.TraceBoundary))]
namespace TyAutoCad.Examples
{
    /// <summary>
    /// Boundary コマンド
    /// </summary>
    public class TraceBoundary
    {
        [CommandMethod("TraceBoundary")]
        public void Command()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            // 境界の内側の点を選択
            PromptPointResult options = ed.GetPoint("\n内側の点を選択:");
            if (options.Status != PromptStatus.OK) return;

            // 境界を構成するオブジェクトを取得します
            DBObjectCollection objects = ed.TraceBoundary(options.Value, true);
            if (objects.Count > 0)
            {
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    // 現在スペースを書込モードで取得
                    var cs = tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;

                    // 境界オブジェクトを図面に追加
                    foreach (DBObject obj in objects)
                    {
                        Entity ent = obj as Entity;
                        if (ent != null)
                        {
                            // 境界オブジェクトの色を設定
                            ent.ColorIndex = 1;

                            // オブジェクトの線の太さを設定
                            ent.LineWeight = LineWeight.LineWeight050;

                            cs.AppendEntity(ent);
                            tr.AddNewlyCreatedDBObject(ent, true);
                        }
                    }
                    tr.Commit();
                }
            }
        }
    }
}
