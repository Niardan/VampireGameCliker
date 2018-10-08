using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vampire_Life_Game_Clicker.Common;

namespace Vampire_Life_Game_Clicker.NewChestGame
{
    class ChestGameInit
    {
        private ImageWorker _imageWorker;
        private readonly UserActivityHook _actHook;
        private readonly WinApiClass _apiClass;
        private MyKeys _left = new MyKeys(0xCB, 37);
        private MyKeys _right = new MyKeys(0xCD, 39);
        private MyKeys _up = new MyKeys(0xC8, 38);
        private MyKeys _down = new MyKeys(0xD0, 40);
        private Pixel _imageUp;
        private Pixel _imageDown;
        private Pixel _imageLeft;
        private Pixel _imageRight;
        private Pixel _cursor;
        private string _pathForImage = @"z:\Vampire Life 0.51.2plus\www\img\chaincmd\";

        public ChestGameInit(ImageWorker imageWorker, UserActivityHook actHook, WinApiClass apiClass)
        {
            _imageWorker = imageWorker;
            _actHook = actHook;
            _apiClass = apiClass;
            var imageDown = new Bitmap(_pathForImage + "down.png");
            var imageUp = new Bitmap(_pathForImage + "up.png");
            var imageLeft = new Bitmap(_pathForImage + "left.png");
            var imageRight = new Bitmap(_pathForImage + "right.png");
            var cursor = new Bitmap(_pathForImage + "cursor.png");

            int offset = 80;
            var bytePixel = GetColorPixel(imageUp, offset);
            _imageUp = new Pixel(bytePixel[0], bytePixel[1], bytePixel[2], bytePixel[3]);
            bytePixel = GetColorPixel(imageDown, offset);
            _imageDown = new Pixel(bytePixel[0], bytePixel[1], bytePixel[2], bytePixel[3]);
            bytePixel = GetColorPixel(imageLeft, offset);
            _imageLeft = new Pixel(bytePixel[0], bytePixel[1], bytePixel[2], bytePixel[3]);
            bytePixel = GetColorPixel(imageRight, offset);
            _imageRight = new Pixel(bytePixel[0], bytePixel[1], bytePixel[2], bytePixel[3]);
            bytePixel = GetColorPixel(cursor, offset);
            _cursor = new Pixel(bytePixel[0], bytePixel[1], bytePixel[2], bytePixel[3]);
            //ViewChestInit view = new ViewChestInit(_imageUp, _imageDown, _imageLeft, _imageRight);
            //view.ShowDialog();
            Activate();
        }

        private byte[] GetColorPixel(Image image, int offset)
        {
            var buffer = _imageWorker.GetBufer(image);
            byte[] pixel = new byte[4];
            for (int i = 0; i < buffer.Length; i += 4)
            {
                if (Math.Abs(buffer[i] - buffer[i + 1]) > offset || Math.Abs(buffer[i + 2] - buffer[i + 1]) > offset)
                {
                    pixel[0] = buffer[i];
                    pixel[1] = buffer[i + 1];
                    pixel[2] = buffer[i + 2];
                    pixel[3] = buffer[i + 3];
                    break;
                }
            }
            return pixel;
        }

        private bool _check;

        private void _actHook_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space)
            {
                _check = !_check;

                //var screenImage = _imageWorker.GetImage(null, leftPoint.X, leftPoint.Y, leftPoint.X + size, leftPoint.Y + size);
                //byte[] image = _imageWorker.GetBufer(screenImage);
                ////_countClick++;

                //var leftPoint = _saveData.ChestLeftPoint;
                //var size = _saveData.ChestSizeCell;
                //var screenImage = _imageWorker.GetImage(null, leftPoint.X, leftPoint.Y, leftPoint.X + size, leftPoint.Y + size);
                //byte[] image = _imageWorker.GetBufer(screenImage);
                //if (_imageWorker.Compareimages(_imageDownBuffer, image))
                //{
                //    _key = _down;
                //    SendKey();
                //}
                //else if (_imageWorker.Compareimages(_imageLeftBuffer, image))
                //{
                //    _key = _left;
                //    SendKey();
                //}
                //else if (_imageWorker.Compareimages(_imageUpBuffer, image))
                //{
                //    _key = _up;
                //    SendKey();
                //}
                //else if (_imageWorker.Compareimages(_imageRightBuffer, image))
                //{
                //    _key = _right;
                //    SendKey();
                //}
            }
        }

        private int _x;
        private int _y;
        private void ActHookOnMouseActions(MouseAction action, int x, int y)
        {

            if (action == MouseAction.RightDown)
            {
                _check = false;
                _x = x;
                _y = y;
                //var screenImage = _imageWorker.GetImage(null, x - 50, y, x + 50, y + 100);
                //ViewChestInit view = new ViewChestInit(new Bitmap(screenImage));
                //view.ShowDialog();
                Thread thread = new Thread(StartCheck);
                thread.Start();
            }
        }

        public void StartCheck()
        {
            for (int i = 0; i < 20; i++)
            {
                var screenImage = _imageWorker.GetImage(null, _x - 25, _y - 25, _x + 25, _y + 25);
                var image = GetToBuffer(screenImage);
                var confines = GetСonfines(_x, _y, 50);

                if (confines.Item1 == 0 && confines.Item2 == 0)
                {
                    return;
                }
                var side = CheckPixels(image, 20, confines.Item1, confines.Item2);
                switch (side)
                {
                    case Sides.Down:
                        SendKey(_down);
                        break;
                    case Sides.Right:
                        SendKey(_right);
                        break;
                    case Sides.Left:
                        SendKey(_left);
                        break;
                    case Sides.Up:
                        SendKey(_up);
                        break;
                }
                Thread.Sleep(10);
            }
          
        }

        private Sides CheckPixels(Pixel[,] image, int offset, int min, int max)
        {
            for (int y = 0; y < image.GetLength(1); y++)
            {
                for (int x = min; x < max; x++)
                {
                    var pixels = image[x, y];

                    if (CheckPixel(pixels, _imageDown, offset)) return Sides.Down;
                    if (CheckPixel(pixels, _imageUp, offset)) return Sides.Up;
                    if (CheckPixel(pixels, _imageLeft, offset)) return Sides.Left;
                    if (CheckPixel(pixels, _imageRight, offset)) return Sides.Right;
                }
            }

            return Sides.Null;
        }

        private bool CheckPixel(Pixel image, Pixel testPixel, int offset)
        {
            if (Math.Abs(image[0] - testPixel[0]) > offset) return false;
            if (Math.Abs(image[1] - testPixel[1]) > offset) return false;
            if (Math.Abs(image[2] - testPixel[2]) > offset) return false;
            return true;
        }

        private Tuple<int, int> GetСonfines(int xPosition, int yPosition, int offset)
        {
            var screenImage = _imageWorker.GetImage(null, xPosition - 25, yPosition, xPosition + 25, yPosition + 100);
            var image = GetToBuffer(screenImage);
            int min = image.GetLength(0);
            int max = 0;
            for(int y = 0; y < image.GetLength(1); y++)
            {
                for (int x = 0; x < image.GetLength(0); x++)
                {
                    var pixels = image[x, y];

                    if (CheckPixel(pixels, _cursor, offset))
                    {
                        if (min > x) min = x;
                        if (max < x) max = x;
                    }
                 
                }
            }
            return new Tuple<int, int>(min, max);
        }


        private Pixel[,] GetToBuffer(Image image)
        {
            var buffer = _imageWorker.GetBufer(image);
            var toBuffer = new Pixel[image.Width, image.Height];
            int z = 0;
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    toBuffer[j, i] = new Pixel(buffer[z], buffer[z + 1], buffer[z + 2], buffer[z + 3]);
                    z += 4;
                }
            }
            return toBuffer;
        }

        private void SendKey(MyKeys key)
        {
            for (int i = 0; i < 500; i++)
            {
                _apiClass.SendKey(key, false);
            }

            _apiClass.SendKey(key, true);
        }
        public void Activate()
        {

            _actHook.KeyDown += _actHook_KeyDown;
            _actHook.MouseActions += ActHookOnMouseActions;
            _actHook.Start();
        }



        public void Deactivate()
        {
            _actHook.KeyDown -= _actHook_KeyDown;
            _actHook.MouseActions -= ActHookOnMouseActions;
            _actHook.Stop();
        }
    }
}
