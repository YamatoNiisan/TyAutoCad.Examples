using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// 単純な多角形クラス
    /// </summary>
    public class SimplePolygon2d
    {
        #region Fields
        private double _tolerance = Tolerance.Global.EqualPoint;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        internal SimplePolygon2d() { }

        /// <summary>
        /// 頂点のコレクションから多角形を作成
        /// </summary>
        /// <param name="vertices"></param>
        public SimplePolygon2d(Point2dCollection vertices)
        {
            Vertices = vertices;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 多角形の頂点のコレクション
        /// </summary>
        public Point2dCollection Vertices { get; internal set; }

        /// <summary>
        /// 多角形の各要素（Line）のリスト
        /// </summary>
        private List<Line> _elements;
        public List<Line> Elements
        {
            get
            {
                _elements = new List<Line>();
                Line line;
                for (int i = 0; i < Vertices.Count - 1; i++)
                {
                    line = new Line(Vertices[i].Convert3d(), Vertices[i + 1].Convert3d());
                    _elements.Add(line);
                }
                line = new Line(Vertices[Vertices.Count - 1].Convert3d(), Vertices[0].Convert3d());
                _elements.Add(line);
                return _elements;
            }
        }

        /// <summary>
        /// 多角形のポリラインオブジェクト
        /// </summary>
        private Polyline _polyline;
        public Polyline Polyline
        {
            get
            {
                _polyline = new Polyline()
                {
                    Closed = true
                };

                for (int i = 0; i < Vertices.Count; i++)
                {
                    _polyline.AddVertexAt(i, Vertices[i], 0, 0, 0);
                }
                return this._polyline;
            }
        }

        /// <summary>
        /// 多角形の MPolygon オブジェクト
        /// </summary>
        private MPolygon _mPolygon;
        public MPolygon MPolygon
        {
            get
            {
                _mPolygon = new MPolygon();
                _mPolygon.AppendLoopFromBoundary(Polyline, true, _tolerance);
                return _mPolygon;
            }
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// クリップされたブロックのクリップ境界の多角形を返す
        /// </summary>
        /// <param name="blockRef"></param>
        /// <returns></returns>
        public static SimplePolygon2d GetXclipBoundary(BlockReference blockRef)
        {
            return new SimplePolygon2d(blockRef.GetXclipBoundaryPoints());
        }

        #endregion

        #region Methods
        /// <summary>
        /// 多角形のポリラインのインスタンスを返す
        /// </summary>
        /// <returns></returns>
        public Polyline GetPolyline()
        {
            var polyline = new Polyline
            {
                Closed = true
            };

            for (int i = 0; i < Vertices.Count; i++)
            {
                polyline.AddVertexAt(i, Vertices[i], 0, 0, 0);
            }
            return polyline;
        }


        /// <summary>
        /// 与えられた点が多角形の頂点のいずれかと一致するか調べる
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsVertex(Point2d point)
        {
            foreach (Point2d v in Vertices)
            {
                if (v == point) return true;
            }
            return false;
        }

        /// <summary>
        /// 多角形を変換
        /// </summary>
        /// <param name="matrix"></param>
        public void TransformBy(Matrix2d matrix)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                var tp = Vertices[i].TransformBy(matrix);
                Vertices[i] = tp;
            }
        }

        public void TransformBy(Matrix3d matrix)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                var tp = Vertices[i].Convert3d().TransformBy(matrix);
                Vertices[i] = tp.Convert2d();
            }
        }

        /// <summary>
        /// Entity を完全分解 
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        private DBObjectCollection Explode(Entity ent)
        {
            // final result
            var fullList = new DBObjectCollection();

            // explode the entity
            var explodedObjects = new DBObjectCollection();
            ent.Explode(explodedObjects);
            foreach (Entity explodedObj in explodedObjects)
            {
                // if the exploded entity is a blockref or mtext
                // then explode again
                if (explodedObj.GetType() == typeof(BlockReference) ||
                    explodedObj.GetType() == typeof(MText))
                {
                    foreach (DBObject item in Explode(explodedObj))
                    {
                        fullList.Add(item);
                    }
                }
                else
                    fullList.Add(explodedObj);
            }
            return fullList;
        }

        /// <summary>
        /// 多角形内の図形かどうか調べる（端点が多角形上の図形も含める）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Contains(Entity entity, Matrix3d? matrix3D = null)
        {
            var ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\nContains...");

            // 対象図形がクリップされたブロックの場合 完全分解した図形がすべて多角形内か調べる
            if (entity.IsClippedBlock())
            {
                var block = entity as BlockReference;
                var objects = new DBObjectCollection();
                block.GetExplodedCopyEntities(ref objects, true);

                foreach (Entity item in objects)
                {
                    if (!Contains(item))
                    {
                        return false;
                    }
                }
                return true;
            }

            // 対象図形が寸法の場合
            if (entity is Dimension)
            {
                var objects = Explode(entity);
                foreach (Entity e in objects)
                {
                    if (matrix3D != null)
                    {
                        e.TransformBy((Matrix3d)matrix3D);
                    }
                    // 1要素でも範囲外があれば false
                    if (!Contains(e))
                    {
                        return false;
                    }
                }
                // すべて範囲内
                return true;
            }
            // 対象図形がマルチ引出線または、引出線または、ポリラインの場合
            if (entity is MLeader || entity is Leader || entity is Polyline)
            {
                // 分解して、各要素を調べる
                var objects = Explode(entity);

                foreach (Entity e in objects)
                {
                    // 1要素でも範囲外があれば false
                    if (!Contains(e))
                    {
                        return false;
                    }
                }
                // すべて範囲内
                return true;
            }
            switch (entity)
            {
                case Line line:
                    // 線分の始点と終点を取得
                    var pointList = new List<Point3d>
                    {
                        line.StartPoint,
                        line.EndPoint,
                    };

                    // 交点を取得
                    var lineIntersections = GetIntersections(line);
                    if (lineIntersections.Count > 0)
                    {
                        foreach (Point3d item in lineIntersections)
                        {
                            pointList.Add(item);
                        }
                    }

                    // 点を並べ替える
                    var points = new Point3dCollection();
                    foreach (Point3d item in pointList.OrderBy(p => p.X).ThenBy(p => p.Y))
                    {
                        points.Add(item);
                    }

                    // 中点を調べる
                    for (int i = 0; i < points.Count - 1; i++)
                    {
                        var mp = points[i].GetMidpoint(points[i + 1]);
                        // 多角形外の点が1つでもあれば false
                        if (!IsPointInside(mp))
                        {
                            return false;
                        }
                    }
                    return true;

                case Circle circle:
                    // 中心が多角形の外部の場合は false
                    if (!IsPointInside(circle.Center))
                    {
                        return false;
                    }

                    // 交点が2つ以上の場合
                    if (GetIntersections(circle).Count > 1)
                    {
                        // 多角形の各要素との交点を調べる
                        Point3dCollection intersections;
                        foreach (var item in Elements)
                        {
                            intersections = new Point3dCollection();
                            item.IntersectWith(circle, Intersect.OnBothOperands, intersections, IntPtr.Zero, IntPtr.Zero);
                            // 交点が2つ以上の場合は false
                            if (intersections.Count > 1)
                            {
                                return false;
                            }
                        }
                    }

                    // 中心が多角形の内部かつ交点数が1以下の場合は true
                    return true;
                case Arc arc:
                    // 始点または終点が多角形の外部の場合は false
                    if (!IsPointInside(arc.StartPoint) || !IsPointInside(arc.EndPoint))
                    {
                        return false;
                    }

                    // 始点、終点共に内部の点である。
                    // 交点の存在を確認
                    if (IsIntersect(arc))
                    {
                        // 交点を取得
                        var intersections = GetIntersections(arc);
                        // 交点の数が2以上かつ始点、終点を含む
                        if (intersections.Count > 1 && intersections.Contains(arc.StartPoint) && intersections.Contains(arc.EndPoint))
                        {
                            // 円弧上の点を交点の数+1 個取得してそのいずれかが多角形の外部の点の場合はfalse
                            // パラメータを分割
                            var div = (arc.EndParam - arc.StartParam) / (intersections.Count + 2);

                            // 分割した図形上の点を取得
                            for (double para = arc.StartParam; para < arc.EndParam; para += div)
                            {
                                // その中に多角形の外部の点があれば false
                                if (!IsPointInside(arc.GetPointAtParameter(para)))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    // 始点、終点が多角形の内部の点かつ交点の数が1以下の場合は true
                    return true;
                case Ellipse ellipse:
                    // 楕円の場合
                    if (ellipse.StartParam == 0.0 && ellipse.EndParam == 0.0)
                    {
                        // 中心が多角形の外部の場合は false
                        if (!IsPointInside(ellipse.Center))
                        {
                            return false;
                        }
                        // 交点が2つ以上の場合
                        if (GetIntersections(ellipse).Count > 1)
                        {
                            // 多角形の各要素との交点を調べる
                            Point3dCollection intersections;
                            foreach (var item in Elements)
                            {
                                intersections = new Point3dCollection();
                                item.IntersectWith(ellipse, Intersect.OnBothOperands, intersections, IntPtr.Zero, IntPtr.Zero);
                                // 交点が2つ以上の場合は false
                                if (intersections.Count > 1)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    // 楕円弧の場合
                    else
                    {
                        // 始点または終点が多角形の外部の場合は false
                        if (!IsPointInside(ellipse.StartPoint) || !IsPointInside(ellipse.EndPoint))
                        {
                            return false;
                        }
                        // 始点、終点共に内部の点である。
                        // 交点の存在を確認
                        if (IsIntersect(ellipse))
                        {
                            // 交点を取得
                            var intersections = GetIntersections(ellipse);
                            // 交点の数が2以上かつ始点、終点を含む
                            if (intersections.Count > 1 && intersections.Contains(ellipse.StartPoint) && intersections.Contains(ellipse.EndPoint))
                            {
                                // 円弧上の点を交点の数+1 個取得してそのいずれかが多角形の外部の点の場合はfalse
                                // パラメータを分割
                                var div = (ellipse.EndParam - ellipse.StartParam) / (intersections.Count + 2);

                                // 分割した図形上の点を取得
                                for (double para = ellipse.StartParam; para < ellipse.EndParam; para += div)
                                {
                                    // その中に多角形の外部の点があれば false
                                    if (!IsPointInside(ellipse.GetPointAtParameter(para)))
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }

                    // 楕円   ･･･ 中心が多角形の内部かつ交点数が1以下の場合は true
                    // 楕円弧 ･･･ 始点、終点が多角形の内部の点かつ交点の数が1以下の場合は true
                    return true;

                case Spline spline:
                    return Contains(spline.ToPolyline());
                case Ray ray:
                    return false;
                case Xline xline:
                    return false;
                default:
                    // 上記以外は、図形の GeometricExtents が多角形に含まれているか調べる
                    if (entity.GeometricExtents != null)
                    {
                        var extents = entity.GeometricExtents;
                        var rectangle = new Rectangle2d(extents.MaxPoint.Convert2d(), extents.MinPoint.Convert2d());
                        return Contains(rectangle.Polyline);
                    }
                    return false;
            }
        }

        /// <summary>
        /// 矩形と交差しているカーブ図形を交点で切断して、矩形内の図形を返す
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="curve"></param>
        /// <returns></returns>
        public DBObjectCollection GetInnerCurves(Curve curve)
        {
            var innerCurves = new DBObjectCollection();

            if (Contains(curve))
            {
                innerCurves.Add(curve);
                return innerCurves;
            }

            // 指定図形を多角形との交点で分解して分解した図形を返す
            var splitCurves = GetSplitCurves(curve);

            if (splitCurves.Count > 0)
            {
                // 分解された図形のうち、矩形内の図形を追加
                foreach (Curve splitCurve in splitCurves)
                {
                    // クリップ境界内の図形は追加する
                    if (Contains(splitCurve))
                    {
                        innerCurves.Add(splitCurve);
                    }
                }
            }
            return innerCurves;
        }

        /// <summary>
        /// 矩形内の図形を返す。交差している図形は、交点で切断して、矩形内の図形を返す。
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="db"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        public DBObjectCollection GetInnerEntities(Database db, ObjectId[] objects)
        {
            // 矩形内の図形を格納するコレクションを準備
            var innerObjects = new DBObjectCollection();

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // objects 内の図形を取得して未処理コレクションにコピー
                    var unprocessedObjects = new DBObjectCollection();
                    foreach (var id in objects)
                    {
                        // 選択した図形を取得
                        var entity = tr.GetObject(id, OpenMode.ForRead, true) as Entity;
                        unprocessedObjects.Add(entity);
                    }

                    if (unprocessedObjects.Count > 0)
                    {
                        do
                        {
                            // 未処理コレクションを仮コレクションにコピー
                            var tmpObjects = new DBObjectCollection();
                            foreach (DBObject item in unprocessedObjects)
                            {
                                tmpObjects.Add(item);
                            }

                            foreach (Entity entity in tmpObjects)
                            {
                                unprocessedObjects.Remove(entity);

                                // 多角形外または,多角形と交差していない図形は排除
                                if (!Contains(entity) && !IsIntersect(entity))
                                {
                                    continue;
                                }

                                // 多角形内の図形はそのまま追加
                                if (Contains(entity))
                                {
                                    innerObjects.Add(entity);
                                    continue;
                                }

                                // 多角形と交差している場合
                                if (IsIntersect(entity))
                                {
                                    // ハッチングのソリッドまたは、ソリッドの場合はスルー
                                    if (entity.IsHatchSolid() || entity is Solid)
                                    {
                                        continue;
                                    }
                                }

                                // ブロックの場合は完全分解した図形を取得して、再処理
                                if (entity is BlockReference block)
                                {
                                    var explodedObjects = new DBObjectCollection();
                                    block.GetExplodedCopyEntities(ref explodedObjects, true);

                                    //block.Explode(explodedObjects);
                                    foreach (Entity item in explodedObjects)
                                    {
                                        unprocessedObjects.Add(item);
                                    }
                                    continue;
                                }

                                // ハッチまたは、マルチ引出線、寸法の場合
                                if (entity is Hatch || entity is MLeader || entity is Dimension)
                                {
                                    // 分解した図形を格納するコレクションを準備
                                    var explodedObjects = new DBObjectCollection();

                                    // Explode メソッド実行
                                    entity.Explode(explodedObjects);

                                    // 分解した図形を走査
                                    foreach (Entity exploded in explodedObjects)
                                    {
                                        unprocessedObjects.Add(exploded);
                                    }
                                    continue;
                                }

                                // カーブ図形以外除外する
                                if (entity is Curve curve)
                                {
                                    foreach (Entity item in GetInnerCurves(curve))
                                    {
                                        innerObjects.Add(item);
                                    }
                                    continue;
                                }
                            }
                        } while (unprocessedObjects.Count > 0);
                    }
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error GetInnerEntities\n\t" + ex.Message);
                }
            }
            return innerObjects;
        }


        /// <summary>
        /// 指定図形との交点を取得する
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Point3dCollection GetIntersections(Entity entity, Matrix3d? matrix3D = null)
        {
            var ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\nGetIntersections...");

            // 交点を格納するコレクションを準備
            var intersections = new Point3dCollection();

            // 対象図形がブロックの場合
            if (entity is BlockReference)
            {
                var blockRef = entity as BlockReference;
                intersections = GetIntersections(blockRef);
                return intersections;
            }

            // 対象図形が寸法の場合
            if (entity is Dimension)
            {
                var objects = new DBObjectCollection();
                entity.Explode(objects);

                foreach (Entity e in objects)
                {
                    if (matrix3D != null)
                    {
                        e.TransformBy((Matrix3d)matrix3D);
                    }
                    if (IsIntersect(e))
                    {
                        foreach (Point3d item in GetIntersections(e))
                        {
                            intersections.Add(item);
                        }
                    }
                }
                return intersections;
            }

            // 対象図形がマルチ引出線の場合
            if (entity is MLeader)
            {
                var objects = new DBObjectCollection();
                entity.Explode(objects);

                foreach (Entity e in objects)
                {
                    if (IsIntersect(e))
                    {
                        foreach (Point3d item in GetIntersections(e))
                        {
                            intersections.Add(item);
                        }
                    }
                }
                return intersections;
            }

            // 対象図形がハッチングのソリッドの場合
            if (entity.IsHatchSolid())
            {
                var hatch = entity as Hatch;
                var curves = hatch.GetLoopObjects();

                foreach (Curve curve in curves)
                {
                    if (IsIntersect(curve))
                    {
                        foreach (Point3d item in GetIntersections(curve))
                        {
                            intersections.Add(item);
                        }
                    }
                }
                return intersections;
            }

            // この多角形と図形の交点を取得
            this.Polyline.IntersectWith(entity, Intersect.OnBothOperands, intersections, IntPtr.Zero, IntPtr.Zero);
            return intersections;
        }

        /// <summary>
        /// 指定ブロックとの交点を取得
        /// </summary>
        /// <param name="blockRef"></param>
        /// <returns></returns>
        public Point3dCollection GetIntersections(BlockReference blockRef)
        {
            // 交点を格納するコレクションを準備
            var intersections = new Point3dCollection();

            // 現在のDocument,Editor,Databaseを取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null)
            {
                return intersections;
            }
            var db = doc.Database;

            using (var tr = db.TransactionManager.StartTransaction())
            {
                // BlockReference の BlockTableRecord を取得
                var btr = tr.GetObject(blockRef.BlockTableRecord, OpenMode.ForRead) as BlockTableRecord;

                // モデルスペースへの変換行列を取得
                var mat = blockRef.BlockTransform;

                // ブロック内の図形を走査
                foreach (ObjectId id in btr)
                {
                    var ent = tr.GetObject(id, OpenMode.ForWrite) as Entity;

                    // 属性は除外
                    if (!ent.Visible || ent is AttributeDefinition || ent is AttributeReference)
                    {
                        continue;
                    }

                    // 図形を変換する
                    ent.TransformBy(mat);

                    // 図形との交点を調べる
                    if (IsIntersect(ent))
                    {
                        // ブロックがクリップされている場合
                        // クリップ境界のポリラインを取得して、交点が境界内の点かどうか調べる
                        if (blockRef.IsClipped())
                        {
                            // 交点を格納するコレクションを準備
                            var tmpIntersections = new Point3dCollection();

                            // ハッチングのソリッドの場合 IntersectWith は使えない
                            // 境界線を取得しての境界線との交点を取得する
                            if (ent.IsHatchSolid())
                            {
                                var hatch = ent as Hatch;
                                var loops = hatch.GetLoopObjects();

                                foreach (Curve curve in loops)
                                {
                                    var loopIntersections = new Point3dCollection();

                                    // この矩形と図形の交点を取得
                                    this.Polyline.IntersectWith(curve, Intersect.OnBothOperands, loopIntersections, IntPtr.Zero, IntPtr.Zero);

                                    foreach (Point3d item in loopIntersections)
                                    {
                                        tmpIntersections.Add(item);
                                    }
                                }
                            }
                            else
                            {
                                // この矩形と図形の交点を取得
                                this.Polyline.IntersectWith(ent, Intersect.OnBothOperands, tmpIntersections, IntPtr.Zero, IntPtr.Zero);
                            }

                            // クリップ境界の多角形を取得
                            var boundaryPolygon = GetXclipBoundary(blockRef);

                            // クリップの反転状態を取得する(反転されている場合が Inverted = true)
                            var inverted = blockRef.GetSpatialFilter().Inverted;

                            foreach (Point3d p in tmpIntersections)
                            {
                                // 交点が多角形内の点か調べる
                                if ((!inverted && boundaryPolygon.IsPointInside(p)) ||
                                    (inverted && (!boundaryPolygon.IsPointInside(p) || boundaryPolygon.IsPointOn(p))))
                                {
                                    intersections.Add(p);
                                }
                            }
                        }
                        else
                        {
                            foreach (Point3d item in GetIntersections(ent))
                            {
                                intersections.Add(item);
                            }
                        }
                    }
                }
            }
            return intersections;

        }

        /// <summary>
        /// 指定図形を多角形との交点で分解して分解した図形を返す
        ///     交点が存在しない場合は、空のコレクションを返す
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        public DBObjectCollection GetSplitCurves(Curve curve)
        {
            // 分割点（多角形と図形の交点）を取得
            var intersections = GetIntersections(curve);

            // 交点がなければ終了
            if (intersections.Count < 1) return new DBObjectCollection();

            // 交点の Line に対するパラメータを取得
            var parameters = new double[intersections.Count];

            for (int i = 0; i < intersections.Count; i++)
            {
                if (IsPointOnCurve(curve, intersections[i]))
                {
                    var p = curve.GetClosestPointTo(intersections[i], false);
                    parameters[i] = curve.GetParameterAtPoint(p);
                }
            }
            // 交差する点パラメータを並べ替える
            Array.Sort(parameters);

            // 点パラメータの DoubleCollection を生成
            var splitParameters = new DoubleCollection(parameters);
            return curve.GetSplitCurves(splitParameters);
        }

        /// <summary>
        /// Curve 上の点かどうか調べる
        /// </summary>
        /// <param name="cv"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        private static bool IsPointOnCurve(Curve cv, Point3d pt)
        {
            try
            {
                // 成功した場合は true

                Point3d p = cv.GetClosestPointTo(pt, false);
                return (p - pt).Length <= Tolerance.Global.EqualPoint;
            }
            catch { }
            // それ以外の場合は false
            return false;
        }

        /// <summary>
        /// 指定図形と交差しているか調べる
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool IsIntersect(Entity entity, Matrix3d? matrix3D = null)
            => GetIntersections(entity, matrix3D).Count > 0;

        /// <summary>
        /// 多角形内の点かどうか調べる（多角形上の点も含む）
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsPointInside(Point3d point)
            => MPolygon.IsPointInsideMPolygon(point, _tolerance).Count == 1;

        /// <summary>
        /// 多角形上の点かどうか調べる
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsPointOn(Point3d point)
            => MPolygon.IsPointOnLoopBoundary(point, 0, _tolerance);
        #endregion
    }
}
