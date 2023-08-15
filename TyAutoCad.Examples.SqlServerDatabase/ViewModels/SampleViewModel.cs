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
    public enum Color
    {
        Red,
        Yellow,
        Green,
        Blue
    }

    public class SampleViewModel : BindableBase
    {
        #region Fields

        #endregion

        #region Constructors
        public SampleViewModel()
        {
            Title = "SampleView";
            ButtonContent = "Clear";

            SetItems();
            Color = Color.Red;
        }

        #endregion

        #region Initialize Method 初期化
        private void Initialize()
        {
            // ここに初期化する内容を書く

        }
        #endregion

        #region Sample properties 
        #region Title Property タイトル
        public string Title { get; }
        #endregion

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

        #region Switch Property ラジオボタンの切替
        private int _switch;
        public int Switch
        {
            get => _switch;
            set
            {
                SetProperty(ref _switch, value);
                if (Switch == 0)
                {
                    IsColor = true;
                    MessageBox.Show("Switch on!!");
                }
                else
                {
                    IsColor = false;
                    MessageBox.Show("Switch off!!");
                }
            }
        }
        #endregion

        #region IsCheck Property チェックボックス切替
        private bool _isCheck1;
        public bool IsCheck1
        {
            get => _isCheck1;
            set
            {
                SetProperty(ref _isCheck1, value);
                if (IsCheck1)
                {
                    MessageBox.Show("Check1 checked!!");
                }
            }
        }
        private bool _isCheck2;
        public bool IsCheck2
        {
            get { return _isCheck2; }
            set
            {
                SetProperty(ref _isCheck2, value);
                if (IsCheck2)
                {
                    MessageBox.Show("Check2 checked!!");
                }
            }
        }
        #endregion

        #region TextBoxText Property テキストボックス
        private string _textBoxText = string.Empty;
        public string TextBoxText
        {
            get => _textBoxText;
            set { SetProperty(ref _textBoxText, value); }
        }
        #endregion

        #region ComboBox Properties コンボボックス
        private string[] _datas = { "A", "B", "C", "D", "E" };

        private void SetItems()
        {
            Items.Clear();
            foreach (var item in _datas)
            {
                Items.Add(item);
            }
        }

        private ObservableCollection<string> _items = new ObservableCollection<string>();
        public ObservableCollection<string> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private string _item;
        public string Item
        {
            get => _item;
            set => SetProperty(ref _item, value);
        }

        private int _itemIndex;
        public int ItemIndex
        {
            get => _itemIndex;
            set => SetProperty(ref _itemIndex, value);
        }
        #endregion

        #region Color Property EnumColor
        private Color _color;
        public Color Color
        {
            get => _color;
            set
            {
                SetProperty(ref _color, value);
                switch (Color)
                {
                    case Color.Red:
                        MessegeColor = new SolidColorBrush(Colors.Red);
                        Message = "Red";
                        break;
                    case Color.Yellow:
                        MessegeColor = new SolidColorBrush(Colors.Yellow);
                        Message = "Yellow";
                        break;
                    case Color.Green:
                        MessegeColor = new SolidColorBrush(Colors.Green);
                        Message = "Green";
                        break;
                    case Color.Blue:
                        MessegeColor = new SolidColorBrush(Colors.Blue);
                        Message = "Blue";
                        break;
                    default:
                        break;
                }
            }
        }

        private bool _isColor = true;
        public bool IsColor
        {
            get => _isColor;
            set => SetProperty(ref _isColor, value);
        }

        private string _message = string.Empty;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private Brush _messegeColor;
        public Brush MessegeColor
        {
            get => _messegeColor;
            set => SetProperty(ref _messegeColor, value);
        }
        #endregion

        #region Control Properties コントロールの見た目を制御
        #region VisibilityProperty Property // 表示･非表示
        //public Visibility _visibilityProperty = Visibility.Collapsed; // 非表示、領域を確保しない
        //public Visibility _visibilityProperty = Visibility.Hidden;    // 非表示、領域を確保する
        public Visibility _visibilityProperty = Visibility.Visible;     // 表示、領域確保ん
        public Visibility VisibilityProperty
        {
            get { return _visibilityProperty; }
            set { SetProperty(ref _visibilityProperty, value); }
        }
        #endregion

        #region GroupBoxHeader Property グループボックスヘッダー
        private string _groupBoxHeader = string.Empty;
        public string GroupBoxHeader
        {
            get { return _groupBoxHeader; }
            set { SetProperty(ref _groupBoxHeader, value); }
        }
        #endregion

        #region ButtonContent Property ボタンコンテンツ
        private string _buttonContent = string.Empty;
        public string ButtonContent
        {
            get { return _buttonContent; }
            set { SetProperty(ref _buttonContent, value); }
        }
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
