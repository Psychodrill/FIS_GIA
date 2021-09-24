namespace Ege.Hsc.Logic.Csv
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Common.Logging;
    using CsvHelper;
    using CsvHelper.Configuration;
    using Ege.Check.Logic.Models.Requests;
    using JetBrains.Annotations;

    interface IRequestCsvParser
    {
        [NotNull]
        IEnumerable<ParticipantBlankRequest> Parse([NotNull]Stream csv);
    }

    class RequestCsvParser : IRequestCsvParser
    {
        [ThreadStatic] private static CsvFactory _csvFactory;

        [ThreadStatic] private static CsvConfiguration _csvConfiguration;

        [NotNull]
        private readonly ILog _logger = LogManager.GetLogger<RequestCsvParser>();

        [NotNull]
        private CsvConfiguration CreateConfiguration()
        {
            var config = new CsvConfiguration
            {
                Delimiter = "%",
                HasHeaderRecord = true,
                ReadingExceptionCallback = ErrorCallback,
            };
            config.RegisterClassMap<ParticipantRequestMap>();
            return config;
        }

        public IEnumerable<ParticipantBlankRequest> Parse(Stream csv)
        {
            _csvFactory = _csvFactory ?? new CsvFactory();
            _csvConfiguration = _csvConfiguration ?? CreateConfiguration();
            var encoding = Encoding.UTF8;
            using (var reader = new StreamReader(csv, encoding))
            using (var csvReader = _csvFactory.CreateReader(reader, _csvConfiguration))
            {
                if (csvReader == null)
                {
                    throw new InvalidOperationException("CsvFactory::CreateReader returned null");
                }
                return csvReader.GetRecords<ParticipantBlankRequest>().ToList();
            }
        }

        private void ErrorCallback(Exception ex, ICsvReader row)
        {
            if (row == null)
            {
                _logger.Error("Null row");
            }
            else if (ex == null)
            {
                _logger.ErrorFormat("Error in row #{0} ({1})", row.Row);
            }
            else
            {
                _logger.ErrorFormat("Error in row #{0} ({1}): {2}", row.Row, row.ToString(), ex);
            }
        }
    }

    public sealed class ParticipantRequestMap : CsvClassMap<ParticipantBlankRequest>
    {
        public ParticipantRequestMap()
        {
            Map(w => w.Surname).Index(0);
            Map(w => w.FirstName).Index(1);
            Map(w => w.Patronymic).Index(2);
            Map(w => w.Document).Index(3).ConvertUsing(MapDocument);
        }

        private string MapDocument(ICsvReaderRow row)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }
            return row.GetField(3).AddLeadingZeroes();
        }
    }

    public static class DocumentNumberExtensions
    {
        public static string AddLeadingZeroes(this string documentNumber)
        {
            return documentNumber != null ? documentNumber.PadLeft(10, '0') : new string('0', 10);
        }
    }
}
