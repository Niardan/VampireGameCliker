using System.Windows.Forms;
using Vampire_Life_Game_Clicker.Common;

namespace Vampire_Life_Game_Clicker.NewBloodGame
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
            SendMessage("Нажмите с зажатым пробелом в центр фиолетовой крови");
        }
        public void State2()
        {
            _state = 2;
            SendMessage("Нажмите с зажатым пробелом в центр зеленой крови");
        }

        public void State3()
        {
            _state = 3;
            SendMessage("Нажмите с зажатым пробелом в центр оранжевой крови");
        }

        public void State4()
        {
            SendMessage("");
            Deactivate();
              SaveData.Save(_saveData);
            OnEndInitiation();
        }

        protected override void ActHook_MouseActions(MouseAction action, int x, int y)
        {
            base.ActHook_MouseActions(action, x, y);
            switch (_state)
            {
                case 1:
                    if (action == MouseAction.LeftDown && _mouseCaption)
                    {
                        _saveData.SetColor("violet", _imageWorker.GetPixelColor(new System.Drawing.Point(x,y)), true);
                        State2();
                    }
                    break;
                case 2:
                    if (action == MouseAction.LeftDown && _mouseCaption)
                    {
                        _saveData.SetColor("green", _imageWorker.GetPixelColor(new System.Drawing.Point(x, y)), true);
                        State3();
                    }
                    break;
                case 3:
                    if (action == MouseAction.LeftDown && _mouseCaption)
                    {
                        _saveData.SetColor("orange", _imageWorker.GetPixelColor(new System.Drawing.Point(x, y)), true);
                        State4();
                    }
                    break;
            }

        }

        protected override void MouseMoved(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _frameForm.SetNewCross(e.X,e.Y);
        }
    } 
}