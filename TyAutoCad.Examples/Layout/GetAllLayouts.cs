using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetAllLayouts))]
namespace TyAutoCad.Examples
{
    public class GetAllLayouts
    {
        /// <summary>
        /// 図面内のすべてのレイアウトを取得
        ///     図面内のすべてのレイアウトを取得して、レイアウト名を出力
        ///     レイアウトの情報は、LayoutDictionary に格納されている
        ///     モデル空間もLayoutDictionaryに格納されているので、除外する必要がある。
        /// </summary>
        [CommandMethod("GetAllLayouts")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            // 取得したレイアウトを格納するコレクションを準備
            var layouts = new List<Layout>();

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                // レイアウトディクショナリを取得
                var layoutDict = db.LayoutDictionaryId.GetObject(OpenMode.ForRead) as DBDictionary;

                foreach (var item in layoutDict)
                {
                    // モデルは除く
                    if (item.Key.ToUpper() == "MODEL") continue;

                    // レイアウト名を取得して追加
                    var layout = layoutDict.GetAt(item.Key).GetObject(OpenMode.ForRead) as Layout;
                    layouts.Add(layout);
                }
                // データベースへの変更を中止
                tr.Abort();
            }

            // レイアウト名を出力
            foreach (var item in layouts)
            {
                ed.WriteMessage("\nレイアウト名 : " + item.LayoutName);
            }
        }

        /// <summary>
        /// 図面内のすべてのレイアウトを取得
        /// </summary>
        [CommandMethod("GetAllLayouts2")]
        public void Command1()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            // 取得したレイアウトを格納するコレクションを準備
            var layouts = new List<BlockTableRecord>();

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // ブロックテーブルを読み込みモードで開く  
                    var bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                    foreach (ObjectId id in bt)
                    {
                        var btr = tr.GetObject(id, OpenMode.ForRead, false) as BlockTableRecord;
                        if (btr.IsLayout)
                        {
                            layouts.Add(btr);
                            ed.WriteMessage("\nレイアウト名 : {0}", btr.Name);
                        }
                    }
                    ed.WriteMessage("\nレイアウトの数 : {0}", layouts.Count);
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage(ex.StackTrace);
                }
            }
        }
    }
}
