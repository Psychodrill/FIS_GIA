namespace Ege.Hsc.Logic.Servers
{
    using System.Collections.Generic;
    using System.IO;

    class ServerFileParser : IServerFileParser
    {
        public ISet<string> GetCodes(Stream file)
        {
            var result = new HashSet<string>();
            using (var reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        result.Add(line);
                    }
                }
            }
            return result;
        }
    }
}
