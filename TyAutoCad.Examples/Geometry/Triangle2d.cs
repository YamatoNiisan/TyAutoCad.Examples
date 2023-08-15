using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// 三角形2D（頂点 A, B, C）
    /// </summary>
    public struct Triangle2d
    {
        #region Constructors
        public Triangle2d(Point2d a, Point2d b, Point2d c)
        {
            A = a;
            B = b;
            C = c;
        }

        public Triangle2d(Point2d[] points)
        {
            if (points.Length != 3)
                throw new ArgumentOutOfRangeException("Needs 3 points.");
            A = points[0];
            B = points[1];
            C = points[2];
        }

        public Triangle2d(Point2dCollection points)
        {
            if (points.Count != 3)
                throw new ArgumentOutOfRangeException("Needs 3 points.");
            A = points[0];
            B = points[1];
            C = points[2];
        }
        #endregion

        #region Indexer
        /// <summary>
        /// 指定されたインデックスの頂点を取得
        /// </summary>
        /// <param name="i">頂点のインデックス</param>
        /// <returns>指定されたインデックスの頂点</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// index が 0 未満または 2 より大きい場合、IndexOutOfRangeException がスローされます。</exception>
        public Point2d this[int i] => Vertexes[i];
        #endregion

        #region Properties
        /// <summary>
        /// 頂点A
        /// </summary>
        public Point2d A { get; }

        /// <summary>
        /// 頂点B
        /// </summary>
        public Point2d B { get; }

        /// <summary>
        /// 頂点C
        /// </summary>
        public Point2d C { get; }

        /// <summary>
        /// 3つの頂点の配列
        /// </summary>
        public Point2d[] Points => new[] { A, B, C };

        /// <summary>
        /// 3つの頂点のコレクション
        /// </summary>
        public Point2dCollection Vertexes => new Point2dCollection { A, B, C };

        /// <summary>
        /// 重心
        /// </summary>
        public Point2d Centroid => (A + B.GetAsVector() + C.GetAsVector()) / 3.0;

        /// <summary>
        /// 外接円
        /// </summary>
        public CircularArc2d CircumscribedCircle
        {
            get
            {
                Line2d l1 = GetSegmentAt(0).GetBisector();
                Line2d l2 = GetSegmentAt(1).GetBisector();
                Point2d[] inters = l1.IntersectWith(l2);
                if (inters == null)
                    return null;
                return new CircularArc2d(inters[0], inters[0].GetDistanceTo(A));
            }
        }

        /// <summary>
        /// 内接円
        /// </summary>
        public CircularArc2d InscribedCircle
        {
            get
            {
                Vector2d v1 = A.GetVectorTo(B).GetNormal();
                Vector2d v2 = A.GetVectorTo(C).GetNormal();
                Vector2d v3 = B.GetVectorTo(C).GetNormal();
                if (v1.IsEqualTo(v2) || v2.IsEqualTo(v3))
                    return null;
                Line2d l1 = new Line2d(A, v1 + v2);
                Line2d l2 = new Line2d(B, v1.Negate() + v3);
                Point2d[] inters = l1.IntersectWith(l2);
                return new CircularArc2d(inters[0], GetSegmentAt(0).GetDistanceTo(inters[0]));
            }
        }

        /// <summary>
        /// 頂点が時計回りに配置されているかどうか調べる
        /// </summary>
        public bool IsClockwise => SignedArea < 0.0;

        /// <summary>
        /// 符号付き面積（点が時計回りの場合は負）
        /// </summary>
        public double SignedArea => ((B.X - A.X) * (C.Y - A.Y) - (C.X - A.X) * (B.Y - A.Y)) / 2.0;

        #endregion

        #region Méthodes 
        /// <summary>
        /// 指定平面に従って、Triangle3d に変換
        /// </summary>
        /// <param name="plane">Triangle3d の平面</param>
        /// <returns>Triangle3d の新しいインスタンス</returns>
        public Triangle3d Convert3d(Plane plane) =>
            new Triangle3d(Array.ConvertAll(Points, x => x.Convert3d(plane)));

        /// <summary>
        /// 法線 と 標高 によって定義された平面に従って、Triangle3d に変換
        /// </summary>
        /// <param name="normal">平面の法線</param>
        /// <param name="elevation">平面の標高</param>
        /// <returns>Triangle3d の新しいインスタンス</returns>
        public Triangle3d Convert3d(Vector3d normal, double elevation) =>
            new Triangle3d(Array.ConvertAll(Points, x => x.Convert3d(normal, elevation)));

        /// <summary>
        /// 指定されたインデックスで 2 つの辺の間の角度を取得
        /// </summary>.
        /// <param name="index">頂点のインデックス</param>
        /// <returns>ラジアン単位の角度</returns>
        public double GetAngleAt(int index)
        {
            double ang =
                this[index].GetVectorTo(this[(index + 1) % 3]).GetAngleTo(
                this[index].GetVectorTo(this[(index + 2) % 3]));
            if (ang > Math.PI * 2)
                return Math.PI * 2 - ang;
            else
                return ang;
        }

        /// <summary>
        /// 指定されたインデックスで LineSegement2d を取得
        /// </summary>
        /// <param name="index">セグメントのインデックス。</param>
        /// <returns>指定されたインデックスの LineSegement2d</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// indes が 0 未満または 2 より大きい場合、IndexOutOfRangeException がスローされます。</exception>
        public LineSegment2d GetSegmentAt(int index)
        {
            if (index > 2)
                throw new IndexOutOfRangeException("Index out of range");
            return new LineSegment2d(this[index], this[(index + 1) % 3]);
        }

        /// <summary>
        /// 現在のインスタンスと線分の交点を取得
        /// </summary>
        /// <param name="line2d">交点を検索する線分</param>
        /// <returns>交点のリスト (交点が存在しない場合は空のリスト)</returns>
        public List<Point2d> IntersectWith(LinearEntity2d line2d) => IntersectWith(line2d, Tolerance.Global);

        /// <summary>
        /// 指定された Tolerance を使用して、現在のインスタンスと線分の交点を取得
        /// </summary>
        /// <param name="line2d">交点を検索する線分</param>
        /// <param name="tol">比較に使用する公差</param>
        /// <returns>交点のリスト (交点が存在しない場合は空のリスト)</returns>
        public List<Point2d> IntersectWith(LinearEntity2d line2d, Tolerance tol)
        {
            List<Point2d> result = new List<Point2d>();
            for (int i = 0; i < 3; i++)
            {
                Point2d[] inters = line2d.IntersectWith(GetSegmentAt(i), tol);
                if (inters != null && inters.Length != 0 && !result.Contains(inters[0]))
                    result.Add(inters[0]);
            }
            return result;
        }

        /// <summary>
        /// 原点を変更せずに頂点の順序を逆に
        /// </summary>
        public Triangle2d Inverse() => new Triangle2d(A, C, B);

        /// <summary>
        /// Tolerance.Global を使用して、現在のインスタンスが別の Triangle2d と等しいかどうかを評価
        /// </summary>
        /// <param name="other">比較対象の三角形</param>
        /// <returns>頂点が等しい場合は true 。 それ以外の場合は false。</returns>
        public bool IsEqualTo(Triangle2d other) => IsEqualTo(other, Tolerance.Global);

        /// <summary>
        /// 指定された Tolerance を使用して、現在のインスタンスが別の Triangle2d と等しいかどうかを評価
        /// </summary>
        /// <param name="other">比較対象の三角形</param>
        /// <param name="tol">比較に使用する公差</param>
        /// <returns>頂点が等しい場合は true 。 それ以外の場合は false。</returns>
        public bool IsEqualTo(Triangle2d other, Tolerance tol) =>
            other[0].IsEqualTo(A, tol) && other[1].IsEqualTo(B, tol) && other[2].IsEqualTo(C, tol);

        /// <summary>
        /// 三角形の内側の点かどうか調べる
        /// 三角形の辺上の点は除く
        /// </summary>
        /// <param name="pt">調べる点</param>
        /// <returns>点が内側にある場合は true 。 それ以外の場合は false。</returns>
        public bool IsPointInside(Point2d pt)
        {
            if (IsPointOn(pt))
                return false;
            List<Point2d> inters = IntersectWith(new Ray2d(pt, Vector2d.XAxis));
            if (inters.Count != 1)
                return false;
            Point2d p = inters[0];
            return !p.IsEqualTo(this[0]) && !p.IsEqualTo(this[1]) && !p.IsEqualTo(this[2]);
        }

        /// <summary>
        /// 三角形の辺上の点かどうか調べる
        /// </summary>
        /// <param name="pt">調べる点</param>
        /// <returns>点が辺上にある場合は true 。 それ以外の場合は false。</returns>
        public bool IsPointOn(Point2d pt) =>
                pt.IsEqualTo(this[0]) ||
                pt.IsEqualTo(this[1]) ||
                pt.IsEqualTo(this[2]) ||
                pt.IsBetween(this[0], this[1]) ||
                pt.IsBetween(this[1], this[2]) ||
                pt.IsBetween(this[2], this[0]);

        /// <summary>
        /// 変換行列を使用して三角形を変換する
        /// </summary>
        /// <param name="mat">2次元の変換行列</param>
        /// <returns>Triangle2dの新しいインスタンス</returns>
        public Triangle2d TransformBy(Matrix2d mat) =>
            new Triangle2d(Array.ConvertAll(Points, new Converter<Point2d, Point2d>(p => p.TransformBy(mat))));

        #endregion

        #region Overrides

        /// <summary>
        /// オブジェクトが Triangle2d の現在のインスタンスと等しいかどうかを評価
        /// </summary>
        /// <param name="obj">比較するオブジェクト</param>
        /// <returns>頂点が等しい場合は true 。 それ以外の場合は false。</returns>
        public override bool Equals(object obj) =>
            obj is Triangle2d && ((Triangle2d)obj).IsEqualTo(this);

        /// <summary>
        /// Triangle2d ハッシュ関数
        /// </summary>
        /// <returns>現在の Triangle2d インスタンスのハッシュ コード</returns>
        public override int GetHashCode() => A.GetHashCode() ^ B.GetHashCode() ^ C.GetHashCode();

        /// <summary>
        /// Triangle2d の現在のインスタンスを表す文字列を返す
        /// </summary>
        /// <returns>カンマで区切られた 3 つのポイントを含む文字列</returns>
        public override string ToString() =>
            $"({A},{B},{C})";

        /// <summary>
        /// Triangle2d の現在のインスタンスを表す文字列を返す
        /// </summary>
        /// <param name="format">ポイントに使用する文字列形式</param>
        /// <returns>カンマで区切られた、指定された形式の 3 つの点を含む文字列</returns>
        public string ToString(string format) =>
            $"({A:format},{B:format},{C:format})";

        /// <summary>
        /// Triangle2d の現在のインスタンスを表す文字列を返す
        /// </summary>
        /// <param name="format">ポイントに使用する文字列形式</param>
        /// <param name="provider">フォーマット プロバイダー</param>
        /// <returns>カンマで区切られた、指定された形式の 3 つの点を含む文字列</returns>
        public string ToString(string format, IFormatProvider provider) =>
            "(" +
            A.ToString(format, provider) +
            "," +
            B.ToString(format, provider) +
            "," +
            C.ToString(format, provider) +
            ")";

        #endregion
    }
}
