using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TyAutoCad.Mvvm;

namespace TyAutoCad.Examples.SqlServerDatabase.ViewModels
{
    public class SonViewModel : BindableBase
    {
        #region Fields

        #endregion

        #region Constructors
        public SonViewModel()
        {
            Title = "SonView";
        }

        #endregion

        #region Initialize Method 初期化
        private void Initialize()
        {
            // ここに初期化する内容を書く

        }
        #endregion

        #region Properties 
        #region Title Property タイトル
        public string Title { get; }
        #endregion

        #region Sample Properties 
        #region Date Property 
        public DateTime Date { get; set; }
        #endregion

        #region BoolProperty Property 
        private bool _boolProperty;
        public bool BoolProperty
        {
            get => _boolProperty;
            set => SetProperty(ref _boolProperty, value);
        }
        #endregion

        #region IntProperty Property 
        private int _intProperty;
        public int IntProperty
        {
            get => _intProperty;
            set => SetProperty(ref _intProperty, value);
        }
        #endregion

        #region DoubleProperty Property 
        private double _doubleProperty;
        public double DoubleProperty
        {
            get => _doubleProperty;
            set => SetProperty(ref _doubleProperty, value);
        }
        #endregion

        #region StringProperty Property 
        private string _stringProperty;
        public string StringProperty
        {
            get => _stringProperty;
            set => SetProperty(ref _stringProperty, value);
        }
        #endregion

        #region Collection Property ※※※のコレクション
        private ObservableCollection<string> _strings = new ObservableCollection<string>();
        public ObservableCollection<string> Strings
        {
            get => _strings;
            set => SetProperty(ref _strings, value);
        }
        #endregion
        #endregion

        #region ErrorMessage Properties
        #region ErrorMessage Property エラーメッセージ
        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        #endregion

        #region ErrorMessageColor Property エラーメッセージの色
        private Brush _errorMessageColor;
        public Brush ErrorMessageColor
        {
            get => _errorMessageColor;
            set => SetProperty(ref _errorMessageColor, value);
        }
        #endregion
        #endregion
        #endregion

        #region ProgressBar1 進捗表示プログレスバー
        #region ProgressBar1Value Property プログレスバー1の値
        private int _progressBar1Value;
        public int ProgressBar1Value
        {
            get => _progressBar1Value;
            set => SetProperty(ref _progressBar1Value, value);
        }
        #endregion

        #region ProgressBar1Visibility Property プログレスバー1 表示非表示
        public Visibility _progressBar1Visibility = Visibility.Hidden;
        public Visibility ProgressBar1Visibility
        {
            get => _progressBar1Visibility;
            set => SetProperty(ref _progressBar1Visibility, value);
        }
        #endregion

        #region ProgressBar1Command プログレスバーに進捗を表示するメソッド
        private async void ProgressBar1Execute()
        {
            // プログレスバーを表示
            ProgressBar1Visibility = Visibility.Visible;

            // 非同期メソッド
            await Task.Run(async () =>
            {
                // ここに時間のかかる処理を書く
                while (ProgressBar1Value < 100)
                {
                    ProgressBar1Value += 1;
                    await Task.Delay(10);
                }
            });

            MessageBox.Show("ProgressBar1 タスクが完了しました。");
            ProgressBar1Value = 0;
        }
        private bool ProgressBar1CanExecute()
        {
            return true;
        }
        public ICommand ProgressBar1Command => new DelegateCommand(ProgressBar1Execute, ProgressBar1CanExecute);
        #endregion
        #endregion

        #region ProgressBar2 進捗が取得できない場合のプログレスバー
        #region ProgressBar2Value Property プログレスバー1の値
        private int _progressBar2Value;
        public int ProgressBar2Value
        {
            get => _progressBar2Value;
            set => SetProperty(ref _progressBar2Value, value);
        }
        #endregion

        #region ProgressBar2Visibility Property プログレスバー2 表示非表示
        public Visibility _progressBar2Visibility = Visibility.Hidden;
        public Visibility ProgressBar2Visibility
        {
            get => _progressBar2Visibility;
            set => SetProperty(ref _progressBar2Visibility, value);
        }
        #endregion

        #region ProgressBar2IsIndeterminate Property 
        private bool _progressBar2IsIndeterminate;
        public bool ProgressBar2IsIndeterminate
        {
            get { return _progressBar2IsIndeterminate; }
            set { SetProperty(ref _progressBar2IsIndeterminate, value); }
        }
        #endregion

        #region ProgressBar2Command プログレスバーに進捗が表現できないメソッド
        private async void ProgressBar2Execute()
        {
            // プログレスバーを表示
            ProgressBar2Visibility = Visibility.Visible;
            ProgressBar2IsIndeterminate = true;

            // 非同期メソッド
            await Task.Run(async () =>
            {
                // ここに時間のかかる処理を書く
                while (ProgressBar2Value < 100)
                {
                    ProgressBar2Value += 1;
                    await Task.Delay(50);
                }
            });
            ProgressBar2IsIndeterminate = false;
            MessageBox.Show("ProgressBar2 タスクが完了しました。");
            ProgressBar2Value = 0;
        }
        private bool ProgressBar2CanExecute()
        {
            return true;
        }
        public ICommand ProgressBar2Command => new DelegateCommand(ProgressBar2Execute, ProgressBar2CanExecute);
        #endregion
        #endregion

        #region Commands
        #region ExecuteCommand 実行コマンド
        private void Execute()
        {
            MessageBox.Show("Execute!!");
        }
        private bool CanExecute()
        {
            ErrorMessageColor = new SolidColorBrush(Colors.Red);
            ErrorMessage = "実行できません。";

            //ErrorMessageColor = new SolidColorBrush(Colors.Blue);
            //ErrorMessage = "実行可能です。";
            return true;
        }
        public ICommand ExecuteCommand => new DelegateCommand(Execute, CanExecute);
        #endregion

        #region ClearCommand クリアコマンド
        private void ClearExecute()
        {
            MessageBox.Show("Clear!!");

            Initialize();
        }
        public ICommand ClearCommand => new DelegateCommand(ClearExecute);
        #endregion

        #region CloseCommand ウインドウを閉じるコマンド
        private bool _isCloseView;
        public bool IsCloseView
        {
            get => _isCloseView;
            set => SetProperty(ref _isCloseView, value);
        }
        private void CloseExecute() => IsCloseView = true;
        public ICommand CloseCommand => new DelegateCommand(CloseExecute);
        #endregion
        #endregion
    }
}
