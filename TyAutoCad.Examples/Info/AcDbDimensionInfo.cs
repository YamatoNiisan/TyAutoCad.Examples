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
    /// Dimension クラス情報
    /// </summary>
    public static class AcDbDimensionInfo
    {
        /// <summary>
        /// Dimension のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="dimension"></param>
        public static void OutputDimensionProperties(Editor ed, Dimension dimension)
        {
            ed.WriteMessage("\n\n--- Dimension Properties ---");
            ed.WriteMessage("\nAlternatePrefix                   : " + dimension.AlternatePrefix);
            ed.WriteMessage("\nAlternateSuffix                   : " + dimension.AlternateSuffix);
            ed.WriteMessage("\nAltSuppressLeadingZeros           : " + dimension.AltSuppressLeadingZeros);
            ed.WriteMessage("\nAltSuppressTrailingZeros          : " + dimension.AltSuppressTrailingZeros);
            ed.WriteMessage("\nAltSuppressZeroFeet               : " + dimension.AltSuppressZeroFeet);
            ed.WriteMessage("\nAltSuppressZeroInches             : " + dimension.AltSuppressZeroInches);
            ed.WriteMessage("\nAltToleranceSuppressLeadingZeros	 : " + dimension.AltToleranceSuppressLeadingZeros);
            ed.WriteMessage("\nAltToleranceSuppressTrailingZeros : " + dimension.AltToleranceSuppressTrailingZeros);
            ed.WriteMessage("\nAltToleranceSuppressZeroFeet      : " + dimension.AltToleranceSuppressZeroFeet);
            ed.WriteMessage("\nAltToleranceSuppressZeroInches    : " + dimension.AltToleranceSuppressZeroInches);
            ed.WriteMessage("\nCenterMarkSize                    : " + dimension.CenterMarkSize);
            ed.WriteMessage("\nCenterMarkType                    : " + dimension.CenterMarkType);
            ed.WriteMessage("\nDimadec                           : " + dimension.Dimadec);
            ed.WriteMessage("\nDimalt                            : " + dimension.Dimalt);
            ed.WriteMessage("\nDimaltd                           : " + dimension.Dimaltd);
            ed.WriteMessage("\nDimaltf                           : " + dimension.Dimaltf);
            ed.WriteMessage("\nDimaltrnd                         : " + dimension.Dimaltrnd);
            ed.WriteMessage("\nDimalttd                          : " + dimension.Dimalttd);
            ed.WriteMessage("\nDimalttz                          : " + dimension.Dimalttz);
            ed.WriteMessage("\nDimaltu                           : " + dimension.Dimaltu);
            ed.WriteMessage("\nDimaltz                           : " + dimension.Dimaltz);
            ed.WriteMessage("\nDimapost                          : " + dimension.Dimapost);
            ed.WriteMessage("\nDimarcsym                         : " + dimension.Dimarcsym);
            ed.WriteMessage("\nDimasz                            : " + dimension.Dimasz);
            ed.WriteMessage("\nDimatfit                          : " + dimension.Dimatfit);
            ed.WriteMessage("\nDimaunit                          : " + dimension.Dimaunit);
            ed.WriteMessage("\nDimazin                           : " + dimension.Dimazin);
            ed.WriteMessage("\nDimblk                            : " + dimension.Dimblk);
            ed.WriteMessage("\nDimblk1                           : " + dimension.Dimblk1);
            ed.WriteMessage("\nDimblk2                           : " + dimension.Dimblk2);
            ed.WriteMessage("\nDimBlockId                        : " + dimension.DimBlockId);
            ed.WriteMessage("\nDimcen                            : " + dimension.Dimcen);
            ed.WriteMessage("\nDimclrd                           : " + dimension.Dimclrd);
            ed.WriteMessage("\nDimclre                           : " + dimension.Dimclre);
            ed.WriteMessage("\nDimclrt                           : " + dimension.Dimclrt);
            ed.WriteMessage("\nDimdec                            : " + dimension.Dimdec);
            ed.WriteMessage("\nDimdle                            : " + dimension.Dimdle);
            ed.WriteMessage("\nDimdli                            : " + dimension.Dimdli);
            ed.WriteMessage("\nDimdsep                           : " + dimension.Dimdsep);
            ed.WriteMessage("\nDimensionStyle                    : " + dimension.DimensionStyle);
            ed.WriteMessage("\nDimensionStyleName                : " + dimension.DimensionStyleName);
            ed.WriteMessage("\nDimensionText                     : " + dimension.DimensionText); // 寸法値
            ed.WriteMessage("\nDimexe                            : " + dimension.Dimexe);
            ed.WriteMessage("\nDimexo                            : " + dimension.Dimexo);
            ed.WriteMessage("\nDimfrac                           : " + dimension.Dimfrac);
            ed.WriteMessage("\nDimfxlen                          : " + dimension.Dimfxlen);
            ed.WriteMessage("\nDimfxlenOn                        : " + dimension.DimfxlenOn);
            ed.WriteMessage("\nDimgap                            : " + dimension.Dimgap);
            ed.WriteMessage("\nDimjogang                         : " + dimension.Dimjogang);
            ed.WriteMessage("\nDimjust                           : " + dimension.Dimjust);
            ed.WriteMessage("\nDimldrblk                         : " + dimension.Dimldrblk);
            ed.WriteMessage("\nDimlfac                           : " + dimension.Dimlfac); // 長さの寸法尺度
            ed.WriteMessage("\nDimlim                            : " + dimension.Dimlim);
            ed.WriteMessage("\nDimltex1                          : " + dimension.Dimltex1);
            ed.WriteMessage("\nDimltex2                          : " + dimension.Dimltex2);
            ed.WriteMessage("\nDimltype                          : " + dimension.Dimltype);
            ed.WriteMessage("\nDimlunit                          : " + dimension.Dimlunit);
            ed.WriteMessage("\nDimlwd                            : " + dimension.Dimlwd);
            ed.WriteMessage("\nDimlwe                            : " + dimension.Dimlwe);
            ed.WriteMessage("\nDimpost                           : " + dimension.Dimpost);
            ed.WriteMessage("\nDimrnd                            : " + dimension.Dimrnd);
            ed.WriteMessage("\nDimsah                            : " + dimension.Dimsah);
            ed.WriteMessage("\nDimscale                          : " + dimension.Dimscale); // 全体の寸法尺度
            ed.WriteMessage("\nDimsd1                            : " + dimension.Dimsd1);
            ed.WriteMessage("\nDimsd2                            : " + dimension.Dimsd2);
            ed.WriteMessage("\nDimse1                            : " + dimension.Dimse1);
            ed.WriteMessage("\nDimse2                            : " + dimension.Dimse2);
            ed.WriteMessage("\nDimsoxd                           : " + dimension.Dimsoxd);
            ed.WriteMessage("\nDimtad                            : " + dimension.Dimtad);
            ed.WriteMessage("\nDimtdec                           : " + dimension.Dimtdec);
            ed.WriteMessage("\nDimtfac                           : " + dimension.Dimtfac);
            ed.WriteMessage("\nDimtfill                          : " + dimension.Dimtfill);
            ed.WriteMessage("\nDimtfillclr                       : " + dimension.Dimtfillclr);
            ed.WriteMessage("\nDimtih                            : " + dimension.Dimtih);
            ed.WriteMessage("\nDimtix                            : " + dimension.Dimtix);
            ed.WriteMessage("\nDimtm                             : " + dimension.Dimtm);
            ed.WriteMessage("\nDimtmove                          : " + dimension.Dimtmove);
            ed.WriteMessage("\nDimtofl                           : " + dimension.Dimtofl);
            ed.WriteMessage("\nDimtoh                            : " + dimension.Dimtoh);
            ed.WriteMessage("\nDimtol                            : " + dimension.Dimtol);
            ed.WriteMessage("\nDimtolj                           : " + dimension.Dimtolj);
            ed.WriteMessage("\nDimtp                             : " + dimension.Dimtp);
            ed.WriteMessage("\nDimtsz                            : " + dimension.Dimtsz);
            ed.WriteMessage("\nDimtvp                            : " + dimension.Dimtvp);
            ed.WriteMessage("\nDimtxt                            : " + dimension.Dimtxt);
            ed.WriteMessage("\nDimtzin                           : " + dimension.Dimtzin);
            ed.WriteMessage("\nDimupt                            : " + dimension.Dimupt);
            ed.WriteMessage("\nDimzin                            : " + dimension.Dimzin);
            ed.WriteMessage("\nDynamicDimension                  : " + dimension.DynamicDimension);
            ed.WriteMessage("\nElevation                         : " + dimension.Elevation);
            ed.WriteMessage("\nHorizontalRotation                : " + dimension.HorizontalRotation);
            ed.WriteMessage("\nMeasurement                       : " + dimension.Measurement);
            ed.WriteMessage("\nNormal                            : " + dimension.Normal);
            ed.WriteMessage("\nPrefix                            : " + dimension.Prefix);
            ed.WriteMessage("\nSuffix                            : " + dimension.Suffix);
            ed.WriteMessage("\nSuppressAngularLeadingZeros       : " + dimension.SuppressAngularLeadingZeros);
            ed.WriteMessage("\nSuppressAngularTrailingZeros      : " + dimension.SuppressAngularTrailingZeros);
            ed.WriteMessage("\nSuppressLeadingZeros              : " + dimension.SuppressLeadingZeros);
            ed.WriteMessage("\nSuppressTrailingZeros             : " + dimension.SuppressTrailingZeros);
            ed.WriteMessage("\nSuppressZeroFeet                  : " + dimension.SuppressZeroFeet);
            ed.WriteMessage("\nSuppressZeroInches                : " + dimension.SuppressZeroInches);
            ed.WriteMessage("\nTextAttachment                    : " + dimension.TextAttachment);
            ed.WriteMessage("\nTextDefinedSize                   : " + dimension.TextDefinedSize);
            ed.WriteMessage("\nTextLineSpacingFactor             : " + dimension.TextLineSpacingFactor);
            ed.WriteMessage("\nTextLineSpacingStyle              : " + dimension.TextLineSpacingStyle);
            ed.WriteMessage("\nTextPosition                      : " + dimension.TextPosition);
            ed.WriteMessage("\nTextRotation                      : " + dimension.TextRotation);
            ed.WriteMessage("\nToleranceSuppressLeadingZeros     : " + dimension.ToleranceSuppressLeadingZeros);
            ed.WriteMessage("\nToleranceSuppressTrailingZeros    : " + dimension.ToleranceSuppressTrailingZeros);
            ed.WriteMessage("\nToleranceSuppressZeroFeet         : " + dimension.ToleranceSuppressZeroFeet);
            ed.WriteMessage("\nToleranceSuppressZeroInches       : " + dimension.ToleranceSuppressZeroInches);
            ed.WriteMessage("\nUsingDefaultTextPosition          : " + dimension.UsingDefaultTextPosition);
        }

        /// <summary>
        /// AlignedDimension のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="alignedDimension"></param>
        public static void OutputAlignedDimensionProperties(Editor ed, AlignedDimension alignedDimension)
        {
            ed.WriteMessage("\n\n--- AlignedDimension Properties ---");
            ed.WriteMessage("\nDimLinePoint : " + alignedDimension.DimLinePoint);
            ed.WriteMessage("\nOblique      : " + alignedDimension.Oblique);
            ed.WriteMessage("\nXLine1Point  : " + alignedDimension.XLine1Point);
            ed.WriteMessage("\nXLine2Point  : " + alignedDimension.XLine2Point);
        }

        /// <summary>
        /// ArcDimension のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="arcDimension"></param>
        public static void OutputArcDimensionProperties(Editor ed, ArcDimension arcDimension)
        {
            ed.WriteMessage("\n\n--- ArcDimension Properties ---");
            ed.WriteMessage("\nArcEndParam   : " + arcDimension.ArcEndParam);
            ed.WriteMessage("\nArcPoint      : " + arcDimension.ArcPoint);
            ed.WriteMessage("\nArcStartParam : " + arcDimension.ArcStartParam);
            ed.WriteMessage("\nArcSymbolType : " + arcDimension.ArcSymbolType);
            ed.WriteMessage("\nCenterPoint   : " + arcDimension.CenterPoint);
            ed.WriteMessage("\nHasLeader     : " + arcDimension.HasLeader);
            ed.WriteMessage("\nIsPartial     : " + arcDimension.IsPartial);
            ed.WriteMessage("\nLeader1Point  : " + arcDimension.Leader1Point);
            ed.WriteMessage("\nLeader2Point  : " + arcDimension.Leader2Point);
            ed.WriteMessage("\nXLine1Point   : " + arcDimension.XLine1Point);
            ed.WriteMessage("\nXLine2Pointn  : " + arcDimension.XLine2Point);
        }

        /// <summary>
        /// DiametricDimension のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="diametricDimension"></param>
        public static void OutputDiametricDimensionProperties(Editor ed, DiametricDimension diametricDimension)
        {
            ed.WriteMessage("\n\n--- DiametricDimension Properties ---");
            ed.WriteMessage("\nChordPoint    : " + diametricDimension.ChordPoint);
            ed.WriteMessage("\nFarChordPoint : " + diametricDimension.FarChordPoint);
            ed.WriteMessage("\nLeaderLength  : " + diametricDimension.LeaderLength);
        }

        /// <summary>
        /// LineAngularDimension2 のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="lineAngularDimension2"></param>
        public static void OutputLineAngularDimension2Properties(Editor ed, LineAngularDimension2 lineAngularDimension2)
        {
            ed.WriteMessage("\n\n--- LineAngularDimension2 Properties ---");
            ed.WriteMessage("\nArcPoint    : " + lineAngularDimension2.ArcPoint);
            ed.WriteMessage("\nXLine1End   : " + lineAngularDimension2.XLine1End);
            ed.WriteMessage("\nXLine1Start : " + lineAngularDimension2.XLine1Start);
            ed.WriteMessage("\nXLine2End   : " + lineAngularDimension2.XLine2End);
            ed.WriteMessage("\nXLine2Start : " + lineAngularDimension2.XLine2Start);
        }

        /// <summary>
        /// Point3AngularDimension のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="point3AngularDimension"></param>
        public static void OutputPoint3AngularDimensionProperties(Editor ed, Point3AngularDimension point3AngularDimension)
        {
            ed.WriteMessage("\n\n--- Point3AngularDimension Properties ---");
            ed.WriteMessage("\nArcPoint    : " + point3AngularDimension.ArcPoint);
            ed.WriteMessage("\nCenterPoint : " + point3AngularDimension.CenterPoint);
            ed.WriteMessage("\nXLine1Point : " + point3AngularDimension.XLine1Point);
            ed.WriteMessage("\nXLine2Point : " + point3AngularDimension.XLine2Point);
        }

        /// <summary>
        /// RadialDimension のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="radialDimension"></param>
        public static void OutputRadialDimensionProperties(Editor ed, RadialDimension radialDimension)
        {
            ed.WriteMessage("\n\n--- RadialDimension Properties ---");
            ed.WriteMessage("\nCenter       : " + radialDimension.Center);
            ed.WriteMessage("\nChordPoint   : " + radialDimension.ChordPoint);
            ed.WriteMessage("\nLeaderLength : " + radialDimension.LeaderLength);
        }

        /// <summary>
        /// RadialDimensionLarge のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="radialDimensionLarge"></param>
        public static void OutputRadialDimensionLargeProperties(Editor ed, RadialDimensionLarge radialDimensionLarge)
        {
            ed.WriteMessage("\n\n--- RadialDimensionLarge Properties ---");
            ed.WriteMessage("\nCenter         : " + radialDimensionLarge.Center);
            ed.WriteMessage("\nChordPoint     : " + radialDimensionLarge.ChordPoint);
            ed.WriteMessage("\nJogAngle       : " + radialDimensionLarge.JogAngle);
            ed.WriteMessage("\nJogPoint       : " + radialDimensionLarge.JogPoint);
            ed.WriteMessage("\nOverrideCenter : " + radialDimensionLarge.OverrideCenter);
        }

        /// <summary>
        /// RotatedDimension のプロパティを出力する
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="rotatedDimension"></param>
        public static void OutputRotatedDimensionProperties(Editor ed, RotatedDimension rotatedDimension)
        {
            ed.WriteMessage("\n\n--- RotatedDimension Properties ---");
            ed.WriteMessage("\nDimLinePoint : " + rotatedDimension.DimLinePoint);
            ed.WriteMessage("\nOblique      : " + rotatedDimension.Oblique);
            ed.WriteMessage("\nRotation     : " + rotatedDimension.Rotation);
            ed.WriteMessage("\nXLine1Point  : " + rotatedDimension.XLine1Point);
            ed.WriteMessage("\nXLine2Point  : " + rotatedDimension.XLine2Point);
        }
    }
}
