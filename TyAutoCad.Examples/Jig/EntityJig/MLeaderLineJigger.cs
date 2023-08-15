using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// マルチ引出線の引出線を操作するJig
    /// </summary>
    public class MLeaderLineJigger : EntityJig
    {
        #region Fields
        private int _index;
        private MLeaderLine.ModifyMode _mode;
        private bool _angleCorrection;

        public Point3d _movePoint;
        public Point3d _fixedPoint;

        private int _flipFlag = 1; // 反転フラグ
        private double _px;
        private double _doglegLength;
        private bool _doglegFree = false;

        private Xline _xline;

        private Line3d _line0 = new Line3d();   // 水平 の Line3d
        private Line3d _line60 = new Line3d();  // 60° の Line3d
        private Line3d _line120 = new Line3d(); // 120° の Line3d

        #endregion

        #region Constructors
        public MLeaderLineJigger(MLeader mleader, int index, MLeaderLine.ModifyMode mode, bool angleCorrection) : base(mleader)
        {
            _index = index;
            _mode = mode;

            // 角度補正機能の有無
            _angleCorrection = angleCorrection;

            // 移動点と固定点をセット
            if (_mode == MLeaderLine.ModifyMode.ArrowHeadMove || _mode == MLeaderLine.ModifyMode.ChangeLength)
            {
                _movePoint = mleader.GetFirstVertex(_index);
                _fixedPoint = mleader.GetLastVertex(_index);
            }
            else
            {
                _movePoint = mleader.GetLastVertex(_index);
                _fixedPoint = mleader.GetFirstVertex(_index);
            }

            // 長さ変更用の基準線を準備
            if (_mode == MLeaderLine.ModifyMode.ChangeLength)
            {
                _xline = new Xline
                {
                    BasePoint = mleader.GetLastVertex(_index),
                    SecondPoint = mleader.GetFirstVertex(_index)
                };
            }

            // 角度補正用基準線を準備
            if (_angleCorrection)
            {
                // 60° の Line3d を作成
                _line60.Set(_fixedPoint, _fixedPoint.Add(new Vector3d(1, Math.Sqrt(3), 0)));
                // 120° の Line3d を作成
                _line120.Set(_fixedPoint, _fixedPoint.Add(new Vector3d(-1, Math.Sqrt(3), 0)));
            }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Entityプロパティをオーバーロード
        /// </summary>
        public new MLeader Entity => base.Entity as MLeader;


        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            var jppo = new JigPromptPointOptions
            {
                Message = "\n矢印の先の点を指示 :",
                UseBasePoint = true,
                BasePoint = _fixedPoint,
                UserInputControls = UserInputControls.Accept3dCoordinates | UserInputControls.GovernedByOrthoMode
                        | UserInputControls.GovernedByUCSDetect | UserInputControls.UseBasePointElevation,
            };

            // キーワードを追加
            if (_mode == MLeaderLine.ModifyMode.ArrowTailMove)
            {
                jppo.Keywords.Add("Flip", "F", "F:Flip");
                if (_angleCorrection)
                {
                    jppo.Keywords.Add("Stretch", "S", "S:dogleg Stretch");
                    jppo.Keywords.Add("Restore", "R", "R:dogleg Restore");
                }
            }

            // カーソルの座標を取得
            PromptPointResult ppr = prompts.AcquirePoint(jppo);

            // キーワードが入力されたら
            if (ppr.Status == PromptStatus.Keyword)
            {
                switch (ppr.StringResult)
                {
                    case "Flip":
                        _flipFlag *= -1;
                        return SamplerStatus.OK;
                    case "Stretch":
                        _doglegFree = !_doglegFree; // _doglegFree を反転
                        return SamplerStatus.OK;
                    case "Restore":
                        _doglegLength = 0.5;  // doglegLength をもとに戻す
                        return SamplerStatus.OK;
                    default:
                        break;
                }
            }

            if (ppr.Status == PromptStatus.Cancel)
            {
                return SamplerStatus.Cancel;
            }

            if (ppr.Value.Equals(_movePoint))
            {
                return SamplerStatus.NoChange;
            }

            // 動点を更新
            _movePoint = ppr.Value;

            return SamplerStatus.OK;
        }

        protected override bool Update()
        {
            // カーソルの X座標を保存
            _px = _movePoint.X;

            // 角度補正機能
            if (_angleCorrection)
            {
                // 移動点に水平な Line3d を作成
                _line0.Set(_movePoint, Vector3d.XAxis);

                Point3d[] points;

                if (_line60.GetDistanceTo(_movePoint) <= _line120.GetDistanceTo(_movePoint))
                {
                    points = _line60.IntersectWith(_line0);
                }
                else
                {
                    points = _line120.IntersectWith(_line0);
                }

                if (points.Length > 0)
                {
                    _movePoint = points[0];
                }
            }

            if (_mode == MLeaderLine.ModifyMode.ArrowHeadMove)
            {
                Entity.SetFirstVertex(_index, _movePoint);
            }
            else if (_mode == MLeaderLine.ModifyMode.ArrowTailMove)
            {
                Entity.SetLastVertex(_index, _movePoint);
                // 参照線の方向を更新
                var direction = new Vector3d((_movePoint.X >= _fixedPoint.X ? 1 * _flipFlag : -1 * _flipFlag), 0, 0);
                Entity.SetDogleg(_index, direction);
            }
            else
            {
                _movePoint = _xline.GetClosestPointTo(_movePoint, true);
                Entity.SetFirstVertex(_index, _movePoint);
            }

            // dogleg の長さ変更
            if (_doglegFree)
            {
                _doglegLength = Math.Abs(Entity.GetLastVertex(0).X - _px) / Entity.Scale;
            }

            if (_doglegLength >= 0.5)
            {
                Entity.DoglegLength = _doglegLength;
            }
            else
            {
                _doglegLength = 0.5;
            }
            return true;
        }
        #endregion

        #region Static method
        public static bool Jig(MLeader mleader, int index, MLeaderLine.ModifyMode mode, bool angleCorrection)
        {
            MLeaderLineJigger jig = null;
            try
            {
                jig = new MLeaderLineJigger(mleader, index, mode, angleCorrection);
                PromptResult pr;
                do
                {
                    pr = Application.DocumentManager.MdiActiveDocument.Editor.Drag(jig);
                } while (pr.Status == PromptStatus.Keyword);

                if (pr.Status == PromptStatus.Cancel || pr.Status == PromptStatus.Error)
                {
                    if (jig != null && jig.Entity != null)
                    {
                        jig.Entity.Dispose();
                    }
                    return false;
                }
                return true;
            }
            catch
            {
                if (jig != null && jig.Entity != null)
                {
                    jig.Entity.Dispose();
                }
                return false;
            }
        }
        #endregion
    }
}
