using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Image = System.Windows.Controls.Image;

namespace Vampire_Life_Game_Clicker.NewChestGame
{
    /// <summary>
    /// Логика взаимодействия для ViewChestInit.xaml
    /// </summary>
    public partial class ViewChestInit : Window
    {
        public ViewChestInit(Bitmap image)
        {
            InitializeComponent();
           
            using (var ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;

                var bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = ms;
                bi.EndInit();
                MyImage.Source = bi;
            }
        }
     
    }
}
