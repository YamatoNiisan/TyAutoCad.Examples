﻿using Autodesk.AutoCAD.ApplicationServices;
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

[assembly: CommandClass(typeof(TyAutoCad.Examples.SquareDrawJig))]
namespace TyAutoCad.Examples
{
    public class SquareDrawJig : DrawJig
    {
        #region Fields
        private Point3d _tempPoint;
        private Polyline _poly = null;
        #endregion

        #region Constructors
        public SquareDrawJig(Polyline poly)
        {
            _poly = poly;
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

        #region Command
        /// <summary>
        /// DrawJig を使って四角形を作成
        /// </summary>
        [CommandMethod("SquareDrawJig")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- DrawJig を使って四角形を作成 ---");

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

                    // ポリラインをJigに渡す
                    var jigger = new SquareDrawJig(poly);

                    PromptResult result = null;
                    while (poly.NumberOfVertices < 5)
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
                                poly.SetPointAt(poly.NumberOfVertices - 1, jigger._tempPoint.Convert2d());
                                jigger.AddDummyVertex();
                                break;
                            case PromptStatus.Keyword:
                                if (result.StringResult == "Undo")
                                    jigger.RemoveLastVertex();
                                break;
                            case PromptStatus.None:
                                if (poly.NumberOfVertices < 4)
                                {
                                    poly.SetPointAt(poly.NumberOfVertices - 1, jigger._tempPoint.Convert2d());
                                    jigger.AddDummyVertex();
                                    break;
                                }
                                if (poly.NumberOfVertices == 4 && !poly.IsSimplePolygon())
                                {
                                    Application.ShowAlertDialog("\n交差しています。");
                                    break;
                                }
                                ed.WriteMessage("\n--- コマンド終了 ---");
                                tr.Commit();
                                return;
                            default:
                                ed.WriteMessage("\n--- コマンドキャンセル ---");
                                return;
                        }
                    }
                    if (result.Status == PromptStatus.OK)
                    {
                        jigger.RemoveLastVertex();
                        tr.Commit();
                    }

                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error SquareDrawJig\n\t" + ex.Message);
                }
            }

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
        #endregion
    }
}
