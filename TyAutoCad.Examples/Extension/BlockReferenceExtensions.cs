using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.DatabaseServices.Filters;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    public static class BlockReferenceExtensions
    {
        enum BaseBlock
        {
            Nomal,
            Clipped,
        }

        /// <summary>
        /// ブロックの有効な名前を返す
        /// ダイナミックブロックの場合、ダイナミックブロック名を返す
        /// </summary>
        /// <param name="blockRef"></param>
        /// <returns></returns>
        public static string GetEffectiveName(this BlockReference blockRef)
        {
            if (blockRef.IsDynamicBlock)
            {
                var btr = blockRef.DynamicBlockTableRecord.GetObject(OpenMode.ForRead) as BlockTableRecord;
                return btr.Name;
            }
            return blockRef.Name;
        }

        #region GetAttributeTextsAsDBText ##--NOTEST--##
        /// <summary>
        /// ブロック内の全ての属性文字をDBTextに変換して返す
        /// </summary>
        /// <param name="blockRef"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        internal static DBObjectCollection GetAttributeTextsAsDBText(this BlockReference blockRef)
        {
            // 文字列を格納するコレクションを準備
            var texts = new DBObjectCollection();

            var db = blockRef.Database;
            if (db == null) return texts;

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                // BlockReferenceのBlockTableRecordを取得
                var blockDef = tr.GetObject(blockRef.BlockTableRecord, OpenMode.ForRead) as BlockTableRecord;

                // テキストスタイルテーブルを読み込みモードで開く
                var tst = tr.GetObject(db.TextStyleTableId, OpenMode.ForRead) as TextStyleTable;

                // BlockReferenceのBlockTableRecordの図形を走査
                foreach (ObjectId id in blockDef)
                {
                    // 属性定義かどうか調べる
                    if (id.ObjectClass.Name == "AcDbAttributeDefinition")
                    {
                        // 属性定義を取得
                        var attDef = tr.GetObject(id, OpenMode.ForRead) as AttributeDefinition;

                        // 空文字は作成しない
                        if (string.IsNullOrEmpty(attDef.TextString))
                        {
                            continue;
                        }

                        // Constantフラグが true かつ Invisible(可視性)フラグが false
                        if ((attDef.Constant && !attDef.Invisible))
                        {
                            // 属性定義の文字情報から新しい文字を作成
                            var text = new DBText()
                            {
                                TextStyleId = tst[attDef.TextStyleName],
                                Height = attDef.Height,
                                TextString = attDef.TextString,
                                Position = attDef.Position.TransformBy(blockRef.BlockTransform),
                                IsMirroredInX = attDef.IsMirroredInX, // 上下反転
                                IsMirroredInY = attDef.IsMirroredInY, // 左右反転
                                Oblique = attDef.Oblique,   // 傾斜角度
                                Rotation = attDef.Rotation, // 回転角度
                                WidthFactor = attDef.WidthFactor, // 幅係数
                                Layer = attDef.Layer,
                                ColorIndex = attDef.ColorIndex,
                                Linetype = attDef.Linetype,
                            };
                            // 新しい文字をDBObjectCollectionに追加
                            texts.Add(text);
                        }
                    }
                }

                // AttributeCollectionがなければ終了
                if (blockRef.AttributeCollection.Count == 0)
                {
                    return texts;
                }

                // BlockReferenceのAttributeCollectionを走査
                foreach (ObjectId attRefId in blockRef.AttributeCollection)
                {
                    // 属性参照を取得
                    var attRef = tr.GetObject(attRefId, OpenMode.ForRead) as AttributeReference;

                    // 空文字は作成しない
                    if (string.IsNullOrEmpty(attRef.TextString))
                    {
                        continue;
                    }

                    // 属性参照の文字情報から新しい文字を作成
                    var text = new DBText()
                    {
                        TextStyleId = tst[attRef.TextStyleName],
                        Height = attRef.Height,
                        TextString = attRef.TextString,
                        Position = attRef.Position,
                        IsMirroredInX = attRef.IsMirroredInX, // 上下反転
                        IsMirroredInY = attRef.IsMirroredInY, // 左右反転
                        Oblique = attRef.Oblique,   // 傾斜角度
                        Rotation = attRef.Rotation, // 回転角度
                        WidthFactor = attRef.WidthFactor, // 幅係数
                        Layer = attRef.Layer,
                        ColorIndex = attRef.ColorIndex,
                        Linetype = attRef.Linetype,
                    };

                    // 新しい文字をDBObjectCollectionに追加
                    texts.Add(text);
                }
            }
            return texts;
        }
        #endregion

        #region GetExplodedCopyEntities ##--NOTEST--##

        /// <summary>
        /// ブロックを分解した図形のコピーを取得
        ///     クリップされたブロックは常に完全分解される
        /// </summary>
        /// <param name="blockRef"></param>
        /// <param name="allObjects"></param>
        /// <param name="isCompletely">完全分解するかどうか</param>
        /// <param name="matrix">再帰の場合、前回のブロック変換行列</param>
        /// <returns></returns>
        public static DBObjectCollection GetExplodedCopyEntities(this BlockReference blockRef, ref DBObjectCollection allObjects, bool isCompletely, Matrix3d? matrix = null)
        {
            var ed = Application.DocumentManager.MdiActiveDocument.Editor;

            ed.WriteMessage("\nGetExplodedCopyEntities...");

            // 再帰中かどうか(matrix が null でなければ再帰中)
            bool isRecursion = matrix != null;

            // 分解されたオブジェクトを格納するコレクションを準備
            var explodedObjects = new DBObjectCollection();

            // ブロックの所属するデータベースを取得
            var db = blockRef.Database;
            if (db == null) return explodedObjects;

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                // 未処理オブジェクトを格納するコレクションを準備
                var unprocessedObjects = new DBObjectCollection();

                // BlockReference の BlockTableRecord を取得
                var btr = tr.GetObject(blockRef.BlockTableRecord, OpenMode.ForRead) as BlockTableRecord;

                // ブロック内の図形を未処理コレクションに追加
                foreach (ObjectId id in btr)
                {
                    // 図形を取得
                    var ent = tr.GetObject(id, OpenMode.ForWrite) as Entity;

                    // 図形が非表示または属性定義またはワイプアウトは追加しない
                    if (!ent.Visible || ent is AttributeDefinition || ent is Wipeout) continue;

                    // 未処理コレクションに追加
                    unprocessedObjects.Add(ent);
                }

                // 親スペースへの変換行列を取得
                Matrix3d mat = blockRef.BlockTransform;

                // ブロックリファレンスのオーナーを書込モードで取得
                var ownerSpace = tr.GetObject(blockRef.OwnerId, OpenMode.ForWrite) as BlockTableRecord;

                // モデルスペースを書き込みモードで取得
                var modelSpace = tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForWrite) as BlockTableRecord;

                // 基底ブロックを Nomal にセット
                BaseBlock baseBlock = BaseBlock.Nomal;

                // 未処理コレクションに図形がなくなるまで続ける
                do
                {
                    // 未処理コレクションを仮コレクションにコピー
                    var tmpObjects = new DBObjectCollection();
                    foreach (DBObject item in unprocessedObjects)
                    {
                        tmpObjects.Add(item);
                    }

                    // コレクション内の図形を一つずつ調べる
                    foreach (Entity entity in tmpObjects)
                    {
                        ed.WriteMessage("\nGetExplodedCopyEntities_Check01");
                        ed.WriteMessage("\nentity : " + entity);
                        ed.WriteMessage("\nentity.ObjectId : " + entity.ObjectId);

                        //if(entity is Ellipse)
                        //{
                        //    unprocessedObjects.Remove(entity);
                        //    continue;
                        //}

                        // 図形を変換してコピーを作成
                        var copyEntity = entity.GetTransformedCopy(mat);

                        ed.WriteMessage("\nGetExplodedCopyEntities_Check02");

                        // 画層"0"の図形はブロック配置画層に変更
                        if (entity.Layer == "0")
                            copyEntity.Layer = blockRef.Layer;


                        // 色が "ByBlock" の場合は "ByLayer" に変更
                        if (entity.Color.IsByBlock)
                            copyEntity.ColorIndex = 256;

                        // 線種が "ByBlock" の場合は "ByLayer" に変更
                        if (entity.Linetype == "ByBlock")
                            copyEntity.Linetype = "ByLayer";

                        // 未処理コレクションから除外（コピー済みなので）
                        unprocessedObjects.Remove(entity);

                        // クリップされたブロックの場合
                        if (blockRef.IsClipped())
                        {
                            if (!isRecursion)
                            {
                                baseBlock = BaseBlock.Clipped;
                            }

                            // クリップの反転状態を取得(反転されている場合が Inverted = true)
                            var inverted = blockRef.GetSpatialFilter().Inverted;

                            // クリップ境界多角形を作成
                            var clipBoundary = SimplePolygon2d.GetXclipBoundary(blockRef);

                            // クリップ境界と交差しない、クリップ境界内の図形をオーナースペースに追加
                            if (!clipBoundary.IsIntersect(copyEntity) && clipBoundary.Contains(copyEntity) ^ inverted)
                            {
                                // ブロックの場合
                                if (entity is BlockReference block)
                                {
                                    // クリップされたブロックまたは、完全分解の場合
                                    if (block.IsClipped() || isCompletely)
                                    {
                                        if (isRecursion)
                                        {
                                            foreach (Entity item in GetExplodedCopyEntities(block, ref allObjects, isCompletely, (Matrix3d)matrix * mat))
                                            {
                                                unprocessedObjects.Add(item);
                                            }
                                        }
                                        else
                                        {
                                            foreach (Entity item in GetExplodedCopyEntities(block, ref allObjects, isCompletely, mat))
                                            {
                                                unprocessedObjects.Add(item);
                                            }
                                        }
                                        continue;
                                    }
                                }

                                // 分解済コレクションに追加
                                explodedObjects.Add(copyEntity);

                                if (!isRecursion)
                                {
                                    allObjects.Add(copyEntity);
                                }
                            }
                            // クリップ境界と交差している場合
                            else if (clipBoundary.IsIntersect(copyEntity, mat))
                            {
                                // ハッチングのソリッドまたは、ソリッドの場合はスルー
                                if (entity.IsHatchSolid() || entity is Solid) continue;

                                // クリップされたブロックの場合は再帰呼出し
                                // 普通のブロックの場合、ブロック内の図形を取得して未処理コレクションに追加
                                if (entity is BlockReference block)
                                {
                                    if (block.IsClipped())
                                    {
                                        if (isRecursion)
                                        {
                                            foreach (Entity item in GetExplodedCopyEntities(block, ref allObjects, isCompletely, (Matrix3d)matrix * mat))
                                            {
                                                unprocessedObjects.Add(item);
                                            }
                                        }
                                        else
                                        {
                                            foreach (Entity item in GetExplodedCopyEntities(block, ref allObjects, isCompletely, mat))
                                            {
                                                unprocessedObjects.Add(item);
                                            }
                                        }
                                        continue;
                                    }

                                    foreach (Entity item in block.GetTransformedCopyEntities())
                                    {
                                        unprocessedObjects.Add(item);
                                    }
                                    continue;
                                }

                                // ハッチまたは、マルチ引出線の場合
                                // 分解した図形を逆変換して未処理コレクションに追加
                                // 寸法の場合
                                // 分解した図形をそのまま未処理コレクションに追加
                                if (entity is Hatch || entity is MLeader || entity is Dimension)
                                {
                                    var tmp = new DBObjectCollection();

                                    copyEntity.Explode(tmp);

                                    foreach (Entity item in tmp)
                                    {
                                        // ハッチまたは、マルチ引出線の場合は分解した図形を逆変換
                                        if (entity is Hatch || entity is MLeader)
                                        {
                                            item.TransformBy(mat.Inverse());
                                        }
                                        unprocessedObjects.Add(item);
                                    }
                                    continue;
                                }

                                // カーブ図形以外除外する
                                if (!(entity is Curve)) continue;

                                // カーブ図形の処理
                                // カーブ図形にキャスト
                                var copyCurve = copyEntity as Curve;

                                // カーブ図形をクリップ境界多角形の交点で分解した図形コレクションを取得
                                DBObjectCollection splitCurves = clipBoundary.GetSplitCurves(copyCurve);

                                // 分解された図形の存在を確認
                                if (splitCurves.Count > 0)
                                {
                                    // 分解された図形のうち、矩形内の図形を追加
                                    foreach (Curve splitCurve in splitCurves)
                                    {
                                        // クリップ境界内の図形は追加する
                                        if (clipBoundary.Contains(splitCurve) ^ inverted)
                                        {
                                            explodedObjects.Add(splitCurve);

                                            if (!isRecursion)
                                                allObjects.Add(splitCurve);
                                        }
                                    }
                                }
                            }
                        }
                        // 普通のブロックの場合
                        else
                        {
                            // 完全分解, またはクリップされたブロックの場合再帰
                            if (isCompletely || entity.IsClippedBlock())
                            {
                                // ブロックの場合は再帰
                                if (entity is BlockReference block)
                                {
                                    if (isRecursion)
                                    {
                                        foreach (Entity item in GetExplodedCopyEntities(block, ref allObjects, isCompletely, (Matrix3d)matrix * mat))
                                        {
                                            unprocessedObjects.Add(item);
                                        }
                                    }
                                    else
                                    {
                                        foreach (Entity item in GetExplodedCopyEntities(block, ref allObjects, isCompletely, mat))
                                        {
                                            unprocessedObjects.Add(item);
                                        }
                                    }
                                    continue;
                                }
                            }
                            if (isRecursion)
                            {
                                copyEntity.TransformBy((Matrix3d)matrix);
                            }
                            else
                            {
                                explodedObjects.Add(copyEntity);
                            }
                            allObjects.Add(copyEntity);
                        }
                    }
                    // 属性参照を調べる
                    // 属性参照の存在を確認して文字データからDBTextを作成
                    if (blockRef.HasAttributeDefinition() || blockRef.AttributeCollection.Count > 0)
                    {
                        // ブロック内の全ての属性参照文字のDBTextを取得
                        foreach (Entity item in blockRef.GetAttributeTextsAsDBText())
                        {
                            explodedObjects.Add(item);

                            // 再帰処理中でない（大本のブロック）場合のみ、allObjects に追加
                            if (!isRecursion)
                            {
                                allObjects.Add(item);
                            }
                        }
                    }
                } while (unprocessedObjects.Count > 0);

                tr.Commit();
            }
            return explodedObjects;
        }
        #endregion

        #region GetSpatialFilter ##--TESTED--##
        /// <summary>
        /// ブロックリファレンスのSpatialFilterを取得する
        ///     ブロックに SpatialFilter が設定されていない場合は、null を返す
        /// </summary>
        /// <param name="blockRef"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        internal static SpatialFilter GetSpatialFilter(this BlockReference blockRef)
        {
            SpatialFilter spatialFilter = null;
            var db = blockRef.Database;
            if (db == null) return spatialFilter;

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                // ExtensionDictionaryの存在を確認
                if (blockRef.ExtensionDictionary != ObjectId.Null)
                {
                    // BlockReference の拡張辞書(ExtensionDictionary)を取得
                    var extdict = tr.GetObject(blockRef.ExtensionDictionary, OpenMode.ForRead) as DBDictionary;

                    // ACAD_FILTER の存在を確認
                    if (extdict.Contains("ACAD_FILTER"))
                    {
                        // "ACAD_FILTER"のDBDictionaryを取得
                        var filterDict = tr.GetObject(extdict.GetAt("ACAD_FILTER"), OpenMode.ForRead) as DBDictionary;

                        // "SPATIAL" の存在を確認
                        if (filterDict.Contains("SPATIAL"))
                        {
                            // SpatialFilterを取得
                            spatialFilter = tr.GetObject(filterDict.GetAt("SPATIAL"), OpenMode.ForRead) as SpatialFilter;
                            return spatialFilter;
                        }
                    }
                }
            }
            return spatialFilter;
        }
        #endregion

        #region GetTransformedCopyEntities ##--NOTEST--##
        /// <summary>
        /// ブロック内の図形のコピーを変換して取得
        /// </summary>
        /// <param name="blockRef"></param>
        /// <returns></returns>
        internal static DBObjectCollection GetTransformedCopyEntities(this BlockReference blockRef)
        {
            // ブロック内の図形のコピーを格納するコレクションを準備
            var copyObjects = new DBObjectCollection();

            // ブロックが所属するデータベースを取得
            var db = blockRef.Database;
            if (db == null) return copyObjects;

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                // BlockReference の BlockTableRecord を取得
                var btr = tr.GetObject(blockRef.BlockTableRecord, OpenMode.ForRead) as BlockTableRecord;

                // ブロック内の図形を走査
                foreach (ObjectId id in btr)
                {
                    var ent = tr.GetObject(id, OpenMode.ForWrite) as Entity;

                    // 非表示または属性定義またはワイプアウトの場合はスルー
                    if (!ent.Visible || ent is AttributeDefinition || ent is Wipeout) continue;

                    // ブロック内の図形を変換してそのコピーを追加
                    copyObjects.Add(ent.GetTransformedCopy(blockRef.BlockTransform));
                }
            }
            return copyObjects;
        }
        #endregion

        #region GetXclipBoundaryPoints ##--TESTED--##
        /// <summary>
        /// ブロックリファレンスののXclip境界の点を返す
        ///     Xclip されていない場合は、null
        /// </summary>
        /// <param name="blockRef"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static Point2dCollection GetXclipBoundaryPoints(this BlockReference blockRef)
        {
            SpatialFilter filter = blockRef.GetSpatialFilter();
            if (filter == null) return null;

            var boundaryPoints = new Point2dCollection();

            // クリップ範囲の頂点を取得
            var points = filter.Definition.GetPoints();

            // 頂点の変換マトリックスを計算
            Matrix3d matrix
                = filter.ClipSpaceToWorldCoordinateSystemTransform.PreMultiplyBy(
                    filter.OriginalInverseBlockTransform).PreMultiplyBy(blockRef.BlockTransform);

            // 点が2つの場合は矩形
            if (points.Count == 2)
            {
                Point2d p1 = points[0].Convert3d().TransformBy(matrix).Convert2d();
                Point2d p2 = points[1].Convert3d().TransformBy(matrix).Convert2d();
                //var rect = new Rectangle2d(p1, p2, blockRef.Rotation);
                //boundaryPoints = rect.Vertices;

                boundaryPoints = p1.GetRectangleVertices(p2, blockRef.Rotation);
            }
            else
            {
                for (int i = 0; i < points.Count; i++)
                {
                    Point2d p = points[i].Convert3d().TransformBy(matrix).Convert2d();
                    boundaryPoints.Add(p);
                }
            }
            return boundaryPoints;
        }
        #endregion

        #region GetXclipBoundaryPolyline ##--TESTED--##
        /// <summary>
        /// ブロックリファレンスのXclip境界のポリラインを返す
        /// </summary>
        /// <param name="blockRef"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static Polyline GetXclipBoundaryPolyline(this BlockReference blockRef)
        {
            // クリップ境界点のコレクションを取得
            Point2dCollection boundaryPoints = blockRef.GetXclipBoundaryPoints();

            if (boundaryPoints == null || boundaryPoints.Count < 1)
            {
                return null;
            }

            var poly = new Polyline();
            for (int i = 0; i < boundaryPoints.Count; i++)
            {
                poly.AddVertexAt(i, boundaryPoints[i], 0, 0, 0);
            }
            poly.Closed = true;
            return poly;
        }
        #endregion

        #region HasAttributeDefinition ##--NOTEST--##
        /// <summary>
        /// ブロック内に属性定義が含まれているかどうか調べる
        /// </summary>
        /// <param name="blockRef"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        internal static bool HasAttributeDefinition(this BlockReference blockRef)
        {
            var db = blockRef.Database;
            if (db == null) return false;

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                // BlockReferenceのBlockTableRecordを取得
                var blockDef = tr.GetObject(blockRef.BlockTableRecord, OpenMode.ForRead) as BlockTableRecord;

                // BlockReferenceのBlockTableRecordの図形を走査
                foreach (ObjectId id in blockDef)
                {
                    // 属性定義かどうか調べる
                    if (id.ObjectClass.Name == "AcDbAttributeDefinition")
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        #endregion

        #region IsClipped ##--TESTED--##
        /// <summary>
        /// ブロックがクリップされているかどうか調べる
        /// </summary>
        /// <param name="blockRef"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static bool IsClipped(this BlockReference blockRef)
            => blockRef.GetSpatialFilter() != null;

        #endregion
    }
}
