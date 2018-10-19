using System.Windows.Forms;

namespace Vampire_Life_Game_Clicker.Common
{
    public delegate void ActivateBaseGemeHandler(BaseGame sender);
    public class BaseGame
    {
        private bool _isActive;

        protected readonly ImageWorker _imageWorker;
        protected readonly UserActivityHook _actHook;
        protected readonly WinApiClass _apiClass;
        protected readonly SaveData _saveData;
        protected readonly FrameForm _frame;

        public event ActivateBaseGemeHandler Activated;
        public event ActivateBaseGemeHandler Deactivated;

        public BaseGame(ImageWorker imageWorker, UserActivityHook actHook, WinApiClass apiClass, SaveData saveData, FrameForm frame)
        {
            _imageWorker = imageWorker;
            _actHook = actHook;
            _apiClass = apiClass;
            _saveData = saveData;
            _frame = frame;
        }
        public bool IsActive
        {
            get { return _isActive; }
        }
        protected virtual void ActHookOnKeyDown(object sender, KeyEventArgs e)
        {
            
        }

        protected virtual void ActHookOnOnMouseActivity(object sender, MouseEventArgs e)
        {
           
        }

        protected virtual void ActHookOnMouseActions(MouseAction action, int x, int y)
        {
          
        }

        public virtual void Activate()
        {
            if (!IsActive)
            {
                Activated?.Invoke(this);
                _isActive = true;
                _actHook.MouseActions += ActHookOnMouseActions;
                _actHook.OnMouseActivity += ActHookOnOnMouseActivity;
                _actHook.KeyDown += ActHookOnKeyDown;
            }
        }

        public virtual void Deactivate()
        {
            if (IsActive)
            {
                Deactivated?.Invoke(this);
                _isActive = false;
                _actHook.MouseActions -= ActHookOnMouseActions;
                _actHook.OnMouseActivity -= ActHookOnOnMouseActivity;
                _actHook.KeyDown -= ActHookOnKeyDown;
            }
        }
    }
}