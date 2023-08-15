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

[assembly: CommandClass(typeof(TyAutoCad.Examples.LineEntityJig))]
namespace TyAutoCad.Examples
{
    /// <summary>
    /// Line EntityJig
    /// </summary>
    public class LineEntityJig : EntityJig
    {
        #region Fields
        private Line _line;
        private Point3d _endPoint;
        #endregion

        #region Constructors
        public LineEntityJig(Line line) : base(line)
        {
            _line = line;
        }
        #endregion

        #region Overrides
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            var options = new JigPromptPointOptions("\n終点を指示: ");
            options.BasePoint = _line.StartPoint;
            options.UseBasePoint = true;
            options.UserInputControls = UserInputControls.Accept3dCoordinates;
            var result = prompts.AcquirePoint(options);
            if (result.Value.IsEqualTo(_endPoint))
                return SamplerStatus.NoChange;
            _endPoint = result.Value;
            return SamplerStatus.OK;
        }

        protected override bool Update()
        {
            _line.EndPoint = _endPoint;
            return true;
        }
        #endregion

        #region Command
        /// <summary>
        /// EntityJig を使って Line を作成
        /// </summary>
        [CommandMethod("LineEntityJig")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- EntityJig を使って Line を作成 ---");

            var result = ed.GetPoint("\n始点を指示: ");
            if (result.Status != PromptStatus.OK)
                return;
            var startPoint = result.Value.TransformBy(ed.CurrentUserCoordinateSystem);

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    using (var line = new Line(startPoint, startPoint + Vector3d.XAxis))
                    {
                        var jigger = new LineEntityJig(line);
                        var pr = ed.Drag(jigger);
                        if (pr.Status == PromptStatus.OK)
                        {
                            var cs = tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;
                            cs.AppendEntity(line);
                            tr.AddNewlyCreatedDBObject(line, true);
                        }
                    }
                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error LineEntityJig\n\t" + ex.Message);
                }
            }

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
        #endregion
    }
}
