using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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
    public class SubViewModel : ValidateableBase
    {
        #region Fields

        #endregion

        #region Constructors
        public SubViewModel()
        {
            Title = "SubView";
            Initialize();
        }

        #endregion

        #region Initialize Method 初期化
        private void Initialize()
        {
            // ここに初期化する内容を書く
            SubText = string.Empty;
        }
        #endregion

        #region Properties 
        #region Title Property タイトル
        public string Title { get; }
        #endregion

        #region SubText Property 
        private string _subText;
        [Required(ErrorMessage = "入力してください")]
        [RegularExpression(@"^([1-9]\d*|0)(\.\d+)?$", ErrorMessage = "正の数値のみ入力できます")]
        public string SubText
        {
            get => _subText;
            set => SetProperty(ref _subText, value);
        }
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
        #region ShowSubViewCommand SonViewを開く
        private void ShowSonViewExecute()
        {
            var son = new SonView();
            son.ShowDialog();
        }
        public ICommand ShowSonViewCommand => new DelegateCommand(ShowSonViewExecute);
        #endregion

        #region ExecuteCommand 実行コマンド
        private void Execute()
        {
            MessageBox.Show("Execute!!");
        }
        private bool CanExecute()
        {
            //ErrorMessageColor = new SolidColorBrush(Colors.Red);
            //ErrorMessage = "実行できません。";

            //ErrorMessageColor = new SolidColorBrush(Colors.Blue);
            //ErrorMessage = "実行可能です。";
            return !HasErrors;
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
        // 画面の表示状態を示すプロパティ。Trueにすると画面を閉じる
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
