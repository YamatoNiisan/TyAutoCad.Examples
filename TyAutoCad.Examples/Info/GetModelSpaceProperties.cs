using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetModelSpaceProperties))]
namespace TyAutoCad.Examples
{
    public class GetModelSpaceProperties
    {
        #region Command
        /// <summary>
        /// ModelSpace のプロパティを取得して出力
        /// </summary>
        [CommandMethod("GetModelSpaceProperties")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = AcAp.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- ModelSpace のプロパティを取得して出力 ---");

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // モデルスペースを取得
                    var ms = tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForRead) as BlockTableRecord;

                    foreach (ObjectId id in ms)
                    {
                        var btr = tr.GetObject(id, OpenMode.ForWrite, false) as BlockTableRecord;
                        ed.WriteMessage("\n\n--- ModelSpace Properties ---");
                        ed.WriteMessage("\nName : {0}", btr.Name);
                        ed.WriteMessage("\n\tAcadObject              : {0}", btr.AcadObject);
                        ed.WriteMessage("\n\tAnnotative              : {0}", btr.Annotative);
                        ed.WriteMessage("\n\tAutoDelete              : {0}", btr.AutoDelete);
                        ed.WriteMessage("\n\tBlockBeginId            : {0}", btr.BlockBeginId);
                        ed.WriteMessage("\n\tBlockEndId              : {0}", btr.BlockEndId);
                        ed.WriteMessage("\n\tBlockScaling            : {0}", btr.BlockScaling);
                        ed.WriteMessage("\n\tBounds                  : {0}", btr.Bounds);
                        ed.WriteMessage("\n\tClassID                 : {0}", btr.ClassID);
                        ed.WriteMessage("\n\tComments                : {0}", btr.Comments);
                        ed.WriteMessage("\n\tDatabase                : {0}", btr.Database);
                        ed.WriteMessage("\n\tDrawable                : {0}", btr.Drawable);
                        ed.WriteMessage("\n\tDrawableType            : {0}", btr.DrawableType);
                        ed.WriteMessage("\n\tDrawOrderTableId        : {0}", btr.DrawOrderTableId);
                        ed.WriteMessage("\n\tDrawStream              : {0}", btr.DrawStream);
                        ed.WriteMessage("\n\tExplodable              : {0}", btr.Explodable);
                        ed.WriteMessage("\n\tExtensionDictionary     : {0}", btr.ExtensionDictionary);
                        ed.WriteMessage("\n\tHandle                  : {0}", btr.Handle);
                        ed.WriteMessage("\n\tHasAttributeDefinitions : {0}", btr.HasAttributeDefinitions);
                        ed.WriteMessage("\n\tHasFields               : {0}", btr.HasFields);
                        ed.WriteMessage("\n\tHasPreviewIcon          : {0}", btr.HasPreviewIcon);
                        ed.WriteMessage("\n\tHasSaveVersionOverride  : {0}", btr.HasSaveVersionOverride);
                        ed.WriteMessage("\n\tHyperlinks              : {0}", btr.Hyperlinks);
                        ed.WriteMessage("\n\tId                      : {0}", btr.Id);
                        ed.WriteMessage("\n\tIncludingErased         : {0}", btr.IncludingErased);
                        ed.WriteMessage("\n\tIsAnonymous             : {0}", btr.IsAnonymous);
                        ed.WriteMessage("\n\tIsAProxy                : {0}", btr.IsAProxy);
                        ed.WriteMessage("\n\tIsCancelling            : {0}", btr.IsCancelling);
                        ed.WriteMessage("\n\tIsDependent             : {0}", btr.IsDependent);
                        ed.WriteMessage("\n\tIsDisposed              : {0}", btr.IsDisposed);
                        ed.WriteMessage("\n\tIsDynamicBlock          : {0}", btr.IsDynamicBlock);
                        ed.WriteMessage("\n\tIsErased                : {0}", btr.IsErased);
                        ed.WriteMessage("\n\tIsEraseStatusToggled    : {0}", btr.IsEraseStatusToggled);
                        ed.WriteMessage("\n\tIsFromExternalReference : {0}", btr.IsFromExternalReference);
                        ed.WriteMessage("\n\tIsFromOverlayReference  : {0}", btr.IsFromOverlayReference);
                        ed.WriteMessage("\n\tIsLayout                : {0}", btr.IsLayout);
                        ed.WriteMessage("\n\tIsModified              : {0}", btr.IsModified);
                        ed.WriteMessage("\n\tIsModifiedGraphics      : {0}", btr.IsModifiedGraphics);
                        ed.WriteMessage("\n\tIsModifiedXData         : {0}", btr.IsModifiedXData);
                        ed.WriteMessage("\n\tIsNewObject             : {0}", btr.IsNewObject);
                        ed.WriteMessage("\n\tIsNotifyEnabled         : {0}", btr.IsNotifyEnabled);
                        ed.WriteMessage("\n\tIsNotifying             : {0}", btr.IsNotifying);
                        ed.WriteMessage("\n\tIsObjectIdsInFlux       : {0}", btr.IsObjectIdsInFlux);
                        ed.WriteMessage("\n\tIsPersistent            : {0}", btr.IsPersistent);
                        ed.WriteMessage("\n\tIsReadEnabled           : {0}", btr.IsReadEnabled);
                        ed.WriteMessage("\n\tIsReallyClosing         : {0}", btr.IsReallyClosing);
                        ed.WriteMessage("\n\tIsResolved              : {0}", btr.IsResolved);
                        ed.WriteMessage("\n\tIsTransactionResident   : {0}", btr.IsTransactionResident);
                        ed.WriteMessage("\n\tIsUndoing               : {0}", btr.IsUndoing);
                        ed.WriteMessage("\n\tIsUnloaded              : {0}", btr.IsUnloaded);
                        ed.WriteMessage("\n\tIsWriteEnabled          : {0}", btr.IsWriteEnabled);
                        ed.WriteMessage("\n\tLayoutId                : {0}", btr.LayoutId);
                        ed.WriteMessage("\n\tMergeStyle              : {0}", btr.MergeStyle);
                        ed.WriteMessage("\n\tName                    : {0}", btr.Name);
                        ed.WriteMessage("\n\tObjectBirthVersion      : {0}", btr.ObjectBirthVersion);
                        ed.WriteMessage("\n\tObjectId                : {0}", btr.ObjectId);
                        ed.WriteMessage("\n\tOrigin                  : {0}", btr.Origin);
                        ed.WriteMessage("\n\tOwnerId                 : {0}", btr.OwnerId);
                        ed.WriteMessage("\n\tPaperOrientation        : {0}", btr.PaperOrientation);
                        ed.WriteMessage("\n\tPathName                : {0}", btr.PathName);
                        ed.WriteMessage("\n\tPreviewIcon             : {0}", btr.PreviewIcon);
                        ed.WriteMessage("\n\tUndoFiler               : {0}", btr.UndoFiler);
                        ed.WriteMessage("\n\tUnits                   : {0}", btr.Units);
                        ed.WriteMessage("\n\tUnmanagedObject         : {0}", btr.UnmanagedObject);
                        ed.WriteMessage("\n\tXData                   : {0}", btr.XData);
                        ed.WriteMessage("\n\tXrefStatus              : {0}", btr.XrefStatus);
                    }

                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    AcAp.ShowAlertDialog("Error Command\n\t" + ex.Message);
                }
            }

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
        #endregion
    }
}
