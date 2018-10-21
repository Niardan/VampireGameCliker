using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Vampire_Life_Game_Clicker.Common;

namespace Vampire_Life_Game_Clicker.NewChestGame
{
    class ChestGame : BaseGame
    {
        private readonly MyKeys _left = new MyKeys(0xCB, 37);
        private readonly MyKeys _right = new MyKeys(0xCD, 39);
        private readonly MyKeys _up = new MyKeys(0xC8, 38);
        private readonly MyKeys _down = new MyKeys(0xD0, 40);
        private readonly Pixel _imageUp;
        private readonly Pixel _imageDown;
        private readonly Pixel _imageLeft;
        private readonly Pixel _imageRight;
        private readonly Pixel _cursor;
        private string _pathForImage = @"C:\Users\NIka\Desktop\Vampire Life 0.51.2plus\www\img\chaincmd\";

        public ChestGame(ImageWorker imageWorker, UserActivityHook actHook, WinApiClass apiClass, SaveData saveData, FrameForm form) : base(imageWorker, actHook, apiClass, saveData, form)
        {
            var imageDown = new Bitmap(_pathForImage + "down.png");
            var imageUp = new Bitmap(_pathForImage + "up.png");
            var imageLeft = new Bitmap(_pathForImage + "left.png");
            var imageRight = new Bitmap(_pathForImage + "right.png");
            var cursor = new Bitmap(_pathForImage + "cursor.png");

            int offset = 70;
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

        protected override void ActHookOnMouseActions(MouseAction action, int x, int y)
        {
            if (action == MouseAction.RightDown)
            {
                Thread thread = new Thread(StartCheck);
                thread.Start(new Point(x, y));
            }
        }

        private List<MyKeys> _keys = new List<MyKeys>();

        public void StartCheck(object stage)
        {
            var point = (Point)stage;

            var confines = GetСonfines(point, 50);
            int min = confines.Item1;
            int max = confines.Item2;
            if (min <= 0 || max <= 0|| min >= max)
            {
                return;
            }
            for (int i = 0; i < 20; i++)
            {
                var screenImage = _imageWorker.GetImage(min, point.Y - 25, max, point.Y + 25);
                var image = _imageWorker.GetToBuffer(screenImage);

                var side = CheckPixels(image, 15);
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
            }

        }

        private Sides CheckPixels(Pixel[,] image, int offset)
        {
            for (int y = 0; y < image.GetLength(1); y++)
            {
                for (int x = 0; x < image.GetLength(0); x++)
                {
                    var pixels = image[x, y];

                    if (pixels.CheckPixel(_imageDown, offset)) return Sides.Down;
                    if (pixels.CheckPixel(_imageUp, offset)) return Sides.Up;
                    if (pixels.CheckPixel(_imageLeft, offset)) return Sides.Left;
                    if (pixels.CheckPixel(_imageRight, offset)) return Sides.Right;
                }
            }

            return Sides.Null;
        }

        private Tuple<int, int> GetСonfines(Point koord, int offset)
        {
            var screenImage = _imageWorker.GetImage(koord.X - 25, koord.Y, koord.X + 25, koord.Y + 100);
            var image = _imageWorker.GetToBuffer(screenImage);
            int min = image.GetLength(0);
            int max = 0;
            for (int y = 0; y < image.GetLength(1); y++)
            {
                for (int x = 0; x < image.GetLength(0); x++)
                {
                    var pixels = image[x, y];

                    if (pixels.CheckPixel(_cursor, offset))
                    {
                        if (min > x) min = x;
                        if (max < x) max = x;
                    }
                }
            }
            return new Tuple<int, int>(koord.X - 25 + min, koord.X - 25 + max);
        }

        private void SendKey(MyKeys key)
        {
            _keys.Add(key);
            for (int i = 0; i < 200; i++)
            {
                _apiClass.SendKey(key, false);
            }

            _apiClass.SendKey(key, true);
        }

        public override void Activate()
        {
            base.Activate();
            _frame.SetChestGameActive(true);
        }

        public override void Deactivate()
        {
            base.Deactivate();
            _frame.SetChestGameActive(false);
        }
    }
}
