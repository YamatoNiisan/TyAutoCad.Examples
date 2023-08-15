using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// 三角形3D（頂点 A, B, C）
    /// </summary>
    public struct Triangle3d
    {
        #region Constructors
        public Triangle3d(Point3d a, Point3d b, Point3d c)
        {
            A = a;
            B = b;
            C = c;
        }

        public Triangle3d(Point3d[] points)
        {
            if (points.Length != 3)
                throw new ArgumentOutOfRangeException("Needs 3 points.");
            A = points[0];
            B = points[1];
            C = points[2];
        }

        public Triangle3d(Point3dCollection points)
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
        public Point3d this[int i] => Vertexes[i];
        #endregion

        #region Properties
        /// <summary>
        /// 頂点A
        /// </summary>
        public Point3d A { get; }

        /// <summary>
        /// 頂点B
        /// </summary>
        public Point3d B { get; }

        /// <summary>
        /// 頂点C
        /// </summary>
        public Point3d C { get; }

        /// <summary>
        /// 3つの頂点の配列
        /// </summary>
        public Point3d[] Points => new[] { A, B, C };

        /// <summary>
        /// 3つの頂点のコレクション
        /// </summary>
        public Point3dCollection Vertexes => new Point3dCollection { A, B, C };

        /// <summary>
        /// 面積
        /// </summary>
        public double Area => Math.Abs(((B.X - A.X) * (C.Y - A.Y) - (C.X - A.X) * (B.Y - A.Y)) / 2.0);

        /// <summary>
        /// 重心
        /// </summary>
        public Point3d Centroid => (A + B.GetAsVector() + C.GetAsVector()) / 3.0;

        /// <summary>
        /// 外接円
        /// </summary>
        public CircularArc3d CircumscribedCircle
        {
            get
            {
                CircularArc2d ca2d = Convert2d().CircumscribedCircle;
                if (ca2d == null)
                    return null;
                return new CircularArc3d(ca2d.Center.Convert3d(GetPlane()), Normal, ca2d.Radius);
            }
        }

        /// <summary>
        /// 三角形が存在する平面の標高
        /// </summary>
        public double Elevation => A.TransformBy(Matrix3d.WorldToPlane(Normal)).Z;

        /// <summary>
        /// 三角形が存在する平面の最大勾配の単位ベクトル
        /// </summary>
        public Vector3d GreatestSlope =>
            Normal.IsParallelTo(Vector3d.ZAxis) ?
                new Vector3d(0.0, 0.0, 0.0) :
                Normal.Z == 0.0 ?
                    Vector3d.ZAxis.Negate() :
                    new Vector3d(-Normal.Y, Normal.X, 0.0).CrossProduct(Normal).GetNormal();

        /// <summary>
        /// 三角形が存在する平面の水平の単位ベクトル
        /// </summary>
        public Vector3d Horizontal =>
            Normal.IsParallelTo(Vector3d.ZAxis) ?
                Vector3d.XAxis :
                new Vector3d(-Normal.Y, Normal.X, 0.0).GetNormal();

        /// <summary>
        /// 内接円
        /// </summary>
        public CircularArc3d InscribedCircle
        {
            get
            {
                CircularArc2d ca2d = Convert2d().InscribedCircle;
                if (ca2d == null)
                    return null;
                return new CircularArc3d(ca2d.Center.Convert3d(GetPlane()), Normal, ca2d.Radius);
            }
        }

        /// <summary>
        /// 三角形が存在する平面が水平かどうか
        /// </summary>
        public bool IsHorizontal => A.Z == B.Z && A.Z == C.Z;

        /// <summary>
        /// 三角形が存在する平面の法線ベクトル
        /// </summary>
        public Vector3d Normal => (B - A).CrossProduct(C - A).GetNormal();

        /// <summary>
        /// 三角形の勾配（パーセント表示）
        /// </summary>
        public double SlopePerCent =>
            Normal.Z == 0.0 ?
                double.PositiveInfinity :
                Math.Abs(100.0 * Math.Sqrt(Normal.X * Normal.X + Normal.Y * Normal.Y) / Normal.Z);

        /// <summary>
        /// 三角形の座標系 (原点 = 重心、X 軸 = 水平、Z 軸 = 法線)
        /// </summary>
        public Matrix3d SlopeUCS
        {
            get
            {
                Point3d origin = Centroid;
                Vector3d zaxis = Normal;
                Vector3d xaxis = Horizontal;
                Vector3d yaxis = zaxis.CrossProduct(xaxis).GetNormal();
                return new Matrix3d(new double[]{
                    xaxis.X, yaxis.X, zaxis.X, origin.X,
                    xaxis.Y, yaxis.Y, zaxis.Y, origin.Y,
                    xaxis.Z, yaxis.Z, zaxis.Z, origin.Z,
                    0.0, 0.0, 0.0, 1.0});
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Triangle2d に変換
        /// </summary>
        /// <returns>Triangle2d の新しいインスタンス</returns>
        public Triangle2d Convert2d()
        {
            var plane = GetPlane();
            return new Triangle2d(Array.ConvertAll(Points, x => x.Convert2d(plane)));
        }

        /// <summary>
        /// 現在のインスタンスを XY 平面に投影
        /// </summary>
        /// <returns>Triangle2d の新しいインスタンス</returns>
        public Triangle2d Flatten() =>
            new Triangle2d(
                new Point2d(this[0].X, this[0].Y),
                new Point2d(this[1].X, this[1].Y),
                new Point2d(this[2].X, this[2].Y));

        /// <summary>
        /// 指定されたインデックスで 2 つの辺の間の角度を取得
        /// </summary>.
        /// <param name="index">頂点のインデックス</param>
        /// <returns>ラジアン単位の角度</returns>
        public double GetAngleAt(int index) =>
            this[index].GetVectorTo(this[(index + 1) % 3]).GetAngleTo(
                this[index].GetVectorTo(this[(index + 2) % 3]));

        /// <summary>
        /// 現在の三角形によって定義された境界平面を取得
        /// </summary>
        /// <returns>境界平面</returns>
        public BoundedPlane GetBoundedPlane() => new BoundedPlane(A, B, C);

        /// <summary>
        /// 現在の三角形によって定義された境界のない平面を取得
        /// </summary>
        /// <returns>平面</returns>
        public Plane GetPlane()
        {
            Point3d origin =
                new Point3d(0.0, 0.0, Elevation).TransformBy(Matrix3d.PlaneToWorld(Normal));
            return new Plane(origin, Normal);
        }

        /// <summary>
        /// 指定されたインデックスで LineSegement3d を取得
        /// </summary>
        /// <param name="index">辺のインデックス</param>
        /// <returns>指定されたインデックスの LineSegement2d</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// indes が 0 未満または 2 より大きい場合、IndexOutOfRangeException がスローされます。</exception>
        public LineSegment3d GetSegmentAt(int index)
        {
            if (index > 2)
                throw new IndexOutOfRangeException("Index out of range");
            return new LineSegment3d(this[index], this[(index + 1) % 3]);
        }

        /// <summary>
        /// 原点を変更せずにポイントの順序を逆に
        /// </summary>
        public Triangle3d Inverse() => new Triangle3d(A, C, B);

        /// <summary>
        /// Tolerance.Global を使用して、現在のインスタンスが別の Triangle2d と等しいかどうかを評価
        /// </summary>
        /// <param name="other">比較対象の三角形</param>
        /// <returns>頂点が等しい場合は true 。 それ以外の場合は false。</returns>
        public bool IsEqualTo(Triangle3d other) => IsEqualTo(other, Tolerance.Global);

        /// <summary>
        /// 指定された Tolerance を使用して、現在のインスタンスが別の Triangle3d と等しいかどうかを評価
        /// </summary>
        /// <param name="other">比較対象の三角形</param>
        /// <param name="tol">比較に使用する公差</param>
        /// <returns>頂点が等しい場合は true 。 それ以外の場合は false。</returns>
        public bool IsEqualTo(Triangle3d other, Tolerance tol)
            => other[0].IsEqualTo(A, tol) && other[1].IsEqualTo(B, tol) && other[2].IsEqualTo(C, tol);

        /// <summary>
        /// 三角形の内側の点かどうか調べる
        /// </summary>
        /// <param name="pt">調べる点</param>
        /// <returns>点が内側にある場合は true 。 それ以外の場合は false。</returns>
        public bool IsPointInside(Point3d pt)
        {
            Tolerance tol = new Tolerance(1e-9, 1e-9);
            Vector3d v1 = pt.GetVectorTo(A).CrossProduct(pt.GetVectorTo(B)).GetNormal();
            Vector3d v2 = pt.GetVectorTo(B).CrossProduct(pt.GetVectorTo(C)).GetNormal();
            Vector3d v3 = pt.GetVectorTo(C).CrossProduct(pt.GetVectorTo(A)).GetNormal();
            return v1.IsEqualTo(v2, tol) && v2.IsEqualTo(v3, tol);
        }

        /// <summary>
        /// 三角形の辺上の点かどうか調べる
        /// </summary>
        /// <param name="pt">調べる点</param>
        /// <returns>点が辺上にある場合は true 。 それ以外の場合は false。</returns>
        public bool IsPointOn(Point3d pt)
        {
            Tolerance tol = new Tolerance(1e-9, 1e-9);
            Vector3d v0 = new Vector3d(0.0, 0.0, 0.0);
            Vector3d v1 = pt.GetVectorTo(A).CrossProduct(pt.GetVectorTo(B));
            Vector3d v2 = pt.GetVectorTo(B).CrossProduct(pt.GetVectorTo(C));
            Vector3d v3 = pt.GetVectorTo(C).CrossProduct(pt.GetVectorTo(A));
            return v1.IsEqualTo(v0, tol) || v2.IsEqualTo(v0, tol) || v3.IsEqualTo(v0, tol);
        }

        /// <summary>
        /// 変換行列を使用して三角形を変換
        /// </summary>
        /// <param name="mat">3D 変換マトリックス</param>
        /// <returns>Triangle3d の新しいインスタンス</returns>
        public Triangle3d Transformby(Matrix3d mat) =>
            new Triangle3d(Array.ConvertAll(Points, new Converter<Point3d, Point3d>(p => p.TransformBy(mat))));

        #endregion

        #region overrides

        /// <summary>
        /// オブジェクトが Triangle3d の現在のインスタンスと等しいかどうかを評価
        /// </summary>
        /// <param name="obj">比較するオブジェクト</param>
        /// <returns>頂点が等しい場合は true 。 それ以外の場合は false。</returns>
        public override bool Equals(object obj) =>
            obj is Triangle3d && ((Triangle3d)obj).IsEqualTo(this);

        /// <summary>
        /// Triangle3d ハッシュ関数
        /// </summary>
        /// <returns>現在の Triangle3d インスタンスのハッシュ コード</returns>
        public override int GetHashCode() => A.GetHashCode() ^ B.GetHashCode() ^ C.GetHashCode();

        /// <summary>
        /// Triangle3d の現在のインスタンスを表す文字列を返す
        /// </summary>
        /// <returns>カンマで区切られた 3 つの点を含む文字列</returns>
        public override string ToString() => $"({A},{B},{C})";

        /// <summary>
        /// Triangle3d の現在のインスタンスを表す文字列を返す
        /// </summary>
        /// <param name="format">点に使用する文字列形式</param>
        /// <returns>カンマで区切られた、指定された形式の 3 つの点を含む文字列</returns>
        public string ToString(string format) => $"({A:format},{B:format},{C:format})";

        /// <summary>
        /// Triangle3d の現在のインスタンスを表す文字列を返す
        /// </summary>
        /// <param name="format">点に使用する文字列形式</param>
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
