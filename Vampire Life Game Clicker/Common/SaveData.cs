using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Point = System.Windows.Point;

namespace Vampire_Life_Game_Clicker.Common
{
    [Serializable]
    public class SaveData
    {
        private Dictionary<string, ColorData> _colors = new Dictionary<string, ColorData>();
        private string _gamePath;

        public string GamePath
        {
            get { return _gamePath; }
            set { _gamePath = value; }
        }

        public void SetColor(string name, Color color, bool active)
        {           
            var colorData = new ColorData();
            colorData.Pixels = new Pixel(color.B, color.G, color.R, color.A );
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
