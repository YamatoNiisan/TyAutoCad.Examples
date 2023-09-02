using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;
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

[assembly: CommandClass(typeof(TyAutoCad.Examples.CircleDrawJig))]
namespace TyAutoCad.Examples
{
    /// <summary>
    /// 円 DrawJig
    /// </summary>
    public class CircleDrawJig : DrawJig
    {
        #region Fields
        protected int _factor = 1;
        private Point3d _center;
        private Point3d _second;
        #endregion

        #region Properties
        public Matrix3d Ucs => AcAp.DocumentManager.MdiActiveDocument.Editor.CurrentUserCoordinateSystem;
        #endregion

        #region Overrides
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            PromptPointResult result;
            JigPromptPointOptions options;
            switch (_factor)
            {
                case 1:
                    options = new JigPromptPointOptions("\n円の中心を指示");
                    result = prompts.AcquirePoint(options);
                    if (result.Status == PromptStatus.Cancel) return SamplerStatus.Cancel;
                    _center = _second = result.Value;
                    break;
                case 2:
                    options = new JigPromptPointOptions("\n円周上の点を指示:")
                    {
                        UseBasePoint = true,
                        BasePoint = _center,
                    };
                    result = prompts.AcquirePoint(options);

                    if (result.Status == PromptStatus.Cancel) return SamplerStatus.Cancel;
                    if (result.Value == _center) return SamplerStatus.NoChange;
                    _second = result.Value;
                    break;

                default:
                    break;
            }

            return SamplerStatus.OK;
        }

        protected override bool WorldDraw(WorldDraw draw)
        {
            if (_factor == 2)
            {
                WorldGeometry geometry = draw.Geometry;
                if (geometry != null)
                {
                    geometry.PushModelTransform(Ucs);
                    geometry.Circle(_center, _center.DistanceTo(_second), Vector3d.ZAxis);
                    geometry.PopModelTransform();
                }
            }
            return true;
        }
        #endregion

        #region Command
        /// <summary>
        /// DrawJig を使って円を作図
        /// </summary>
        [CommandMethod("CircleDrawJig")]
        public static void Command()
        {

            // Document, Editor, Database を取得
            var doc = AcAp.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- DrawJig を使って円を作図 ---");

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    var jigger = new CircleDrawJig();

                    PromptResult result;
                    do
                    {
                        result = ed.Drag(jigger);
                    } while (result.Status == PromptStatus.OK && jigger._factor++ < 3);

                    if (result.Status != PromptStatus.OK) return;

                    using (var circle = new Circle(jigger._center, Vector3d.ZAxis, jigger._center.DistanceTo(jigger._second)))
                    {
                        // 現在スペースを書込モードで取得
                        var cs = tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;
                        cs.AppendEntity(circle);
                        tr.AddNewlyCreatedDBObject(circle, true);
                    }
                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    AcAp.ShowAlertDialog("Error CircleDrawJig\n\t" + ex.Message);
                }
            }
            ed.WriteMessage("\n--- コマンド終了 ---");
        }
        #endregion
    }
}
