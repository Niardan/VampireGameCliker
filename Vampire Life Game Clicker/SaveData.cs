using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Point = System.Windows.Point;

namespace Vampire_Life_Game_Clicker
{
    [Serializable]
    public class SaveData
    {
        private Dictionary<string, ColorData> _colors = new Dictionary<string, ColorData>();
        private Point _chestStartPoint;

        private Point _bloodStartPoint;

        private int _chestSizeCell = 50;
        private int _bloodSizeCell = 24;

        private int _coutCellHor = 4;
        private int _countCellVert = 15;

        private Image _leftArrowImage;
        private Image _rightArrowImage;
        private Image _upArrowImage;
        private Image _downArrowImage;

      
        public int ChestSizeCell { get => _chestSizeCell; set => _chestSizeCell = value; }
        public int CoutCellHor { get => _coutCellHor; set => _coutCellHor = value; }
        public int CountCellVert { get => 15; set => _countCellVert = value; }
        public Point ChestLeftPoint { get => _chestStartPoint; set => _chestStartPoint = value; }
        public Point BloodStartPoint { get => new Point(885,297); set => _bloodStartPoint = value; }
        public int BloodSizeCell { get => 31; set => _bloodSizeCell = value; }
        public Image LeftArrowImage { get => _leftArrowImage; set => _leftArrowImage = value; }
        public Image RightArrowImage { get => _rightArrowImage; set => _rightArrowImage = value; }
        public Image UpArrowImage { get => _upArrowImage; set => _upArrowImage = value; }
        public Image DownArrowImage { get => _downArrowImage; set => _downArrowImage = value; }

        public void SetColor(string name, Color color, bool active)
        {           
            var colorData = new ColorData();
            colorData.Pixels = new byte[] { color.B, color.G, color.R, color.A };
            colorData.Check = active;
            _colors[name] = colorData;          
        }

        public ColorData GetColor(string name)
        {
            ColorData color;
            _colors.TryGetValue(name, out color);
            return color;
        }

        public void SetBloodChecked(string name, bool active)
        {
            _colors[name].Check = active;
        }

        public void ClearArrowImage()
        {
            _upArrowImage = null;
            _rightArrowImage = null;
            _leftArrowImage = null;
            _downArrowImage = null;
        }

        public static void Save(SaveData data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream("save.dat", FileMode.Create);
            try
            {
                formatter.Serialize(fs, data);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
            }
            finally
            {
                fs.Close();
            }
        }

        public static bool Load(out SaveData saveData)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            SaveData data;
            FileStream fs = new FileStream("save.dat", FileMode.OpenOrCreate);
            bool success;
            try
            {
                saveData = (SaveData) formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                saveData = new SaveData();
                return false;
            }
            finally
            {
                fs.Close();
            }
            return true;
        }
    }
}
