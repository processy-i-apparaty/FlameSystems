using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FlameBase.Models
{
    public static class BinaryFlamesModel
    {
        public static void SaveObject(object obj, string path)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                formatter.Serialize(stream, obj);
            }
        }

        public static object LoadObject(string path)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return formatter.Deserialize(stream);
            }
        }
    }
}