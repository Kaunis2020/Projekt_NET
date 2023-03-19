using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Projekt.Models
{
    public class JSONSerializer
    {
        public static bool JSONFileSerialize(Object obj, string filePath)
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    JsonWriter writer = new JsonTextWriter(sw)
                    {
                        Formatting = Formatting.Indented
                    };
                    serializer.Serialize(writer, obj);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
