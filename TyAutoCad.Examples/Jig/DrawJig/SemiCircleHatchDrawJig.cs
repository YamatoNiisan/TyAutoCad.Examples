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

[assembly: CommandClass(typeof(TyAutoCad.Examples.SemiCircleHatchDrawJig))]
namespace TyAutoCad.Examples
{
    /// <summary>
    /// 半円ハッチング DrawJig
    /// </summary>
    public class SemiCircleHatchDrawJig : DrawJig
    {
        #region Fields
        private Matrix3d _ucs;
        private Polyline _poly = null;
        private Hatch _hatch = null;
        private bool _hasCenterPoint = false;
        #endregion

        #region Constructors
        public SemiCircleHatchDrawJig(Matrix3d ucs, Polyline poly, Hatch hatch)
        {
            _ucs = ucs;
            _poly = poly;
            _hatch = hatch;
        }
        #endregion

        #region Properties
        public Point3d Center { get; private set; }
        public Point3d PointOnSemiCircle { get; private set; }
        public double Bulge => 1.0;
        public Point3d P1 => PointOnSemiCircle.RotateBy(-Math.PI / 2, Vector3d.ZAxis, Center);
        public Point3d P2 => PointOnSemiCircle.RotateBy(Math.PI / 2, Vector3d.ZAxis, Center);
        public double PatternAngle => Math.Atan2(PointOnSemiCircle.Y - Center.Y, PointOnSemiCircle.X - Center.X) - Math.PI / 2;
        #endregion

        #region Methods
        /// <summary>
        /// ハッチングに境界ループを追加する
        /// </summary>
        private void HatchAppendLoop()
        {
            var ids = new ObjectIdCollection();
            ids.Add(_poly.ObjectId);
            _hatch.Associative = true;
            _hatch.AppendLoop(HatchLoopTypes.Default, ids);
        }
        #endregion

        #region Overrides
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            var options = new JigPromptPointOptions();
            options.UserInputControls = UserInputControls.NoZeroResponseAccepted;

            if (_hasCenterPoint)
            {
                options.Message = "\n半円上の点を指示: ";
                options.UseBasePoint = true;
                options.BasePoint = Center.TransformBy(_ucs);
            }
            else
                options.Message = "\n半円の中心を指示: ";

            PromptPointResult result = prompts.AcquirePoint(options);
            if (!_hasCenterPoint)
                Center = PointOnSemiCircle = result.Value.TransformBy(_ucs.Inverse());
            else if (Center == result.Value.TransformBy(_ucs.Inverse())) //前回と同じ点の場合は変更なし
                return SamplerStatus.NoChange;
            else
                PointOnSemiCircle = result.Value.TransformBy(_ucs.Inverse());

            return SamplerStatus.OK;
        }

        protected override bool WorldDraw(WorldDraw draw)
        {
            if (_hasCenterPoint)
            {
                _poly.SetPointAt(0, P1.TransformBy(_ucs).Convert2d());
                _poly.SetBulgeAt(0, Bulge);
                _poly.SetPointAt(1, P2.TransformBy(_ucs).Convert2d());
                if (!draw.RegenAbort)
                {
                    draw.Geometry.Draw(_poly);
                    if (_poly.Area > 0.001)
                    {
                        HatchAppendLoop(); //境界ループ追加 
                        _hatch.PatternAngle = PatternAngle;
                        _hatch.EvaluateHatch(true);
                        if (!draw.RegenAbort)
                            draw.Geometry.Draw(_hatch);
                        _hatch.RemoveLoopAt(0); //境界ループ除去 
                    }
                }
            }
            return true;
        }
        #endregion

        #region Command
        /// <summary>
        /// DrawJig を使って半円のハッチングを作成
        /// </summary>
        [CommandMethod("SemiCircleHatchDrawJig")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;
            var ucs = ed.CurrentUserCoordinateSystem;

            ed.WriteMessage("\n--- DrawJig を使って半円のハッチングを作成 ---");

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // 現在スペースを書込モードで取得
                    var cs = tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;

                    // 頂点が2つあるポリラインを作成してデータベースに追加
                    var poly = new Polyline();
                    for (int i = 0; i < 2; i++)
                        poly.AddVertexAt(i, Point2d.Origin, 0, 0, 0);
                    poly.Closed = true;
                    poly.Normal = Vector3d.ZAxis.TransformBy(ed.CurrentUserCoordinateSystem);
                    poly.Transparency = new Transparency(100);
                    cs.AppendEntity(poly);
                    tr.AddNewlyCreatedDBObject(poly, true);

                    // ハッチングを作成してデータベースに追加
                    var hatch = new Hatch();
                    hatch.SetHatchPattern(HatchPatternType.PreDefined, "ANSI37");
                    hatch.Transparency = new Transparency(100);
                    cs.AppendEntity(hatch);
                    tr.AddNewlyCreatedDBObject(hatch, true);

                    // ジグにUCSとポリラインとハッチングを渡す
                    var jigger = new SemiCircleHatchDrawJig(ucs, poly, hatch);

                    PromptResult result;
                    do
                    {
                        result = ed.Drag(jigger);
                        if (result.Status == PromptStatus.OK)
                        {
                            if (!jigger._hasCenterPoint)
                            {
                                jigger._hasCenterPoint = true;
                                continue;
                            }
                            jigger.HatchAppendLoop();
                            poly.SetPointAt(0, jigger.P1.TransformBy(ucs).Convert2d());
                            poly.SetBulgeAt(0, jigger.Bulge);
                            poly.SetPointAt(1, jigger.P2.TransformBy(ucs).Convert2d());
                            poly.Transparency = new Transparency();
                            hatch.Transparency = new Transparency();
                            tr.Commit();
                            break;
                        }
                    } while (result.Status == PromptStatus.OK);

                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error SemiCircleHatchDrawJig\n\t" + ex.Message);
                }
            }

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
        #endregion
    }
}
