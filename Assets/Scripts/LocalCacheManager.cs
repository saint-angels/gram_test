using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Tactics
{
    public class LocalCacheManager : MonoBehaviour
    {
        private const string jsonFileExtension = "json";

        // single file

        public T Load<T>()
        {
            string filePath = GetFilePath<T>(jsonFileExtension);

            if (File.Exists(filePath) == true)
            {
                try
                {
                    var jObject = JObject.Parse(File.ReadAllText(filePath));
                    T objectData = jObject.ToObject<T>();
                    return objectData;
                }
                catch
                {
                    return default;
                }
            }
            return default;
        }

        public bool Save<T>(T data, bool allowOverwrite)
        {
            string filePath = GetFilePath<T>(jsonFileExtension);
            print("saved to file path:" + filePath);

            return SaveAtPath<T>(data, filePath, allowOverwrite);
        }

        public bool FileExists<T>()
        {
            string filePath = GetFilePath<T>(jsonFileExtension);
            return File.Exists(filePath);
        }

        // multi file

        public T LoadFromTypedFolder<T>(string filename)
        {
            string filePath = GetFileInFolderPath<T>(filename, jsonFileExtension);

            if (File.Exists(filePath) == true)
            {
                try
                {
                    var jObject = JObject.Parse(File.ReadAllText(filePath));
                    T objectData = jObject.ToObject<T>();
                    return objectData;
                }
                catch
                {
                    return default;
                }
            }
            return default;
        }

        public string[] ListTypedFolder<T>()
        {
            string folderPath = GetTypedFolderPath<T>();
            string[] result;
            if (Directory.Exists(folderPath) == false)
            {
                result = new string[] { };
            }
            else
            {
                result = Directory.GetFiles(folderPath);
            }
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Path.GetFileNameWithoutExtension(result[i]);
            }
            return result;
        }

        public bool FileExistsInTypedFolder<T>(string fileName)
        {
            foreach (string fileInFolder in ListTypedFolder<T>())
            {
                if (fileInFolder == fileName)
                {
                    return true;
                }
            }
            return false;
        }

        public bool SaveInTypedFolder<T>(T data, string filename, bool allowOverwrite)
        {
            string filePath = GetFileInFolderPath<T>(filename, jsonFileExtension);

            return SaveAtPath<T>(data, filePath, allowOverwrite);
        }

        public void Clear<T>()
        {
            string filePath = GetFilePath<T>(jsonFileExtension);
            if (File.Exists(filePath) == true)
            {
                File.Delete(filePath);
            }
        }

        // private helpers

        private bool SaveAtPath<T>(T data, string filePath, bool allowOverwrite)
        {
            if (File.Exists(filePath) == true && allowOverwrite == true)
            {
                File.Delete(filePath);
            }

            if (File.Exists(filePath) == false)
            {
                string textData = JsonConvert.SerializeObject(data, Formatting.Indented,
                                    new JsonConverter[] { new Newtonsoft.Json.Converters.StringEnumConverter() });
                File.WriteAllText(filePath, textData);
            }
            else
            {
                return false;
            }

            return true;
        }

        private string GetFilePath<T>(string fileType)
        {
            return Path.Combine(Application.persistentDataPath, typeof(T).FullName + "." + fileType);
        }

        private string GetFileInFolderPath<T>(string filename, string fileType)
        {
            string folderPath = GetTypedFolderPath<T>();

            Directory.CreateDirectory(folderPath);

            return Path.Combine(folderPath, filename + "." + fileType);
        }

        private static string GetTypedFolderPath<T>()
        {
            return Path.Combine(Application.persistentDataPath, typeof(T).FullName);
        }
    }
}
