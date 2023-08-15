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
    /// Entity クラス情報
    /// </summary>
    public static class AcDbEntityInfo
    {
        public static void Output(Editor ed, Entity entity)
        {
            ed.WriteMessage("\n---------------------------------------------------------------------------------------- SelectAndOutputEntityProperties");

            // ObjectClass のプロパティを出力
            AcDbInfo.OutputObjectClassProperties(ed, entity.ObjectId);

            // ObjectId のプロパティを出力
            AcDbInfo.OutputObjectIdProperties(ed, entity.ObjectId);

            // DBObject のプロパティを出力
            AcDbInfo.OutputDBObjectProperties(ed, entity);

            // Entityにキャスト
            //var entity = dbObject as Entity;

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
                        //AcDbEntityInfo.OutputDynamicBlockProperties(tr, ed, blockReference);
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

        /// <summary>
        /// Entity のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="entity"></param>
        public static void OutputEntityProperties(Editor ed, Entity entity)
        {
            string blockId = string.Empty;
            string blockName = string.Empty;
            if (entity.BlockId == ObjectId.Null)
            {
                blockId = ObjectId.Null.ToString();
                blockName = "noname";
            }
            else
            {
                blockId = entity.BlockId.ToString();
                blockName = entity.BlockName;
            }

            ed.WriteMessage("\n\n--- Entity Properties ---");
            ed.WriteMessage("\n\tBlockId             : " + blockId);
            ed.WriteMessage("\n\tBlockName           : " + blockName);
            ed.WriteMessage("\n\tCastShadows         : " + entity.CastShadows);
            ed.WriteMessage("\n\tCloneMeForDragging  : " + entity.CloneMeForDragging);
            ed.WriteMessage("\n\tCollisionType       : " + entity.CollisionType);
            ed.WriteMessage("\n\tColor               : " + entity.Color);
            ed.WriteMessage("\n\tColorIndex          : " + entity.ColorIndex);
            //ed.WriteMessage("\n\tCompoundObjectTransform : " + ent.CompoundObjectTransform);
            ed.WriteMessage("\n\tEcs                 : " + entity.Ecs);
            ed.WriteMessage("\n\tEdgeStyleId         : " + entity.EdgeStyleId);
            ed.WriteMessage("\n\tEntityColor         : " + entity.EntityColor);
            ed.WriteMessage("\n\tFaceStyleId         : " + entity.FaceStyleId);
            ed.WriteMessage("\n\tForceAnnoAllVisible : " + entity.ForceAnnoAllVisible);
            //ed.WriteMessage("\n\tGeometricExtents    : " + ent.GeometricExtents);
            ed.WriteMessage("\n\tHyperlinks          : " + entity.Hyperlinks);
            ed.WriteMessage("\n\tIsPlanar            : " + entity.IsPlanar);
            ed.WriteMessage("\n\tLayer               : " + entity.Layer);
            ed.WriteMessage("\n\tLinetype            : " + entity.Linetype);
            ed.WriteMessage("\n\tLinetypeId          : " + entity.LinetypeId);
            ed.WriteMessage("\n\tLinetypeScale       : " + entity.LinetypeScale);
            ed.WriteMessage("\n\tLineWeight          : " + entity.LineWeight);
            ed.WriteMessage("\n\tMaterial            : " + entity.Material);
            ed.WriteMessage("\n\tMaterialId          : " + entity.MaterialId);
            ed.WriteMessage("\n\tMaterialMapper      : " + entity.MaterialMapper);
            ed.WriteMessage("\n\tPlotStyleName       : " + entity.PlotStyleName);
            ed.WriteMessage("\n\tPlotStyleNameId     : " + entity.PlotStyleNameId);
            ed.WriteMessage("\n\tReceiveShadows      : " + entity.ReceiveShadows);
            ed.WriteMessage("\n\tTransparency        : " + entity.Transparency);
            ed.WriteMessage("\n\tVisible             : " + entity.Visible);
            ed.WriteMessage("\n\tVisualStyleId       : " + entity.VisualStyleId);
        }


        /// <summary>
        /// BlockReference のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="blockReference"></param>
        public static void OutputBlockReferenceProperties(Editor ed, BlockReference blockReference)
        {
            var attributeCollectionCount = blockReference.AttributeCollection.Count;

            ed.WriteMessage("\n\n--- BlockReference Properties ---");
            ed.WriteMessage("\n\tAnonymousBlockTableRecord               : " + blockReference.AnonymousBlockTableRecord);
            ed.WriteMessage("\n\tAttributeCollection                     : " + blockReference.AttributeCollection);
            ed.WriteMessage("\n\t\tAttributeCollection.Count             : " + attributeCollectionCount);
            ed.WriteMessage("\n\tBlockTableRecord                        : " + blockReference.BlockTableRecord);
            ed.WriteMessage("\n\tBlockTransform                          : " + blockReference.BlockTransform);
            ed.WriteMessage("\n\tBlockUnit                               : " + blockReference.BlockUnit);
            ed.WriteMessage("\n\tDynamicBlockReferencePropertyCollection : " + blockReference.DynamicBlockReferencePropertyCollection);
            ed.WriteMessage("\n\tDynamicBlockTableRecord                 : " + blockReference.DynamicBlockTableRecord);
            ed.WriteMessage("\n\tIsDynamicBlock                          : " + blockReference.IsDynamicBlock);
            ed.WriteMessage("\n\tName                                    : " + blockReference.Name);
            ed.WriteMessage("\n\tNormal                                  : " + blockReference.Normal);
            ed.WriteMessage("\n\tPosition                                : " + blockReference.Position);
            ed.WriteMessage("\n\tRotation                                : " + blockReference.Rotation);
            ed.WriteMessage("\n\tScaleFactors                            : " + blockReference.ScaleFactors);
            ed.WriteMessage("\n\tTreatAsBlockRefForExplode               : " + blockReference.TreatAsBlockRefForExplode);
            ed.WriteMessage("\n\tUnitFactor                              : " + blockReference.UnitFactor);
        }

        /// <summary>
        /// DynamicBlock のプロパティを出力する
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="ed"></param>
        /// <param name="blockReference"></param>
        public static void OutputDynamicBlockProperties(Transaction tr, Editor ed, BlockReference blockReference)
        {
            // ダイナミックブロックのブロックテーブルレコードを取得
            var dybBtr = tr.GetObject(blockReference.DynamicBlockTableRecord, OpenMode.ForRead) as BlockTableRecord;
            ed.WriteMessage("\n\n\t\tDynamicBlock :");
            ed.WriteMessage("\n\t\t\tName           : " + dybBtr.Name);
            ed.WriteMessage("\n\t\t\tIsAnonymous    : " + dybBtr.IsAnonymous);
            ed.WriteMessage("\n\t\t\tIsDynamicBlock : " + dybBtr.IsDynamicBlock);

            // ダイナミックブロックのプロパティコレクションを取得
            var dybProperties = blockReference.DynamicBlockReferencePropertyCollection;

            foreach (DynamicBlockReferenceProperty dybProp in dybProperties)
            {
                ed.WriteMessage("\n\t\t\tPropertyName : " + dybProp.PropertyName);
                ed.WriteMessage("\n\t\t\tUnitsType    : " + dybProp.UnitsType);
                ed.WriteMessage("\n\t\t\tValue        : " + dybProp.Value);
            }
        }

        /// <summary>
        /// MInsertBlock のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="mInsertBlock"></param>
        public static void OutputMInsertBlockProperties(Editor ed, MInsertBlock mInsertBlock)
        {
            ed.WriteMessage("\n\n--- MInsertBlock Properties ---");
            ed.WriteMessage("\n\tColumns       : " + mInsertBlock.Columns);
            ed.WriteMessage("\n\tColumnSpacing : " + mInsertBlock.ColumnSpacing);
            ed.WriteMessage("\n\tRows          : " + mInsertBlock.Rows);
            ed.WriteMessage("\n\tRowSpacing    : " + mInsertBlock.RowSpacing);
        }

        /// <summary>
        /// Table のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="table"></param>
        public static void OutputTableProperties(Editor ed, Table table)
        {
            ed.WriteMessage("\n\n--- Table Properties ---");
            ed.WriteMessage("\n\tBreakEnabled         : " + table.BreakEnabled);
            ed.WriteMessage("\n\tBreakFlowDirection   : " + table.BreakFlowDirection);
            ed.WriteMessage("\n\tBreakOptions         : " + table.BreakOptions);
            ed.WriteMessage("\n\tDirection            : " + table.Direction);
            ed.WriteMessage("\n\tFlowDirection        : " + table.FlowDirection);
            ed.WriteMessage("\n\tHasSubSelection      : " + table.HasSubSelection);
            ed.WriteMessage("\n\tHeight               : " + table.Height);
            //ed.WriteMessage("\n\tHorizontalCellMargin : " + table.HorizontalCellMargin); // ※旧式
            //ed.WriteMessage("\n\tIsHeaderSuppressed   : " + table.IsHeaderSuppressed); // ※旧式
            //ed.WriteMessage("\n\tIsTitleSuppressed    : " + table.IsTitleSuppressed); // ※旧式
            ed.WriteMessage("\n\tMinimumTableHeight   : " + table.MinimumTableHeight);
            ed.WriteMessage("\n\tMinimumTableWidth    : " + table.MinimumTableWidth);
            //ed.WriteMessage("\n\tNumColumns           : " + table.NumColumns); // ※旧式
            //ed.WriteMessage("\n\tNumRows              : " + table.NumRows); // ※旧式

            if (table.HasSubSelection)
            {
                ed.WriteMessage("\n\tSubSelection         : " + table.SubSelection);
            }
            else
            {
                ed.WriteMessage("\n\tSubSelection         : ---");
            }

            ed.WriteMessage("\n\tTableStyle           : " + table.TableStyle);
            ed.WriteMessage("\n\tTableStyleName       : " + table.TableStyleName);
            //ed.WriteMessage("\n\tVerticalCellMargin   : " + table.VerticalCellMargin); // ※旧式
            ed.WriteMessage("\n\tWidth       : " + table.Width);
        }

        /// <summary>
        /// ViewRepBlockReference のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="viewRepBlockReference"></param>
        public static void OutputViewRepBlockReferenceProperties(Editor ed, ViewRepBlockReference viewRepBlockReference)
        {
            ed.WriteMessage("\n\n--- ViewRepBlockReference Properties ---");
            ed.WriteMessage("\n\tOwnerViewportId : " + viewRepBlockReference.OwnerViewportId);
        }

        /// <summary>
        /// DBPoint のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="dBPoint"></param>
        public static void OutputDBPointProperties(Editor ed, DBPoint dBPoint)
        {
            ed.WriteMessage("\n\n--- DBPoint Properties ---");
            ed.WriteMessage("\n\tEcsRotation : " + dBPoint.EcsRotation);
            ed.WriteMessage("\n\tNormal      : " + dBPoint.Normal);
            ed.WriteMessage("\n\tPosition    : " + dBPoint.Position);
            ed.WriteMessage("\n\tThickness   : " + dBPoint.Thickness);
        }

        /// <summary>
        /// DBText のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="dBText"></param>
        public static void OutputDBTextProperties(Editor ed, DBText dBText)
        {
            ed.WriteMessage("\n\n--- DBText Properties ---");
            ed.WriteMessage("\n\tAlignmentPoint     : " + dBText.AlignmentPoint);
            ed.WriteMessage("\n\tHeight             : " + dBText.Height);
            ed.WriteMessage("\n\tHorizontalMode     : " + dBText.HorizontalMode);
            ed.WriteMessage("\n\tIsDefaultAlignment : " + dBText.IsDefaultAlignment);
            ed.WriteMessage("\n\tIsMirroredInX      : " + dBText.IsMirroredInX);
            ed.WriteMessage("\n\tIsMirroredInY      : " + dBText.IsMirroredInY);
            ed.WriteMessage("\n\tJustify            : " + dBText.Justify);
            ed.WriteMessage("\n\tNormal             : " + dBText.Normal);
            ed.WriteMessage("\n\tOblique            : " + dBText.Oblique);
            ed.WriteMessage("\n\tPosition           : " + dBText.Position);
            ed.WriteMessage("\n\tRotation           : " + dBText.Rotation);
            ed.WriteMessage("\n\tTextString         : " + dBText.TextString);
            ed.WriteMessage("\n\tTextStyleName      : " + dBText.TextStyleName);
            ed.WriteMessage("\n\tThickness          : " + dBText.Thickness);
            ed.WriteMessage("\n\tVerticalMode       : " + dBText.VerticalMode);
            ed.WriteMessage("\n\tWidthFactor        : " + dBText.WidthFactor);
        }

        /// <summary>
        /// AttributeDefinition のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="attributeDefinition"></param>
        public static void OutputAttributeDefinitionProperties(Editor ed, AttributeDefinition attributeDefinition)
        {
            ed.WriteMessage("\n\n--- AttributeDefinition Properties ---");
            ed.WriteMessage("\n\tConstant                   : " + attributeDefinition.Constant);
            ed.WriteMessage("\n\tFieldLength                : " + attributeDefinition.FieldLength);
            ed.WriteMessage("\n\tInvisible                  : " + attributeDefinition.Invisible);
            ed.WriteMessage("\n\tIsMTextAttributeDefinition : " + attributeDefinition.IsMTextAttributeDefinition);
            ed.WriteMessage("\n\tLockPositionInBlock        : " + attributeDefinition.LockPositionInBlock);

            if (attributeDefinition.IsMTextAttributeDefinition)
            {
                ed.WriteMessage("\n\tMTextAttributeDefinition   : " + attributeDefinition.MTextAttributeDefinition);
            }
            else
            {
                ed.WriteMessage("\n\tMTextAttributeDefinition   : ---");
            }

            ed.WriteMessage("\n\tPreset                     : " + attributeDefinition.Preset);
            ed.WriteMessage("\n\tPrompt                     : " + attributeDefinition.Prompt);
            ed.WriteMessage("\n\tTag                        : " + attributeDefinition.Tag);
            ed.WriteMessage("\n\tVerifiable                 : " + attributeDefinition.Verifiable);
        }

        /// <summary>
        /// AttributeReference のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="attributeReference"></param>
        public static void OutputAttributeReferenceProperties(Editor ed, AttributeReference attributeReference)
        {
            ed.WriteMessage("\n\n--- AttributeReference Properties ---");
            ed.WriteMessage("\n\tFieldLength         : " + attributeReference.FieldLength);
            ed.WriteMessage("\n\tInvisible           : " + attributeReference.Invisible);
            ed.WriteMessage("\n\tIsConstant          : " + attributeReference.IsConstant);
            ed.WriteMessage("\n\tIsMTextAttribute    : " + attributeReference.IsMTextAttribute);
            ed.WriteMessage("\n\tIsPreset            : " + attributeReference.IsPreset);
            ed.WriteMessage("\n\tIsVerifiable        : " + attributeReference.IsVerifiable);
            ed.WriteMessage("\n\tLockPositionInBlock : " + attributeReference.LockPositionInBlock);

            if (attributeReference.IsMTextAttribute)
            {
                ed.WriteMessage("\n\tMTextAttribute      : " + attributeReference.MTextAttribute);
            }
            else
            {
                ed.WriteMessage("\n\tMTextAttribute      : ---");
            }
            ed.WriteMessage("\n\tTag                 : " + attributeReference.Tag);
        }

        /// <summary>
        /// Hatch のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="hatch"></param>
        public static void OutputHatchProperties(Editor ed, Hatch hatch)
        {
            ed.WriteMessage("\n\n--- Hatch Properties ---");
            ed.WriteMessage("\nArea                       : " + hatch.Area);
            ed.WriteMessage("\nAssociative                : " + hatch.Associative);
            ed.WriteMessage("\nBackgroundColor            : " + hatch.BackgroundColor);
            ed.WriteMessage("\nElevation                  : " + hatch.Elevation);
            ed.WriteMessage("\nGradientAngle              : " + hatch.GradientAngle);
            ed.WriteMessage("\nGradientName               : " + hatch.GradientName);
            ed.WriteMessage("\nGradientOneColorMode       : " + hatch.GradientOneColorMode);
            ed.WriteMessage("\nGradientShift              : " + hatch.GradientShift);
            ed.WriteMessage("\nGradientType               : " + hatch.GradientType);
            ed.WriteMessage("\nHatchObjectType            : " + hatch.HatchObjectType);
            ed.WriteMessage("\nHatchStyle                 : " + hatch.HatchStyle);
            ed.WriteMessage("\nIsGradient                 : " + hatch.IsGradient);
            ed.WriteMessage("\nIsHatch                    : " + hatch.IsHatch);
            ed.WriteMessage("\nIsSolidFill                : " + hatch.IsSolidFill);
            ed.WriteMessage("\nNormal                     : " + hatch.Normal);
            ed.WriteMessage("\nNumberOfHatchLines         : " + hatch.NumberOfHatchLines);
            ed.WriteMessage("\nNumberOfLoops              : " + hatch.NumberOfLoops);
            ed.WriteMessage("\nNumberOfPatternDefinitions : " + hatch.NumberOfPatternDefinitions);
            ed.WriteMessage("\nOrigin                     : " + hatch.Origin);
            ed.WriteMessage("\nPatternAngle               : " + hatch.PatternAngle);
            ed.WriteMessage("\nPatternDouble              : " + hatch.PatternDouble);
            ed.WriteMessage("\nPatternName                : " + hatch.PatternName);
            ed.WriteMessage("\nPatternScale               : " + hatch.PatternScale);
            ed.WriteMessage("\nPatternSpace               : " + hatch.PatternSpace);
            ed.WriteMessage("\nPatternType                : " + hatch.PatternType);
            ed.WriteMessage("\nShadeTintValue             : " + hatch.ShadeTintValue);
        }

        /// <summary>
        /// MLeader のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="mLeader"></param>
        public static void OutputMLeaderProperties(Editor ed, MLeader mLeader)
        {
            ed.WriteMessage("\n\n--- MLeader Properties ---");
            ed.WriteMessage("\n\tArrowSize               : " + mLeader.ArrowSize);
            ed.WriteMessage("\n\tArrowSymbolId           : " + mLeader.ArrowSymbolId);
            ed.WriteMessage("\n\tBlockColor              : " + mLeader.BlockColor);
            ed.WriteMessage("\n\tBlockConnectionType     : " + mLeader.BlockConnectionType);
            ed.WriteMessage("\n\tBlockContentId          : " + mLeader.BlockContentId);

            if (mLeader.ContentType == ContentType.BlockContent)
            {
                ed.WriteMessage("\n\tBlockPosition           : " + mLeader.BlockPosition);
            }
            else
            {
                ed.WriteMessage("\n\tBlockPosition           : ---");
            }

            ed.WriteMessage("\n\tBlockRotation           : " + mLeader.BlockRotation);
            ed.WriteMessage("\n\tBlockScale              : " + mLeader.BlockScale);
            ed.WriteMessage("\n\tContentType             : " + mLeader.ContentType);
            ed.WriteMessage("\n\tDoglegLength            : " + mLeader.DoglegLength);
            ed.WriteMessage("\n\tEnableAnnotationScale   : " + mLeader.EnableAnnotationScale);
            ed.WriteMessage("\n\tEnableDogleg            : " + mLeader.EnableDogleg);
            ed.WriteMessage("\n\tEnableFrameText         : " + mLeader.EnableFrameText);
            ed.WriteMessage("\n\tEnableLanding           : " + mLeader.EnableLanding);
            ed.WriteMessage("\n\tExtendLeaderToText      : " + mLeader.ExtendLeaderToText);
            ed.WriteMessage("\n\tLandingGap              : " + mLeader.LandingGap);
            ed.WriteMessage("\n\tLeaderCount             : " + mLeader.LeaderCount);
            ed.WriteMessage("\n\tLeaderLineColor         : " + mLeader.LeaderLineColor);
            ed.WriteMessage("\n\tLeaderLineCount         : " + mLeader.LeaderLineCount);
            ed.WriteMessage("\n\tLeaderLineType          : " + mLeader.LeaderLineType);
            ed.WriteMessage("\n\tLeaderLineTypeId        : " + mLeader.LeaderLineTypeId);
            ed.WriteMessage("\n\tLeaderLineWeight        : " + mLeader.LeaderLineWeight);
            ed.WriteMessage("\n\tMLeaderStyle            : " + mLeader.MLeaderStyle);

            if (mLeader.ContentType == ContentType.MTextContent)
            {
                ed.WriteMessage("\n\tMText                   : " + mLeader.MText);
            }
            else
            {
                ed.WriteMessage("\n\tMText                   : ---");
            }

            ed.WriteMessage("\n\tNormal                  : " + mLeader.Normal);
            ed.WriteMessage("\n\tScale                   : " + mLeader.Scale);
            ed.WriteMessage("\n\tTextAlignmentType       : " + mLeader.TextAlignmentType);
            ed.WriteMessage("\n\tTextAngleType           : " + mLeader.TextAngleType);
            ed.WriteMessage("\n\tTextAttachmentDirection : " + mLeader.TextAttachmentDirection);
            ed.WriteMessage("\n\tTextAttachmentType      : " + mLeader.TextAttachmentType);
            ed.WriteMessage("\n\tTextColor               : " + mLeader.TextColor);
            ed.WriteMessage("\n\tTextHeight              : " + mLeader.TextHeight);

            if (mLeader.ContentType == ContentType.MTextContent)
            {
                ed.WriteMessage("\n\tTextLocation            : " + mLeader.TextLocation);
            }
            else
            {
                ed.WriteMessage("\n\tTextLocation            : ---");
            }

            ed.WriteMessage("\n\tTextStyleId             : " + mLeader.TextStyleId);

            if (mLeader.ContentType == ContentType.ToleranceContent)
            {
                ed.WriteMessage("\n\tToleranceLocation       : " + mLeader.ToleranceLocation);
            }
            else
            {
                ed.WriteMessage("\n\tToleranceLocation       : ---");
            }
        }

        /// <summary>
        /// Mline のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="mline"></param>
        public static void OutputMlineProperties(Editor ed, Mline mline)
        {
            ed.WriteMessage("\n\n--- Mline Properties ---");
            ed.WriteMessage("\n\tIsClosed         : " + mline.IsClosed);
            ed.WriteMessage("\n\tJustification    : " + mline.Justification);
            ed.WriteMessage("\n\tNormal           : " + mline.Normal);
            ed.WriteMessage("\n\tNumberOfVertices : " + mline.NumberOfVertices);
            ed.WriteMessage("\n\tScale            : " + mline.Scale);
            ed.WriteMessage("\n\tStyle            : " + mline.Style);
            ed.WriteMessage("\n\tSupressEndCaps   : " + mline.SupressEndCaps);
            ed.WriteMessage("\n\tSupressStartCaps : " + mline.SupressStartCaps);
        }

        /// <summary>
        /// MText のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="mText"></param>
        public static void OutputMTextProperties(Editor ed, MText mText)
        {
            ed.WriteMessage("\n\n--- MText Properties ---");
            ed.WriteMessage("\n\tActualHeight             : " + mText.ActualHeight);
            ed.WriteMessage("\n\tActualWidth              : " + mText.ActualWidth);
            ed.WriteMessage("\n\t(static)AlignChange      : " + MText.AlignChange);
            ed.WriteMessage("\n\tAscent                   : " + mText.Ascent);
            ed.WriteMessage("\n\tAttachment               : " + mText.Attachment);
            ed.WriteMessage("\n\tBackgroundFill           : " + mText.BackgroundFill);  // 背景マスクを使用しているかどうか

            if (mText.BackgroundFill)
            {
                ed.WriteMessage("\n\tBackgroundFillColor      : " + mText.BackgroundFillColor);    // 背景マスクの色
                ed.WriteMessage("\n\tBackgroundScaleFactor    : " + mText.BackgroundScaleFactor);  // 背景マスクの境界のオフセット係数
                ed.WriteMessage("\n\tBackgroundTransparency   : " + mText.BackgroundTransparency); // 背景マスクの透明度
            }
            else
            {
                ed.WriteMessage("\n\tBackgroundFillColor      : ---");
                ed.WriteMessage("\n\tBackgroundScaleFactor    : ---");
                ed.WriteMessage("\n\tBackgroundTransparency   : ---");
            }

            ed.WriteMessage("\n\t(static)BlockBegin       : " + MText.BlockBegin);
            ed.WriteMessage("\n\t(static)BlockEnd         : " + MText.BlockEnd);
            ed.WriteMessage("\n\t(static)ColorChange      : " + MText.ColorChange);
            ed.WriteMessage("\n\tColumnAutoHeight         : " + mText.ColumnAutoHeight);
            ed.WriteMessage("\n\tColumnCount              : " + mText.ColumnCount);
            ed.WriteMessage("\n\tColumnFlowReversed       : " + mText.ColumnFlowReversed);
            ed.WriteMessage("\n\tColumnGutterWidth        : " + mText.ColumnGutterWidth);
            ed.WriteMessage("\n\tColumnType               : " + mText.ColumnType);
            ed.WriteMessage("\n\tColumnWidth              : " + mText.ColumnWidth);
            ed.WriteMessage("\n\tContents                 : " + mText.Contents);
            ed.WriteMessage("\n\tContentsRTF              : " + mText.ContentsRTF);
            ed.WriteMessage("\n\tDescent                  : " + mText.Descent);
            ed.WriteMessage("\n\tDirection                : " + mText.Direction);
            ed.WriteMessage("\n\tFlowDirection            : " + mText.FlowDirection);
            ed.WriteMessage("\n\t(static)FontChange       : " + MText.FontChange);
            ed.WriteMessage("\n\tHeight                   : " + mText.Height);
            ed.WriteMessage("\n\t(static)HeightChange     : " + MText.HeightChange);
            ed.WriteMessage("\n\t(static)LineBreak        : " + MText.LineBreak);
            ed.WriteMessage("\n\tLineSpaceDistance        : " + mText.LineSpaceDistance);
            ed.WriteMessage("\n\tLineSpacingFactor        : " + mText.LineSpacingFactor);
            ed.WriteMessage("\n\tLineSpacingStyle         : " + mText.LineSpacingStyle);
            ed.WriteMessage("\n\tLocation                 : " + mText.Location);
            ed.WriteMessage("\n\t(static)NonBreakSpace    : " + MText.NonBreakSpace);
            ed.WriteMessage("\n\tNormal                   : " + mText.Normal);
            ed.WriteMessage("\n\t(static)ObliqueChange    : " + MText.ObliqueChange);
            ed.WriteMessage("\n\t(static)OverlineOff      : " + MText.OverlineOff);
            ed.WriteMessage("\n\t(static)OverlineOn       : " + MText.OverlineOn);
            ed.WriteMessage("\n\t(static)ParagraphBreak   : " + MText.ParagraphBreak);
            ed.WriteMessage("\n\tRotation                 : " + mText.Rotation);
            ed.WriteMessage("\n\tShowBorders              : " + mText.ShowBorders);
            ed.WriteMessage("\n\t(static)StackStart       : " + MText.StackStart);
            ed.WriteMessage("\n\t(static)StrikethroughOff : " + MText.StrikethroughOff);
            ed.WriteMessage("\n\t(static)StrikethroughOn  : " + MText.StrikethroughOn);
            ed.WriteMessage("\n\tText                     : " + mText.Text);
            ed.WriteMessage("\n\tTextHeight               : " + mText.TextHeight);
            ed.WriteMessage("\n\tTextStyleId              : " + mText.TextStyleId);
            ed.WriteMessage("\n\tTextStyleName            : " + mText.TextStyleName);
            ed.WriteMessage("\n\t(static)TrackChange      : " + MText.TrackChange);
            ed.WriteMessage("\n\t(static)UnderlineOff     : " + MText.UnderlineOff);
            ed.WriteMessage("\n\t(static)UnderlineOn      : " + MText.UnderlineOn);
            ed.WriteMessage("\n\tUseBackgroundColor       : " + mText.UseBackgroundColor);
            ed.WriteMessage("\n\tWidth                    : " + mText.Width);
            ed.WriteMessage("\n\t(static)WidthChange      : " + MText.WidthChange);
        }

        /// <summary>
        /// Solid のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="solid"></param>
        public static void OutputSolidProperties(Editor ed, Solid solid)
        {
            ed.WriteMessage("\n\tNormal        : " + solid.Normal);
            ed.WriteMessage("\n\tThickness     : " + solid.Thickness);
            ed.WriteMessage("\n\tGetPointAt(0) : " + solid.GetPointAt(0));
            ed.WriteMessage("\n\tGetPointAt(1) : " + solid.GetPointAt(1));
            ed.WriteMessage("\n\tGetPointAt(2) : " + solid.GetPointAt(2));
            ed.WriteMessage("\n\tGetPointAt(3) : " + solid.GetPointAt(3));
        }

        /// <summary>
        /// Viewport のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="viewport"></param>
        public static void OutputViewportProperties(Editor ed, Viewport viewport)
        {
            ed.WriteMessage("\n\n--- Viewport Properties ---");
            ed.WriteMessage("\n\tAmbientLightColor         : " + viewport.AmbientLightColor);
            ed.WriteMessage("\n\tAnnotationScale           : " + viewport.AnnotationScale);
            ed.WriteMessage("\n\tBackClipDistance          : " + viewport.BackClipDistance);
            ed.WriteMessage("\n\tBackClipOn                : " + viewport.BackClipOn);
            ed.WriteMessage("\n\tBackground                : " + viewport.Background);
            ed.WriteMessage("\n\tBrightness                : " + viewport.Brightness);
            ed.WriteMessage("\n\tCenterPoint               : " + viewport.CenterPoint);
            ed.WriteMessage("\n\tCircleSides               : " + viewport.CircleSides);
            ed.WriteMessage("\n\tContrast                  : " + viewport.Contrast);
            ed.WriteMessage("\n\tCustomScale               : " + viewport.CustomScale);
            ed.WriteMessage("\n\tDefaultLightingOn         : " + viewport.DefaultLightingOn);
            ed.WriteMessage("\n\tDefaultLightingType       : " + viewport.DefaultLightingType);
            ed.WriteMessage("\n\tEffectivePlotStyleSheet   : " + viewport.EffectivePlotStyleSheet);
            ed.WriteMessage("\n\tElevation                 : " + viewport.Elevation);
            ed.WriteMessage("\n\tFastZoomOn                : " + viewport.FastZoomOn);
            ed.WriteMessage("\n\tFrontClipAtEyeOn          : " + viewport.FrontClipAtEyeOn);
            ed.WriteMessage("\n\tFrontClipDistance         : " + viewport.FrontClipDistance);
            ed.WriteMessage("\n\tFrontClipOn               : " + viewport.FrontClipOn);
            ed.WriteMessage("\n\tGridAdaptive              : " + viewport.GridAdaptive);
            ed.WriteMessage("\n\tGridBoundToLimits         : " + viewport.GridBoundToLimits);
            ed.WriteMessage("\n\tGridFollow                : " + viewport.GridFollow);
            ed.WriteMessage("\n\tGridIncrement             : " + viewport.GridIncrement);
            ed.WriteMessage("\n\tGridMajor                 : " + viewport.GridMajor);
            ed.WriteMessage("\n\tGridOn                    : " + viewport.GridOn);
            ed.WriteMessage("\n\tGridSubdivisionRestricted : " + viewport.GridSubdivisionRestricted);
            ed.WriteMessage("\n\tHeight                    : " + viewport.Height);
            ed.WriteMessage("\n\tHiddenLinesRemoved        : " + viewport.HiddenLinesRemoved);
            ed.WriteMessage("\n\tLensLength                : " + viewport.LensLength);
            ed.WriteMessage("\n\tLinkedToSheetView         : " + viewport.LinkedToSheetView);
            ed.WriteMessage("\n\tLocked                    : " + viewport.Locked);
            ed.WriteMessage("\n\tNonRectClipEntityId       : " + viewport.NonRectClipEntityId);
            ed.WriteMessage("\n\tNonRectClipOn             : " + viewport.NonRectClipOn);
            ed.WriteMessage("\n\tNumber                    : " + viewport.Number);
            ed.WriteMessage("\n\tOn                        : " + viewport.On);
            ed.WriteMessage("\n\tPerspectiveOn             : " + viewport.PerspectiveOn);
            ed.WriteMessage("\n\tPlotAsRaster              : " + viewport.PlotAsRaster);
            //ed.WriteMessage("\n\tPlotStyleSheet            : " + viewport.PlotStyleSheet); // このビューポート内のオブジェクトに適用されたスタイルシートにアクセス
            ed.WriteMessage("\n\tPlotWireframe             : " + viewport.PlotWireframe);
            ed.WriteMessage("\n\tShadePlot                 : " + viewport.ShadePlot);
            ed.WriteMessage("\n\tShadePlotId               : " + viewport.ShadePlotId);
            ed.WriteMessage("\n\tSnapAngle                 : " + viewport.SnapAngle);
            ed.WriteMessage("\n\tSnapBasePoint             : " + viewport.SnapBasePoint);
            ed.WriteMessage("\n\tSnapIncrement             : " + viewport.SnapIncrement);
            ed.WriteMessage("\n\tSnapIsometric             : " + viewport.SnapIsometric);
            ed.WriteMessage("\n\tSnapIsoPair               : " + viewport.SnapIsoPair);
            ed.WriteMessage("\n\tSnapOn                    : " + viewport.SnapOn);
            ed.WriteMessage("\n\tStandardScale             : " + viewport.StandardScale);
            ed.WriteMessage("\n\tSunId                     : " + viewport.SunId);
            ed.WriteMessage("\n\tThumbnail                 : " + viewport.Thumbnail);
            ed.WriteMessage("\n\tToneOperatorParameters    : " + viewport.ToneOperatorParameters);
            ed.WriteMessage("\n\tTransparent               : " + viewport.Transparent);
            ed.WriteMessage("\n\tTwistAngle                : " + viewport.TwistAngle);
            ed.WriteMessage("\n\tUcsFollowModeOn           : " + viewport.UcsFollowModeOn);
            ed.WriteMessage("\n\tUcs                       : " + viewport.Ucs);
            ed.WriteMessage("\n\tUcsIconAtOrigin           : " + viewport.UcsIconAtOrigin);
            ed.WriteMessage("\n\tUcsIconVisible            : " + viewport.UcsIconVisible);
            ed.WriteMessage("\n\tUcsName                   : " + viewport.UcsName);
            ed.WriteMessage("\n\tUcsOrthographic           : " + viewport.UcsOrthographic);
            ed.WriteMessage("\n\tUcsPerViewport            : " + viewport.UcsPerViewport);
            ed.WriteMessage("\n\tViewCenter                : " + viewport.ViewCenter);
            ed.WriteMessage("\n\tViewDirection             : " + viewport.ViewDirection);
            ed.WriteMessage("\n\tViewHeight                : " + viewport.ViewHeight);
            ed.WriteMessage("\n\tViewOrthographic          : " + viewport.ViewOrthographic);
            ed.WriteMessage("\n\tViewTarget                : " + viewport.ViewTarget);
            ed.WriteMessage("\n\tVisualStyleId             : " + viewport.VisualStyleId);
            ed.WriteMessage("\n\tWidth                     : " + viewport.Width);
        }
    }
}
