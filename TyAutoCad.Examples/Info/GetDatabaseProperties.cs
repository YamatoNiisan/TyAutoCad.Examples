using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetDatabaseProperties))]
namespace TyAutoCad.Examples
{
    public class GetDatabaseProperties
    {
        /// <summary>
        /// Database のプロパティを取得する
        ///     現在図面の Database のプロパティを取得して出力する。
        /// </summary>
        [CommandMethod("GetDatabaseProperties")]
        public void Command()
        {
            // Document, Editor, Databaseを取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            var ed = doc.Editor;
            var db = doc.Database;

            try
            {
                // Databaseのプロパティを出力
                ed.WriteMessage("\n\n--- Database Properties ---");
                ed.WriteMessage("\n\tAcadDatabase                          : " + db.AcadDatabase);
                ed.WriteMessage("\n\tAllowExtendedNames                    : " + db.AllowExtendedNames);
                ed.WriteMessage("\n\tAngbase                               : " + db.Angbase);
                ed.WriteMessage("\n\tAngdir                                : " + db.Angdir);
                ed.WriteMessage("\n\tAnnoAllVisible                        : " + db.AnnoAllVisible);
                ed.WriteMessage("\n\tAnnotativeDwg                         : " + db.AnnotativeDwg);
                ed.WriteMessage("\n\tApproxNumObjects                      : " + db.ApproxNumObjects);
                ed.WriteMessage("\n\tAttmode                               : " + db.Attmode);
                ed.WriteMessage("\n\tAunits                                : " + db.Aunits);
                ed.WriteMessage("\n\tAuprec                                : " + db.Auprec);
                ed.WriteMessage("\n\tByBlockLinetype                       : " + db.ByBlockLinetype);
                ed.WriteMessage("\n\tByLayerLinetype                       : " + db.ByLayerLinetype);
                ed.WriteMessage("\n\tCameraDisplay                         : " + db.CameraDisplay);
                ed.WriteMessage("\n\tCameraHeight                          : " + db.CameraHeight);
                ed.WriteMessage("\n\tCannoscale                            : " + db.Cannoscale);
                ed.WriteMessage("\n\tCecolor                               : " + db.Cecolor);
                ed.WriteMessage("\n\tCeltscale                             : " + db.Celtscale);
                ed.WriteMessage("\n\tCeltype                               : " + db.Celtype);
                ed.WriteMessage("\n\tCelweight                             : " + db.Celweight);
                ed.WriteMessage("\n\tCetransparency                        : " + db.Cetransparency);
                ed.WriteMessage("\n\tChamfera                              : " + db.Chamfera);
                ed.WriteMessage("\n\tChamferb                              : " + db.Chamferb);
                ed.WriteMessage("\n\tChamferc                              : " + db.Chamferc);
                ed.WriteMessage("\n\tChamferd                              : " + db.Chamferd);
                ed.WriteMessage("\n\tClayer                                : " + db.Clayer);
                ed.WriteMessage("\n\tCmaterial                             : " + db.Cmaterial);
                ed.WriteMessage("\n\tCmljust                               : " + db.Cmljust);
                ed.WriteMessage("\n\tCmlscale                              : " + db.Cmlscale);
                ed.WriteMessage("\n\tCmlstyleID                            : " + db.CmlstyleID);
                ed.WriteMessage("\n\tColorDictionaryId                     : " + db.ColorDictionaryId);
                ed.WriteMessage("\n\tContinuousLinetype                    : " + db.ContinuousLinetype);
                ed.WriteMessage("\n\tCshadow                               : " + db.Cshadow);
                ed.WriteMessage("\n\tCurrentSpaceId                        : " + db.CurrentSpaceId);
                ed.WriteMessage("\n\tCurrentViewportTableRecordId          : " + db.CurrentViewportTableRecordId);
                ed.WriteMessage("\n\tDataLinkDictionaryId                  : " + db.DataLinkDictionaryId);
                ed.WriteMessage("\n\tDataLinkManager                       : " + db.DataLinkManager);
                ed.WriteMessage("\n\tDetailViewStyle                       : " + db.DetailViewStyle);
                ed.WriteMessage("\n\tDetailViewStyleDictionaryId           : " + db.DetailViewStyleDictionaryId);
                ed.WriteMessage("\n\tDgnFrame                              : " + db.DgnFrame);
                ed.WriteMessage("\n\tDimadec                               : " + db.Dimadec);
                ed.WriteMessage("\n\tDimalt                                : " + db.Dimalt);
                ed.WriteMessage("\n\tDimaltd                               : " + db.Dimaltd);
                ed.WriteMessage("\n\tDimaltf                               : " + db.Dimaltf);
                ed.WriteMessage("\n\tDimaltrnd                             : " + db.Dimaltrnd);
                ed.WriteMessage("\n\tDimalttd                              : " + db.Dimalttd);
                ed.WriteMessage("\n\tDimalttz                              : " + db.Dimalttz);
                ed.WriteMessage("\n\tDimaltu                               : " + db.Dimaltu);
                ed.WriteMessage("\n\tDimaltz                               : " + db.Dimaltz);
                ed.WriteMessage("\n\tDimapost                              : " + db.Dimapost);
                ed.WriteMessage("\n\tDimarcsym                             : " + db.Dimarcsym);
                ed.WriteMessage("\n\tDimaso                                : " + db.Dimaso);
                ed.WriteMessage("\n\tDimAssoc                              : " + db.DimAssoc);
                ed.WriteMessage("\n\tDimasz                                : " + db.Dimasz);
                ed.WriteMessage("\n\tDimatfit                              : " + db.Dimatfit);
                ed.WriteMessage("\n\tDimaunit                              : " + db.Dimaunit);
                ed.WriteMessage("\n\tDimazin                               : " + db.Dimazin);
                ed.WriteMessage("\n\tDimblk                                : " + db.Dimblk);
                ed.WriteMessage("\n\tDimblk1                               : " + db.Dimblk1);
                ed.WriteMessage("\n\tDimblk2                               : " + db.Dimblk2);
                ed.WriteMessage("\n\tDimcen                                : " + db.Dimcen);
                ed.WriteMessage("\n\tDimclrd                               : " + db.Dimclrd);
                ed.WriteMessage("\n\tDimclre                               : " + db.Dimclre);
                ed.WriteMessage("\n\tDimclrt                               : " + db.Dimclrt);
                ed.WriteMessage("\n\tDimdec                                : " + db.Dimdec);
                ed.WriteMessage("\n\tDimdle                                : " + db.Dimdle);
                ed.WriteMessage("\n\tDimdli                                : " + db.Dimdli);
                ed.WriteMessage("\n\tDimdsep                               : " + db.Dimdsep);
                ed.WriteMessage("\n\tDimexe                                : " + db.Dimexe);
                ed.WriteMessage("\n\tDimexo                                : " + db.Dimexo);
                ed.WriteMessage("\n\tDimfrac                               : " + db.Dimfrac);
                ed.WriteMessage("\n\tDimfxlen                              : " + db.Dimfxlen);
                ed.WriteMessage("\n\tDimfxlenOn                            : " + db.DimfxlenOn);
                ed.WriteMessage("\n\tDimgap                                : " + db.Dimgap);
                ed.WriteMessage("\n\tDimjogang                             : " + db.Dimjogang);
                ed.WriteMessage("\n\tDimjust                               : " + db.Dimjust);
                ed.WriteMessage("\n\tDimldrblk                             : " + db.Dimldrblk);
                ed.WriteMessage("\n\tDimlfac                               : " + db.Dimlfac);
                ed.WriteMessage("\n\tDimlim                                : " + db.Dimlim);
                ed.WriteMessage("\n\tDimltex1                              : " + db.Dimltex1);
                ed.WriteMessage("\n\tDimltex2                              : " + db.Dimltex2);
                ed.WriteMessage("\n\tDimltype                              : " + db.Dimltype);
                ed.WriteMessage("\n\tDimlunit                              : " + db.Dimlunit);
                ed.WriteMessage("\n\tDimlwd                                : " + db.Dimlwd);
                ed.WriteMessage("\n\tDimlwe                                : " + db.Dimlwe);
                ed.WriteMessage("\n\tDimpost                               : " + db.Dimpost);
                ed.WriteMessage("\n\tDimrnd                                : " + db.Dimrnd);
                ed.WriteMessage("\n\tDimsah                                : " + db.Dimsah);
                ed.WriteMessage("\n\tDimscale                              : " + db.Dimscale);
                ed.WriteMessage("\n\tDimsd1                                : " + db.Dimsd1);
                ed.WriteMessage("\n\tDimsd2                                : " + db.Dimsd2);
                ed.WriteMessage("\n\tDimse1                                : " + db.Dimse1);
                ed.WriteMessage("\n\tDimse2                                : " + db.Dimse2);
                ed.WriteMessage("\n\tDimsho                                : " + db.Dimsho);
                ed.WriteMessage("\n\tDimsoxd                               : " + db.Dimsoxd);
                ed.WriteMessage("\n\tDimstyle                              : " + db.Dimstyle);
                ed.WriteMessage("\n\tDimStyleTableId                       : " + db.DimStyleTableId);
                ed.WriteMessage("\n\tDimtad                                : " + db.Dimtad);
                ed.WriteMessage("\n\tDimtdec                               : " + db.Dimtdec);
                ed.WriteMessage("\n\tDimtfac                               : " + db.Dimtfac);
                ed.WriteMessage("\n\tDimtfill                              : " + db.Dimtfill);
                ed.WriteMessage("\n\tDimtfillclr                           : " + db.Dimtfillclr);
                ed.WriteMessage("\n\tDimtih                                : " + db.Dimtih);
                ed.WriteMessage("\n\tDimtix                                : " + db.Dimtix);
                ed.WriteMessage("\n\tDimtm                                 : " + db.Dimtm);
                ed.WriteMessage("\n\tDimtmove                              : " + db.Dimtmove);
                ed.WriteMessage("\n\tDimtofl                               : " + db.Dimtofl);
                ed.WriteMessage("\n\tDimtoh                                : " + db.Dimtoh);
                ed.WriteMessage("\n\tDimtol                                : " + db.Dimtol);
                ed.WriteMessage("\n\tDimtolj                               : " + db.Dimtolj);
                ed.WriteMessage("\n\tDimtp                                 : " + db.Dimtp);
                ed.WriteMessage("\n\tDimtsz                                : " + db.Dimtsz);
                ed.WriteMessage("\n\tDimtvp                                : " + db.Dimtvp);
                ed.WriteMessage("\n\tDimtxsty                              : " + db.Dimtxsty);
                ed.WriteMessage("\n\tDimtxt                                : " + db.Dimtxt);
                ed.WriteMessage("\n\tDimtzin                               : " + db.Dimtzin);
                ed.WriteMessage("\n\tDimupt                                : " + db.Dimupt);
                ed.WriteMessage("\n\tDimzin                                : " + db.Dimzin);
                ed.WriteMessage("\n\tDispSilh                              : " + db.DispSilh);
                ed.WriteMessage("\n\tdragvs                                : " + db.dragvs);
                ed.WriteMessage("\n\tDwfFrame                              : " + db.DwfFrame);
                ed.WriteMessage("\n\tDwgFileWasSavedByAutodeskSoftware     : " + db.DwgFileWasSavedByAutodeskSoftware);
                ed.WriteMessage("\n\tDxEval                                : " + db.DxEval);
                ed.WriteMessage("\n\tElevation                             : " + db.Elevation);
                ed.WriteMessage("\n\tEndCaps                               : " + db.EndCaps);
                ed.WriteMessage("\n\tExtmax                                : " + db.Extmax);
                ed.WriteMessage("\n\tExtmin                                : " + db.Extmin);
                ed.WriteMessage("\n\tFacetres                              : " + db.Facetres);
                ed.WriteMessage("\n\tFilename                              : " + db.Filename);
                ed.WriteMessage("\n\tFilletrad                             : " + db.Filletrad);
                ed.WriteMessage("\n\tFillmode                              : " + db.Fillmode);
                ed.WriteMessage("\n\tFingerprintGuid                       : " + db.FingerprintGuid);

                //ed.WriteMessage("\n\tGeoDataObject                         : " + db.GeoDataObject); // eKeyNotFound

                ed.WriteMessage("\n\tGroupDictionaryId                     : " + db.GroupDictionaryId);
                ed.WriteMessage("\n\tHaloGap                               : " + db.HaloGap);
                ed.WriteMessage("\n\tHandseed                              : " + db.Handseed);
                ed.WriteMessage("\n\tHideText                              : " + db.HideText);

                //ed.WriteMessage("\n\tHomeView                              : " + db.HomeView); // eKeyNotFound

                ed.WriteMessage("\n\tHpInherit                             : " + db.HpInherit);
                ed.WriteMessage("\n\tHpOrigin                              : " + db.HpOrigin);
                ed.WriteMessage("\n\tHyperlinkBase                         : " + db.HyperlinkBase);
                ed.WriteMessage("\n\tIndexctl                              : " + db.Indexctl);
                ed.WriteMessage("\n\tInsbase                               : " + db.Insbase);
                ed.WriteMessage("\n\tInsunits                              : " + db.Insunits);
                ed.WriteMessage("\n\tInterferecolor                        : " + db.Interferecolor);
                ed.WriteMessage("\n\tInterfereobjvs                        : " + db.Interfereobjvs);
                ed.WriteMessage("\n\tInterferevpvs                         : " + db.Interferevpvs);
                ed.WriteMessage("\n\tIntersectColor                        : " + db.IntersectColor);
                ed.WriteMessage("\n\tIntersectDisplay                      : " + db.IntersectDisplay);
                ed.WriteMessage("\n\tIsBeingDestroyed                      : " + db.IsBeingDestroyed);
                ed.WriteMessage("\n\tIsEmr	                               : " + db.IsEmr); // new
                ed.WriteMessage("\n\tIsolines                              : " + db.Isolines);
                ed.WriteMessage("\n\tIsPartiallyOpened                     : " + db.IsPartiallyOpened);
                ed.WriteMessage("\n\tJoinStyle                             : " + db.JoinStyle);
                ed.WriteMessage("\n\tLastSavedAsMaintenanceVersion         : " + db.LastSavedAsMaintenanceVersion);
                ed.WriteMessage("\n\tLastSavedAsVersion                    : " + db.LastSavedAsVersion);
                ed.WriteMessage("\n\tLatitude                              : " + db.Latitude);
                ed.WriteMessage("\n\tLayerEval                             : " + db.LayerEval);
                ed.WriteMessage("\n\tLayerFilters                          : " + db.LayerFilters);
                ed.WriteMessage("\n\tLayerNotify                           : " + db.LayerNotify);
                ed.WriteMessage("\n\tLayerStateManager                     : " + db.LayerStateManager);
                ed.WriteMessage("\n\tLayerTableId                          : " + db.LayerTableId);
                ed.WriteMessage("\n\tLayerZero                             : " + db.LayerZero);
                ed.WriteMessage("\n\tLayoutDictionaryId                    : " + db.LayoutDictionaryId);
                ed.WriteMessage("\n\tLensLength                            : " + db.LensLength);
                ed.WriteMessage("\n\tLightGlyphDisplay                     : " + db.LightGlyphDisplay);
                ed.WriteMessage("\n\tLightingUnits                         : " + db.LightingUnits);
                ed.WriteMessage("\n\tLightsInBlocks                        : " + db.LightsInBlocks);
                ed.WriteMessage("\n\tLimcheck                              : " + db.Limcheck);
                ed.WriteMessage("\n\tLimmax                                : " + db.Limmax);
                ed.WriteMessage("\n\tLimmin                                : " + db.Limmin);
                ed.WriteMessage("\n\tLinetypeTableId                       : " + db.LinetypeTableId);
                ed.WriteMessage("\n\tLineWeightDisplay                     : " + db.LineWeightDisplay);
                ed.WriteMessage("\n\tLoftAng1                              : " + db.LoftAng1);
                ed.WriteMessage("\n\tLoftAng2                              : " + db.LoftAng2);
                ed.WriteMessage("\n\tLoftMag1                              : " + db.LoftMag1);
                ed.WriteMessage("\n\tLoftMag2                              : " + db.LoftMag2);
                ed.WriteMessage("\n\tLoftNormals                           : " + db.LoftNormals);
                ed.WriteMessage("\n\tLoftParam                             : " + db.LoftParam);
                ed.WriteMessage("\n\tLongitude                             : " + db.Longitude);
                ed.WriteMessage("\n\tLtscale                               : " + db.Ltscale);
                ed.WriteMessage("\n\tLunits                                : " + db.Lunits);
                ed.WriteMessage("\n\tLuprec                                : " + db.Luprec);
                ed.WriteMessage("\n\tMaintenanceReleaseVersion             : " + db.MaintenanceReleaseVersion);
                ed.WriteMessage("\n\tMaterialDictionaryId                  : " + db.MaterialDictionaryId);
                ed.WriteMessage("\n\tMaxactvp                              : " + db.Maxactvp);
                ed.WriteMessage("\n\tMeasurement                           : " + db.Measurement);
                ed.WriteMessage("\n\tMenu                                  : " + db.Menu);
                ed.WriteMessage("\n\tMirrtext                              : " + db.Mirrtext);
                ed.WriteMessage("\n\tMLeaderscale                          : " + db.MLeaderscale);
                ed.WriteMessage("\n\tMLeaderstyle                          : " + db.MLeaderstyle);
                ed.WriteMessage("\n\tMLeaderStyleDictionaryId              : " + db.MLeaderStyleDictionaryId);
                ed.WriteMessage("\n\tMLStyleDictionaryId                   : " + db.MLStyleDictionaryId);
                ed.WriteMessage("\n\tMsLtScale                             : " + db.MsLtScale);
                ed.WriteMessage("\n\tMsOleScale                            : " + db.MsOleScale);
                ed.WriteMessage("\n\tNamedObjectsDictionaryId              : " + db.NamedObjectsDictionaryId);
                ed.WriteMessage("\n\tNeedsRecovery                         : " + db.NeedsRecovery);
                ed.WriteMessage("\n\tNorthDirection                        : " + db.NorthDirection);
                ed.WriteMessage("\n\tNumberOfSaves                         : " + db.NumberOfSaves);
                ed.WriteMessage("\n\tObjectContextManager                  : " + db.ObjectContextManager);
                ed.WriteMessage("\n\tObscuredColor                         : " + db.ObscuredColor);
                ed.WriteMessage("\n\tObscuredLineType                      : " + db.ObscuredLineType);
                ed.WriteMessage("\n\tOleStartUp                            : " + db.OleStartUp);
                ed.WriteMessage("\n\tOriginalFileMaintenanceVersion        : " + db.OriginalFileMaintenanceVersion);
                ed.WriteMessage("\n\tOriginalFileName                      : " + db.OriginalFileName);
                ed.WriteMessage("\n\tOriginalFileSavedByMaintenanceVersion : " + db.OriginalFileSavedByMaintenanceVersion);
                ed.WriteMessage("\n\tOriginalFileSavedByVersion            : " + db.OriginalFileSavedByVersion);
                ed.WriteMessage("\n\tOriginalFileVersion                   : " + db.OriginalFileVersion);
                ed.WriteMessage("\n\tOrthomode                             : " + db.Orthomode);
                ed.WriteMessage("\n\tPaperSpaceVportId                     : " + db.PaperSpaceVportId);
                ed.WriteMessage("\n\tPdfFrame                              : " + db.PdfFrame);
                ed.WriteMessage("\n\tPdmode                                : " + db.Pdmode);
                ed.WriteMessage("\n\tPdsize                                : " + db.Pdsize);
                ed.WriteMessage("\n\tPelevation                            : " + db.Pelevation);
                ed.WriteMessage("\n\tPextmax                               : " + db.Pextmax);
                ed.WriteMessage("\n\tPextmin                               : " + db.Pextmin);
                ed.WriteMessage("\n\tPinsbase                              : " + db.Pinsbase);
                ed.WriteMessage("\n\tPlimcheck                             : " + db.Plimcheck);
                ed.WriteMessage("\n\tPlimmax                               : " + db.Plimmax);
                ed.WriteMessage("\n\tPlimmin                               : " + db.Plimmin);
                ed.WriteMessage("\n\tPlineEllipse                          : " + db.PlineEllipse);
                ed.WriteMessage("\n\tPlinegen                              : " + db.Plinegen);
                ed.WriteMessage("\n\tPlinewid                              : " + db.Plinewid);
                ed.WriteMessage("\n\tPlotSettingsDictionaryId              : " + db.PlotSettingsDictionaryId);
                ed.WriteMessage("\n\tPlotStyleMode                         : " + db.PlotStyleMode);
                ed.WriteMessage("\n\tPlotStyleNameDictionaryId             : " + db.PlotStyleNameDictionaryId);
                ed.WriteMessage("\n\tPlotStyleNameId                       : " + db.PlotStyleNameId);
                ed.WriteMessage("\n\tProjectName                           : " + db.ProjectName);
                ed.WriteMessage("\n\tPsltscale                             : " + db.Psltscale);
                ed.WriteMessage("\n\tPsolHeight                            : " + db.PsolHeight);
                ed.WriteMessage("\n\tPsolWidth                             : " + db.PsolWidth);

                //ed.WriteMessage("\n\tPucs                                  : " + db.Pucs); // Write only

                ed.WriteMessage("\n\tPucsBase                              : " + db.PucsBase);
                ed.WriteMessage("\n\tPucsname                              : " + db.Pucsname);
                ed.WriteMessage("\n\tPucsorg                               : " + db.Pucsorg);
                ed.WriteMessage("\n\tPucsOrthographic                      : " + db.PucsOrthographic);
                ed.WriteMessage("\n\tPucsxdir                              : " + db.Pucsxdir);
                ed.WriteMessage("\n\tPucsydir                              : " + db.Pucsydir);
                ed.WriteMessage("\n\tQtextmode                             : " + db.Qtextmode);
                ed.WriteMessage("\n\tRegAppTableId                         : " + db.RegAppTableId);
                ed.WriteMessage("\n\tRegenmode                             : " + db.Regenmode);
                ed.WriteMessage("\n\tRetainOriginalThumbnailBitmap         : " + db.RetainOriginalThumbnailBitmap);
                ed.WriteMessage("\n\tSaveproxygraphics                     : " + db.Saveproxygraphics);
                ed.WriteMessage("\n\tSectionManagerId                      : " + db.SectionManagerId);
                ed.WriteMessage("\n\tSectionViewStyle                      : " + db.SectionViewStyle);
                ed.WriteMessage("\n\tSectionViewStyleDictionaryId          : " + db.SectionViewStyleDictionaryId);
                ed.WriteMessage("\n\tSecurityParameters                    : " + db.SecurityParameters);
                ed.WriteMessage("\n\tShadedge                              : " + db.Shadedge);
                ed.WriteMessage("\n\tShadedif                              : " + db.Shadedif);
                ed.WriteMessage("\n\tShadowPlaneLocation                   : " + db.ShadowPlaneLocation);
                ed.WriteMessage("\n\tShowHist                              : " + db.ShowHist);
                ed.WriteMessage("\n\tSketchinc                             : " + db.Sketchinc);
                ed.WriteMessage("\n\tSkpoly                                : " + db.Skpoly);
                ed.WriteMessage("\n\tSolidHist                             : " + db.SolidHist);
                ed.WriteMessage("\n\tSortEnts                              : " + db.SortEnts);
                ed.WriteMessage("\n\tSplframe                              : " + db.Splframe);
                ed.WriteMessage("\n\tSplinesegs                            : " + db.Splinesegs);
                ed.WriteMessage("\n\tSplinetype                            : " + db.Splinetype);
                ed.WriteMessage("\n\tStepSize                              : " + db.StepSize);
                ed.WriteMessage("\n\tStepsPerSec                           : " + db.StepsPerSec);
                ed.WriteMessage("\n\tStyleSheet                            : " + db.StyleSheet);
                ed.WriteMessage("\n\tSummaryInfo                           : " + db.SummaryInfo);
                ed.WriteMessage("\n\tSurftab1                              : " + db.Surftab1);
                ed.WriteMessage("\n\tSurftab2                              : " + db.Surftab2);
                ed.WriteMessage("\n\tSurftype                              : " + db.Surftype);
                ed.WriteMessage("\n\tSurfu                                 : " + db.Surfu);
                ed.WriteMessage("\n\tSurfv                                 : " + db.Surfv);
                ed.WriteMessage("\n\tTablestyle                            : " + db.Tablestyle);
                ed.WriteMessage("\n\tTableStyleDictionaryId                : " + db.TableStyleDictionaryId);
                ed.WriteMessage("\n\tTdcreate                              : " + db.Tdcreate);
                ed.WriteMessage("\n\tTdindwg                               : " + db.Tdindwg);
                ed.WriteMessage("\n\tTducreate                             : " + db.Tducreate);
                ed.WriteMessage("\n\tTdupdate                              : " + db.Tdupdate);
                ed.WriteMessage("\n\tTdusrtimer                            : " + db.Tdusrtimer);
                ed.WriteMessage("\n\tTduupdate                             : " + db.Tduupdate);
                ed.WriteMessage("\n\tTextsize                              : " + db.Textsize);
                ed.WriteMessage("\n\tTextstyle                             : " + db.Textstyle);
                ed.WriteMessage("\n\tTextStyleTableId                      : " + db.TextStyleTableId);
                ed.WriteMessage("\n\tThickness                             : " + db.Thickness);
                ed.WriteMessage("\n\tThumbnailBitmap                       : " + db.ThumbnailBitmap);
                ed.WriteMessage("\n\tTileMode                              : " + db.TileMode);
                ed.WriteMessage("\n\tTileModeLightSynch                    : " + db.TileModeLightSynch);
                ed.WriteMessage("\n\tTimeZone                              : " + db.TimeZone);
                ed.WriteMessage("\n\tTracewid                              : " + db.Tracewid);
                ed.WriteMessage("\n\tTransactionManager                    : " + db.TransactionManager);
                ed.WriteMessage("\n\tTreedepth                             : " + db.Treedepth);
                ed.WriteMessage("\n\tTStackAlign                           : " + db.TStackAlign);
                ed.WriteMessage("\n\tTstackSize                            : " + db.TstackSize);

                //ed.WriteMessage("\n\tUcs                                   : " + db.Ucs); // getなし

                ed.WriteMessage("\n\tUcsBase                               : " + db.UcsBase);
                ed.WriteMessage("\n\tUcsname                               : " + db.Ucsname);
                ed.WriteMessage("\n\tUcsorg                                : " + db.Ucsorg);
                ed.WriteMessage("\n\tUcsOrthographic                       : " + db.UcsOrthographic);
                ed.WriteMessage("\n\tUcsTableId                            : " + db.UcsTableId);
                ed.WriteMessage("\n\tUcsxdir                               : " + db.Ucsxdir);
                ed.WriteMessage("\n\tUcsydir                               : " + db.Ucsydir);
                ed.WriteMessage("\n\tUndoRecording                         : " + db.UndoRecording);
                ed.WriteMessage("\n\tUnitmode                              : " + db.Unitmode);
                ed.WriteMessage("\n\tUpdateThumbnail                       : " + db.UpdateThumbnail);
                ed.WriteMessage("\n\tUseri1                                : " + db.Useri1);
                ed.WriteMessage("\n\tUseri2                                : " + db.Useri2);
                ed.WriteMessage("\n\tUseri3                                : " + db.Useri3);
                ed.WriteMessage("\n\tUseri4                                : " + db.Useri4);
                ed.WriteMessage("\n\tUseri5                                : " + db.Useri5);
                ed.WriteMessage("\n\tUserr1                                : " + db.Userr1);
                ed.WriteMessage("\n\tUserr2                                : " + db.Userr2);
                ed.WriteMessage("\n\tUserr3                                : " + db.Userr3);
                ed.WriteMessage("\n\tUserr4                                : " + db.Userr4);
                ed.WriteMessage("\n\tUserr5                                : " + db.Userr5);
                ed.WriteMessage("\n\tUsrtimer                              : " + db.Usrtimer);
                ed.WriteMessage("\n\tVersionGuid                           : " + db.VersionGuid);
                ed.WriteMessage("\n\tViewportScaleDefault                  : " + db.ViewportScaleDefault);
                ed.WriteMessage("\n\tViewportTableId                       : " + db.ViewportTableId);
                ed.WriteMessage("\n\tViewTableId                           : " + db.ViewTableId);
                ed.WriteMessage("\n\tVisretain                             : " + db.Visretain);
                ed.WriteMessage("\n\tVisualStyleDictionaryId               : " + db.VisualStyleDictionaryId);
                ed.WriteMessage("\n\tWorldview                             : " + db.Worldview);
                ed.WriteMessage("\n\tXclipFrame                            : " + db.XclipFrame);
                ed.WriteMessage("\n\tXrefBlockId                           : " + db.XrefBlockId);
                ed.WriteMessage("\n\tXrefEditEnabled                       : " + db.XrefEditEnabled);
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                ed.WriteMessage("Error GetDatabaseProperties\n\t" + ex.Message);
            }
        }
    }
}
