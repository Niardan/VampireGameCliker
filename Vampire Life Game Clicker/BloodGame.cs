using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Vampire_Life_Game_Clicker
{
    public class BloodGame
    {
        private readonly UserActivityHook _actHook;
        private readonly WinApiClass _apiClass;
        private readonly SaveData _saveData;
        private readonly ImageWorker _imageWorker;
        private readonly FrameForm _frameForm;
        private Dictionary<Point, byte[]> _cells = new Dictionary<Point, byte[]>();
        private DispatcherTimer _timer = new DispatcherTimer();
        private DispatcherTimer _timer2 = new DispatcherTimer();
        private bool _isEnabled;

        private List<byte[]> _pixels;
        public BloodGame(UserActivityHook actHook, SaveData saveData, WinApiClass apiClass, ImageWorker imageWorker, FrameForm frameForm)
        {
            _actHook = actHook;
            _saveData = saveData;
            _apiClass = apiClass;
            _imageWorker = imageWorker;
            _frameForm = frameForm;
            RefreshScanColor();
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            _timer.Tick += _timer_Tick;
            _timer2.Interval = new TimeSpan(0, 0, 0, 0, 100);
            _timer2.Tick += _timer2_Tick; ;
        }

        private void _timer2_Tick(object sender, EventArgs e)
        {
            _timer2.Stop();
            _apiClass.PressLeftMouse();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            var point = _saveData.BloodStartPoint;
            var size = _saveData.BloodSizeCell;
            var image = _imageWorker.GetImage("test", point.X, point.Y, point.X + size * _saveData.CoutCellHor, point.Y + size * _saveData.CountCellVert);

            Point point1;
            byte[] buffer;
            _imageWorker.GetBufer(image, out buffer);
            byte[] pixel;
            if (CheckFullImage(buffer, _pixels, out point1, out pixel))
            {
                var xPoint = (int)point.X + size * point1.X + size / 2;
                var yPoint = (int)point.Y + size * point1.Y + size / 2;
                _apiClass.SetCursorPosition(xPoint, yPoint);
                var pixelImage = _imageWorker.GetImage("test1", xPoint, yPoint, xPoint +1, yPoint + 1);
                byte[] bufferPixel;
                _imageWorker.GetBufer(pixelImage, out bufferPixel);
                if (CheckPixel(bufferPixel, pixel, 20))
                {
                    _apiClass.PressLeftMouse();
                    _apiClass.PressLeftMouse();
                }
            }
        }

        public void RefreshScanColor()
        {
            _pixels = new List<byte[]>();
            var pixel = _saveData.GetColor("violet");
            if (pixel.Check)
            {
                _pixels.Add(pixel.Pixels);
            }

            pixel = _saveData.GetColor("blue");
            if (pixel.Check)
            {
                _pixels.Add(pixel.Pixels);
            }

            pixel = _saveData.GetColor("black");
            if (pixel.Check)
            {
                _pixels.Add(pixel.Pixels);
            }

            pixel = _saveData.GetColor("orange");
            if (pixel.Check)
            {
                _pixels.Add(pixel.Pixels);
            }
            pixel = _saveData.GetColor("green");
            if (pixel.Check)
            {
                _pixels.Add(pixel.Pixels);
            }
        }

        private void ActHook_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space)
            {
                if (_isEnabled)
                {
                    _timer.Stop();
                }
                else
                {
                    _timer.Start();
                }
                _isEnabled = !_isEnabled;
            }
        }

        private void GenCells(byte[] image)
        {
            int size = _saveData.BloodSizeCell;
            int cellLineNumber = 0;
            int lineCount = 0;
            for (int i = 0; i < image.Length; i += size * 4)
            {
                int number = (i / size / 4);
                if (number % _saveData.CoutCellHor == 0)
                {
                    lineCount++;
                }
                //if (lineCount%4==0)
                //{
                byte[] buffer = new byte[size * 4];
                Array.Copy(image, i, buffer, 0, size * 4);
                AddLineCell(new Point(number % _saveData.CoutCellHor, cellLineNumber), buffer);
                //}
                if (lineCount == size)
                {
                    cellLineNumber++;
                    lineCount = 0;
                }
            }
        }

        private void AddLineCell(Point idCell, byte[] buffer)
        {
            byte[] cellBuffer;
            if (_cells.TryGetValue(idCell, out cellBuffer))
            {
                var z = new byte[cellBuffer.Length + buffer.Length];
                cellBuffer.CopyTo(z, 0);
                buffer.CopyTo(z, cellBuffer.Length);
                _cells[idCell] = z;
            }
            else
            {
                _cells[idCell] = buffer;
            }
        }

        private bool CheckPixel(byte[] image, byte[] pixel, int offset)
        {
            for (int i = 0; i < image.Length; i += 4)
            {
                if (Math.Abs(image[i] - pixel[0]) > offset) continue;
                if (Math.Abs(image[i + 1] - pixel[1]) > offset) continue;
                if (Math.Abs(image[i + 2] - pixel[2]) > offset) continue;
                return true;
            }
            return false;
        }

        private bool CheckFullImage(byte[] image, List<byte[]> pixels, out Point point, out byte[] outPixel)
        {
            _cells.Clear();
            GenCells(image);
            foreach (var pixel in pixels)
            {
                foreach (var item in _cells)
                {
                    if (CheckPixel(item.Value, pixel, 5))
                    {
                        point = item.Key;
                        outPixel = pixel;
                        return true;
                    }
                }
            }

            point = new Point();
            outPixel = null;
            return false;
        }

        public void Activate()
        {
            _actHook.KeyDown += ActHook_KeyDown;
            _actHook.Start();
        }

        public void Deactivate()
        {
            _timer.Stop();
            _actHook.KeyDown -= ActHook_KeyDown;
        }
    }
}