using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Projekt
{
    public static class BinSerializerUtility
    {
        public static void BinaryFileSerialize(Object obj, string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                try
                {
                    BinaryFormatter b = new BinaryFormatter();
                    b.Serialize(fileStream, obj);
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        public static T BinaryFileDeserialize<T>(string filePath)
        {
            Object obj = null;

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                try
                {
                    BinaryFormatter b = new BinaryFormatter();
                    obj = b.Deserialize(fileStream);
                }
                catch (Exception)
                {
                    return default(T);
                }
            }
            return (T)obj;
        }
    }
}
