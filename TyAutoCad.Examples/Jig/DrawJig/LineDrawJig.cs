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

[assembly: CommandClass(typeof(TyAutoCad.Examples.LineDrawJig))]
namespace TyAutoCad.Examples
{
    /// <summary>
    /// Line DrawJig
    /// </summary>
    public class LineDrawJig : DrawJig
    {
        #region Fields
        Point3d _start, _end;
        #endregion

        #region Constructors
        public LineDrawJig(Point3d start)
        {
            _start = start;
        }
        #endregion

        #region Properties
        public Matrix3d Ucs => Application.DocumentManager.MdiActiveDocument.Editor.CurrentUserCoordinateSystem;
        #endregion

        #region Overrides
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            // 2点目を取得
            var options = new JigPromptPointOptions("\n終点を指示: ");
            options.UserInputControls = UserInputControls.Accept3dCoordinates;
            options.UseBasePoint = true;
            options.BasePoint = _start;
            PromptPointResult result = prompts.AcquirePoint(options);

            if (result.Status == PromptStatus.Cancel || result.Status == PromptStatus.Error)
                return SamplerStatus.Cancel;

            if (result.Value.Equals(_end)) 
                return SamplerStatus.NoChange;

            _end = result.Value;

            return SamplerStatus.OK;
        }

        protected override bool WorldDraw(WorldDraw draw)
        {
            WorldGeometry geo = draw.Geometry;
            if (geo != null)
            {
                geo.PushModelTransform(Ucs);
                draw.SubEntityTraits.Color = 251;
                geo.WorldLine(_start, _end);
                geo.PopModelTransform();
            }
            return true;
        }
        #endregion

        #region Command
        /// <summary>
        /// DrawJigを使って2点指示線分を作成
        /// </summary>
        [CommandMethod("LineDrawJig")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- DrawJigを使って2点指示線分を作成 ---");

            // 1点目を取得
            var options = new PromptPointOptions("\n始点を指示:");
            options.AllowNone = false;
            PromptPointResult ppr = ed.GetPoint(options);
            if (ppr.Status != PromptStatus.OK) return;
            Point3d start = ppr.Value;

            // ジグに点を渡す
            var jigger = new LineDrawJig(start);

            // ジグ操作開始
            PromptResult pr = ed.Drag(jigger);
            if (pr.Status != PromptStatus.OK) return;

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // 現在スペースを書込モードで取得
                    var cs = tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;

                    // 線分を作成
                    using (var line = new Line(jigger._start, jigger._end))
                    {
                        // モデルスペースに追加
                        cs.AppendEntity(line);
                        tr.AddNewlyCreatedDBObject(line, true);
                    }
                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error LineDrawJig\n\t" + ex.Message);
                }
            }
            ed.WriteMessage("\n--- コマンド終了 ---");
        }
        #endregion
    }
}
