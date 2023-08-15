using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.SelectAndGetCurve))]
namespace TyAutoCad.Examples
{
    public class SelectAndGetCurve
    {
        /// <summary>
        /// カーブ図形を選択して取得
        /// </summary>
        [CommandMethod("SelectAndGetCurve")]
        public void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- カーブ図形を選択して取得 ---");

            var curves = new List<string>
            {
                "ARC", "CIRCLE", "ELLIPSE", "LEADER", "LINE",
                "LWPOLYLINE", "POLYLINE", "RAY", "SPLINE", "XLINE"
            };

            // カーブ図形を選択 ( GetEntity Method )
            var options = new PromptEntityOptions("\nカーブ図形を選択:");
            PromptEntityResult result;
            do
            {
                result = ed.GetEntity(options);
                if (result.Status != PromptStatus.OK) return;
            } while (!curves.Contains(result.ObjectId.ObjectClass.DxfName));

            ed.WriteMessage("\n選択した図形の DxfName:{0}", result.ObjectId.ObjectClass.DxfName);

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // 現在スペースを読込モードで取得
                    var cs = tr.GetObject(db.CurrentSpaceId, OpenMode.ForRead) as BlockTableRecord;

                    // 選択したカーブ図形を取得
                    var curve = tr.GetObject(result.ObjectId, OpenMode.ForRead) as Curve;

                    // Curve クラスのプロパティを出力
                    ed.WriteMessage("\n\n--- Curve Properties ---");
                    if (curve is Leader || curve is Ray || curve is Xline)
                        ed.WriteMessage("\n\tArea       : ---");
                    else
                        ed.WriteMessage("\n\tArea       : " + curve.Area);
                    ed.WriteMessage("\n\tClosed     : " + curve.Closed);

                    if (curve is Ray || curve is Xline)
                    {
                        ed.WriteMessage("\n\tEndParam   : ---");
                        ed.WriteMessage("\n\tEndPoint   : ---");
                    }
                    else
                    {
                        ed.WriteMessage("\n\tEndParam   : " + curve.EndParam);
                        ed.WriteMessage("\n\tEndPoint   : " + curve.EndPoint);
                    }
                    ed.WriteMessage("\n\tIsPeriodic : " + curve.IsPeriodic);

                    if (curve is Leader || curve is Ray || curve is Xline || curve is Polyline)
                        ed.WriteMessage("\n\tSpline     : ---");
                    else
                        ed.WriteMessage("\n\tSpline     : " + curve.Spline);

                    if (curve is Xline)
                    {
                        ed.WriteMessage("\n\tStartParam : ---");
                        ed.WriteMessage("\n\tStartPoint : ---");
                    }
                    else
                    {
                        ed.WriteMessage("\n\tStartParam : {0}", curve.StartParam);
                        ed.WriteMessage("\n\tStartPoint : {0}", curve.StartPoint);
                    }

                    // 各クラスにキャストしてそのクラスのプロパティを出力
                    if (curve is Arc)
                    {
                        ed.WriteMessage("\n選択した図形は Arc です。");
                        var arc = curve as Arc;
                        ed.WriteMessage("\n\n--- Arc Properties ---");
                        ed.WriteMessage("\n\tCenter     : " + arc.Center);
                        ed.WriteMessage("\n\tEndAngle   : " + arc.EndAngle);
                        ed.WriteMessage("\n\tLength     : " + arc.Length);
                        ed.WriteMessage("\n\tNormal     : " + arc.Normal);
                        ed.WriteMessage("\n\tRadius     : " + arc.Radius);
                        ed.WriteMessage("\n\tStartAngle : " + arc.StartAngle);
                        ed.WriteMessage("\n\tThickness  : " + arc.Thickness);
                        ed.WriteMessage("\n\tTotalAngle : " + arc.TotalAngle);
                    }

                    if (curve is Circle)
                    {
                        ed.WriteMessage("\n選択した図形は Circle です。");
                        var circle = curve as Circle;
                        ed.WriteMessage("\n\n--- Circle Properties ---");
                        ed.WriteMessage("\n\tCenter        : " + circle.Center);
                        ed.WriteMessage("\n\tCircumference : " + circle.Circumference);
                        ed.WriteMessage("\n\tDiameter      : " + circle.Diameter);
                        ed.WriteMessage("\n\tNormal        : " + circle.Normal);
                        ed.WriteMessage("\n\tRadius        : " + circle.Radius);
                        ed.WriteMessage("\n\tThickness     : " + circle.Thickness);
                    }

                    if (curve is Ellipse)
                    {
                        ed.WriteMessage("\n選択した図形は Ellipse です。");
                        var ellipse = curve as Ellipse;
                        ed.WriteMessage("\n\n--- Ellipse Properties ---");
                        ed.WriteMessage("\n\tCenter      : " + ellipse.Center);
                        ed.WriteMessage("\n\tEndAngle    : " + ellipse.EndAngle);
                        ed.WriteMessage("\n\tEndParam    : " + ellipse.EndParam);
                        ed.WriteMessage("\n\tIsNull      : " + ellipse.IsNull);
                        ed.WriteMessage("\n\tMajorAxis   : " + ellipse.MajorAxis);
                        ed.WriteMessage("\n\tMajorRadius : " + ellipse.MajorRadius);
                        ed.WriteMessage("\n\tMinorAxis   : " + ellipse.MinorAxis);
                        ed.WriteMessage("\n\tMinorRadius : " + ellipse.MinorRadius);
                        ed.WriteMessage("\n\tNormal      : " + ellipse.Normal);
                        ed.WriteMessage("\n\tRadiusRatio : " + ellipse.RadiusRatio);
                        ed.WriteMessage("\n\tStartAngle  : " + ellipse.StartAngle);
                        ed.WriteMessage("\n\tStartParam  : " + ellipse.StartParam);
                    }

                    if (curve is Leader)
                    {
                        ed.WriteMessage("\n選択した図形は Leader です。");
                        var leader = curve as Leader;
                        ed.WriteMessage("\n\n--- Leader Properties ---");
                        ed.WriteMessage("\n\tAnnoHeight         : " + leader.AnnoHeight);
                        ed.WriteMessage("\n\tAnnotation         : " + leader.Annotation);
                        ed.WriteMessage("\n\tAnnotationOffset   : " + leader.AnnotationOffset);
                        ed.WriteMessage("\n\tAnnoType           : " + leader.AnnoType);
                        ed.WriteMessage("\n\tAnnoWidth          : " + leader.AnnoWidth);
                        ed.WriteMessage("\n\tDimasz             : " + leader.Dimasz);
                        ed.WriteMessage("\n\tDimclrd            : " + leader.Dimclrd);
                        ed.WriteMessage("\n\tDimensionStyle     : " + leader.DimensionStyle);
                        ed.WriteMessage("\n\tDimensionStyleName : " + leader.DimensionStyleName);
                        ed.WriteMessage("\n\tDimgap             : " + leader.Dimgap);
                        ed.WriteMessage("\n\tDimldrblk          : " + leader.Dimldrblk);
                        ed.WriteMessage("\n\tDimlwd             : " + leader.Dimlwd);
                        ed.WriteMessage("\n\tDimsah             : " + leader.Dimsah);
                        ed.WriteMessage("\n\tDimscale           : " + leader.Dimscale);
                        ed.WriteMessage("\n\tDimtad             : " + leader.Dimtad);
                        ed.WriteMessage("\n\tDimtxt             : " + leader.Dimtxt);
                        ed.WriteMessage("\n\tFirstVertex        : " + leader.FirstVertex);
                        ed.WriteMessage("\n\tHasArrowHead       : " + leader.HasArrowHead);
                        ed.WriteMessage("\n\tHasHookLine        : " + leader.HasHookLine);
                        ed.WriteMessage("\n\tIsSplined          : " + leader.IsSplined);
                        ed.WriteMessage("\n\tLastVertex         : " + leader.LastVertex);
                        ed.WriteMessage("\n\tNormal             : " + leader.Normal);
                        ed.WriteMessage("\n\tNumVertices        : " + leader.NumVertices);
                    }

                    if (curve is Line)
                    {
                        ed.WriteMessage("\n選択した図形は Line です。");
                        var line = curve as Line;
                        ed.WriteMessage("\n\n--- Line Properties ---");
                        ed.WriteMessage("\n\tAngle      : " + line.Angle);
                        ed.WriteMessage("\n\tDelta      : " + line.Delta);
                        ed.WriteMessage("\n\tEndPoint   : " + line.EndPoint);
                        ed.WriteMessage("\n\tLength     : " + line.Length);
                        ed.WriteMessage("\n\tNormal     : " + line.Normal);
                        ed.WriteMessage("\n\tStartPoint : " + line.StartPoint);
                        ed.WriteMessage("\n\tThickness  : " + line.Thickness);
                    }

                    if (curve is Polyline)
                    {
                        ed.WriteMessage("\n選択した図形は Polyline です。");
                        var polyline = curve as Polyline;
                        ed.WriteMessage("\n\n--- Polyline Properties ---");
                        ed.WriteMessage("\n\tClosed           : " + polyline.Closed);
                        ed.WriteMessage("\n\tConstantWidth    : " + polyline.ConstantWidth);
                        ed.WriteMessage("\n\tElevation        : " + polyline.Elevation);
                        ed.WriteMessage("\n\tHasBulges        : " + polyline.HasBulges);
                        ed.WriteMessage("\n\tHasWidth         : " + polyline.HasWidth);
                        ed.WriteMessage("\n\tIsOnlyLines      : " + polyline.IsOnlyLines);
                        ed.WriteMessage("\n\tLength           : " + polyline.Length);
                        ed.WriteMessage("\n\tNormal           : " + polyline.Normal);
                        ed.WriteMessage("\n\tNumberOfVertices : " + polyline.NumberOfVertices);
                        ed.WriteMessage("\n\tPlinegen         : " + polyline.Plinegen);
                        ed.WriteMessage("\n\tThickness        : " + polyline.Thickness);
                    }

                    if (curve is Polyline2d)
                    {
                        ed.WriteMessage("\n選択した図形は Polyline2d です。");
                        var polyline2D = curve as Polyline2d;
                        ed.WriteMessage("\n\n--- Polyline2d Properties ---");
                        ed.WriteMessage("\n\tClosed               : " + polyline2D.Closed);
                        ed.WriteMessage("\n\tConstantWidth        : " + polyline2D.ConstantWidth);
                        ed.WriteMessage("\n\tDefaultEndWidth      : " + polyline2D.DefaultEndWidth);
                        ed.WriteMessage("\n\tDefaultStartWidth    : " + polyline2D.DefaultStartWidth);
                        ed.WriteMessage("\n\tElevation            : " + polyline2D.Elevation);
                        ed.WriteMessage("\n\tLength               : " + polyline2D.Length);
                        ed.WriteMessage("\n\tLinetypeGenerationOn : " + polyline2D.LinetypeGenerationOn);
                        ed.WriteMessage("\n\tNormal               : " + polyline2D.Normal);
                        ed.WriteMessage("\n\tPolyType             : " + polyline2D.PolyType);
                        ed.WriteMessage("\n\tThickness            : " + polyline2D.Thickness);
                    }

                    if (curve is Polyline3d)
                    {
                        ed.WriteMessage("\n選択した図形は Polyline3d です。");
                        var polyline3D = curve as Polyline3d;
                        ed.WriteMessage("\n\n--- Polyline3d Properties ---");
                        ed.WriteMessage("\n\tClosed   : " + polyline3D.Closed);
                        ed.WriteMessage("\n\tLength   : " + polyline3D.Length);
                        ed.WriteMessage("\n\tPolyType : " + polyline3D.PolyType);
                    }

                    if (curve is Ray)
                    {
                        ed.WriteMessage("\n選択した図形は Ray です。");
                        var ray = curve as Ray;
                        ed.WriteMessage("\n\n--- Ray Properties ---");
                        ed.WriteMessage("\n\tBasePoint   : " + ray.BasePoint);
                        ed.WriteMessage("\n\tSecondPoint : " + ray.SecondPoint);
                        ed.WriteMessage("\n\tUnitDir     : " + ray.UnitDir);
                    }

                    if (curve is Spline)
                    {
                        ed.WriteMessage("\n選択した図形は Spline です。");
                        var spline = curve as Spline;
                        ed.WriteMessage("\n\n--- Spline Properties ---");
                        ed.WriteMessage("\n\tDegree           : " + spline.Degree);

                        if (spline.HasFitData)
                        {
                            ed.WriteMessage("\n\tEndFitTangent    : " + spline.EndFitTangent);
                            ed.WriteMessage("\n\tFitData          : " + spline.FitData);
                            ed.WriteMessage("\n\tFitTolerance     : " + spline.FitTolerance);
                        }
                        else
                        {
                            ed.WriteMessage("\n\tEndFitTangent    : ---");
                            ed.WriteMessage("\n\tFitData          : ---");
                            ed.WriteMessage("\n\tFitTolerance     : ---");
                        }

                        //ed.WriteMessage("\n\tEndFitTangent    : " + spline.EndFitTangent);
                        //ed.WriteMessage("\n\tFitData          : " + spline.FitData);
                        //ed.WriteMessage("\n\tFitTolerance     : " + spline.FitTolerance);
                        ed.WriteMessage("\n\tHasFitData       : " + spline.HasFitData);
                        ed.WriteMessage("\n\tIsNull           : " + spline.IsNull);
                        ed.WriteMessage("\n\tIsPlanar         : " + spline.IsPlanar);
                        ed.WriteMessage("\n\tIsRational       : " + spline.IsRational);
                        ed.WriteMessage("\n\tNumControlPoints : " + spline.NumControlPoints);
                        ed.WriteMessage("\n\tNumFitPoints     : " + spline.NumFitPoints);
                        ed.WriteMessage("\n\tNurbsData        : " + spline.NurbsData);

                        if (spline.HasFitData)
                        {
                            ed.WriteMessage("\n\tStartFitTangent  : " + spline.StartFitTangent);
                        }
                        else
                        {
                            ed.WriteMessage("\n\tStartFitTangent  : ---");
                        }
                        //ed.WriteMessage("\n\tStartFitTangent  : " + spline.StartFitTangent);
                        ed.WriteMessage("\n\tType             : " + spline.Type);

                        ed.WriteMessage("\nGetControlPointAt Method");
                        for (int i = 0; i < spline.NumControlPoints; i++)
                        {
                            ed.WriteMessage("\n\tControlPoint " + i + " : " + spline.GetControlPointAt(i));
                        }

                        ed.WriteMessage("\nGetFitPointAt Method");
                        for (int i = 0; i < spline.NumFitPoints; i++)
                        {
                            ed.WriteMessage("\n\tFitPoint " + i + " : " + spline.GetFitPointAt(i));
                        }

                        if (spline is Helix)
                        {
                            var helix = spline as Helix;
                            ed.WriteMessage("\n\nSpline is Helix...");
                            ed.WriteMessage("\n\n--- Helix Properties ---");
                            ed.WriteMessage("\n\tAxisVector  : " + helix.AxisVector);
                            ed.WriteMessage("\n\tBaseRadius  : " + helix.BaseRadius);
                            ed.WriteMessage("\n\tConstrain   : " + helix.Constrain);
                            ed.WriteMessage("\n\tHeight      : " + helix.Height);
                            ed.WriteMessage("\n\tStartPoint  : " + helix.StartPoint);
                            ed.WriteMessage("\n\tTopRadius   : " + helix.TopRadius);
                            ed.WriteMessage("\n\tTotalLength : " + helix.TotalLength);
                            ed.WriteMessage("\n\tTurnHeight  : " + helix.TurnHeight);
                            ed.WriteMessage("\n\tTurns       : " + helix.Turns);
                            ed.WriteMessage("\n\tTurnSlope   : " + helix.TurnSlope);
                            ed.WriteMessage("\n\tTwist       : " + helix.Twist);
                        }
                    }

                    if (curve is Xline)
                    {
                        ed.WriteMessage("\n選択した図形は Xline です。");
                        var xline = curve as Xline;
                        ed.WriteMessage("\n\n--- Xline Properties ---");
                        ed.WriteMessage("\n\tBasePoint   : " + xline.BasePoint);
                        ed.WriteMessage("\n\tSecondPoint : " + xline.SecondPoint);
                        ed.WriteMessage("\n\tUnitDir     : " + xline.UnitDir);
                    }
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error SelectAndGetCurve\n\t" + ex.Message);
                }
            }
            ed.WriteMessage("\n--- コマンド終了 ---");
        }
    }
}
