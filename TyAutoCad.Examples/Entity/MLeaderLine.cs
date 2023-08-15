using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// MLeader の引出線を扱うためのクラス
    /// </summary>
    public class MLeaderLine
    {
        #region Enums
        public enum ModifyMode
        {
            /// <summary>
            /// 矢印の先の位置を移動
            /// </summary>
            ArrowHeadMove,

            /// <summary>
            /// 矢印の根本の位置を移動
            /// </summary>
            ArrowTailMove,

            /// <summary>
            /// 長さ変更
            /// </summary>
            ChangeLength
        }

        public enum StretchMode
        {
            /// <summary>
            /// トリム
            /// </summary>
            Trim,

            /// <summary>
            /// 延長
            /// </summary>
            Extend
        }
        #endregion

        #region Constructors
        public MLeaderLine(int index, Point3d firstVertex, Point3d lastVertex)
        {
            Index = index;
            FirstVertex = firstVertex;
            LastVertex = lastVertex;
        }
        #endregion

        #region Properties 
        /// <summary>
        /// 引出線のインデックス
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 始点（頂点）の座標
        /// </summary>
        public Point3d FirstVertex { get; set; }

        /// <summary>
        /// 終点（頂点）の座標
        /// </summary>
        public Point3d LastVertex { get; set; }

        /// <summary>
        /// 始点-終点を結ぶ線分
        /// </summary>
        public Line Line => new Line(FirstVertex, LastVertex);

        #endregion

        #region Methods 
        /// <summary>
        /// 指定点から引出線までの距離を返す
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public double GetDistanceTo(Point3d point)
        {
            var line = new Line3d(FirstVertex, LastVertex);
            return line.GetDistanceTo(point);
        }

        #endregion

        #region StaticMethods
        /// <summary>
        /// 引出線を修正
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="msg"></param>
        /// <param name="angleCorrection"></param>
        /// <returns></returns>
        public static bool Modify(MLeaderLine.ModifyMode mode, bool angleCorrection = false)
        {
            // 現在のDocument,Editor,Databaseを取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null)
            {
                return false;
            }
            var ed = doc.Editor;
            var db = doc.Database;

            // マルチ引出線を選択(GetEntity Method)
            var peo = new PromptEntityOptions("\nマルチ引出線を選択")
            {
                AllowNone = true, // 空 Enter 
            };
            PromptEntityResult per;
            // マルチ引出線を選択するまで続ける
            do
            {
                per = ed.GetEntity(peo);
                if (per.Status != PromptStatus.OK)
                {
                    return false;
                }
            } while (per.ObjectId.ObjectClass.DxfName != "MULTILEADER");

            // トランザクション開始
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // 選択したマルチ引出線を書き込みモードで取得
                    var mleader = tr.GetObject(per.ObjectId, OpenMode.ForWrite, false) as MLeader;

                    // 選択点から一番近い引出線を探す
                    MLeaderLine target;

                    // 引出線なし
                    if (mleader.LeaderLineCount < 1)
                    {
                        return false;
                    }
                    // 引出線が 1本
                    if (mleader.LeaderLineCount == 1)
                    {
                        target = mleader.GetLeaderLines().FirstOrDefault();
                    }
                    // 引出線が 2本以上
                    else
                    {
                        target = mleader.GetLeaderLines().OrderBy(ll => ll.GetDistanceTo(per.PickedPoint)).FirstOrDefault();
                    }

                    // Jig操作で引出線を編集
                    if (MLeaderLineJigger.Jig(mleader, target.Index, mode, angleCorrection))
                    {
                        tr.Commit();
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error MLeaderLine.Modify" + "\n" + ex.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// 引出線をトリム
        /// </summary>
        public static bool Trim() => Stretch(StretchMode.Trim);
        //{
        //    return Stretch(StretchMode.Trim);
        //}

        /// <summary>
        /// 引出線を延長
        /// </summary>
        public static bool Extend() => Stretch(StretchMode.Extend);
        //{
        //    return StretchLeaderLine(StretchMode.Extend, msg);
        //}

        private static bool Stretch(StretchMode mode)
        {
            // 現在のDocument,Editor,Databaseを取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null)
            {
                return false;
            }
            var ed = doc.Editor;
            var db = doc.Database;

            // マルチ引出線を選択(GetEntity Method)
            var peo = new PromptEntityOptions("\nマルチ引出線を選択")
            {
                AllowNone = true, // 空 Enter 
            };
            PromptEntityResult per;
            // マルチ引出線を選択するまで続ける
            do
            {
                per = ed.GetEntity(peo);
                if (per.Status != PromptStatus.OK)
                {
                    return false;
                }
            } while (per.ObjectId.ObjectClass.DxfName != "MULTILEADER");

            // トランザクション開始
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // 選択したマルチ引出線を書き込みモードで取得
                    var mleader = tr.GetObject(per.ObjectId, OpenMode.ForWrite, false) as MLeader;

                    // 選択点から一番近い引出線を探す
                    MLeaderLine target;

                    // 引出線なし
                    if (mleader.LeaderLineCount < 1)
                    {
                        return false;
                    }
                    // 引出線が 1本
                    if (mleader.LeaderLineCount == 1)
                    {
                        target = mleader.GetLeaderLines().FirstOrDefault();
                    }
                    // 引出線が 2本以上
                    else
                    {
                        target = mleader.GetLeaderLines().OrderBy(ll => ll.GetDistanceTo(per.PickedPoint)).FirstOrDefault();
                    }

                    // フェンス選択用の点コレクションを準備
                    Line fenceLine;
                    var fence = new Point3dCollection();
                    fence.Add(target.FirstVertex);
                    if (mode == StretchMode.Trim)
                    {
                        fence.Add(target.LastVertex);
                        fenceLine = target.Line;
                    }
                    else
                    {
                        var v = (target.FirstVertex - target.LastVertex);
                        var tp = target.FirstVertex + (1000 * mleader.Scale) * v.ToUnitVector();
                        fence.Add(tp);
                        fenceLine = new Line(target.FirstVertex, tp);
                    }

                    // フェンス選択を実行
                    PromptSelectionResult psr = ed.SelectFence(fence);
                    if (psr.Status != PromptStatus.OK)
                    {
                        return false;
                    }

                    // フェンス選択された図形を配列に格納
                    ObjectId[] obs = psr.Value.GetObjectIds();

                    Entity ent;
                    // 交点を取得するためのコレクションを準備
                    var pt3ds = new Point3dCollection();
                    List<Point3d> points = new List<Point3d>();

                    foreach (ObjectId id in obs)
                    {
                        // 自分自身は除外
                        if (id == mleader.ObjectId)
                            continue;
                        // ObjectId から Entity を取得
                        ent = tr.GetObject(id, OpenMode.ForWrite, true) as Entity;
                        pt3ds.Clear();
                        fenceLine.IntersectWith(ent, Intersect.OnBothOperands, pt3ds, IntPtr.Zero, IntPtr.Zero);
                        if (pt3ds.Count > 0)
                        {
                            foreach (Point3d p in pt3ds)
                            {
                                // 引出線の先端の点は除外
                                if (p == target.FirstVertex)
                                    continue;
                                points.Add(p);
                            }
                        }
                    }

                    // 交点が 0 の場合は終了
                    if (points.Count < 1)
                    {
                        return false;
                    }

                    // 一番近い交点を取得
                    Point3d point = points.OrderBy(a => a.DistanceTo(target.FirstVertex)).FirstOrDefault();
                    // 引出線の先端の位置を設定
                    mleader.SetFirstVertex(target.Index, point);
                    // 変更をデータベースに保存し、トランザクションを終了
                    tr.Commit();
                    return true;
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error MLeaderLine.Stretch" + "\n" + ex.Message);
                    return false;
                }
            }
        }

        #endregion
    }
}
