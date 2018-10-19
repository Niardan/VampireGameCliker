using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Vampire_Life_Game_Clicker
{
    /// <summary>
    /// Логика взаимодействия для FrameForm.xaml
    /// </summary>
    public partial class FrameForm : Window
    {
        private Line _line1 = new Line();
        private Line _line2 = new Line();
        private Line _line3 = new Line();
        private Line _line4 = new Line();

        private List<Line> _linesGrid = new List<Line>();

        private double _x;
        private double _y;

        public FrameForm()
        {
            InitializeComponent();
            ShowInTaskbar = false;
            Topmost = true;
            var colorBruses = Brushes.GreenYellow;
            this.Background = new SolidColorBrush(Color.FromArgb(0, 34, 34, 34));
            _line1.Stroke = colorBruses;
            myCanvas.Children.Add(_line1);
            _line2.Stroke = colorBruses;
            myCanvas.Children.Add(_line2);
            _line3.Stroke = colorBruses;
            myCanvas.Children.Add(_line3);
            _line4.Stroke = colorBruses;
            myCanvas.Children.Add(_line4);
            BloodGame.Content = "Blood Game: Disable";
            BloodGame.Foreground = Brushes.OrangeRed;
            ChestGame.Content = "Chest Game: Disable";
            ChestGame.Foreground = Brushes.OrangeRed;
            ChestClicker.Content = "Chest Clicker: Disable";
            ChestClicker.Foreground = Brushes.OrangeRed;
        }

        public void SetBloodGameActive(bool active)
        {
            if (active)
            {
                BloodGame.Content = "Blood Game: Enable";
                BloodGame.Foreground = Brushes.GreenYellow;
            }
            else
            {
                BloodGame.Content = "Blood Game: Disable";
                BloodGame.Foreground = Brushes.OrangeRed;
            }
        }

        public void SetChestClickerActive(bool active)
        {
            if (active)
            {
                ChestClicker.Content = "Chest Clicker: Enable";
                ChestClicker.Foreground = Brushes.GreenYellow;
            }
            else
            {
                ChestClicker.Content = "Chest Clicker: Disable";
                ChestClicker.Foreground = Brushes.OrangeRed;
            }
        }

        public void SetChestGameActive(bool active)
        {
            if (active)
            {
                ChestGame.Content = "Chest Game: Enable";
                ChestGame.Foreground = Brushes.GreenYellow;
            }
            else
            {
                ChestGame.Content = "Chest Game: Disable";
                ChestGame.Foreground = Brushes.OrangeRed;
            }
        }

        public void SetOverlayEnabled(bool enabled)
        {
            if (enabled)
            {
                ChestGame.Visibility = Visibility.Visible;
                ChestClicker.Visibility = Visibility.Visible;
                BloodGame.Visibility = Visibility.Visible;
            }
            else
            {
                ChestGame.Visibility = Visibility.Hidden;
                ChestClicker.Visibility = Visibility.Hidden;
                BloodGame.Visibility = Visibility.Hidden;
            }

        }

        public void SetPosition(Point point)
        {
            Canvas.SetLeft(PanelOverlay, point.X + 10);
            Canvas.SetTop(PanelOverlay, point.Y);
        }

        public void SetNewRectangle(double x1, double y1, double x2, double y2)
        {
            //x1 += 7;
            //y1 += 7;
            //x2 += 7;
            //y2 = (x2-x1)+y1;
            _line1.X1 = x1;
            _line1.Y1 = y1;
            _line1.X2 = x1;
            _line1.Y2 = y2;

            _line2.X1 = x1;
            _line2.Y1 = y1;
            _line2.X2 = x2;
            _line2.Y2 = y1;

            _line3.X1 = x2;
            _line3.Y1 = y1;
            _line3.X2 = x2;
            _line3.Y2 = y2;

            _line4.X1 = x1;
            _line4.Y1 = y2;
            _line4.X2 = x2;
            _line4.Y2 = y2;
        }

        public void SetNewCross(double x1, double y1)
        {
            x1 += 6;
            y1 += 6;
            _line1.X1 = x1;
            _line1.Y1 = y1 - 5;
            _line1.X2 = x1;
            _line1.Y2 = y1 + 5;

            _line2.X1 = x1;
            _line2.Y1 = y1 - 5;
            _line2.X2 = x1;
            _line2.Y2 = y1 + 5;

            _line3.X1 = x1 - 5;
            _line3.Y1 = y1;
            _line3.X2 = x1 + 5;
            _line3.Y2 = y1;

            _line4.X1 = x1 - 5;
            _line4.Y1 = y1;
            _line4.X2 = x1 + 5;
            _line4.Y2 = y1;
        }

        public void SetFormGrid(Point newPoint, int sizeCell, int countCellHor, int countCellVert)
        {
            foreach (var line in _linesGrid)
            {
                myCanvas.Children.Remove(line);
            }
            var point = new Point(newPoint.X + 7, newPoint.Y + 7);

            _linesGrid.Clear();
            for (int i = 0; i < countCellHor; i++)
            {
                Line line = new Line();
                line.X1 = point.X + sizeCell * i;
                line.Y1 = point.Y;
                line.X2 = point.X + sizeCell * i;
                line.Y2 = point.Y + sizeCell * countCellVert;
                line.Stroke = Brushes.Black;
                myCanvas.Children.Add(line);
                _linesGrid.Add(line);
            }
            for (int i = 0; i < countCellVert; i++)
            {
                Line line = new Line();
                line.X1 = point.X;
                line.Y1 = point.Y + sizeCell * i;
                line.X2 = point.X + sizeCell * countCellHor;
                line.Y2 = point.Y + sizeCell * i;
                line.Stroke = Brushes.Black;
                myCanvas.Children.Add(line);
                _linesGrid.Add(line);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        public void SetMessageLabel(string text)
        {
            Message.Content = text;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(this);
            _x = point.X;
            _y = point.Y;
        }
    }
}
