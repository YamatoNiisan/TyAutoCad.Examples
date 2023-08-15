using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// DatabaseServices クラス情報
    /// </summary>
    public static class AcDbInfo
    {
        /// <summary>
        /// ObjectClassのプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="objectId"></param>
        public static void OutputObjectClassProperties(Editor ed, ObjectId objectId)
        {
            ed.WriteMessage("\n\n--- ObjectClass Properties ---");
            ed.WriteMessage("\n\tAppName         : " + objectId.ObjectClass.AppName);
            ed.WriteMessage("\n\tAutoDelete      : " + objectId.ObjectClass.AutoDelete);
            ed.WriteMessage("\n\tClassVersion    : " + objectId.ObjectClass.ClassVersion);
            ed.WriteMessage("\n\tDxfName         : " + objectId.ObjectClass.DxfName);
            ed.WriteMessage("\n\tIsDisposed      : " + objectId.ObjectClass.IsDisposed);
            ed.WriteMessage("\n\tMyParent        : " + objectId.ObjectClass.MyParent);
            ed.WriteMessage("\n\tName            : " + objectId.ObjectClass.Name);
            ed.WriteMessage("\n\tProxyFlags      : " + objectId.ObjectClass.ProxyFlags);
            ed.WriteMessage("\n\tUnmanagedObject : " + objectId.ObjectClass.UnmanagedObject);
        }

        /// <summary>
        /// ObjectId のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="objectId"></param>
        public static void OutputObjectIdProperties(Editor ed, ObjectId objectId)
        {
            ed.WriteMessage("\n\n--- ObjectId Properties ---");
            ed.WriteMessage("\n\tObjectId            : " + objectId);
            ed.WriteMessage("\n\tDatabase            : " + objectId.Database);
            ed.WriteMessage("\n\tHandle              : " + objectId.Handle);
            ed.WriteMessage("\n\tIsEffectivelyErased : " + objectId.IsEffectivelyErased);
            ed.WriteMessage("\n\tIsErased            : " + objectId.IsErased);
            ed.WriteMessage("\n\tIsNull              : " + objectId.IsNull);
            ed.WriteMessage("\n\tIsResident          : " + objectId.IsResident);
            ed.WriteMessage("\n\tIsValid             : " + objectId.IsValid);
            ed.WriteMessage("\n\tNonForwardedHandle  : " + objectId.NonForwardedHandle);
            ed.WriteMessage("\n\t(static)Null        : " + ObjectId.Null);
            ed.WriteMessage("\n\tObjectClass         : " + objectId.ObjectClass);
            // ed.WriteMessage("\n\tOldId               : " + objectId.OldId); // 旧式
            ed.WriteMessage("\n\tOldIdPtr            : " + objectId.OldIdPtr);
            ed.WriteMessage("\n\tOriginalDatabase    : " + objectId.OriginalDatabase);
            ed.WriteMessage("\n\tIsWellBehaved       : " + objectId.IsWellBehaved);
        }

        /// <summary>
        /// DBObject のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="dbObject"></param>
        public static void OutputDBObjectProperties(Editor ed, DBObject dbObject)
        {
            ed.WriteMessage("\n\n--- DBObject Properties ---");
            ed.WriteMessage("\n\tAcadObject             : " + dbObject.AcadObject);
            ed.WriteMessage("\n\tAnnotative             : " + dbObject.Annotative);
            ed.WriteMessage("\n\tClassID                : " + dbObject.ClassID);
            ed.WriteMessage("\n\tDatabase               : " + dbObject.Database);
            ed.WriteMessage("\n\tDrawable               : " + dbObject.Drawable);
            ed.WriteMessage("\n\tHandle                 : " + dbObject.Handle);
            ed.WriteMessage("\n\tHasFields              : " + dbObject.HasFields);
            ed.WriteMessage("\n\tHasSaveVersionOverride : " + dbObject.HasSaveVersionOverride);
            ed.WriteMessage("\n\tId                     : " + dbObject.Id);
            ed.WriteMessage("\n\tIsAProxy               : " + dbObject.IsAProxy);
            ed.WriteMessage("\n\tIsCancelling           : " + dbObject.IsCancelling);
            ed.WriteMessage("\n\tIsErased               : " + dbObject.IsErased);
            ed.WriteMessage("\n\tIsEraseStatusToggled   : " + dbObject.IsEraseStatusToggled);
            ed.WriteMessage("\n\tIsModified             : " + dbObject.IsModified);
            ed.WriteMessage("\n\tIsModifiedGraphics     : " + dbObject.IsModifiedGraphics);
            ed.WriteMessage("\n\tIsModifiedXData        : " + dbObject.IsModifiedXData);
            ed.WriteMessage("\n\tIsNewObject            : " + dbObject.IsNewObject);
            ed.WriteMessage("\n\tIsNotifyEnabled        : " + dbObject.IsNotifyEnabled);
            ed.WriteMessage("\n\tIsNotifying            : " + dbObject.IsNotifying);
            ed.WriteMessage("\n\tIsObjectIdsInFlux      : " + dbObject.IsObjectIdsInFlux);
            ed.WriteMessage("\n\tIsPersistent           : " + dbObject.IsPersistent);
            ed.WriteMessage("\n\tIsReadEnabled          : " + dbObject.IsReadEnabled);
            ed.WriteMessage("\n\tIsReallyClosing        : " + dbObject.IsReallyClosing);
            ed.WriteMessage("\n\tIsTransactionResident  : " + dbObject.IsTransactionResident);
            ed.WriteMessage("\n\tIsUndoing              : " + dbObject.IsUndoing);
            ed.WriteMessage("\n\tIsWriteEnabled         : " + dbObject.IsWriteEnabled);
            ed.WriteMessage("\n\tMergeStyle             : " + dbObject.MergeStyle);
            ed.WriteMessage("\n\tObjectBirthVersion     : " + dbObject.ObjectBirthVersion);
            ed.WriteMessage("\n\tObjectId               : " + dbObject.ObjectId);
            ed.WriteMessage("\n\tOwnerId                : " + dbObject.OwnerId);
            ed.WriteMessage("\n\tPaperOrientation       : " + dbObject.PaperOrientation);
            ed.WriteMessage("\n\tUndoFiler              : " + dbObject.UndoFiler);
            ed.WriteMessage("\n\tXData                  : " + dbObject.XData);
        }

        /// <summary>
        /// SymbolTableのプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="symbolTable"></param>
        public static void OutputSymbolTableProperties(Editor ed, SymbolTable symbolTable)
        {
            ed.WriteMessage("\n\n--- SymbolTable Properties ---");
            ed.WriteMessage("\n\tIncludingErased     : " + symbolTable.IncludingErased);
        }

        /// <summary>
        /// MLeaderStyle のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="mleaderStyle"></param>
        public static void OutputMLeaderStyleProperties(Editor ed, MLeaderStyle mleaderStyle)
        {
            ed.WriteMessage("\n\n--- MLeaderStyle Properties ---");
            ed.WriteMessage("\n\tArrowSize                    : " + mleaderStyle.ArrowSize);                    // 矢印のサイズ
            ed.WriteMessage("\n\tArrowSymbolId                : " + mleaderStyle.ArrowSymbolId);                // 矢印記号のobjectId
            ed.WriteMessage("\n\tBlockColor                   : " + mleaderStyle.BlockColor);                   // ブロックコンテンツのブロックカラー
            ed.WriteMessage("\n\tBlockConnectionType          : " + mleaderStyle.BlockConnectionType);          // ブロック接続タイプ
            ed.WriteMessage("\n\tBlockId                      : " + mleaderStyle.BlockId);                      // MLeaderによって参照されるブロックテーブルレコードのID
            ed.WriteMessage("\n\tBlockRotation                : " + mleaderStyle.BlockRotation);                // MLeaderによって参照されるブロックの回転
            ed.WriteMessage("\n\tBlockScale                   : " + mleaderStyle.BlockScale);                   // MLeaderによって参照されるブロックのスケール
            ed.WriteMessage("\n\tBreakSize                    : " + mleaderStyle.BreakSize);                    // 引出線を分割するために使用されるギャップのサイズ
            ed.WriteMessage("\n\tContentType                  : " + mleaderStyle.ContentType);                  // コンテンツタイプ
            ed.WriteMessage("\n\tDefaultMText                 : " + mleaderStyle.DefaultMText);                 // デフォルトのMText
            ed.WriteMessage("\n\tDoglegLength                 : " + mleaderStyle.DoglegLength);                 // ドッグレッグリーダーラインの長さ
            ed.WriteMessage("\n\tDrawLeaderOrderType          : " + mleaderStyle.DrawLeaderOrderType);          // リーダーの描画順序のタイプ
            ed.WriteMessage("\n\tDrawMLeaderOrderType         : " + mleaderStyle.DrawMLeaderOrderType);         // MLeaderの描画順序のタイプ
            ed.WriteMessage("\n\tEnableBlockRotation          : " + mleaderStyle.EnableBlockRotation);          // blockRotation値が有効かどうか
            ed.WriteMessage("\n\tEnableBlockScale             : " + mleaderStyle.EnableBlockScale);             // blockScale値が有効かどうか
            ed.WriteMessage("\n\tEnableDogleg                 : " + mleaderStyle.EnableDogleg);                 // ドッグレッグリーダーラインが有効かどうか
            ed.WriteMessage("\n\tEnableFrameText              : " + mleaderStyle.EnableFrameText);              // MTextの周囲にテキストフレームを表示するかどうか
            ed.WriteMessage("\n\tEnableLanding                : " + mleaderStyle.EnableLanding);                // 引出線のランディングが有効かどうか
            ed.WriteMessage("\n\tExtendLeaderToText           : " + mleaderStyle.ExtendLeaderToText);           // 水平引出線が自動的にテキストまで延長されるかどうかを示す値
            ed.WriteMessage("\n\tFirstSegmentAngleConstraint  : " + mleaderStyle.FirstSegmentAngleConstraint);  // 引出線の最初のセグメントの角度拘束
            ed.WriteMessage("\n\tLandingGap                   : " + mleaderStyle.LandingGap);                   // MTextと引出線の尾の間のギャップ
            ed.WriteMessage("\n\tLeaderLineColor              : " + mleaderStyle.LeaderLineColor);              // 引出線の色
            ed.WriteMessage("\n\tLeaderLineType               : " + mleaderStyle.LeaderLineType);               // 引出線のタイプ
            ed.WriteMessage("\n\tLeaderLineTypeId             : " + mleaderStyle.LeaderLineTypeId);             // 引出線の線種
            ed.WriteMessage("\n\tLeaderLineWeight             : " + mleaderStyle.LeaderLineWeight);             // 引出線の線の太さ
            ed.WriteMessage("\n\tMaxLeaderSegmentsPoints      : " + mleaderStyle.MaxLeaderSegmentsPoints);      // リーダーセグメントポイントの最大数
            ed.WriteMessage("\n\tName                         : " + mleaderStyle.Name);                         // MLeaderスタイルの名前
            ed.WriteMessage("\n\tScale                        : " + mleaderStyle.Scale);                        // MLeaderのスケール
            ed.WriteMessage("\n\tSecondSegmentAngleConstraint : " + mleaderStyle.SecondSegmentAngleConstraint); // 引出線の2番目のセグメントの角度拘束
            ed.WriteMessage("\n\tTextAlignAlwaysLeft          : " + mleaderStyle.TextAlignAlwaysLeft);          // テキストが常に左揃えに設定されている場合、true
            ed.WriteMessage("\n\tTextAlignmentType            : " + mleaderStyle.TextAlignmentType);            // テキストの配置タイプ
            ed.WriteMessage("\n\tTextAngleType                : " + mleaderStyle.TextAngleType);                // 最後の引出線セグメントに対するテキストの角度タイプ
            ed.WriteMessage("\n\tTextAttachmentDirection      : " + mleaderStyle.TextAttachmentDirection);      // テキスト添付の値にアクセスしますMTextコンテンツの方向
            ed.WriteMessage("\n\tTextAttachmentType           : " + mleaderStyle.TextAttachmentType);           // テキストのアタッチメントタイプ
            ed.WriteMessage("\n\tTextColor                    : " + mleaderStyle.TextColor);                    // MTextのテキストの色
            ed.WriteMessage("\n\tTextHeight                   : " + mleaderStyle.TextHeight);                   // MTextのテキストの高さ
            ed.WriteMessage("\n\tTextStyleId                  : " + mleaderStyle.TextStyleId);                  // テキストスタイルのobjectId
        }

    }
}
