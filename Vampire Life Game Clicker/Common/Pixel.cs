namespace Vampire_Life_Game_Clicker.Common
{
    public class Pixel
    {
        private byte[] pixel = new byte [4];

        public Pixel(byte a, byte r, byte g, byte b)
        {
            pixel[0] = a;
            pixel[1] = r;
            pixel[2] = g;
            pixel[3] = b;
        }
        public byte this[int index]
        {
            get { return pixel[index];}
            set { pixel[index] = value; }
        }
    }
}