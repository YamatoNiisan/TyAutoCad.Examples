using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Geometry;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// Line DrawJig Create Entity First
    /// </summary>
    internal class LineDrawJigCreateEntityFirst : DrawJig
    {
        #region Fields
        private Line _line = null;
        private bool _hasFirstPoint = false;
        #endregion

        #region Constructors
        public LineDrawJigCreateEntityFirst(Line line)
        {
            _line = line;
        }
        #endregion

        #region Properties
        public Point3d P1 { get; set; }

        public Point3d P2 { get; set; }

        #endregion

        #region Overrides
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            var options = new JigPromptPointOptions();

            options.UserInputControls = UserInputControls.NullResponseAccepted;
            if (_hasFirstPoint)
            {
                options.Message = "\n終点を指示: ";
                options.UseBasePoint = true;
                options.BasePoint = P1;
                options.Cursor = CursorType.RubberBand;
            }
            else
                options.Message = "\n始点を指示: ";

            PromptPointResult result = prompts.AcquirePoint(options);
            if (!_hasFirstPoint)
                P1 = P2 = result.Value;
            else if (P1 == result.Value) //前回と同じ点の場合は変更なし
                return SamplerStatus.NoChange;
            else
                P2 = result.Value;

            return SamplerStatus.OK;
        }

        protected override bool WorldDraw(WorldDraw draw)
        {
            if (_hasFirstPoint)
            {
                _line.EndPoint = P2;
                if (!draw.RegenAbort)
                {
                    draw.Geometry.Draw(_line);
                }
            }
            return true;
        }
        #endregion

        #region Command
        /// <summary>
        /// DrawJigを使って2点指示線分を作成
        /// 先に線分図形ををデータベースに追加する
        /// </summary>
        [CommandMethod("LineDrawJigCreateEntityFirst")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = AcAp.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- DrawJig を使って線分を作成 ---");
            ed.WriteMessage("\n\t先に線分図形ををデータベースに追加する");

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // 現在スペースを書込モードで取得
                    var cs = tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;

                    // 線分を作成
                    using (var line = new Line())
                    {
                        // データベースに追加
                        cs.AppendEntity(line);
                        tr.AddNewlyCreatedDBObject(line, true);

                        // ジグに線分を渡す
                        var jigger = new LineDrawJigCreateEntityFirst(line);

                        PromptResult result;
                        do
                        {
                            // ジグ操作開始
                            result = ed.Drag(jigger);

                            if (result.Status == PromptStatus.OK)
                            {
                                if (!jigger._hasFirstPoint)
                                {
                                    jigger._line.StartPoint = jigger.P1;
                                    jigger._hasFirstPoint = true;
                                    continue;
                                }
                                jigger._line.EndPoint = jigger.P2;

                                // コミットしてループから抜ける
                                tr.Commit();
                                break;
                            }

                        } while (result.Status == PromptStatus.OK);
                    }
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    AcAp.ShowAlertDialog("Error LineDrawJigCreateEntityFirst\n\t" + ex.Message);
                }
            }
            ed.WriteMessage("\n--- コマンド終了 ---");
        }
        #endregion
    }
}
