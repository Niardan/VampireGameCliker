using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vampire_Life_Game_Clicker.Common;

namespace Vampire_Life_Game_Clicker
{
    [Serializable]
    public class ColorData
    {
        private Pixel _pixels;
        private bool _check;

        public Pixel Pixels { get => _pixels; set => _pixels = value; }
        public bool Check { get => _check; set => _check = value; }
    }
}
