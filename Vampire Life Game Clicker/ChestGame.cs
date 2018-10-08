using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Vampire_Life_Game_Clicker
{
    class ChestGame
    {          
        private readonly UserActivityHook _actHook;
        private readonly WinApiClass _apiClass;
        private readonly SaveData _saveData;
        private readonly ImageWorker _imageWorker;
        private MyKeys _key;
        private DispatcherTimer _timer = new DispatcherTimer();

        private MyKeys _left = new MyKeys(0xCB, 37);
        private MyKeys _right = new MyKeys(0xCD, 39);
        private MyKeys _up = new MyKeys(0xC8, 38);
        private MyKeys _down = new MyKeys(0xD0, 40);

        private byte[] _imageUpBuffer;
        private byte[] _imageDownBuffer;
        private byte[] _imageRightBuffer;
        private byte[] _imageLeftBuffer;


        public ChestGame(UserActivityHook actHook, WinApiClass apiClass, SaveData saveData, ImageWorker imageWorker)
        {
            _actHook = actHook;
            _apiClass = apiClass;
            _saveData = saveData;
            _imageWorker = imageWorker;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            _timer.Tick += _timer_Tick;
        }

      
        public void Activate()
        {
            _imageUpBuffer = _imageWorker.GetBufer(_saveData.UpArrowImage);
            _imageDownBuffer = _imageWorker.GetBufer(_saveData.DownArrowImage);
            _imageLeftBuffer = _imageWorker.GetBufer(_saveData.LeftArrowImage);
            _imageRightBuffer =  _imageWorker.GetBufer(_saveData.RightArrowImage);

            _actHook.KeyDown += _actHook_KeyDown;
            _actHook.Start();
        }

        private int _countClick =0;
        private int countPressed=0;
        private void _actHook_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space)
            {
                 _countClick++;
               
                var leftPoint = _saveData.ChestLeftPoint;
                var size = _saveData.ChestSizeCell;
                var screenImage = _imageWorker.GetImage(null, leftPoint.X, leftPoint.Y, leftPoint.X + size, leftPoint.Y + size);
                byte[] image = _imageWorker.GetBufer(screenImage);
                 if (_imageWorker.Compareimages(_imageDownBuffer, image))
                {
                       _key = _down;
                    SendKey();
                }
                else if (_imageWorker.Compareimages(_imageLeftBuffer, image))
                {
                    _key = _left;
                     SendKey();
                }
                else if (_imageWorker.Compareimages(_imageUpBuffer, image))
                {
                    _key = _up;
                    SendKey();
                }
                else if (_imageWorker.Compareimages(_imageRightBuffer, image))
                {
                    _key = _right;
                    SendKey();
                }
            }
        }

        private void SendKey()
        {
            countPressed++;
            for (int i = 0; i < 500; i++)
            {
                _apiClass.SendKey(_key, false);
            }

            _apiClass.SendKey(_key, true);
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            
        }

        public void Deactivate()
        {
            _timer.Tick -= _timer_Tick;
            _actHook.KeyDown -= _actHook_KeyDown;
            _actHook.Stop();
        }
    }
}
