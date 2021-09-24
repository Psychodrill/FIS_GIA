using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GVUZ.Helper.Import
{
    public class Serializer
    {
        /// <summary>
        ///     Сериализация с шифрованием MD5
        /// </summary>
        /// <typeparam name="T">Тип сериализуемого объекта (можно не указывать)</typeparam>
        /// <param name="filePath">Путь к результирующему файлу</param>
        /// <param name="data">Данные для сериализации</param>
        public void Serialize<T>(string filePath, T data) where T : class
        {
            Serialize(filePath, data, false);
        }

        public void Serialize<T>(string filePath, T data, bool isEncryptionUsed) where T : class
        {
            if (data == null)
                return;

            var serializer = new XmlSerializer(typeof (T));
            using (var file = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(file, data);
            }

            /*
             * Шифруем и сохраняем
             */
            byte[] buffer = File.ReadAllBytes(filePath);

            if (isEncryptionUsed)
                buffer = buffer.Encrypt();

            File.WriteAllBytes(filePath, buffer);
        }

        public byte[] Serialize<T>(T data) where T : class
        {
            if (data == null)
                return null;

            var serializer = new XmlSerializer(typeof (T));
            using (var ms = new MemoryStream())
            {
                serializer.Serialize(ms, data);
                return ms.GetBuffer().Encrypt();
            }
        }

        public string Serialize<T>(T[] dtos) where T : class
        {
            using (var ms = new MemoryStream())
            {
                var xmlSerializer = new XmlSerializer(typeof (T[]));
                xmlSerializer.Serialize(ms, dtos);
                byte[] buffer = ms.GetBuffer();
                string stringResult = Encoding.UTF8.GetString(buffer);
                var doc = new XmlDocument();
                doc.LoadXml(stringResult);
                return doc.InnerXml;
            }
        }

        public string SerializeToXml<T>(T dto) where T : class
        {
            using (var ms = new MemoryStream())
            {
                var xmlSerializer = new XmlSerializer(typeof (T));
                xmlSerializer.Serialize(ms, dto);
                byte[] buffer = ms.GetBuffer();
                string stringResult = Encoding.UTF8.GetString(buffer);
                var doc = new XmlDocument();
                doc.LoadXml(stringResult);
                return doc.InnerXml;
            }
        }

        public XElement SerializeToXElement<T>(T dto) where T : class
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof (T));
                    xmlSerializer.Serialize(streamWriter, dto);
                    return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
        }

        public T DeserializeFromXElement<T>(XElement xElement)
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xElement.ToString())))
            {
                var xmlSerializer = new XmlSerializer(typeof (T));
                return (T) xmlSerializer.Deserialize(memoryStream);
            }
        }

        /// <summary>
        ///     Десериализация
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public T DeserializeFromFile<T>(string filePath) where T : class
        {
            return DeserializeFromFile<T>(filePath, false);
        }

        public T DeserializeFromFile<T>(string filePath, bool isEncryptionUsed) where T : class
        {
            if (!File.Exists(filePath))
            {
                string msg = "Десереализация невозможна! Файл '{0}' не найден!";
                //Logger.GetLogger().WarnFormat(msg, filePath);
                throw new FileNotFoundException(string.Format(msg, filePath));
            }

            byte[] bytes = File.ReadAllBytes(filePath);
            if (bytes.Length == 0)
            {
                //Logger.GetLogger().WarnFormat("Десириализация пустого потока в типе {0}", typeof (T).FullName);
                return null;
            }

            /*
             * Прочитали и расшифровали
             */
            if (isEncryptionUsed)
                bytes = bytes.Decrypt();

            using (var ms = new MemoryStream(bytes))
            {
                var serializer = new XmlSerializer(typeof (T));
                return serializer.Deserialize(ms) as T;
            }
        }

        /// <summary>
        ///     Десериализация из Base64 строки
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <returns></returns>
        public T Deserialize<T>(string text) where T : class
        {
            using (var ms = new StringReader(text))
            {
                var serializer = new XmlSerializer(typeof (T));
                return serializer.Deserialize(ms) as T;
            }
        }

        public T Deserialize<T>(byte[] bytes) where T : class
        {
            using (var ms = new MemoryStream(bytes.Decrypt()))
            {
                var serializer = new XmlSerializer(typeof (T));
                return serializer.Deserialize(ms) as T;
            }
        }

        public T Deserialize<T>(XElement xml) where T : class
        {
            foreach (XElement node in xml.Descendants())
                if (node.NodeType == XmlNodeType.Element && !node.HasElements) node.Value = node.Value.Trim();

            using (XmlReader reader = xml.CreateReader())
            {
                var xmlSerializer = new XmlSerializer(typeof (T));
                return xmlSerializer.Deserialize(reader) as T;
            }
        }
    }
}