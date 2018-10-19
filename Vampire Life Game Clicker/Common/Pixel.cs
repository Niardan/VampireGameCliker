using System;

namespace Vampire_Life_Game_Clicker.Common
{
    [Serializable]
    public class Pixel
    {
        private byte[] pixel = new byte[4];

        public Pixel(byte a, byte r, byte g, byte b)
        {
            pixel[0] = a;
            pixel[1] = r;
            pixel[2] = g;
            pixel[3] = b;
        }
        public byte this[int index]
        {
            get { return pixel[index]; }
            set { pixel[index] = value; }
        }

        public bool CheckPixel(Pixel testPixel, int offset)
        {
            if (Math.Abs(pixel[0] - testPixel[0]) > offset) return false;
            if (Math.Abs(pixel[1] - testPixel[1]) > offset) return false;
            if (Math.Abs(pixel[2] - testPixel[2]) > offset) return false;
            return true;
        }
    }
}