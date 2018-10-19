using System;
using System.Threading;
using System.Windows.Forms;
using Vampire_Life_Game_Clicker.Common;

namespace Vampire_Life_Game_Clicker.ChestClicker
{
    public class ChestClick : BaseGame
    {
        private bool _clickEnabled;
        private int _currentX;
        private int _currentY;
        public ChestClick(ImageWorker imageWorker, UserActivityHook actHook, WinApiClass apiClass, SaveData saveData, FrameForm frame) : base(imageWorker, actHook, apiClass, saveData, frame)
        {
        }

        protected override void ActHookOnOnMouseActivity(object sender, MouseEventArgs e)
        {
            if (_clickEnabled && (Math.Abs(_currentX - e.X) > 5 || Math.Abs(_currentY - e.Y) > 5))
            {
                _clickEnabled = false;
            }
        }


        protected override void ActHookOnMouseActions(MouseAction action, int x, int y)
        {
            if (!_clickEnabled && action == MouseAction.RightDown)
            {
                _currentY = y;
                _currentX = x;
                _clickEnabled = true;
                Thread thread = new Thread(Click);
                thread.Start();
            }
        }

        private void Click()
        {
            while (_clickEnabled)
            {
                _apiClass.PressLeftMouse();
                Thread.Sleep(300);
            }
        }

        public override void Activate()
        {
            _frame.SetChestClickerActive(true);
            base.Activate();
        }

        public override void Deactivate()
        {
            _clickEnabled = false;
            _frame.SetChestClickerActive(false);
            base.Deactivate();
        }
    }
}