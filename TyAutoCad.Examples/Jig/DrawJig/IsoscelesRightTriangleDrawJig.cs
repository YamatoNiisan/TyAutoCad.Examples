using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polyline = Autodesk.AutoCAD.DatabaseServices.Polyline;

[assembly: CommandClass(typeof(TyAutoCad.Examples.IsoscelesRightTriangleDrawJig))]
namespace TyAutoCad.Examples
{
    /// <summary>
    /// 直角二等辺三角形 DrawJig
    /// </summary>
    public class IsoscelesRightTriangleDrawJig : DrawJig
    {
        #region Fields
        private Matrix3d _ucs;
        private Polyline _poly = null;
        private bool _hasFirstVertex = false;
        #endregion

        #region Constructors
        public IsoscelesRightTriangleDrawJig(Matrix3d ucs, Polyline poly)
        {
            _ucs = ucs;
            _poly = poly;
        }
        #endregion

        #region Properties
        public Point3d FirstVertex { get; private set; }
        public Point3d PointOnHypotenuse { get; private set; }

        public double Angle => Math.Atan2(PointOnHypotenuse.Y - FirstVertex.Y, PointOnHypotenuse.X - FirstVertex.X);

        /// <summary>
        /// 斜辺
        /// </summary>
        public Line3d Hypotenuse
        {
            get
            {
                if (Angle >= 0 && Math.PI / 2 >= Angle)
                    return new Line3d(PointOnHypotenuse, new Vector3d(-1, 1, 0));
                if (Angle > Math.PI / 2 && Math.PI >= Angle)
                    return new Line3d(PointOnHypotenuse, new Vector3d(-1, -1, 0));
                if (Angle > -Math.PI && -Math.PI / 2 >= Angle)
                    return new Line3d(PointOnHypotenuse, new Vector3d(1, -1, 0));
                return new Line3d(PointOnHypotenuse, new Vector3d(1, 1, 0));
            }
        }
        public Point3d P1 => Hypotenuse.IntersectWith(new Line3d(FirstVertex, new Vector3d(1, 0, 0)))[0];
        public Point3d P2 => Hypotenuse.IntersectWith(new Line3d(FirstVertex, new Vector3d(0, 1, 0)))[0];

        #endregion

        #region Overrides
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            var options = new JigPromptPointOptions();
            options.UserInputControls = UserInputControls.NoZeroResponseAccepted;

            if (_hasFirstVertex)
            {
                options.Message = "\n斜辺上の点を指示: ";
                options.UseBasePoint = true;
                options.BasePoint = FirstVertex.TransformBy(_ucs);
            }
            else
                options.Message = "\n三角形の頂点を指示: ";

            PromptPointResult result = prompts.AcquirePoint(options);
            if (!_hasFirstVertex)
                FirstVertex = PointOnHypotenuse = result.Value.TransformBy(_ucs.Inverse());
            else if (FirstVertex == result.Value.TransformBy(_ucs.Inverse())) //前回と同じ点の場合は変更なし
                return SamplerStatus.NoChange;
            else
                PointOnHypotenuse = result.Value.TransformBy(_ucs.Inverse());

            return SamplerStatus.OK;
        }

        protected override bool WorldDraw(WorldDraw draw)
        {
            if (_hasFirstVertex)
            {
                _poly.SetPointAt(1, P1.TransformBy(_ucs).Convert2d());
                _poly.SetPointAt(2, P2.TransformBy(_ucs).Convert2d());
                if (!draw.RegenAbort)
                    draw.Geometry.Draw(_poly);
            }
            return true;
        }
        #endregion

        #region Command
        /// <summary>
        /// DrawJig を使って直角二等辺三角形を作成
        /// </summary>
        [CommandMethod("IsoscelesRightTriangleDrawJig")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;
            var ucs = ed.CurrentUserCoordinateSystem;

            ed.WriteMessage("\n--- DrawJig を使って直角二等辺三角形を作成 ---");

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // 現在スペースを書込モードで取得
                    var cs = tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;

                    // 頂点が3つあるポリラインを作成してデータベースに追加
                    var poly = new Polyline();
                    for (int i = 0; i < 3; i++)
                        poly.AddVertexAt(i, Point2d.Origin, 0, 0, 0);
                    poly.Closed = true;
                    poly.Normal = Vector3d.ZAxis.TransformBy(ed.CurrentUserCoordinateSystem);
                    poly.Transparency = new Transparency(100);
                    cs.AppendEntity(poly);
                    tr.AddNewlyCreatedDBObject(poly, true);

                    // ジグにUCSとポリラインを渡す
                    var jigger = new IsoscelesRightTriangleDrawJig(ucs, poly);

                    PromptResult result;
                    do
                    {
                        result = ed.Drag(jigger);
                        if (result.Status == PromptStatus.OK)
                        {
                            if (!jigger._hasFirstVertex)
                            {
                                poly.SetPointAt(0, jigger.FirstVertex.TransformBy(ucs).Convert2d());
                                jigger._hasFirstVertex = true;
                                continue;
                            }
                            poly.SetPointAt(1, jigger.P1.TransformBy(ucs).Convert2d());
                            poly.SetPointAt(2, jigger.P2.TransformBy(ucs).Convert2d());
                            poly.Transparency = new Transparency();
                            tr.Commit();
                            break;
                        }
                    } while (result.Status == PromptStatus.OK);

                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error IsoscelesRightTriangleDrawJig\n\t" + ex.Message);
                }
            }

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
        #endregion
    }
}
