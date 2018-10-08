using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Vampire_Life_Game_Clicker.NewChestGame;
using Color = System.Windows.Media.Color;

namespace Vampire_Life_Game_Clicker
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer _timer = new DispatcherTimer();
        private FrameForm _form;
        UserActivityHook _actHook;
        private SaveData _saveData;
        private ImageWorker _imageWorker;
        private WinApiClass _apiClass = new WinApiClass();
        private BloodGame _bloodGame;
        private ChestGame _chestGame;
        private double _x;
        private double _y;

        private bool _frameStart;

        public MainWindow()
        {
            InitializeComponent();
            _imageWorker = new ImageWorker(this);
            _form = new FrameForm();
            _actHook = new UserActivityHook();
            _actHook.OnMouseActivity += new System.Windows.Forms.MouseEventHandler(MouseMoved);
            _actHook.MouseActions += ActHook_MouseActions;
            if (SaveData.Load(out _saveData))
            {
                ChestLoad();
                BloodLoad();
            }

            var init = new ChestGameInit(_imageWorker, _actHook, _apiClass);
        }

        private void ActHook_MouseActions(MouseAction action, int x, int y)
        {
        }

        public void MouseMoved(object sender, System.Windows.Forms.MouseEventArgs e)
        {

        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _form.Show();
            var init = new ChestInitiation(_form, _actHook, _saveData, _imageWorker);
            init.EndInitiation += Init_EndInitiation;
        }

        private void Init_EndInitiation(Initiation sender)
        {
            sender.EndInitiation -= Init_EndInitiation;
            _form.Hide();
            ChestLoad();
        }

        private void Init_BloodEndInitiation(Initiation sender)
        {
            sender.EndInitiation -= Init_EndInitiation;
            _form.Hide();
            BloodLoad();
        }

        private void ChestLoad()
        {
            LeftArrow.Source = _imageWorker.GetImageSource(_saveData.LeftArrowImage);
            RightArrow.Source = _imageWorker.GetImageSource(_saveData.RightArrowImage);
            UpArrow.Source = _imageWorker.GetImageSource(_saveData.UpArrowImage);
            DownArrow.Source = _imageWorker.GetImageSource(_saveData.DownArrowImage);
            SizeChestCell.Content = _saveData.ChestSizeCell.ToString();
            PositionChestCell.Content = _saveData.ChestLeftPoint.ToString();
        }

        public void SetImage(Image image)
        {
            LeftArrow.Source = _imageWorker.GetImageSource(image);
        }

        private void BloodLoad()
        {
            LeftUpBloodPole.Content = _saveData.BloodStartPoint.ToString();
            CellSizeBloodPole.Content = _saveData.BloodSizeCell.ToString();
            CountHorCellBloodPole.Content = _saveData.CoutCellHor.ToString();
            CountVertCellBloodPole.Content = _saveData.CountCellVert.ToString();

            var color = _saveData.GetColor("violet").Pixels;
            VioletBloodCollor.Background = new SolidColorBrush(Color.FromArgb(color[3], color[2], color[1], color[0]));
            color = _saveData.GetColor("black").Pixels;
            BlackBloodCollor.Background = new SolidColorBrush(Color.FromArgb(color[3], color[2], color[1], color[0]));
            color = _saveData.GetColor("green").Pixels;
            GreenBloodCollor.Background = new SolidColorBrush(Color.FromArgb(color[3], color[2], color[1], color[0]));
            color = _saveData.GetColor("orange").Pixels;
            OrangeBloodCollor.Background = new SolidColorBrush(Color.FromArgb(color[3], color[2], color[1], color[0]));
            color = _saveData.GetColor("blue").Pixels;
            BlueBloodCollor.Background = new SolidColorBrush(Color.FromArgb(color[3], color[2], color[1], color[0]));
            BlackBlood.IsChecked = _saveData.GetColor("black").Check;
            GreenBlood.IsChecked = _saveData.GetColor("green").Check;
            VioletBlood.IsChecked = _saveData.GetColor("violet").Check;
            OrangeBlood.IsChecked = _saveData.GetColor("orange").Check;
            BlueBlood.IsChecked = _saveData.GetColor("blue").Check;
        }


        private void ChestFrameButton_Click(object sender, RoutedEventArgs e)
        {
            var point = _saveData.ChestLeftPoint;
            var size = _saveData.ChestSizeCell;
            _form.Show();
            _form.SetNewCoord(point.X, point.Y, point.X + size, point.Y + size);
            _timer.Interval = new TimeSpan(0, 0, 2);
            _timer.Tick += _timerChest_Tick;
            _timer.Start();
        }

        private void _timerChest_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            _timer.Tick -= _timerChest_Tick;
            _form.Hide();
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            _form.Close();
        }

        private void BloodInitiationButton_Click(object sender, RoutedEventArgs e)
        {
            _form.Show();
            var init = new BloosInitiation(_form, _actHook, _saveData, _imageWorker);
            init.EndInitiation += Init_BloodEndInitiation;
        }

        private void BloodStartGame_OnClick(object sender, RoutedEventArgs e)
        {
            if (_bloodGame == null)
            {
                _bloodGame = new BloodGame(_actHook, _saveData, _apiClass, _imageWorker, _form);
                _bloodGame.Activate();
                BloodStartGame.Background = System.Windows.Media.Brushes.Green;
            }
            else
            {
                _bloodGame.Deactivate();
                _bloodGame = null;
                BloodStartGame.Background = System.Windows.Media.Brushes.Gray;
            }

        }

        private void OrangeBlood_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void GreenBlood_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void BlackBlood_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void VioletBlood_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void GreenBlood_Click(object sender, RoutedEventArgs e)
        {
            _saveData.SetBloodChecked("green", GreenBlood.IsChecked.Value);
            SaveData.Save(_saveData);
            if (_bloodGame != null) _bloodGame.RefreshScanColor();
        }

        private void OrangeBlood_Click(object sender, RoutedEventArgs e)
        {
            _saveData.SetBloodChecked("orange", OrangeBlood.IsChecked.Value);
            SaveData.Save(_saveData);
            if (_bloodGame != null) _bloodGame.RefreshScanColor();
        }

        private void BlackBlood_Click(object sender, RoutedEventArgs e)
        {
            _saveData.SetBloodChecked("black", BlackBlood.IsChecked.Value);
            SaveData.Save(_saveData);
            if (_bloodGame != null) _bloodGame.RefreshScanColor();
        }

        private void VioletBlood_Click(object sender, RoutedEventArgs e)
        {
            _saveData.SetBloodChecked("violet", VioletBlood.IsChecked.Value);
            SaveData.Save(_saveData);
            if (_bloodGame != null) _bloodGame.RefreshScanColor();
        }

        private void BlueBlood_Click(object sender, RoutedEventArgs e)
        {
            _saveData.SetBloodChecked("blue", BlueBlood.IsChecked.Value);
            SaveData.Save(_saveData);
            if (_bloodGame != null) _bloodGame.RefreshScanColor();
        }

        private void ChestStartGame_OnClick(object sender, RoutedEventArgs e)
        {
            if (_chestGame == null)
            {
                _chestGame = new ChestGame(_actHook, _apiClass, _saveData, _imageWorker);
                _chestGame.Activate();
                ChestStartGame.Background = System.Windows.Media.Brushes.Green;
            }
            else
            {
                _chestGame.Deactivate();
                _chestGame = null;
                ChestStartGame.Background = System.Windows.Media.Brushes.Gray;
            }
        }
    }

}
