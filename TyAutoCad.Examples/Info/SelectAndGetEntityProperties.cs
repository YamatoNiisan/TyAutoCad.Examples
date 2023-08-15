using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.EditorInput;

[assembly: CommandClass(typeof(TyAutoCad.Examples.SelectAndGetEntityProperties))]
namespace TyAutoCad.Examples
{
    public class SelectAndGetEntityProperties
    {
        #region Command
        /// <summary>
        /// Entityを選択してそのプロパティを取得
        /// </summary>
        [CommandMethod("SelectAndGetEntityProperties")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = AcAp.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            ed.WriteMessage("\n--- Entityを選択してそのプロパティを取得 ---");

            // トランザクション開始
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // 図形を選択( GetEntity Method )
                    // Entity が選択されるまで続ける
                    var options = new PromptEntityOptions("\n図形を選択");
                    PromptEntityResult result;
                    DBObject dbObject;
                    do
                    {
                        result = ed.GetEntity(options);
                        if (result.Status != PromptStatus.OK) return;
                        dbObject = tr.GetObject(result.ObjectId, OpenMode.ForRead);
                    } while (!(dbObject is Entity));

                    // ObjectClass のプロパティを出力
                    AcDbInfo.OutputObjectClassProperties(ed, result.ObjectId);

                    // ObjectId のプロパティを出力
                    AcDbInfo.OutputObjectIdProperties(ed, result.ObjectId);

                    // DBObject のプロパティを出力
                    AcDbInfo.OutputDBObjectProperties(ed, dbObject);

                    // Entityにキャスト
                    var entity = dbObject as Entity;

                    // Entity のプロパティを出力
                    AcDbEntityInfo.OutputEntityProperties(ed, entity);

                    switch (entity)
                    {
                        case BlockBegin blockBegin:
                            ed.WriteMessage("\nEntity is BlockBegin...");
                            break;
                        case BlockEnd blockEnd:
                            ed.WriteMessage("\nEntity is BlockEnd...");
                            break;
                        case BlockReference blockReference:
                            ed.WriteMessage("\nEntity is BlockReference...");
                            AcDbEntityInfo.OutputBlockReferenceProperties(ed, blockReference);

                            // ダイナミックブロックの場合
                            if (blockReference.IsDynamicBlock)
                            {
                                ed.WriteMessage("\n\nBlockReference is DynamicBlock...");
                                AcDbEntityInfo.OutputDynamicBlockProperties(tr, ed, blockReference);
                            }

                            switch (blockReference)
                            {
                                case MInsertBlock mInsertBlock:
                                    ed.WriteMessage("\n\nnBlockReference is MInsertBlock...");
                                    AcDbEntityInfo.OutputMInsertBlockProperties(ed, mInsertBlock);
                                    break;
                                case Table table:
                                    ed.WriteMessage("\n\nBlockReference is Table...");
                                    AcDbEntityInfo.OutputTableProperties(ed, table);
                                    break;
                                case ViewRepBlockReference viewRepBlockReference:
                                    ed.WriteMessage("\n\nnBlockReference is ViewRepBlockReference...");
                                    AcDbEntityInfo.OutputViewRepBlockReferenceProperties(ed, viewRepBlockReference);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case Body body:
                            ed.WriteMessage("\n\nEntity is Body...");
                            break;
                        case Curve curve:
                            ed.WriteMessage("\n\nEntity is Curve...");
                            AcDbCurveInfo.OutputCurveProperties(ed, curve);

                            switch (curve)
                            {
                                case Arc arc:
                                    ed.WriteMessage("\n\nCurve is Arc...");
                                    AcDbCurveInfo.OutputArcProperties(ed, arc);
                                    break;
                                case Circle circle:
                                    ed.WriteMessage("\n\nCurve is Circle...");
                                    break;
                                case Ellipse ellipse:
                                    ed.WriteMessage("\n\nCurve is Ellipse...");
                                    AcDbCurveInfo.OutputEllipseProperties(ed, ellipse);
                                    break;
                                case Leader leader:
                                    ed.WriteMessage("\n\nCurve is Leader...");
                                    AcDbCurveInfo.OutputLeaderProperties(ed, leader);
                                    break;
                                case Line line:
                                    ed.WriteMessage("\n\nCurve is Line...");
                                    AcDbCurveInfo.OutputLineProperties(ed, line);
                                    break;
                                case Polyline polyline:
                                    ed.WriteMessage("\n\nCurve is Polyline...");
                                    AcDbCurveInfo.OutputPolylineProperties(ed, polyline);
                                    break;
                                case Polyline2d polyline2D:
                                    ed.WriteMessage("\n\nCurve is Polyline2d...");
                                    AcDbCurveInfo.OutputPolyline2dProperties(ed, polyline2D);
                                    break;
                                case Polyline3d polyline3D:
                                    ed.WriteMessage("\n\nCurve is Polyline3d...");
                                    AcDbCurveInfo.OutputPolyline3dProperties(ed, polyline3D);
                                    break;
                                case Ray ray:
                                    ed.WriteMessage("\n\nCurve is Ray...");
                                    AcDbCurveInfo.OutputRayProperties(ed, ray);
                                    break;
                                case Spline spline:
                                    ed.WriteMessage("\n\nCurve is Spline...");
                                    AcDbCurveInfo.OutputSplineProperties(ed, spline);
                                    break;
                                case Xline xline:
                                    ed.WriteMessage("\n\nCurve is Xline...");
                                    AcDbCurveInfo.OutputXlineProperties(ed, xline);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case DBPoint dBPoint:
                            ed.WriteMessage("\n\nEntity is DBPoint...");
                            AcDbEntityInfo.OutputDBPointProperties(ed, dBPoint);
                            break;
                        case DBText dBText:
                            ed.WriteMessage("\n\nEntity is DBText...");
                            AcDbEntityInfo.OutputDBTextProperties(ed, dBText);
                            switch (dBText)
                            {
                                case AttributeDefinition attributeDefinition:
                                    ed.WriteMessage("\n\nDBText is AttributeDefinition...");
                                    AcDbEntityInfo.OutputAttributeDefinitionProperties(ed, attributeDefinition);
                                    break;
                                case AttributeReference attributeReference:
                                    ed.WriteMessage("\n\nDBText is AttributeReference...");
                                    AcDbEntityInfo.OutputAttributeReferenceProperties(ed, attributeReference);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case DetailSymbol detailSymbol:
                            ed.WriteMessage("\n\nEntity is DetailSymbol...");
                            break;
                        case Dimension dimension:
                            ed.WriteMessage("\n\nEntity is Dimension...");
                            AcDbDimensionInfo.OutputDimensionProperties(ed, dimension);
                            switch (dimension)
                            {
                                case AlignedDimension alignedDimension:
                                    ed.WriteMessage("\n\nDimension is AlignedDimension...");
                                    AcDbDimensionInfo.OutputAlignedDimensionProperties(ed, alignedDimension);
                                    break;
                                case ArcDimension arcDimension:
                                    ed.WriteMessage("\n\nDimension is ArcDimension...");
                                    AcDbDimensionInfo.OutputArcDimensionProperties(ed, arcDimension);
                                    break;
                                case DiametricDimension diametricDimension:
                                    ed.WriteMessage("\n\nDimension is DiametricDimension...");
                                    AcDbDimensionInfo.OutputDiametricDimensionProperties(ed, diametricDimension);
                                    break;
                                case LineAngularDimension2 lineAngularDimension2:
                                    ed.WriteMessage("\n\nDimension is LineAngularDimension2...");
                                    AcDbDimensionInfo.OutputLineAngularDimension2Properties(ed, lineAngularDimension2);
                                    break;
                                case Point3AngularDimension point3AngularDimension:
                                    ed.WriteMessage("\n\nDimension is Point3AngularDimension...");
                                    AcDbDimensionInfo.OutputPoint3AngularDimensionProperties(ed, point3AngularDimension);
                                    break;
                                case RadialDimension radialDimension:
                                    ed.WriteMessage("\n\nDimension is RadialDimension...");
                                    AcDbDimensionInfo.OutputRadialDimensionProperties(ed, radialDimension);
                                    break;
                                case RadialDimensionLarge radialDimensionLarge:
                                    ed.WriteMessage("\n\nDimension is RadialDimensionLarge...");
                                    AcDbDimensionInfo.OutputRadialDimensionLargeProperties(ed, radialDimensionLarge);
                                    break;
                                case RotatedDimension rotatedDimension:
                                    ed.WriteMessage("\n\nDimension is RotatedDimension...");
                                    AcDbDimensionInfo.OutputRotatedDimensionProperties(ed, rotatedDimension);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case Face face:
                            ed.WriteMessage("\n\nEntity is Face...");
                            break;
                        case FeatureControlFrame featureControlFrame:
                            ed.WriteMessage("\n\nEntity is FeatureControlFrame...");
                            break;
                        case GeoPositionMarker geoPositionMarker:
                            ed.WriteMessage("\n\nEntity is GeoPositionMarker...");
                            break;
                        case Hatch hatch:
                            ed.WriteMessage("\n\nEntity is Hatch...");
                            AcDbEntityInfo.OutputHatchProperties(ed, hatch);
                            break;
                        case Image image:
                            ed.WriteMessage("\n\nEntity is Image...");
                            break;
                        case Light light:
                            ed.WriteMessage("\n\nEntity is Light...");
                            break;
                        case MLeader mLeader:
                            ed.WriteMessage("\n\nEntity is MLeader...");
                            AcDbEntityInfo.OutputMLeaderProperties(ed, mLeader);
                            break;
                        case Mline mline:
                            ed.WriteMessage("\n\nEntity is Mline...");
                            AcDbEntityInfo.OutputMlineProperties(ed, mline);
                            break;
                        case MText mText:
                            ed.WriteMessage("\n\nEntity is MText...");
                            AcDbEntityInfo.OutputMTextProperties(ed, mText);
                            break;
                        case Ole2Frame ole2Frame:
                            ed.WriteMessage("\n\nEntity is Ole2Frame...");
                            break;
                        case PointCloud pointCloud:
                            ed.WriteMessage("\n\nEntity is PointCloud...");
                            break;
                        case PointCloudEx pointCloudEx:
                            ed.WriteMessage("\n\nEntity is PointCloudEx...");
                            break;
                        case PolyFaceMesh polyFaceMesh:
                            ed.WriteMessage("\n\nEntity is PolyFaceMesh...");
                            break;
                        case PolygonMesh polygonMesh:
                            ed.WriteMessage("\n\nEntity is PolygonMesh...");
                            break;
                        case ProxyEntity proxyEntity:
                            ed.WriteMessage("\n\nEntity is ProxyEntity...");
                            break;
                        case Region region:
                            ed.WriteMessage("\n\nEntity is Region...");
                            break;
                        case Section section:
                            ed.WriteMessage("\n\nEntity is Section...");
                            break;
                        case SectionSymbol sectionSymbol:
                            ed.WriteMessage("\nEntity is SectionSymbol...");
                            break;
                        case SequenceEnd sequenceEnd:
                            ed.WriteMessage("\n\nEntity is SequenceEnd...");
                            break;
                        case Shape shape:
                            ed.WriteMessage("\n\nEntity is Shape...");
                            break;
                        case Solid solid:
                            ed.WriteMessage("\n\nEntity is Solid...");
                            AcDbEntityInfo.OutputSolidProperties(ed, solid);
                            break;
                        case Solid3d solid3d:
                            ed.WriteMessage("\n\nEntity is Solid3d...");
                            break;
                        case SubDMesh subDMesh:
                            ed.WriteMessage("\n\nEntity is SubDMesh...");
                            break;
                        case Autodesk.AutoCAD.DatabaseServices.Surface surface:
                            ed.WriteMessage("\n\nEntity is Surface...");
                            break;
                        case UnderlayReference underlayReference:
                            ed.WriteMessage("\n\nEntity is UnderlayReference...");
                            break;
                        case Vertex vertex:
                            ed.WriteMessage("\n\nEntity is Vertex...");
                            break;
                        case ViewBorder viewBorder:
                            ed.WriteMessage("\n\nEntity is ViewBorder...");
                            break;
                        case Viewport viewport:
                            ed.WriteMessage("\n\nEntity is Viewport...");
                            AcDbEntityInfo.OutputViewportProperties(ed, viewport);
                            break;
                        default:
                            break;
                    }
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    AcAp.ShowAlertDialog("Error SelectAndGetEntityProperties\n\t" + ex.Message);
                }
            }
            ed.WriteMessage("\n--- コマンド終了 ---");
        }
        #endregion
    }
}
