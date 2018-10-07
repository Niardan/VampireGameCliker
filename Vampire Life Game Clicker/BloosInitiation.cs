using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using MessageBoxOptions = System.Windows.Forms.MessageBoxOptions;

namespace Vampire_Life_Game_Clicker
{
    public class BloosInitiation : Initiation
    {
        private int _state;

        public BloosInitiation(FrameForm frameForm, UserActivityHook actHook, SaveData saveData, ImageWorker imageWorker) : base(frameForm, actHook, saveData, imageWorker)
        {
            State1();
        }

        public void State1()
        {
            _state = 1;
            SendMessage("Выберите рамку игрового поля (обведите ёё мышью с зажатым пробелом)");
            _actHook.Start();
        }

        public void State2()
        {
            _state = 2;
            SendMessage("Нажмите с зажатым пробелом в центр фиолетовой крови");
            _actHook.Start();
        }
        public void State3()
        {
            _state = 3;
            SendMessage("Нажмите с зажатым пробелом в центр черной крови");
            _actHook.Start();
        }
        public void State4()
        {
            _state = 4;
            SendMessage("Нажмите с зажатым пробелом в центр зеленой крови");
            _actHook.Start();
        }
        public void State5()
        {
            _state = 5;
            SendMessage("Нажмите с зажатым пробелом в центр оранжевой крови");
            _actHook.Start();
        }
        public void State6()
        {
            _state = 6;
            SendMessage("Нажмите с зажатым пробелом в центр синей крови");
            _actHook.Start();
        }
        public void State7()
        {
            SendMessage("Успешно");
            Deactivate();
            SaveData.Save(_saveData);
            OnEndInitiation();
        }

        protected override void ActHook_KeyDown(object sender, KeyEventArgs e)
        {
            base.ActHook_KeyDown(sender, e);
            if (_state == 1&& e.KeyData == Keys.Y)
            {
                _actHook.Stop();
                State2();
            }
        }

        protected override void ActHook_MouseActions(MouseAction action, int x, int y)
        {
            base.ActHook_MouseActions(action, x, y);
            switch (_state)
            {
                case 1:
                    if (action == MouseAction.LeftDown && _mouseCaption)
                    {
                        _saveData.BloodStartPoint = new Point(x, y);
                        _frameForm.SetFormGrid(new Point(x, y), _saveData.BloodSizeCell, _saveData.CoutCellHor, _saveData.CountCellVert);
                        
                    }
                    break;

                case 2:
                    if (action == MouseAction.LeftDown && _mouseCaption)
                    {
                        _actHook.Stop();
                        _saveData.SetColor("violet", _imageWorker.GetPixelColor(new System.Drawing.Point(x,y)), true);
                        State3();
                    }
                    break;
                case 3:
                    if (action == MouseAction.LeftDown && _mouseCaption)
                    {
                        _actHook.Stop();
                        _saveData.SetColor("black", _imageWorker.GetPixelColor(new System.Drawing.Point(x, y)), true);
                        State4();
                       
                    }
                    break;
                case 4:
                    if (action == MouseAction.LeftDown && _mouseCaption)
                    {
                        _actHook.Stop();
                        _saveData.SetColor("green", _imageWorker.GetPixelColor(new System.Drawing.Point(x, y)), true);
                        State5();
                    }
                    break;
                case 5:
                    if (action == MouseAction.LeftDown && _mouseCaption)
                    {
                        _actHook.Stop();
                        _saveData.SetColor("orange", _imageWorker.GetPixelColor(new System.Drawing.Point(x, y)), true);
                        State6();
                    }
                    break;
                case 6:
                    if (action == MouseAction.LeftDown && _mouseCaption)
                    {
                        _actHook.Stop();
                        _saveData.SetColor("blue", _imageWorker.GetPixelColor(new System.Drawing.Point(x, y)), true);
                        State7();
                    }
                    break;
            }
        }
    } 
}