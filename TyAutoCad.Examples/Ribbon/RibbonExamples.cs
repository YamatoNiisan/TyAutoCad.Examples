using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Autodesk.AutoCAD.ApplicationServices;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using System.Windows.Controls;

[assembly: CommandClass(typeof(TyAutoCad.Examples.RibbonExamples))]
namespace TyAutoCad.Examples
{
    /// <summary>
    /// リボン作成のサンプル
    /// </summary>
    public class RibbonExamples
    {
        public RibbonCombo combo11 = new RibbonCombo();
        public RibbonCombo combo41 = new RibbonCombo();

        [CommandMethod("RibbonExamples")]
        public void MyRibbon()
        {
            // リボンコントロールを取得
            RibbonControl ribbonControl = ComponentManager.Ribbon;

            // リボンタブを作成
            var tab = new RibbonTab
            {
                Title = "RibbonExamples",
                Id = "RibbonSample_TAB_ID"
            };

            // リボンコントロールにタブを追加
            ribbonControl.Tabs.Add(tab);

            #region Panel1
            // リボンパネルを作成
            var panelSource1 = new RibbonPanelSource { Title = "Panel1" };
            var Panel1 = new RibbonPanel { Source = panelSource1 };
            tab.Panels.Add(Panel1);

            var button11 = new RibbonButton
            {
                Text = "Button11",
                ShowText = true,
                ShowImage = true,
                Image = Images.GetBitmapImage(Properties.Resources.Small),
                LargeImage = Images.GetBitmapImage(Properties.Resources.large),
                Orientation = System.Windows.Controls.Orientation.Vertical,
                Size = RibbonItemSize.Large,
                CommandHandler = new RibbonCommandHandler()
            };

            var button12 = new RibbonButton
            {
                Text = "Button12",
                ShowText = true,
                ShowImage = true,
                Image = Images.GetBitmapImage(Properties.Resources.Small),
                LargeImage = Images.GetBitmapImage(Properties.Resources.large),
                CommandHandler = new RibbonCommandHandler()
            };
 
            var button13 = new RibbonButton
            {
                Text = "Button13",
                ShowText = true,
                ShowImage = true,
                Image = Images.GetBitmapImage(Properties.Resources.Small),
                LargeImage = Images.GetBitmapImage(Properties.Resources.large),
                CommandHandler = new RibbonCommandHandler()
            };

            // リボンコンボのプロパティを設定する
            combo11.Text = " ";
            combo11.ShowText = true;
            combo11.MinWidth = 150;

            // リボン行パネルを作成
            RibbonRowPanel panelRow11 = new RibbonRowPanel();

            // 行にボタンを追加
            panelRow11.Items.Add(button12);
            panelRow11.Items.Add(new RibbonRowBreak());
            panelRow11.Items.Add(button13);
            panelRow11.Items.Add(new RibbonRowBreak());
            panelRow11.Items.Add(combo11);

            // パネルにボタンと行を追加
            panelSource1.Items.Add(button11);
            panelSource1.Items.Add(new RibbonSeparator()); // 区切り線
            panelSource1.Items.Add(panelRow11);
            #endregion

            #region Panel2
            var panelSource2 = new RibbonPanelSource { Title = "Panel2" };
            var panel2 = new RibbonPanel { Source = panelSource2 };
            tab.Panels.Add(panel2);

            RibbonSplitButton splitButton21 = new RibbonSplitButton
            {
                Text = "SplitButton",
                CommandHandler = new RibbonCommandHandler(),
                ShowText = true,
                ShowImage = true,
                Image = Images.GetBitmapImage(Properties.Resources.Small),
                LargeImage = Images.GetBitmapImage(Properties.Resources.large),
                IsSplit = true,
                Size = RibbonItemSize.Large
            };
 
            var button21 = new RibbonButton
            {
                Text = "Button21",
                ShowText = true,
                ShowImage = true,
                Image = Images.GetBitmapImage(Properties.Resources.Small),
                LargeImage = Images.GetBitmapImage(Properties.Resources.large),
                Size = RibbonItemSize.Large,
                Orientation = System.Windows.Controls.Orientation.Vertical,
                CommandHandler = new RibbonCommandHandler()
            };
 
            var button22 = new RibbonButton
            {
                Text = "Button22",
                ShowText = true,
                ShowImage = true,
                Image = Images.GetBitmapImage(Properties.Resources.Small),
                LargeImage = Images.GetBitmapImage(Properties.Resources.large),

                CommandHandler = new RibbonCommandHandler()
            };

            var button23 = new RibbonButton
            {
                Text = "button23",
                ShowText = true,
                ShowImage = true,
                Image = Images.GetBitmapImage(Properties.Resources.Small),
                LargeImage = Images.GetBitmapImage(Properties.Resources.large),
                CommandHandler = new RibbonCommandHandler()
            };
 
            splitButton21.Items.Add(button21);
            splitButton21.Items.Add(button22);

            var panelRow21 = new RibbonRowPanel();
            panelRow21.Items.Add(button22);
            panelRow21.Items.Add(new RibbonRowBreak());
            panelRow21.Items.Add(button23);
            panelRow21.Items.Add(new RibbonRowBreak());
            panelRow21.Items.Add(new RibbonCombo());

            panelSource2.Items.Add(splitButton21);
            panelSource2.Items.Add(panelRow21);
            #endregion

            #region Panel3
            var panelSource3 = new RibbonPanelSource { Title = "Panel3" };
            var panel3 = new RibbonPanel { Source = panelSource3 };
            tab.Panels.Add(panel3);

            var button31 = new RibbonButton
            {
                Text = "Button31",
                ShowText = true,
                ShowImage = true,
                Image = Images.GetBitmapImage(Properties.Resources.Small),
                LargeImage = Images.GetBitmapImage(Properties.Resources.large),
                Orientation = System.Windows.Controls.Orientation.Vertical,
                CommandHandler = new RibbonCommandHandler()
            };

            var button32 = new RibbonButton
            {
                Text = "Button32",
                ShowText = true,
                ShowImage = true,
                Image = Images.GetBitmapImage(Properties.Resources.Small),
                LargeImage = Images.GetBitmapImage(Properties.Resources.large),
                Size = RibbonItemSize.Large,
                Orientation = System.Windows.Controls.Orientation.Vertical,
                CommandHandler = new RibbonCommandHandler()
            };
 
            var button33 = new RibbonButton
            {
                Text = "Button33",
                ShowText = true,
                ShowImage = true,
                Image = Images.GetBitmapImage(Properties.Resources.Small),
                LargeImage = Images.GetBitmapImage(Properties.Resources.large),
                CommandHandler = new RibbonCommandHandler()
            };
 
            var button34 = new RibbonButton
            {
                Text = "Button34",
                ShowText = true,
                ShowImage = true,
                Image = Images.GetBitmapImage(Properties.Resources.Small),
                LargeImage = Images.GetBitmapImage(Properties.Resources.large),
                CommandHandler = new RibbonCommandHandler()
            };
 
            var button35 = new RibbonButton
            {
                Text = "Button35",
                ShowText = true,
                ShowImage = true,
                Image = Images.GetBitmapImage(Properties.Resources.Small),
                LargeImage = Images.GetBitmapImage(Properties.Resources.large),
                CommandHandler = new RibbonCommandHandler()
            };

            var panelRow31 = new RibbonRowPanel();
            panelRow31.Items.Add(button33);
            panelRow31.Items.Add(new RibbonRowBreak());
            panelRow31.Items.Add(button34);
            panelRow31.Items.Add(new RibbonRowBreak());
            panelRow31.Items.Add(button35);

            panelSource3.Items.Add(button31);
            panelSource3.Items.Add(panelRow31);
            #endregion

            #region Panel4
            var panelSource4 = new RibbonPanelSource { Title = "Panel4" };
            var panel4 = new RibbonPanel { Source = panelSource4 };
            tab.Panels.Add(panel4);

            var button41 = new RibbonButton
            {
                Text = "Button41",
                ShowText = true,
                ShowImage = true,
                Image = Images.GetBitmapImage(Properties.Resources.Small),
                LargeImage = Images.GetBitmapImage(Properties.Resources.large),
                Size = RibbonItemSize.Large,
                Orientation = System.Windows.Controls.Orientation.Vertical,
                CommandHandler = new RibbonCommandHandler()
            };
 
            var button42 = new RibbonButton
            {
                Text = "Button42",
                ShowText = true,
                ShowImage = true,
                Image = Images.GetBitmapImage(Properties.Resources.Small),
                LargeImage = Images.GetBitmapImage(Properties.Resources.large),
                CommandHandler = new RibbonCommandHandler()
            };
 
            var button43 = new RibbonButton
            {
                Text = "Button43",
                ShowText = true,
                ShowImage = true,
                Image = Images.GetBitmapImage(Properties.Resources.Small),
                LargeImage = Images.GetBitmapImage(Properties.Resources.large),
                CommandHandler = new RibbonCommandHandler()
            };
 
            var button44 = new RibbonButton
            {
                Text = "Button44",
                ShowText = true,
                ShowImage = true,
                Image = Images.GetBitmapImage(Properties.Resources.Small),
                LargeImage = Images.GetBitmapImage(Properties.Resources.large),
                Size = RibbonItemSize.Large,
                Orientation = System.Windows.Controls.Orientation.Vertical,
                CommandHandler = new RibbonCommandHandler()
            };

            var comboButton41 = new RibbonButton
            {
                Text = "ComboButton41",
                ShowText = true,
                ShowImage = true,
                Image = Images.GetBitmapImage(Properties.Resources.Small),
                LargeImage = Images.GetBitmapImage(Properties.Resources.large),
                CommandHandler = new RibbonCommandHandler()
            };
 
            var comboButton42 = new RibbonButton
            {
                Text = "ComboButton42",
                ShowText = true,
                ShowImage = true,
                Image = Images.GetBitmapImage(Properties.Resources.Small),
                LargeImage = Images.GetBitmapImage(Properties.Resources.large),
                CommandHandler = new RibbonCommandHandler()
            };
 
            combo41.Width = 150;
            combo41.Items.Add(comboButton41);
            combo41.Items.Add(comboButton42);
            combo41.Current = comboButton41;

            var row41 = new RibbonRowPanel();
            row41.Items.Add(button42);
            row41.Items.Add(new RibbonRowBreak());
            row41.Items.Add(button43);
            row41.Items.Add(new RibbonRowBreak());
            row41.Items.Add(combo41);

            panelSource4.Items.Add(button41);
            panelSource4.Items.Add(row41);
            panelSource4.Items.Add(new RibbonSeparator());
            panelSource4.Items.Add(button44);
            #endregion

            #region Panel5
            var panelSource5 = new RibbonPanelSource { Title = "Panel5" };
            var panel5 = new RibbonPanel { Source = panelSource5 };
            tab.Panels.Add(panel5);

            // リボンコンボ-1 を作成
            var combo51 = new RibbonCombo()
            {
                Name = "RibbonCombo1",
                Id = "ID_COMBO1",
                Text = "Frame : ",
                ShowImage = false,
                ShowText = true,
                Width = 50,
            };
            // 用紙枠リスト
            var frameList = new List<RibbonLabel>() {
                new RibbonLabel() { Text = "A1", }, new RibbonLabel() { Text = "A2", },
                new RibbonLabel() { Text = "A3", }, new RibbonLabel() { Text = "A4", },
            };
            // 用紙枠リストをコンボ-1 にセット
            frameList.ForEach(l => combo51.Items.Add(l));

            // ラジオボタングループを作成
            var rbg = new RibbonRadioButtonGroup()
            {
                Name = "RadioButtonGroup",
                Id = "ID_RADIOBUTTONGROUP",
                Text = "rbg",
                ShowText = true,
            };

            // トグルボタンのリストを作成
            var tgList = new List<RibbonToggleButton> {
                new RibbonToggleButton
                {
                    Name = "rb1",
                    Id = "ID_RADIOBUTTON1",
                    Text = "1",
                    ShowText = true
                },
                new RibbonToggleButton
                {
                    Name = "rb2",
                    Id = "ID_RADIOBUTTON2",
                    Text = "2",
                    ShowText = true
                },
                new RibbonToggleButton
                {
                    Name = "rb3",
                    Id = "ID_RADIOBUTTON3",
                    Text = "3",
                    ShowText = true
                },
                new RibbonToggleButton
                {
                    Name = "rb4",
                    Id = "ID_RADIOBUTTON4",
                    Text = "4",
                    ShowText = true
                },
                new RibbonToggleButton
                {
                    Name = "rb5",
                    Id = "ID_RADIOBUTTON5",
                    Text = "5",
                    ShowText = true
                },
            };

            // ラジオボタングループにトグルボタンを追加
            tgList.ForEach(t => rbg.Items.Add(t));

            // イベント登録
            rbg.CurrentChanged += new EventHandler<RibbonPropertyChangedEventArgs>(rbg_CurrentChanged);

            // スケールコンボボックス
            var combo52 = new RibbonCombo()
            {
                Name = "RibbonCombo2",
                Id = "ID_COMBO2",
                Text = "   S = 1 / ",
                ShowImage = false,
                ShowText = true,
                Width = 50,
            };

            // 縮尺リスト
            var scaleList = new List<RibbonLabel>() {
                new RibbonLabel() { Text = "1", }, new RibbonLabel() { Text = "2", },
                new RibbonLabel() { Text = "3", }, new RibbonLabel() { Text = "4", },
                new RibbonLabel() { Text = "5", }, new RibbonLabel() { Text = "6", },
                new RibbonLabel() { Text = "8", },
                new RibbonLabel() { Text =  "10" }, new RibbonLabel() { Text =  "12" },
                new RibbonLabel() { Text =  "15" }, new RibbonLabel() { Text =  "20" },
                new RibbonLabel() { Text =  "25" }, new RibbonLabel() { Text =  "30" },
                new RibbonLabel() { Text =  "40" }, new RibbonLabel() { Text =  "50" },
                new RibbonLabel() { Text =  "60" }, new RibbonLabel() { Text =  "80" },
                new RibbonLabel() { Text = "100" }, new RibbonLabel() { Text = "120" },
                new RibbonLabel() { Text = "150" }, new RibbonLabel() { Text = "200" },
                new RibbonLabel() { Text = "250" }, new RibbonLabel() { Text = "300" },
                new RibbonLabel() { Text = "400" }, new RibbonLabel() { Text = "500" },
                new RibbonLabel() { Text = "600" }, new RibbonLabel() { Text = "700" },
                new RibbonLabel() { Text = "800" }, new RibbonLabel() { Text = "900" },
                new RibbonLabel() { Text = "1000" },
            };

            // 用紙枠リストをコンボ-1 にセット
            scaleList.ForEach(s => combo52.Items.Add(s));

            var row51 = new RibbonRowPanel();

            row51.Items.Add(combo51);
            row51.Items.Add(new RibbonRowBreak());
            row51.Items.Add(rbg);
            row51.Items.Add(new RibbonRowBreak());
            row51.Items.Add(combo52);

            panelSource5.Items.Add(row51);
            #endregion

            #region Panel6
            var panelSource6 = new RibbonPanelSource { Title = "Panel6" };
            var panel6 = new RibbonPanel { Source = panelSource6 };
            tab.Panels.Add(panel6);

            // チェックリストボタンを作成
            var checklistButton = new RibbonChecklistButton
            {
                Text = "Check List :",
                Id = "CHECKLIST",
                ShowText = true
            };

            var label = new RibbonLabel
            {
                Text = "ラベル",
                ShowText = true
            };

            var textBox = new RibbonTextBox
            {
                Width = 50,
            };

            // トグルボタンのリストを作成
            var toggles = new List<RibbonToggleButton> {
                new RibbonToggleButton
                {
                    Name = "Check1",
                    Id = "ID_TOGGLE61",
                },
                new RibbonToggleButton
                {
                    Name = "Check2",
                    Id = "ID_TOGGLE62",
                },
                new RibbonToggleButton
                {
                    Name = "Check3", Id = "ID_TOGGLE63"
                },
                new RibbonToggleButton
                {
                    Name = "Check4",
                    Id = "ID_TOGGLE64",
                },
                new RibbonToggleButton
                {
                    Name = "Check5",
                    Id = "ID_TOGGLE65",
                },
            };

            // ラジオボタングループにトグルボタンを追加
            toggles.ForEach(t => checklistButton.Items.Add(t));

            RibbonSpinner spinner = new RibbonSpinner
            {
                Maximum = 100,
                Minimum = 10,
                Value = 50,
            };

            var row61 = new RibbonRowPanel();

            row61.Items.Add(label);
            row61.Items.Add(textBox);
            row61.Items.Add(new RibbonRowBreak());
            row61.Items.Add(checklistButton);
            row61.Items.Add(new RibbonRowBreak());
            row61.Items.Add(spinner);

            panelSource6.Items.Add(row61);
            #endregion

            // タブをアクティブにする
            tab.IsActive = true;
        }

        // コマンドハンドラー
        public class RibbonCommandHandler : System.Windows.Input.ICommand
        {
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                Document doc = Application.DocumentManager.MdiActiveDocument;

                if (parameter is RibbonButton)
                {
                    RibbonButton button = parameter as RibbonButton;
                    doc.Editor.WriteMessage("\nRibbonButton Executed: " + button.Text + "\n");
                }
            }
        }

        // ラジオボタングループイベント
        private void rbg_CurrentChanged(object sender, RibbonPropertyChangedEventArgs e)
        {
            var rbg = sender as RibbonRadioButtonGroup;
            var tg = rbg.Current as RibbonToggleButton;

            Document doc = Application.DocumentManager.MdiActiveDocument;
            doc.Editor.WriteMessage("\nラジオボタン {0} が押されました。", tg.Name);
        }

        public class Images
        {
            public static BitmapImage GetBitmapImage(Bitmap image)
            {
                var stream = new MemoryStream();
                image.Save(stream, ImageFormat.Png);
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = stream;
                bmp.EndInit();

                return bmp;
            }
        }
    }
}
