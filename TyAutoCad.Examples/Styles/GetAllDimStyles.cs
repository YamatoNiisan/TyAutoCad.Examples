using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.GetAllDimStyles))]
namespace TyAutoCad.Examples
{
    public class GetAllDimStyles
    {
        /// <summary>
        /// 全ての寸法スタイルを取得
        /// </summary>
        [CommandMethod("GetAllDimStyles")]
        public static void Command()
        {
            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            var ed = doc.Editor;
            var db = doc.Database;

            using (var tr = db.TransactionManager.StartOpenCloseTransaction())
            {
                try
                {
                    // 寸法スタイルテーブルを読込モードで開く
                    var dimStyleTable = tr.GetObject(db.DimStyleTableId, OpenMode.ForRead) as DimStyleTable;

                    int count = 0;

                    foreach (ObjectId id in dimStyleTable)
                    {
                        var dimStyle = tr.GetObject(id, OpenMode.ForRead) as DimStyleTableRecord;
                        ed.WriteMessage("\n寸法スタイル名 : {0}", dimStyle.Name);
                        count++;
                    }


                    ed.WriteMessage("\n\n寸法スタイルの数 = {0}", count);
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error GetAllDimStyles Command\n\t" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 全ての寸法スタイルを取得してプロパティを出力
        /// </summary>
        [CommandMethod("GetAllDimStylesAndOutputProperties")]
        public static void Command1()
        {
            var sw = new Stopwatch();
            sw.Start(); // 計測開始

            // Document, Editor, Database を取得
            var doc = Application.DocumentManager.MdiActiveDocument;
            var ed = doc.Editor;
            var db = doc.Database;

            using (var tr = db.TransactionManager.StartOpenCloseTransaction())
            {
                try
                {
                    // 寸法スタイルテーブルを読込モードで開く
                    var dimStyleTable = tr.GetObject(db.DimStyleTableId, OpenMode.ForRead) as DimStyleTable;

                    // DimStyleTableRecord の変数を準備
                    DimStyleTableRecord dimStyle;

                    // カウンターを準備
                    int count = 0;

                    foreach (ObjectId id in dimStyleTable)
                    {
                        dimStyle = tr.GetObject(id, OpenMode.ForRead) as DimStyleTableRecord;

                        ed.WriteMessage("\n\n--- SymbolTableRecord Properties ---");
                        ed.WriteMessage("\n\tIsDependent : " + dimStyle.IsDependent);
                        ed.WriteMessage("\n\tIsResolved  : " + dimStyle.IsResolved);
                        ed.WriteMessage("\n\tName        : " + dimStyle.Name);        // 寸法スタイル名

                        ed.WriteMessage("\n\n--- DimStyleTableRecord Properties ---");
                        ed.WriteMessage("\n\tDimadec : " + dimStyle.Dimadec);
                        ed.WriteMessage("\n\tDimalt : " + dimStyle.Dimalt);
                        ed.WriteMessage("\n\tDimaltd : " + dimStyle.Dimaltd);
                        ed.WriteMessage("\n\tDimaltf : " + dimStyle.Dimaltf);
                        ed.WriteMessage("\n\tDimaltrnd : " + dimStyle.Dimaltrnd);
                        ed.WriteMessage("\n\tDimalttd : " + dimStyle.Dimalttd);
                        ed.WriteMessage("\n\tDimalttz : " + dimStyle.Dimalttz);
                        ed.WriteMessage("\n\tDimaltu : " + dimStyle.Dimaltu);
                        ed.WriteMessage("\n\tDimaltz : " + dimStyle.Dimaltz);
                        ed.WriteMessage("\n\tDimapost : " + dimStyle.Dimapost);
                        ed.WriteMessage("\n\tDimarcsym : " + dimStyle.Dimarcsym);
                        ed.WriteMessage("\n\tDimasz : " + dimStyle.Dimasz);
                        ed.WriteMessage("\n\tDimatfit : " + dimStyle.Dimatfit);
                        ed.WriteMessage("\n\tDimaunit : " + dimStyle.Dimaunit);
                        ed.WriteMessage("\n\tDimazin : " + dimStyle.Dimazin);
                        ed.WriteMessage("\n\tDimblk : " + dimStyle.Dimblk);
                        ed.WriteMessage("\n\tDimblk1 : " + dimStyle.Dimblk1);
                        ed.WriteMessage("\n\tDimblk2 : " + dimStyle.Dimblk2);
                        ed.WriteMessage("\n\tDimcen : " + dimStyle.Dimcen);
                        ed.WriteMessage("\n\tDimclrd : " + dimStyle.Dimclrd);
                        ed.WriteMessage("\n\tDimclre : " + dimStyle.Dimclre);
                        ed.WriteMessage("\n\tDimclrt : " + dimStyle.Dimclrt);
                        ed.WriteMessage("\n\tDimdec : " + dimStyle.Dimdec);
                        ed.WriteMessage("\n\tDimdle : " + dimStyle.Dimdle);
                        ed.WriteMessage("\n\tDimdli : " + dimStyle.Dimdli);
                        ed.WriteMessage("\n\tDimdsep : " + dimStyle.Dimdsep);
                        ed.WriteMessage("\n\tDimexe : " + dimStyle.Dimexe);
                        ed.WriteMessage("\n\tDimexo : " + dimStyle.Dimexo);
                        ed.WriteMessage("\n\tDimfrac : " + dimStyle.Dimfrac);
                        ed.WriteMessage("\n\tDimfxlen : " + dimStyle.Dimfxlen);
                        ed.WriteMessage("\n\tDimfxlenOn : " + dimStyle.DimfxlenOn);
                        ed.WriteMessage("\n\tDimgap : " + dimStyle.Dimgap);
                        ed.WriteMessage("\n\tDimjogang : " + dimStyle.Dimjogang);
                        ed.WriteMessage("\n\tDimjust : " + dimStyle.Dimjust);
                        ed.WriteMessage("\n\tDimldrblk : " + dimStyle.Dimldrblk);

                        ed.WriteMessage("\n\tDimlfac : " + dimStyle.Dimlfac);
                        ed.WriteMessage("\n\tDimlim : " + dimStyle.Dimlim);
                        ed.WriteMessage("\n\tDimltex1 : " + dimStyle.Dimltex1);
                        ed.WriteMessage("\n\tDimltex2 : " + dimStyle.Dimltex2);
                        ed.WriteMessage("\n\tDimltype : " + dimStyle.Dimltype);
                        ed.WriteMessage("\n\tDimlunit : " + dimStyle.Dimlunit);
                        ed.WriteMessage("\n\tDimlwd : " + dimStyle.Dimlwd);
                        ed.WriteMessage("\n\tDimlwe : " + dimStyle.Dimlwe);
                        ed.WriteMessage("\n\tDimpost : " + dimStyle.Dimpost);
                        ed.WriteMessage("\n\tDimrnd : " + dimStyle.Dimrnd);
                        ed.WriteMessage("\n\tDimsah : " + dimStyle.Dimsah);
                        ed.WriteMessage("\n\tDimscale : " + dimStyle.Dimscale);
                        ed.WriteMessage("\n\tDimsd1 : " + dimStyle.Dimsd1);
                        ed.WriteMessage("\n\tDimsd2 : " + dimStyle.Dimsd2);
                        ed.WriteMessage("\n\tDimse1 : " + dimStyle.Dimse1);
                        ed.WriteMessage("\n\tDimse2 : " + dimStyle.Dimse2);
                        ed.WriteMessage("\n\tDimsoxd : " + dimStyle.Dimsoxd);
                        ed.WriteMessage("\n\tDimtad : " + dimStyle.Dimtad);
                        ed.WriteMessage("\n\tDimtdec : " + dimStyle.Dimtdec);
                        ed.WriteMessage("\n\tDimtfac : " + dimStyle.Dimtfac);

                        ed.WriteMessage("\n\tDimtfill : " + dimStyle.Dimtfill);
                        ed.WriteMessage("\n\tDimtfillclr : " + dimStyle.Dimtfillclr);
                        ed.WriteMessage("\n\tDimtih : " + dimStyle.Dimtih);
                        ed.WriteMessage("\n\tDimtix : " + dimStyle.Dimtix);
                        ed.WriteMessage("\n\tDimtm : " + dimStyle.Dimtm);
                        ed.WriteMessage("\n\tDimtmove : " + dimStyle.Dimtmove);
                        ed.WriteMessage("\n\tDimtofl : " + dimStyle.Dimtofl);
                        ed.WriteMessage("\n\tDimtoh : " + dimStyle.Dimtoh);
                        ed.WriteMessage("\n\tDimtol : " + dimStyle.Dimtol);
                        ed.WriteMessage("\n\tDimtolj : " + dimStyle.Dimtolj);
                        ed.WriteMessage("\n\tDimtp : " + dimStyle.Dimtp);
                        ed.WriteMessage("\n\tDimtsz : " + dimStyle.Dimtsz);
                        ed.WriteMessage("\n\tDimtvp : " + dimStyle.Dimtvp);
                        ed.WriteMessage("\n\tDimtxsty : " + dimStyle.Dimtxsty);
                        ed.WriteMessage("\n\tDimtxt : " + dimStyle.Dimtxt);
                        ed.WriteMessage("\n\tDimtzin : " + dimStyle.Dimtzin);
                        ed.WriteMessage("\n\tDimupt : " + dimStyle.Dimupt);
                        ed.WriteMessage("\n\tDimzin : " + dimStyle.Dimzin);
                        ed.WriteMessage("\n\tIsModifiedForRecompute : " + dimStyle.IsModifiedForRecompute);


                        ed.WriteMessage("\n---------------------------------------------------------------------");
                        count++;
                    }

                    ed.WriteMessage("\n\nNumber of DimStyle = " + count);
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("Error GetAllDimStylesAndOutputProperties Command\n\t" + ex.Message);
                }
            }

            sw.Stop(); // 計測終了
            TimeSpan span = sw.Elapsed;    //  計測した時間を span に代入
            ed.WriteMessage("\nTime : " + span.TotalMilliseconds);
        }
    }
}
