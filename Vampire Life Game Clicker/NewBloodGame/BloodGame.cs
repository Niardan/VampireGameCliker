using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Vampire_Life_Game_Clicker.Common;

namespace Vampire_Life_Game_Clicker.NewBloodGame
{
    public class BloodGame : BaseGame
    {
        private Point _leftAngle;
        private Point _rightAngle;
      
        private Thread _gameThread;

        public BloodGame(ImageWorker imageWorker, UserActivityHook actHook, WinApiClass apiClass, SaveData saveData, FrameForm frame) : base(imageWorker, actHook, apiClass, saveData, frame)
        {
            
        }

        protected override void ActHookOnMouseActions(MouseAction action, int x, int y)
        {

            if (action == MouseAction.RightDown)
            {
                InitGame(new Point(x, y));
                var listPixel = RefreshScanColor();
                if (_gameThread != null)
                {
                    _gameThread.Abort();
                }
                _gameThread = new Thread(CheckGame);
                _gameThread.Start(listPixel);
            }
        }

        public List<Pixel> RefreshScanColor()
        {
            var listPixel = new List<Pixel>();
            var pixel = _saveData.GetColor("violet");
            if (pixel.Check)
            {
                listPixel.Add(pixel.Pixels);
            }
            pixel = _saveData.GetColor("green");
            if (pixel.Check)
            {
                listPixel.Add(pixel.Pixels);
            }
            pixel = _saveData.GetColor("orange");
            if (pixel.Check)
            {
                listPixel.Add(pixel.Pixels);
            }

            return listPixel;
        }

        private void CheckGame(object stage)
        {
            var listPixel = (List<Pixel>)stage;
            while (true)
            {
                var screenImage = _imageWorker.GetImage(_leftAngle.X, _leftAngle.Y, _rightAngle.X, _rightAngle.Y);
                var image = _imageWorker.GetToBuffer(screenImage);
                int step = (_rightAngle.X - _leftAngle.X) / 20;
                foreach (var item in listPixel)
                {
                    CheckGameField(image, item, step, 20);
                }
            }
        }

        private void CheckGameField(Pixel[,] field, Pixel check, int step, int offset)
        {
            for (int i = 0; i < field.GetLength(1); i += step)
            {
                for (int j = 0; j < field.GetLength(0); j += step)
                {
                    if (field[j, i].CheckPixel(check, offset))
                    {
                        ClickPixel(check, j, i, offset);
                        break;
                    }
                }
            }
        }

        private void ClickPixel(Pixel check, int x, int y, int offset)
        {
            var xPoint = _leftAngle.X + x;
            var yPoint = _leftAngle.Y + y;
            _apiClass.SetCursorPosition(xPoint, yPoint);
            var currentPixel = _imageWorker.GetPixel(new Point(xPoint, yPoint));

            if (check.CheckPixel(currentPixel, offset))
            {
                _apiClass.PressLeftMouse();
                Thread.Sleep(200);
            }
        }

        private void InitGame(Point point)
        {
            var screenImage = _imageWorker.GetScreenImage();
            var image = _imageWorker.GetToBuffer(screenImage);
            var pixel = image[point.X, point.Y];
            int minX = image.GetLength(0);
            int maxX = 0;
            int stage = 0;
            int count = 0;
            int countIndex = 15;

            int offset = 55;
            for (int i = 0; i < image.GetLength(0); i++)
            {
                if (pixel.CheckPixel(image[i, point.Y], offset))
                {
                    if (stage == 0)
                    {
                        if (count < countIndex)
                        {
                            count++;
                        }
                        else
                        {
                            minX = i;
                            stage = 1;
                            count = 0;
                        }
                    }

                    if (stage == 1)
                    {
                        maxX = i;
                    }
                }
                else
                {
                    if (stage == 1)
                    {
                        if (count < countIndex)
                        {
                            count++;
                        }
                        else
                        {
                            stage = 2;
                            count = 0;
                        }
                    }
                }
            }

            int minY = image.GetLength(1);
            int maxY = 0;
            stage = 0;
            count = 0;
            for (int i = 0; i < image.GetLength(1); i++)
            {
                if (pixel.CheckPixel(image[point.X, i], offset))
                {
                    if (stage == 0)
                    {
                        if (count < countIndex)
                        {
                            count++;
                        }
                        else
                        {
                            minY = i;
                            stage = 1;
                            count = 0;
                        }
                    }

                    if (stage == 1)
                    {
                        maxY = i;
                    }
                }
                else
                {
                    if (stage == 1)
                    {
                        if (count < countIndex)
                        {
                            count++;
                        }
                        else
                        {
                            stage = 2;
                            count = 0;
                        }
                    }
                }
            }
            _leftAngle = new Point(minX, minY);
            _rightAngle = new Point(maxX, maxY);
            
            _frame.SetNewRectangle(_leftAngle.X, _leftAngle.Y, _rightAngle.X, _rightAngle.Y);
        }

        protected override void ActHookOnKeyDown(object sender, KeyEventArgs e)
        {
            base.ActHookOnKeyDown(sender, e);
            if (e.KeyData == Keys.Z)
            {
                Deactivate();
            }
        }

        public override void Activate()
        {
            base.Activate();
            _frame.SetBloodGameActive(true);
        }

        public override void Deactivate()
        {
            _frame.SetNewRectangle(0,0,0,0);
            _gameThread?.Abort();
            base.Deactivate();
            _frame.SetBloodGameActive(false);
        }
    }
}