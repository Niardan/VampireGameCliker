using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using Vampire_Life_Game_Clicker.ChestClicker;
using Vampire_Life_Game_Clicker.Common;
using Vampire_Life_Game_Clicker.NewBloodGame;
using Vampire_Life_Game_Clicker.NewChestGame;
using Color = System.Windows.Media.Color;
using MouseAction = Vampire_Life_Game_Clicker.Common.MouseAction;

namespace Vampire_Life_Game_Clicker
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly FrameForm _form;
        private readonly UserActivityHook _actHook;
        private readonly SaveData _saveData;
        private readonly ImageWorker _imageWorker;
        private readonly WinApiClass _apiClass = new WinApiClass();
        private readonly BloodGame _bloodGame;
        private readonly ChestGame _chestGame;
        private readonly ChestClick _chestClicker;
        private bool _isOverlay = true;
        private Point _currenntPoint;
        private readonly Timer _timer = new Timer();

        public MainWindow()
        {
            InitializeComponent();
            _timer.Interval = 100;
            _timer.Tick+=TimerOnTick;
            _timer.Start();
            _imageWorker = new ImageWorker(this);
            _form = new FrameForm();
            _actHook = new UserActivityHook();
            _actHook.OnMouseActivity += MouseMoved;
            _actHook.MouseActions += ActHook_MouseActions;
            _actHook.KeyDown += ActHookOnKeyDown;
            _actHook.Start();
            if (SaveData.Load(out _saveData))
            {
                BloodLoad();
            }

            _bloodGame = new BloodGame(_imageWorker, _actHook, _apiClass, _saveData, _form);
            _bloodGame.Activated += BloodGameOnActivated;
            _bloodGame.Deactivated += BloodGameOnDeactivated;
            _chestGame = new ChestGame(_imageWorker, _actHook, _apiClass, _saveData, _form);
            _chestGame.Activated += ChestGameOnActivated;
            _chestGame.Deactivated += ChestGameOnDeactivated;
            _chestClicker = new ChestClick(_imageWorker, _actHook, _apiClass, _saveData, _form);
            _chestClicker.Activated+=ChestClickerOnActivated;
            _chestClicker.Deactivated+=ChestClickerOnDeactivated;
            BloodStartGame.Background = System.Windows.Media.Brushes.Gray;
            ActivateChest.Background = System.Windows.Media.Brushes.Gray;
            ActivateChestClick.Background = System.Windows.Media.Brushes.Gray;
            _form.Show();
        }

        private void TimerOnTick(object sender, EventArgs e)
        {
            string activeName = _apiClass.GetActiveWindowName();
            if (!_isOverlay )
            {
                if (activeName == "VampireLife")
                {
                    _currenntPoint = _apiClass.GetActiveWindowCoord();
                    _isOverlay = true;
                    SetOverlay(_isOverlay);
                    var point = _apiClass.GetActiveWindowCoord();
                    SetOverlayPosition(point);
                }
            }
            else
            {
                if (activeName != "VampireLife")
                {
                    _isOverlay = false;
                    SetOverlay(false);

                }
                else
                {
                    var point = _apiClass.GetActiveWindowCoord();
                    if (_currenntPoint != point)
                    {
                        SetOverlayPosition(point);
                    }
                }
            }
        }

        private void SetOverlay(bool active)
        {
            _form.SetOverlayEnabled(active);
        }

        private void SetOverlayPosition(Point point)
        {
            _form.SetPosition(point);
        }

        private void ChestClickerOnDeactivated(BaseGame sender)
        {
            ActivateChestClick.Background = System.Windows.Media.Brushes.Gray;
        }

        private void ChestClickerOnActivated(BaseGame sender)
        {
            ActivateChestClick.Background = System.Windows.Media.Brushes.Green;
        }

        private void ActHookOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Oem3)
            {
                if (!_form.IsVisible)
                {
                    _isOverlay = true;
                    _form.Show();
                }
                else
                {
                    _isOverlay = false;
                    _form.Hide();
                }
            }
            if (e.KeyData == Keys.D1)
            {
                StartBloodGame();
            }
            if (e.KeyData == Keys.D2)
            {
                StartChestGame();
            }
            if (e.KeyData == Keys.D3)
            {
                StartChestClicker();
            }
            if (e.KeyData == Keys.D5)
            {
                _chestGame.Deactivate();
                _bloodGame.Deactivate();
                _chestClicker.Deactivate();
            }
        }

        private void StartBloodInitiation()
        {
            _chestGame.Deactivate();
            _bloodGame.Deactivate();
            _form.Show();
            var init = new BloosInitiation(_form, _actHook, _saveData, _imageWorker);
            init.EndInitiation += Init_BloodEndInitiation;
        }

        private void StartBloodGame()
        {
            _chestGame.Deactivate();
            _chestClicker.Deactivate();
            _bloodGame.Activate();
        }

        private void StartChestGame()
        {
            _bloodGame.Deactivate();
            _chestClicker.Deactivate();
            _chestGame.Activate();
        }
        private void StartChestClicker()
        {
            _chestGame.Deactivate();
            _bloodGame.Deactivate();
            _chestClicker.Activate();
        }

        private void ChestGameOnDeactivated(BaseGame sender)
        {
            ActivateChest.Background = System.Windows.Media.Brushes.Gray;
        }

        private void ChestGameOnActivated(BaseGame sender)
        {
            ActivateChest.Background = System.Windows.Media.Brushes.Green;

        }

        private void BloodGameOnDeactivated(BaseGame sender)
        {
            BloodStartGame.Background = System.Windows.Media.Brushes.Gray;
        }

        private void BloodGameOnActivated(BaseGame sender)
        {
            BloodStartGame.Background = System.Windows.Media.Brushes.Green;
        }

        private void ActHook_MouseActions(MouseAction action, int x, int y)
        {
        }

        public void MouseMoved(object sender, System.Windows.Forms.MouseEventArgs e)
        {

        }

        private void Init_BloodEndInitiation(Initiation sender)
        {
            sender.EndInitiation -= Init_BloodEndInitiation;

            BloodLoad();
        }

        private void BloodLoad()
        {
            var color = _saveData.GetColor("violet").Pixels;
            VioletBloodCollor.Background = new SolidColorBrush(Color.FromArgb(color[3], color[2], color[1], color[0]));
            color = _saveData.GetColor("green").Pixels;
            GreenBloodCollor.Background = new SolidColorBrush(Color.FromArgb(color[3], color[2], color[1], color[0]));
            color = _saveData.GetColor("orange").Pixels;
            OrangeBloodCollor.Background = new SolidColorBrush(Color.FromArgb(color[3], color[2], color[1], color[0]));

            GreenBlood.IsChecked = _saveData.GetColor("green").Check;
            VioletBlood.IsChecked = _saveData.GetColor("violet").Check;
            OrangeBlood.IsChecked = _saveData.GetColor("orange").Check;
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            _form.Close();
            Environment.Exit(0);
        }

        private void BloodInitiationButton_Click(object sender, RoutedEventArgs e)
        {
            StartBloodInitiation();
        }

        private void BloodStartGame_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_bloodGame.IsActive)
            {
                StartBloodGame();
            }
            else
            {
                _bloodGame.Deactivate();
            }

        }

        private void GreenBlood_Click(object sender, RoutedEventArgs e)
        {
            _saveData.SetBloodChecked("green", GreenBlood.IsChecked.Value);
            SaveData.Save(_saveData);
            if (_bloodGame.IsActive) _bloodGame.RefreshScanColor();
        }

        private void OrangeBlood_Click(object sender, RoutedEventArgs e)
        {
            _saveData.SetBloodChecked("orange", OrangeBlood.IsChecked.Value);
            SaveData.Save(_saveData);
            if (_bloodGame.IsActive) _bloodGame.RefreshScanColor();
        }

        private void VioletBlood_Click(object sender, RoutedEventArgs e)
        {
            _saveData.SetBloodChecked("violet", VioletBlood.IsChecked.Value);
            SaveData.Save(_saveData);
            if (_bloodGame.IsActive) _bloodGame.RefreshScanColor();
        }

        private void ActivateChest_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_chestGame.IsActive)
            {
                StartChestGame();
            }
            else
            {
                _chestGame.Deactivate();
            }
        }

        private void ActivateChestClick_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_chestClicker.IsActive)
            {
                StartChestClicker();
            }
            else
            {
                _chestClicker.Deactivate();
            }
        }
    }

}
