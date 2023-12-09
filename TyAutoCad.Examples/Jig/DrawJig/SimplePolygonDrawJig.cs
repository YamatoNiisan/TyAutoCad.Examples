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

[assembly: CommandClass(typeof(TyAutoCad.Examples.SimplePolygonDrawJig))]
namespace TyAutoCad.Examples
{
    /// <summary>
    /// 単純な多角形 DrawJig
    /// </summary>
    public class SimplePolygonDrawJig : DrawJig
    {
        #region Fields
        private Point3d _tempPoint;
        private Polyline _poly = null;
        #endregion

        #region Constructors
        public SimplePolygonDrawJig(Polyline poly)
        {
            _poly = poly;
            AddDummyVertex();
        }

        public SimplePolygonDrawJig(Polyline poly, Point3d basePoint)
        {
            _poly = poly;
            _poly.AddVertexAt(_poly.NumberOfVertices, basePoint.Convert2d(), 0, 0, 0);
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
        }
        #endregion

        #region Overrides
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            var options = new JigPromptPointOptions();
            options.UserInputControls = UserInputControls.NullResponseAccepted;
            options.Message = "\n始点を指示: ";
            options.UseBasePoint = false;

            if (_poly.NumberOfVertices > 1)
            {
                options.Message = "\n頂点を指示: ";
                options.UseBasePoint = true;
                options.BasePoint = _poly.GetPoint3dAt(_poly.NumberOfVertices - 2);
            }

            // キーワードを追加
            if (_poly.NumberOfVertices > 2)
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
            if (_poly.NumberOfVertices > 1)
                _poly.SetBulgeAt(_poly.NumberOfVertices - 2, 0);

            _poly.SetPointAt(_poly.NumberOfVertices - 1, _tempPoint.Convert2d());

            if (_poly.NumberOfVertices > 2)
                _poly.Closed = true;
            if (!draw.RegenAbort)
                draw.Geometry.Draw(_poly);
            return true;
        }
        #endregion

        #region Method
        /// <summary>
        /// DrawJig を使って単純な多角形を作成
        /// </summary>
        public static void Execute(Point3d? basePoint = null)
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- DrawJig を使って単純な多角形を作成 ---");

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
                    poly.Transparency = new Transparency(100);
                    cs.AppendEntity(poly);
                    tr.AddNewlyCreatedDBObject(poly, true);

                    // ポリラインをJigに渡す
                    SimplePolygonDrawJig jigger = null;
                    if (basePoint == null)
                    {
                        jigger = new SimplePolygonDrawJig(poly);
                    }
                    else
                    {
                        jigger = new SimplePolygonDrawJig(poly, (Point3d)basePoint);
                    }

                    PromptResult result;
                    do
                    {
                        result = ed.Drag(jigger);
                        switch (result.Status)
                        {
                            case PromptStatus.OK:
                                if (poly.NumberOfVertices > 2)
                                {
                                    if (!poly.IsSimplePolygon())
                                    {
                                        Application.ShowAlertDialog("\n交差しています。");
                                        break;
                                    }
                                }
                                jigger.AddDummyVertex();
                                break;
                            case PromptStatus.Keyword:
                                if (result.StringResult == "Undo")
                                    jigger.RemoveLastVertex();
                                break;
                            case PromptStatus.None:
                                jigger.RemoveLastVertex();
                                poly.Transparency = new Transparency();
                                tr.Commit();
                                return;
                            default:
                                break;
                        }
                    } while (result.Status == PromptStatus.OK || result.Status == PromptStatus.Keyword);
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error SimplePolygonDrawJig\n\t" + ex.Message);
                }
            }
            ed.WriteMessage("\n--- コマンド終了 ---");
        }
        #endregion

        #region Command
        /// <summary>
        /// DrawJig を使って単純な多角形を作成
        /// </summary>
        [CommandMethod("SimplePolygonDrawJig")]
        public static void Command()
        {
            SimplePolygonDrawJig.Execute();
        }
        #endregion
    }
}
