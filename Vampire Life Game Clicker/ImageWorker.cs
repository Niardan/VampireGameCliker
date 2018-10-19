using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Vampire_Life_Game_Clicker.Common;
using Color = System.Drawing.Color;

namespace Vampire_Life_Game_Clicker
{
    public class ImageWorker
    {
        private MainWindow _window;

        public ImageWorker(MainWindow window)
        {
            _window = window;
        }

        public Image ResizeImage(Image image, int x, int y, int xRight, int yDown)
        {
            Bitmap bmp = image as Bitmap;

            // Check if it is a bitmap:
            if (bmp == null)
                throw new ArgumentException("No valid bitmap");

            // Crop the image:
            Bitmap cropBmp = bmp.Clone(new Rectangle(x, y, (xRight - x), yDown - y), bmp.PixelFormat);

            // Release the resources:
            image.Dispose();
            return cropBmp;
        }

        public Image GetImage(double x, double y, double xRight, double yDown)
        {

            Size ScreenSize = Screen.PrimaryScreen.Bounds.Size;
            Bitmap image = new Bitmap(ScreenSize.Width, ScreenSize.Height);
            Image newImage;
            using (Graphics g = Graphics.FromImage(image))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, ScreenSize);

                newImage = ResizeImage(image, (int)x, (int)y, (int)xRight, (int)yDown);
            }
            return newImage;
        }
        public Image GetScreenImage()
        {
            Size screenSize = Screen.PrimaryScreen.Bounds.Size;
            Bitmap image = new Bitmap(screenSize.Width, screenSize.Height);
            Image newImage;
            using (Graphics g = Graphics.FromImage(image))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, screenSize);
            }
            return image;
        }

        public Color GetPixelColor(Point point)
        {
            Image image = GetImage(point.X, point.Y, point.X + 1, point.Y + 1);

            byte[] buffer = GetBufer(image);
            return Color.FromArgb(buffer[3], buffer[2], buffer[1], buffer[0]);
        }

        public Pixel GetPixel(Point point)
        {
            Image image = GetImage(point.X, point.Y, point.X + 1, point.Y + 1);
            byte[] buffer = GetBufer(image);
            return new Pixel(buffer[0], buffer[1], buffer[2], buffer[3]);
        }

        public byte[] GetBufer(Image image)
        {
            byte[] imageBuffer;
            Bitmap first = image as Bitmap;
            BitmapData datefirst = first.LockBits(new Rectangle(0, 0, first.Width, first.Height), ImageLockMode.ReadOnly, first.PixelFormat);
            int size1 = datefirst.Stride * datefirst.Height;
            imageBuffer = new byte[size1];
            Marshal.Copy(datefirst.Scan0, imageBuffer, 0, imageBuffer.Length);
            first.UnlockBits(datefirst);
            return imageBuffer;
        }

        public ImageSource GetImageSource(Image image)
        {
            var bi = new BitmapImage();

            if (image != null)
            {
                Bitmap bmp = new Bitmap(image);
                using (var ms = new MemoryStream())
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ms.Position = 0;
                    bi.BeginInit();
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.StreamSource = ms;
                    bi.EndInit();
                }
            }

            return bi;
        }

        public bool Compareimages(byte[] firstBuffer, byte[] secondBuffer)
        {

            if (firstBuffer.Length != secondBuffer.Length) return false;

            int missing = 0;
            for (int i = 0; i < firstBuffer.Length; i++)
            {
                if (firstBuffer[i] != secondBuffer[i])
                {
                    missing++;
                }
            }

            return missing < 20;
        }

        public Pixel[,] GetToBuffer(Image image)
        {
            var buffer = GetBufer(image);
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

    }
}