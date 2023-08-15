using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polyline = Autodesk.AutoCAD.DatabaseServices.Polyline;

[assembly: CommandClass(typeof(TyAutoCad.Examples.TriangleAndOrthocenterDrawJig))]
namespace TyAutoCad.Examples
{
    /// <summary>
    /// 三角形と垂心
    /// Triangle And Orthocenter
    /// </summary>
    public class TriangleAndOrthocenterDrawJig : DrawJig
    {
        #region Fields
        private Point3d _tempPoint;
        private Polyline _triangle = null;
        #endregion

        #region Constructors
        public TriangleAndOrthocenterDrawJig(Polyline triangle)
        {
            _triangle = triangle;
            AddDummyVertex();
        }
        #endregion

        #region Properties
        public Matrix3d Ucs => Application.DocumentManager.MdiActiveDocument.Editor.CurrentUserCoordinateSystem;
        public Point3d Hp { get; set; }
        public Point3dCollection Points { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// ダミーの頂点を追加する
        /// </summary>
        private void AddDummyVertex()
        {
            _triangle.AddVertexAt(_triangle.NumberOfVertices, new Point2d(0, 0), 0, 0, 0);
        }

        /// <summary>
        /// 最後の頂点を除去する
        /// </summary>
        private void RemoveLastVertex()
        {
            if (_triangle.NumberOfVertices > 0)
                _triangle.RemoveVertexAt(_triangle.NumberOfVertices - 1);
        }
        #endregion

        #region Overrides
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            var options = new JigPromptPointOptions("\n" + _triangle.NumberOfVertices + "点目を指示: ");
            options.UserInputControls = UserInputControls.NullResponseAccepted;
            if (_triangle.NumberOfVertices == 1)
                options.UseBasePoint = false;
            else
            {
                options.UseBasePoint = true;
                options.BasePoint = _triangle.GetPoint3dAt(_triangle.NumberOfVertices - 2);
            }

            // キーワードを追加
            if (_triangle.NumberOfVertices > 2)
                options.Keywords.Add("Undo", "U", "U:戻る");

            PromptPointResult result = prompts.AcquirePoint(options);

            // 前回と同じ点の場合は変更しない
            if (_tempPoint == result.Value)
                return SamplerStatus.NoChange;

            _tempPoint = result.Value;

            return SamplerStatus.OK;
        }

        protected override bool WorldDraw(WorldDraw draw)
        {
            if (_triangle.NumberOfVertices > 1)
                _triangle.SetBulgeAt(_triangle.NumberOfVertices - 2, 0);
            if (_triangle.NumberOfVertices > 2)
                _triangle.Closed = true;

            _triangle.SetPointAt(_triangle.NumberOfVertices - 1, _tempPoint.Convert2d());

            if (!draw.RegenAbort)
            {
                draw.Geometry.Draw(_triangle);
                if (_triangle.NumberOfVertices == 3)
                {
                    Points = new Point3dCollection();

                    var v1 = _triangle.GetPoint3dAt(1).GetVectorTo(_tempPoint).GetPerpendicularVector();
                    var v2 = _triangle.GetPoint3dAt(0).GetVectorTo(_tempPoint).GetPerpendicularVector();
                    var v3 = _triangle.GetPoint3dAt(0).GetVectorTo(_triangle.GetPoint3dAt(1)).GetPerpendicularVector();

                    var line1 = new Line3d(_triangle.GetPoint3dAt(0), v1);
                    var points1 = line1.IntersectWith(new Line3d(_triangle.GetPoint3dAt(1), _tempPoint));
                    if (points1 == null) return true;
                    Points.Add(points1[0]);

                    var line2 = new Line3d(_triangle.GetPoint3dAt(1), v2);
                    var points2 = line2.IntersectWith(new Line3d(_triangle.GetPoint3dAt(0), _tempPoint));
                    if (points2 == null) return true;
                    Points.Add(points2[0]);

                    var line3 = new Line3d(_tempPoint, v3);
                    var points3 = line3.IntersectWith(new Line3d(_triangle.GetPoint3dAt(0), _triangle.GetPoint3dAt(1)));
                    if (points3 == null) return true;
                    Points.Add(points3[0]);

                    var points0 = line1.IntersectWith(line2);
                    if (points0 == null) return true;
                    Hp = points0[0];

                    WorldGeometry geometry = draw.Geometry;
                    if (geometry != null && Points.Count == 3)
                    {
                        geometry.PushModelTransform(Ucs);
                        draw.SubEntityTraits.Color = 1;
                        geometry.WorldLine(_triangle.GetPoint3dAt(0), Points[0]);
                        geometry.WorldLine(_triangle.GetPoint3dAt(1), Points[1]);
                        geometry.WorldLine(_tempPoint, Points[2]);
                        geometry.WorldLine(_triangle.GetPoint3dAt(0), Hp);
                        geometry.WorldLine(_triangle.GetPoint3dAt(1), Hp);
                        geometry.WorldLine(_tempPoint, Hp);
                        geometry.PopModelTransform();
                    }
                }
            }
            return true;
        }
        #endregion

        #region Command
        /// <summary>
        /// 三角形とその垂心を作図
        /// </summary>
        [CommandMethod("TriangleAndOrthocenterDrawJig")]
        public static void Command()
        {
            var sw = new Stopwatch();
            sw.Start(); // 計測開始

            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- 三角形とその垂心を作図 ---");

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // 現在スペースを書込モードで取得
                    var cs = tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;

                    // ポリライン（三角形）を作成してデータベースに追加
                    var triangle = new Polyline();
                    triangle.Normal = Vector3d.ZAxis.TransformBy(ed.CurrentUserCoordinateSystem);
                    triangle.Transparency = new Transparency(100);
                    cs.AppendEntity(triangle);
                    tr.AddNewlyCreatedDBObject(triangle, true);

                    // ポリラインをJigに渡す
                    var jigger = new TriangleAndOrthocenterDrawJig(triangle);

                    PromptResult result = null;
                    do
                    {
                        result = ed.Drag(jigger);
                        switch (result.Status)
                        {
                            case PromptStatus.OK:
                                triangle.SetPointAt(triangle.NumberOfVertices - 1, jigger._tempPoint.Convert2d());
                                jigger.AddDummyVertex();
                                if (triangle.NumberOfVertices > 3)
                                {
                                    jigger.RemoveLastVertex();
                                    triangle.Transparency = new Transparency();

                                    var triangle3D = new Triangle3d(triangle.GetPoint3dAt(0), triangle.GetPoint3dAt(1), triangle.GetPoint3dAt(2));

                                    for (int i = 0; i < triangle.NumberOfVertices; i++)
                                    {
                                        var line = new Line(triangle.GetPoint3dAt(i), (triangle3D.IsPointInside(jigger.Hp)) ? jigger.Points[i] : jigger.Hp);
                                        line.ColorIndex = 1;
                                        cs.AppendEntity(line);
                                        tr.AddNewlyCreatedDBObject(line, true);
                                    }

                                    tr.Commit();
                                    ed.WriteMessage("\n三角形とその垂心を作図しました。\n--- コマンド終了 ---");
                                    return;
                                }
                                break;
                            case PromptStatus.Keyword:
                                if (result.StringResult == "Undo")
                                    jigger.RemoveLastVertex();
                                break;
                            default:
                                break;
                        }
                    } while (result.Status == PromptStatus.OK || result.Status == PromptStatus.Keyword);
                    ed.WriteMessage("\n--- コマンドキャンセル ---");
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error TriangleAndOrthocenterDrawJig\n\t" + ex.Message);
                }
            }
        }
        #endregion
    }
}
