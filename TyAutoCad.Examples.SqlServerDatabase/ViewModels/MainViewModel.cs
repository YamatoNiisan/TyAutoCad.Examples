using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TyAutoCad.Examples.SqlServerDatabase.Views;
using TyAutoCad.Mvvm;

namespace TyAutoCad.Examples.SqlServerDatabase.ViewModels
{
    public class MainViewModel : BindableBase
    {
        #region Fields
        private Models.MainModel _model;
        #endregion

        #region Constructors
        public MainViewModel()
        {
            _model = new Models.MainModel();
            Initialize();
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
        public string Title => "SqlServerDbExamples 230407";
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

        #region Commands
        #region ShowSampleViewCommand SampleViewを開く
        private void ShowSampleViewExecute()
        {
            var sub = new SampleView();
            sub.Show();
        }
        public ICommand ShowSampleViewCommand => new DelegateCommand(ShowSampleViewExecute);
        #endregion

        #region ShowSubViewCommand SubViewを開く
        private void ShowSubViewExecute()
        {
            var sub = new SubView();
            sub.ShowDialog();
        }
        public ICommand ShowSubViewCommand => new DelegateCommand(ShowSubViewExecute);
        #endregion

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
