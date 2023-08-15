using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetAttribute))]
namespace TyAutoCad.Examples
{
    public class GetAttribute
    {
        /// <summary>
        /// ブロックの属性を取得する
        ///     属性付きブロックを選択して、属性参照のプロパティを出力する
        /// </summary>
        [CommandMethod("GetAttribute")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- ブロックの属性を取得する ---");

            var peo = new PromptEntityOptions("\nブロック図形を選択");
            peo.SetRejectMessage("\nブロック図形のみを選択してください");
            peo.AddAllowedClass(typeof(BlockReference), false);
            PromptEntityResult per = ed.GetEntity(peo);

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // 選択したブロックを取得
                    var blockRef = tr.GetObject(per.ObjectId, OpenMode.ForRead) as BlockReference;

                    // 属性コレクションを取得
                    AttributeCollection attributes = blockRef.AttributeCollection;

                    // 属性が付加されているかどうか調べる
                    if (attributes.Count > 0)
                    {
                        int n = 0;

                        // 属性参照のプロパティを出力
                        foreach (ObjectId attRefId in attributes)
                        {
                            var attRef = tr.GetObject(attRefId, OpenMode.ForRead) as AttributeReference;
                            n++;
                            ed.WriteMessage("\n--- AttributeReference {0} ---", n);
                            ed.WriteMessage("\n\tTag                 : " + attRef.Tag);
                            ed.WriteMessage("\n\tTextString          : " + attRef.TextString);
                            ed.WriteMessage("\n\tFieldLength         : " + attRef.FieldLength);
                            ed.WriteMessage("\n\tInvisible           : " + attRef.Invisible);
                            ed.WriteMessage("\n\tIsConstant          : " + attRef.IsConstant);
                            ed.WriteMessage("\n\tIsMTextAttribute    : " + attRef.IsMTextAttribute);
                            ed.WriteMessage("\n\tIsPreset            : " + attRef.IsPreset);
                            ed.WriteMessage("\n\tIsVerifiable        : " + attRef.IsVerifiable);
                            ed.WriteMessage("\n\tLockPositionInBlock : " + attRef.LockPositionInBlock);
                            
                        }
                    }
                    else
                    {
                        string msg = "このブロックに属性は付加されていません。";
                        Application.ShowAlertDialog(msg);
                        ed.WriteMessage("\n" + msg);
                    }
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error GetAttribute\n\t" + ex.Message);
                }
            }

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
