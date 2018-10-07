using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

        public Image GetImage(string name, double x, double y, double xRight, double yDown)
        {
            Size ScreenSize = Screen.PrimaryScreen.Bounds.Size;
            Bitmap image = new Bitmap(ScreenSize.Width, ScreenSize.Height);
            Image newImage;
            using (Graphics g = Graphics.FromImage(image))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, ScreenSize);

                newImage = ResizeImage(image,(int) x,(int) y,(int) xRight,(int) yDown);
            }
            return newImage;
        }

        public Color GetPixelColor(Point point)
        {
            Image image = GetImage("pixel", point.X, point.Y, point.X + 20, point.Y + 20);
            byte[] buffer;
            GetBufer(image, out buffer);
            return Color.FromArgb(buffer[3], buffer[2], buffer[1], buffer[0]);
        }

        public void GetBufer(Image image, out byte[] _imageBuffer)
        {
            Bitmap first = image as Bitmap;
            BitmapData datefirst = first.LockBits(new Rectangle(0, 0, first.Width, first.Height), ImageLockMode.ReadOnly, first.PixelFormat);
            int size1 = datefirst.Stride * datefirst.Height;
            _imageBuffer = new byte[size1];
            Marshal.Copy(datefirst.Scan0, _imageBuffer, 0, _imageBuffer.Length);
            first.UnlockBits(datefirst);
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

    }
}