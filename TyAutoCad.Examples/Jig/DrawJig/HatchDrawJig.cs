using Autodesk.AutoCAD.ApplicationServices;
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

[assembly: CommandClass(typeof(TyAutoCad.Examples.HatchDrawJig))]
namespace TyAutoCad.Examples
{
    /// <summary>
    /// Hatch DrawJig
    /// </summary>
    public class HatchDrawJig : DrawJig
    {
        #region Fields
        private Point3d _tempPoint;
        private Polyline _poly = null;
        private Hatch _hatch = null;
        #endregion

        #region Constructors
        public HatchDrawJig(Polyline poly, Hatch hatch)
        {
            _poly = poly;
            _hatch = hatch;
            AddDummyVertex();
        }
        #endregion

        #region Methods
        /// <summary>
        /// ダミーの頂点を追加する
        /// </summary>
        private void AddDummyVertex()
        {
            _poly.AddVertexAt(_poly.NumberOfVertices, new Point2d(0, 0), 0, 0, 0);
        }

        /// <summary>
        /// 最後の頂点を除去する
        /// </summary>
        private void RemoveLastVertex()
        {
            if (_poly.NumberOfVertices > 0)
                _poly.RemoveVertexAt(_poly.NumberOfVertices - 1);
            HatchAppendLoop();
        }

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

            options.UserInputControls = UserInputControls.NullResponseAccepted;
            options.Message = "\n頂点を指示: ";

            PromptPointResult result = prompts.AcquirePoint(options);
            if (_tempPoint == result.Value)
                return SamplerStatus.NoChange;

            _tempPoint = result.Value;
            return SamplerStatus.OK;
        }

        protected override bool WorldDraw(WorldDraw draw)
        {
            if (_poly.NumberOfVertices > 1)
                _poly.SetBulgeAt(_poly.NumberOfVertices - 2, 0);

            _poly.SetPointAt(_poly.NumberOfVertices - 1, _tempPoint.Convert2d());

            if (_poly.NumberOfVertices > 2)
            {
                _poly.Closed = true;
                HatchAppendLoop();
            }

            if (!draw.RegenAbort)
            {
                draw.Geometry.Draw(_poly);
                if (_poly.NumberOfVertices > 2)
                {
                    _hatch.EvaluateHatch(true);
                    if (!draw.RegenAbort)
                        draw.Geometry.Draw(_hatch);
                    _hatch.RemoveLoopAt(0);
                }
            }
            return true;
        }
        #endregion

        #region Command
        /// <summary>
        /// DrawJig を使ってハッチングを作成
        /// </summary>
        [CommandMethod("HatchDrawJig")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- DrawJig を使ってハッチングを作成 ---");

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // 現在スペースを書込モードで取得
                    var cs = tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;

                    // ポリラインを作成してデータベースに追加
                    var poly = new Polyline();
                    poly.Normal = Vector3d.ZAxis.TransformBy(ed.CurrentUserCoordinateSystem);
                    cs.AppendEntity(poly);
                    tr.AddNewlyCreatedDBObject(poly, true);

                    // ハッチングを作成してデータベースに追加
                    var hatch = new Hatch();
                    hatch.SetHatchPattern(HatchPatternType.PreDefined, "ANSI37");
                    cs.AppendEntity(hatch);
                    tr.AddNewlyCreatedDBObject(hatch, true);

                    // ジグにポリラインとハッチングを渡す
                    var jigger = new HatchDrawJig(poly, hatch);

                    PromptResult result;
                    do
                    {
                        result = ed.Drag(jigger);
                        switch (result.Status)
                        {
                            case PromptStatus.OK:
                                jigger.AddDummyVertex();
                                break;
                            case PromptStatus.None:
                                jigger.RemoveLastVertex();
                                tr.Commit();
                                return;
                             default:
                                return;
                        }
                    } while (result.Status == PromptStatus.OK);
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error Command\n\t" + ex.Message);
                }
            }
            ed.WriteMessage("\n--- コマンド終了 ---");
        }
        #endregion
    }
}
