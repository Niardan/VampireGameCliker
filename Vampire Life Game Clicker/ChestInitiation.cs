using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using MessageBoxOptions = System.Windows.Forms.MessageBoxOptions;

namespace Vampire_Life_Game_Clicker
{
    public class ChestInitiation : Initiation
    {
        private int _state;
        public ChestInitiation(FrameForm frameForm, UserActivityHook actHook, SaveData saveData, ImageWorker imageWorker) : base(frameForm, actHook, saveData, imageWorker)
        {
            State1();
        }

        public void State1()
        {
            _state = 1;

            MessageBox.Show("Выберите рамку игрового поля (обведите ёё мышью с зажатым пробелом)", "Поверх всех окон", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            
            _actHook.Start();
        }

        protected override void ActHook_MouseActions(MouseAction action, int x, int y)
        {
            base.ActHook_MouseActions(action, x, y);
            switch (_state)
            {
                case 1:
                    if (action == MouseAction.LeftDown && _mouseCaption)
                    {
                        _saveData.ChestLeftPoint = new Point(x, y);
                    }
                    else if (action == MouseAction.LeftUp && _mouseCaption)
                    {
                        _saveData.ChestSizeCell = (int)(x - _x);
                        _actHook.Stop();
                        State2();
                    }
                    break;
            }
        }

        protected override void ActHook_KeyDown(object sender, KeyEventArgs e)
        {
            base.ActHook_KeyDown(sender, e);

            if (_state == 2)
            {
                if (e.KeyData == Keys.Up && _saveData.UpArrowImage == null)
                {
                    _saveData.UpArrowImage = _imageWorker.GetImage("key_up", _saveData.ChestLeftPoint.X, _saveData.ChestLeftPoint.Y, _saveData.ChestLeftPoint.X + _saveData.ChestSizeCell, _saveData.ChestLeftPoint.Y + _saveData.ChestSizeCell);
                }
                if (e.KeyData == Keys.Down && _saveData.DownArrowImage == null)
                {
                    _saveData.DownArrowImage = _imageWorker.GetImage("key_down", _saveData.ChestLeftPoint.X, _saveData.ChestLeftPoint.Y, _saveData.ChestLeftPoint.X + _saveData.ChestSizeCell, _saveData.ChestLeftPoint.Y + _saveData.ChestSizeCell);
                }
                if (e.KeyData == Keys.Right && _saveData.RightArrowImage == null)
                {
                    _saveData.RightArrowImage = _imageWorker.GetImage("key_right", _saveData.ChestLeftPoint.X, _saveData.ChestLeftPoint.Y, _saveData.ChestLeftPoint.X + _saveData.ChestSizeCell, _saveData.ChestLeftPoint.Y + _saveData.ChestSizeCell);
                }
                if (e.KeyData == Keys.Left && _saveData.LeftArrowImage == null)
                {
                    _saveData.LeftArrowImage = _imageWorker.GetImage("key_left", _saveData.ChestLeftPoint.X, _saveData.ChestLeftPoint.Y, _saveData.ChestLeftPoint.X + _saveData.ChestSizeCell, _saveData.ChestLeftPoint.Y + _saveData.ChestSizeCell);
                }

                if (CheckAllImage())
                {
                    State3();
                }
            }

        }

        private bool CheckAllImage()
        {
            if (_saveData.UpArrowImage == null || _saveData.DownArrowImage == null || _saveData.LeftArrowImage == null || _saveData.RightArrowImage == null)
            {
                return false;
            }
            return true;
        }

        public void State2()
        {
            _saveData.ClearArrowImage();
            _state = 2;
            MessageBox.Show("Нажимайте стрелки пока не заполнятся все четрые положения.", "Поверх всех окон", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
         
            _actHook.Start();
        }

        public void State3()
        {
            _actHook.Stop();
            _state = 3;
            MessageBox.Show("Инициализация завершена", "Поверх всех окон", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
           
            SaveData.Save(_saveData);
            Deactivate();
            OnEndInitiation();
        }
    }
}