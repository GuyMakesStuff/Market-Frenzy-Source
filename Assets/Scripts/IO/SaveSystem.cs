using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MarketFrenzy.IO
{
    public static class SaveSystem
    {
        static string Format = "MSS";

        public static bool SaveObject(object Data, string FileName, string _Path)
        {
            string path = _Path + "/" + FileName + "." + Format;
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                FileStream stream = new FileStream(path, FileMode.Create);
                formatter.Serialize(stream, Data);
                stream.Close();
            }
            catch (DirectoryNotFoundException)
            {
                return false;
                throw;
            }

            return true;
        }

        public static object LoadObject(string FileName, string _Path)
        {
            string path = _Path + "/" + FileName + "." + Format;
            if (!File.Exists(path))
            {
                return null;
            }

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            object LoadedData = formatter.Deserialize(stream);
            stream.Close();
            return LoadedData;
        }

        public static void DeleteObject(string FileName, string Path)
        {
            string FullPath = Path + "/" + FileName + "." + Format;
            if(!File.Exists(FullPath))
            {
                return;
            }

            File.Delete(FullPath);
        }
    }
}