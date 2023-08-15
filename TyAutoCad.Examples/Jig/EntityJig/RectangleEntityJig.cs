using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.RectangleEntityJig))]
namespace TyAutoCad.Examples
{
    public class RectangleEntityJig : EntityJig
    {
        #region Fields
        private Matrix3d _ucs;
        private Polyline _poly;
        public int _factor = 1;
        #endregion

        #region Constructors
        public RectangleEntityJig(Matrix3d ucs, Polyline poly) : base(poly)
        {
            _ucs = ucs;
            _poly = poly;
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
            switch (_factor)
            {
                case 1:
                    var options1 = new JigPromptPointOptions("\n始点を指示:");
                    PromptPointResult result1 = prompts.AcquirePoint(options1);
                    if (result1.Status == PromptStatus.Cancel)
                        return SamplerStatus.Cancel;
                    P1 = P3 = result1.Value;
                    return SamplerStatus.OK;
                case 2:
                    var options2 = new JigPromptPointOptions("\n終点を指示:");
                    PromptPointResult result2 = prompts.AcquirePoint(options2);
                    if (result2.Status == PromptStatus.Cancel)
                        return SamplerStatus.Cancel;
                    if (P1 == result2.Value) //前回と同じ点の場合は変更なし
                        return SamplerStatus.NoChange;
                    P3 = result2.Value;
                    return SamplerStatus.OK;
                default:
                    break;
            }
            return SamplerStatus.OK;
        }

        protected override bool Update()
        {
            switch (_factor)
            {
                case 1:
                    break;
                case 2:
                    _poly.SetPointAt(0, P1.Convert2d());
                    _poly.SetPointAt(1, P2.TransformBy(_ucs).Convert2d());
                    _poly.SetPointAt(2, P3.Convert2d());
                    _poly.SetPointAt(3, P4.TransformBy(_ucs).Convert2d());
                    break;
                default:
                    break;
            }
            return true;
        }
        #endregion

        #region Command
        /// <summary>
        /// EntityJig を使って 矩形ポリライン を作成
        /// </summary>
        [CommandMethod("TestRectangleEntityJig")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;
            var ucs = ed.CurrentUserCoordinateSystem;

            ed.WriteMessage("\n--- EntityJig を使って 矩形ポリライン を作成 ---");

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    using (var poly = new Polyline())
                    {
                        for (int i = 0; i < 4; i++)
                            poly.AddVertexAt(i, Point2d.Origin, 0, 0, 0);
                        poly.Closed = true;
                        var jigger = new RectangleEntityJig(ucs, poly);
                        PromptResult pr;
                        do
                        {
                            pr = ed.Drag(jigger);
                        } while (pr.Status != PromptStatus.Cancel && pr.Status != PromptStatus.Error && jigger._factor++ <= 2);

                        if (pr.Status == PromptStatus.OK)
                        {
                            var cs = tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;
                            cs.AppendEntity(poly);
                            tr.AddNewlyCreatedDBObject(poly, true);
                        }
                    }
                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error TestRectangleEntityJig\n\t" + ex.Message);
                }
            }

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
        #endregion
    }
}
