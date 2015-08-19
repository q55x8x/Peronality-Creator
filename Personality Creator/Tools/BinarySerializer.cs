using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Personality_Creator.Tools
{
    public static class BinarySerializer
    {
        public static void Serialize<T>(T obj, string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
           
            bf.Serialize(fs,obj);

            fs.Close();
        }

        public static T Deserialize<T>(string fileName)
        {
            T obj;

            FileStream fs = new FileStream(fileName, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
           
            obj = (T)bf.Deserialize(fs);

            fs.Close();

            return obj;
        }
    }
}
