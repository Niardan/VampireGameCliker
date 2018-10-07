using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vampire_Life_Game_Clicker
{
    [Serializable]
    public class ColorData
    {
        private byte[] _pixels;
        private bool _check;

        public byte[] Pixels { get => _pixels; set => _pixels = value; }
        public bool Check { get => _check; set => _check = value; }
    }
}
