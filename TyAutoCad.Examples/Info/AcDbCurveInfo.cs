using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TyAutoCad.Examples
{
    /// <summary>
    /// Curve クラス情報
    /// </summary>
    public static class AcDbCurveInfo
    {
        /// <summary>
        /// Curve のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="curve"></param>
        public static void OutputCurveProperties(Editor ed, Curve curve)
        {
            ed.WriteMessage("\n\n--- Curve Properties ---");
            if (curve is Leader || curve is Ray || curve is Xline)
            {
                ed.WriteMessage("\n\tArea       : ---");
            }
            else
            {
                ed.WriteMessage("\n\tArea       : " + curve.Area);
            }
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
            {
                ed.WriteMessage("\n\tSpline     : ---");
            }
            else
            {
                ed.WriteMessage("\n\tSpline     : " + curve.Spline);
            }

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
        }

        /// <summary>
        /// Arc のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="arc"></param>
        public static void OutputArcProperties(Editor ed, Arc arc)
        {
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

        /// <summary>
        /// Circle のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="circle"></param>
        public static void OutputCircleProperties(Editor ed, Circle circle)
        {
            ed.WriteMessage("\n\n--- Circle Properties ---");
            ed.WriteMessage("\n\tCenter        : " + circle.Center);
            ed.WriteMessage("\n\tCircumference : " + circle.Circumference);
            ed.WriteMessage("\n\tDiameter      : " + circle.Diameter);
            ed.WriteMessage("\n\tNormal        : " + circle.Normal);
            ed.WriteMessage("\n\tRadius        : " + circle.Radius);
            ed.WriteMessage("\n\tThickness     : " + circle.Thickness);
        }

        /// <summary>
        /// Ellipse のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="ellipse"></param>
        public static void OutputEllipseProperties(Editor ed, Ellipse ellipse)
        {
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

        /// <summary>
        /// Leader のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="leader"></param>
        public static void OutputLeaderProperties(Editor ed, Leader leader)
        {
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

        /// <summary>
        /// Line のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="line"></param>
        public static void OutputLineProperties(Editor ed, Line line)
        {
            ed.WriteMessage("\n\n--- Line Properties ---");
            ed.WriteMessage("\n\tAngle      : " + line.Angle);
            ed.WriteMessage("\n\tDelta      : " + line.Delta);
            ed.WriteMessage("\n\tEndPoint   : " + line.EndPoint);
            ed.WriteMessage("\n\tLength     : " + line.Length);
            ed.WriteMessage("\n\tNormal     : " + line.Normal);
            ed.WriteMessage("\n\tStartPoint : " + line.StartPoint);
            ed.WriteMessage("\n\tThickness  : " + line.Thickness);
        }

        /// <summary>
        /// Polyline のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="polyline"></param>
        public static void OutputPolylineProperties(Editor ed, Polyline polyline)
        {
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

        /// <summary>
        /// Polyline2d のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="polyline2D"></param>
        public static void OutputPolyline2dProperties(Editor ed, Polyline2d polyline2D)
        {
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

        /// <summary>
        /// Polyline3d のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="polyline3D"></param>
        public static void OutputPolyline3dProperties(Editor ed, Polyline3d polyline3D)
        {
            ed.WriteMessage("\n\n--- Polyline3d Properties ---");
            ed.WriteMessage("\n\tClosed   : " + polyline3D.Closed);
            ed.WriteMessage("\n\tLength   : " + polyline3D.Length);
            ed.WriteMessage("\n\tPolyType : " + polyline3D.PolyType);
        }

        /// <summary>
        /// Ray のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="ray"></param>
        public static void OutputRayProperties(Editor ed, Ray ray)
        {
            ed.WriteMessage("\n\n--- Ray Properties ---");
            ed.WriteMessage("\n\tBasePoint   : " + ray.BasePoint);
            ed.WriteMessage("\n\tSecondPoint : " + ray.SecondPoint);
            ed.WriteMessage("\n\tUnitDir     : " + ray.UnitDir);
        }

        /// <summary>
        /// Spline のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="spline"></param>
        public static void OutputSplineProperties(Editor ed, Spline spline)
        {
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

        /// <summary>
        /// Xline のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="xline"></param>
        public static void OutputXlineProperties(Editor ed, Xline xline)
        {
            ed.WriteMessage("\n\n--- Xline Properties ---");
            ed.WriteMessage("\n\tBasePoint   : " + xline.BasePoint);
            ed.WriteMessage("\n\tSecondPoint : " + xline.SecondPoint);
            ed.WriteMessage("\n\tUnitDir     : " + xline.UnitDir);
        }

    }
}
