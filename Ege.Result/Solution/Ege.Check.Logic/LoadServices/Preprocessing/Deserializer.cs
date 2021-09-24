namespace Ege.Check.Logic.LoadServices.Preprocessing
{
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class Deserializer : IDeserializer, ISerializer
    {
        public TDto[] Deserialize<TDto>(Stream stream)
        {
            // Из бд приходит кодировка 1251
            var encoding = Encoding.GetEncoding(1251);
            using (var sw = new StreamReader(stream, encoding))
            using (var xr = XmlReader.Create(sw, new XmlReaderSettings{CheckCharacters = false}))
            {
                var xml = new XmlSerializer(typeof(TDto[]));
                var data = xml.Deserialize(xr) as TDto[];
                return data;
            }
        }

        public Stream Serialize<TDto>(TDto[] dtos)
        {
            var ms = new MemoryStream();
            var encoding = Encoding.GetEncoding(1251);
            using (var sw = new StreamWriter(ms, encoding, 1024, true))
            using (var xw = XmlWriter.Create(sw, new XmlWriterSettings {CheckCharacters = false}))
            {
                new XmlSerializer(typeof(TDto[])).Serialize(xw, dtos);
                ms.Position = 0;
                return ms;
            }
        }
    }
}