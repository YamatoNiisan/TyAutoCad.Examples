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

[assembly: CommandClass(typeof(TyAutoCad.Examples.CircleEntityJig))]
namespace TyAutoCad.Examples
{
    /// <summary>
    /// Circle EntityJig
    /// </summary>
    public class CircleEntityJig : EntityJig
    {
        #region Fields
        private Circle _circle;
        private double _radius;
        #endregion

        #region Constructors
        public CircleEntityJig(Circle circle) : base(circle)
        {
            _circle = circle;
            _radius = circle.Radius;
        }
        #endregion

        #region Overrides
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            var options = new JigPromptDistanceOptions("\n円周上の点を指示: ");
            options.BasePoint = _circle.Center;
            options.UseBasePoint = true;
            var result = prompts.AcquireDistance(options);
            if (result.Value.Equals(_radius))
                return SamplerStatus.NoChange;
            if (result.Value < 0.0001)
                return SamplerStatus.NoChange;
            
            _radius = result.Value;
            return SamplerStatus.OK;
        }

        protected override bool Update()
        {
            _circle.Radius = _radius;
            return true;
        }
        #endregion

        #region Command
        /// <summary>
        /// EntityJig を使って Circle を作成
        /// </summary>
        [CommandMethod("CircleEntityJig")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- EntityJig を使って Circle を作成 ---");

            var result = ed.GetPoint("\n円の中心を指示: ");
            if (result.Status != PromptStatus.OK)
                return;
            var centerPoint = result.Value;

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    using (var circle = new Circle(Point3d.Origin, Vector3d.ZAxis, 1))
                    {
                        circle.Center = centerPoint;
                        var jigger = new CircleEntityJig(circle);
                        var pr = ed.Drag(jigger);
                        if (pr.Status == PromptStatus.OK)
                        {
                            var cs = tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;
                            cs.AppendEntity(circle);
                            tr.AddNewlyCreatedDBObject(circle, true);
                        }
                    }
                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error CircleEntityJig\n\t" + ex.Message);
                }
            }

            ed.WriteMessage("\n--- コマンド終了 ---");
        }
        #endregion
    }
}
