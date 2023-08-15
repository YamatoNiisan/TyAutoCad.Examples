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

[assembly: CommandClass(typeof(TyAutoCad.Examples.ThreePointsArcEntityJig))]
namespace TyAutoCad.Examples
{
    /// <summary>
    /// 3点指示円弧 EntityJig
    /// </summary>
    public class ThreePointsArcEntityJig : EntityJig
    {
        #region Fields
        private Point3d _first;
        private Point3d _second;
        private Point3d _third;
        #endregion

        #region Constructors
        public ThreePointsArcEntityJig(Point3d first, Point3d second) : base(new Arc())
        {
            _first = first.TransformBy(Ucs);
            _second = second.TransformBy(Ucs);
        }
        #endregion

        #region Properties
        public Matrix3d Ucs => Application.DocumentManager.MdiActiveDocument.Editor.CurrentUserCoordinateSystem;
        #endregion

        #region Overrides
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            var options = new JigPromptPointOptions("\n3点目を指示: ");
            options.UserInputControls = UserInputControls.Accept3dCoordinates;
            options.UseBasePoint = true;
            options.BasePoint = _second;

            PromptPointResult result = prompts.AcquirePoint(options);
            // 同じ点でないか確認
            if (_third.DistanceTo(result.Value) < Tolerance.Global.EqualPoint)
                return SamplerStatus.NoChange;

            // 3点目を設定
            _third = result.Value;
            return SamplerStatus.OK;
        }

        protected override bool Update()
        {
            // CircularArc3dクラスを使って3点円を作成して、Base Entity に反映する
            var tempArc = new CircularArc3d(_first, _third, _second);

            var arc = Entity as Arc;
            arc.Center = tempArc.Center;
            arc.Radius = tempArc.Radius;
            if (arc.Center.IsClockwise(_first, _second))
            {
                arc.StartAngle = arc.Center.GetAngle(_second);
                arc.EndAngle = arc.Center.GetAngle(_first);
            }
            else
            {
                arc.StartAngle = arc.Center.GetAngle(_first);
                arc.EndAngle = arc.Center.GetAngle(_second);
            }
            return true;
        }
        #endregion

        #region Command
        /// <summary>
        /// EntityJig を使って 3点指示円弧 を作成
        /// </summary>
        [CommandMethod("ThreePointsArcEntityJig")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- EntityJig を使って 3点指示円弧 を作成 ---");

            // 1点目を取得
            var options = new PromptPointOptions("\n1点目を指示:");
            options.AllowNone = false;
            PromptPointResult result = ed.GetPoint(options);
            if (result.Status != PromptStatus.OK) return;
            Point3d first = result.Value;

            // 2点目を取得
            options.Message = "\n2点目を指示:";
            options.UseBasePoint = true;
            options.BasePoint = first;

            result = ed.GetPoint(options);
            if (result.Status != PromptStatus.OK) return;
            Point3d second = result.Value;

            // ジグに2点を渡す
            var jigger = new ThreePointsArcEntityJig(first, second);

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

                    Entity ent = jigger.Entity;
                    cs.AppendEntity(ent);
                    tr.AddNewlyCreatedDBObject(ent, true);
                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error ThreePointsArcEntityJig\n\t" + ex.Message);
                }
            }
            ed.WriteMessage("\n--- コマンド終了 ---");
        }
        #endregion
    }
}
