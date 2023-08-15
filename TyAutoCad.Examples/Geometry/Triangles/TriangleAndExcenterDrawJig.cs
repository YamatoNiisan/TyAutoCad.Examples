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

[assembly: CommandClass(typeof(TyAutoCad.Examples.TriangleAndExcenterDrawJig))]
namespace TyAutoCad.Examples
{
    /// <summary>
    /// 三角形と傍心
    /// Triangle and Excenter
    /// </summary>
    public class TriangleAndExcenterDrawJig : DrawJig
    {
        #region Fields
        private Point3d _tempPoint;
        private Polyline _triangle = null;
        #endregion

        #region Constructors
        public TriangleAndExcenterDrawJig(Polyline triangle)
        {
            _triangle = triangle;
            AddDummyVertex();
        }

        #endregion

        #region Properties
        public Matrix3d Ucs => Application.DocumentManager.MdiActiveDocument.Editor.CurrentUserCoordinateSystem;

        public Point3d Ia { get; set; }

        public Point3d Ib { get; set; }

        public Point3d Ic { get; set; }
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
                    var va =
                        (_triangle.GetPoint3dAt(0).GetVectorTo(_triangle.GetPoint3dAt(1)).GetNormal() +
                        _triangle.GetPoint3dAt(0).GetVectorTo(_tempPoint).GetNormal()) / 2;
                    var vb =
                        (_triangle.GetPoint3dAt(1).GetVectorTo(_triangle.GetPoint3dAt(0)).GetNormal() +
                        _triangle.GetPoint3dAt(1).GetVectorTo(_tempPoint).GetNormal()) / 2;
                    var vc =
                        (_tempPoint.GetVectorTo(_triangle.GetPoint3dAt(1)).GetNormal() +
                        _tempPoint.GetVectorTo(_triangle.GetPoint3dAt(0)).GetNormal()) / 2;


                    var line1a = new Line3d(_triangle.GetPoint3dAt(0), va);
                    var line2a = new Line3d(_triangle.GetPoint3dAt(1), vb.GetPerpendicularVector());

                    var pointsA = line1a.IntersectWith(line2a);
                    if (pointsA == null) return true;
                    Ia = pointsA[0];

                    var line1b = new Line3d(_triangle.GetPoint3dAt(1), vb);
                    var line2b = new Line3d(_tempPoint, vc.GetPerpendicularVector());

                    var pointsB = line1b.IntersectWith(line2b);
                    if (pointsB == null) return true;
                    Ib = pointsB[0];

                    var line1c = new Line3d(_tempPoint, vc);
                    var line2c = new Line3d(_triangle.GetPoint3dAt(0), va.GetPerpendicularVector());

                    var pointsC = line1c.IntersectWith(line2c);
                    if (pointsC == null) return true;
                    Ic = pointsC[0];

                    WorldGeometry geometry = draw.Geometry;
                    if (geometry != null)
                    {
                        geometry.PushModelTransform(Ucs);
                        short color = 1;
                        foreach (Point3d ip in new Point3dCollection { Ia, Ib, Ic })
                        {
                            draw.SubEntityTraits.Color = color;
                            geometry.WorldLine(_triangle.GetPoint3dAt(0), ip);
                            geometry.WorldLine(_triangle.GetPoint3dAt(1), ip);
                            geometry.WorldLine(_tempPoint, ip);
                            color++;
                        }
                        geometry.PopModelTransform();
                    }
                }
            }
            return true;
        }
        #endregion

        #region Command
        /// <summary>
        /// 三角形とその傍心を作図
        /// </summary>
        [CommandMethod("TriangleAndExcenterDrawJig")]
        public static void Command()
        {
            var sw = new Stopwatch();
            sw.Start(); // 計測開始

            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- 三角形とその傍心を作図 ---");

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
                    var jigger = new TriangleAndExcenterDrawJig(triangle);

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

                                    int color = 1;
                                    foreach (Point3d ip in new Point3dCollection { jigger.Ia, jigger.Ib, jigger.Ic })
                                    {
                                        for (int i = 0; i < triangle.NumberOfVertices; i++)
                                        {
                                            var line = new Line(triangle.GetPoint3dAt(i), ip);
                                            line.ColorIndex = color;
                                            cs.AppendEntity(line);
                                            tr.AddNewlyCreatedDBObject(line, true);
                                        }
                                        color++;
                                    }

                                    tr.Commit();
                                    ed.WriteMessage("\n三角形とその傍心を作図しました。\n--- コマンド終了 ---");
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
                    Application.ShowAlertDialog("Error TriangleAndExcenterDrawJig\n\t" + ex.Message);
                }
            }
        }
        #endregion
    }
}
