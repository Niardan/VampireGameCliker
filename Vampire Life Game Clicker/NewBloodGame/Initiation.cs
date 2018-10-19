using System.Windows.Forms;
using Vampire_Life_Game_Clicker.Common;

namespace Vampire_Life_Game_Clicker.NewBloodGame
{
    public delegate void InitInitiation(Initiation sender);
    public class Initiation
    {
        protected readonly FrameForm _frameForm;
        protected readonly UserActivityHook _actHook;
        protected readonly SaveData _saveData;

        protected double _x;
        protected double _y;

        protected bool _frameStart;
        protected bool _mouseCaption;
        protected readonly ImageWorker _imageWorker;

        public event InitInitiation EndInitiation;


        public Initiation(FrameForm frameForm, UserActivityHook actHook, SaveData saveData, ImageWorker imageWorker)
        {
            _frameForm = frameForm;
            _actHook = actHook;
            _saveData = saveData;
            _imageWorker = imageWorker;
            _actHook.OnMouseActivity += MouseMoved;
            _actHook.MouseActions += ActHook_MouseActions;
            _actHook.KeyDown += ActHook_KeyDown;
            _actHook.KeyUp += _actHook_KeyUp;
        }

        private void _actHook_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space)
            {
                _mouseCaption = false;
                _frameStart = false;
            }
        }

        protected virtual void ActHook_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space)
            {
                _mouseCaption = true;
            }
        }

        protected virtual void ActHook_MouseActions(MouseAction action, int x, int y)
        {
            if (_mouseCaption)
            {
                if (action == MouseAction.LeftDown)
                {
                    _x = x;
                    _y = y;
                    _frameStart = true;
                }
                else if (action == MouseAction.LeftUp)
                {
                    _frameStart = false;
                }
            }

        }

        protected virtual void MouseMoved(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
        }

        protected void Deactivate()
        {
            _actHook.Stop();
            _actHook.OnMouseActivity -= new MouseEventHandler(MouseMoved);
            _actHook.MouseActions -= ActHook_MouseActions;
            _actHook.KeyDown -= ActHook_KeyDown;
        }

        protected void OnEndInitiation()
        {
            EndInitiation?.Invoke(this);
        }

        protected void SendMessage(string text)
        {
            _frameForm.SetMessageLabel(text);
        }

    }
}