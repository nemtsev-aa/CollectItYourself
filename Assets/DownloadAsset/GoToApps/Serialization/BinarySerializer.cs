using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GoToApps.Serialization
{
    /// <summary>
    /// Binary serializer 
    /// </summary>
    public static class BinarySerializer 
    {
        /// <summary>
        /// Serialize data to binary file.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <param name="data">Data to serialize.</param>
        public static void Serialize(string path, object data)
        {
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, data);
            }
        }
        
        /// <summary>
        /// Deserialize data from binary file.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <typeparam name="T">Data type.</typeparam>
        /// <returns>Deserialized data. (T)</returns>
        public static T Deserialize<T>(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                T data = (T) formatter.Deserialize(stream);
                return data;
            }
        }
    }
}
