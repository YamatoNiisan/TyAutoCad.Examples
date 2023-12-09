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

[assembly: CommandClass(typeof(TyAutoCad.Examples.RectangleDrawJig))]
namespace TyAutoCad.Examples
{
    /// <summary>
    /// 長方形 DrawJig
    /// </summary>
    public class RectangleDrawJig : DrawJig
    {
        #region Fields
        private Matrix3d _ucs;
        private Polyline _poly = null;
        private bool _hasFirstPoint = false;
        #endregion

        #region Constructors
        public RectangleDrawJig(Matrix3d ucs, Polyline poly)
        {
            _ucs = ucs;
            _poly = poly;
        }

        public RectangleDrawJig(Matrix3d ucs, Polyline poly, Point3d basePoint)
        {
            _ucs = ucs;
            _poly = poly;
            P1 = P3 = basePoint;
            _hasFirstPoint = true;
        }
        #endregion

        #region Properties
        public Point3d P1 { get; set; }
        public Point3d P3 { get; set; }
        public Point3d P1a => P1.TransformBy(_ucs.Inverse());
        public Point3d P3a => P3.TransformBy(_ucs.Inverse());
        public Point3d P2 => new Point3d(P3a.X, P1a.Y, 0);
        public Point3d P4 => new Point3d(P1a.X, P3a.Y, 0);
        #endregion

        #region Overrides
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            var options = new JigPromptPointOptions();
            options.UserInputControls = UserInputControls.NoZeroResponseAccepted;

            if (_hasFirstPoint)
            {
                options.Message = "\n終点を指示: ";
                options.UseBasePoint = true;
                options.BasePoint = P1;
            }
            else
                options.Message = "\n始点を指示: ";
 
            PromptPointResult result = prompts.AcquirePoint(options);
            if (!_hasFirstPoint)
                P1 = P3 = result.Value;
            else if (P1 == result.Value) //前回と同じ点の場合は変更なし
                return SamplerStatus.NoChange;
            else
                P3 = result.Value;

            return SamplerStatus.OK;
        }

        protected override bool WorldDraw(WorldDraw draw)
        {
            if (_hasFirstPoint)
            {
                _poly.SetPointAt(1, P2.TransformBy(_ucs).Convert2d());
                _poly.SetPointAt(2, P3.Convert2d());
                _poly.SetPointAt(3, P4.TransformBy(_ucs).Convert2d());
                if (!draw.RegenAbort)
                    draw.Geometry.Draw(_poly);
            }
            return true;
        }
        #endregion

        #region Method
        /// <summary>
        /// DrawJig を使って四角形を作成
        /// </summary>
        public static void Execute(Point3d? basePoint = null)
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;
            var ucs = ed.CurrentUserCoordinateSystem;

            ed.WriteMessage("\n--- DrawJig を使って四角形を作成 ---");

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // 現在スペースを書込モードで取得
                    var cs = tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;

                    // 頂点が4つあるポリラインを作成してデータベースに追加
                    var poly = new Polyline();
                    for (int i = 0; i < 4; i++)
                        poly.AddVertexAt(i, Point2d.Origin, 0, 0, 0);

                    if (basePoint != null)
                    {
                        Point3d bp = (Point3d)basePoint;
                        poly.SetPointAt(0, bp.Convert2d());
                    }
                    poly.Closed = true;
                    poly.Normal = Vector3d.ZAxis.TransformBy(ed.CurrentUserCoordinateSystem);
                    poly.Transparency = new Transparency(100);
                    cs.AppendEntity(poly);
                    tr.AddNewlyCreatedDBObject(poly, true);

                    // ジグにUCSとポリラインを渡す
                    RectangleDrawJig jigger = null;
                    if (basePoint == null)
                    {
                        jigger = new RectangleDrawJig(ucs, poly);
                    }
                    else
                    {
                        jigger = new RectangleDrawJig(ucs, poly, (Point3d)basePoint);
                    }

                    PromptResult result;
                    do
                    {
                        result = ed.Drag(jigger);
                        if (result.Status == PromptStatus.OK)
                        {
                            if (!jigger._hasFirstPoint)
                            {
                                poly.SetPointAt(0, jigger.P1.Convert2d());
                                jigger._hasFirstPoint = true;
                                continue;
                            }
                            poly.SetPointAt(1, jigger.P2.TransformBy(ucs).Convert2d());
                            poly.SetPointAt(2, jigger.P3.Convert2d());
                            poly.SetPointAt(3, jigger.P4.TransformBy(ucs).Convert2d());
                            poly.Transparency = new Transparency();
                            tr.Commit();
                            break;
                        }
                    } while (result.Status == PromptStatus.OK);
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error RectangleDrawJig\n\t" + ex.Message);
                }
            }
            ed.WriteMessage("\n--- コマンド終了 ---");
        }
        #endregion

        #region Command
        /// <summary>
        /// DrawJig を使って四角形を作成
        /// </summary>
        [CommandMethod("RectangleDrawJig")]
        public static void Command()
        {
            RectangleDrawJig.Execute();
        }
        #endregion

    }
}
