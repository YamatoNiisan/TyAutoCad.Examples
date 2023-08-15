using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetBlockReferenceProperties))]
namespace TyAutoCad.Examples
{
    public class GetBlockReferenceProperties
    {
        /// <summary>
        /// ブロックリファレンスを選択してプロパティを出力
        /// </summary>
        [CommandMethod("GetBlockReferenceProperties")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = AcAp.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            // BlockReference図形を選択 ( GetEntity Method )
            var options = new PromptEntityOptions("\nブロックを選択");
            options.SetRejectMessage("ブロックのみを選択してください。");
            options.AddAllowedClass(typeof(BlockReference), true);      //SetRejectMessageの後ろに書かないとエラーになる,抽象クラスは指定できない
            PromptEntityResult result = ed.GetEntity(options);
            if (result.Status != PromptStatus.OK) return;

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // 現在スペースを読込モードで取得
                    var cs = tr.GetObject(db.CurrentSpaceId, OpenMode.ForRead) as BlockTableRecord;

                    // 選択したブロックを取得
                    var blockRef = tr.GetObject(result.ObjectId, OpenMode.ForRead) as BlockReference;

                    AcDbInfo.OutputDBObjectProperties(ed, blockRef);
                    AcDbEntityInfo.OutputEntityProperties(ed, blockRef);

                    ed.WriteMessage("\n\n--- BlockReference Properties ---");
                    ed.WriteMessage("\nAnonymousBlockTableRecord               : {0}", blockRef.AnonymousBlockTableRecord);
                    ed.WriteMessage("\nAttributeCollection                     : {0}", blockRef.AttributeCollection);
                    ed.WriteMessage("\nBlockTableRecord                        : {0}", blockRef.BlockTableRecord);
                    ed.WriteMessage("\nBlockTransform                          : {0}", blockRef.BlockTransform);
                    ed.WriteMessage("\nBlockUnit                               : {0}", blockRef.BlockUnit);
                    ed.WriteMessage("\nDynamicBlockReferencePropertyCollection : {0}", blockRef.DynamicBlockReferencePropertyCollection);
                    ed.WriteMessage("\nDynamicBlockTableRecord                 : {0}", blockRef.DynamicBlockTableRecord);
                    ed.WriteMessage("\nIsDynamicBlock                          : {0}", blockRef.IsDynamicBlock);
                    ed.WriteMessage("\nName                                    : {0}", blockRef.Name);
                    ed.WriteMessage("\nNormal                                  : {0}", blockRef.Normal);
                    ed.WriteMessage("\nPosition                                : {0}", blockRef.Position);
                    ed.WriteMessage("\nRotation                                : {0}", blockRef.Rotation);
                    ed.WriteMessage("\nScaleFactors                            : {0}", blockRef.ScaleFactors);
                    ed.WriteMessage("\nTreatAsBlockRefForExplode               : {0}", blockRef.TreatAsBlockRefForExplode);
                    ed.WriteMessage("\nUnitFactor                              : {0}", blockRef.UnitFactor);
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error Command\n\t" + ex.Message);
                }
            }
        }
    }
}
