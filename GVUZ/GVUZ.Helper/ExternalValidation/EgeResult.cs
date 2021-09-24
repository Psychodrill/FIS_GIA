using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using FogSoft.Helpers;

namespace GVUZ.Helper.ExternalValidation
{
    [XmlRoot("checkResults")]
    public class EgeResult
    {
        public const string ActualCertificate = "Действующий";
        public const string CancelledCertificate = "Аннулировано";
        private static readonly XmlSerializer Serializer;
        private static readonly XmlReaderSettings ReaderSettings;

        static EgeResult()
        {
            Serializer = new XmlSerializer(typeof(EgeResult));
            var schemas = new XmlSchemaSet();
            schemas.Add("", XmlReader.Create(Resources.GetResourceStream<EgeResult>("EgeResult.xsd")));
            ReaderSettings = new XmlReaderSettings
                {
                    Schemas = schemas,
                    ValidationType = ValidationType.Schema
                };
        }

        // roman.n.bukin: было protected
        public  EgeResult()
        {
            Errors = new List<string>();
            Certificates = new List<EgeCertificate>();
        }

        [XmlElement("certificate")]
        public List<EgeCertificate> Certificates { get; set; }

        [XmlElement("batchId")]
        public string BatchId { get; set; }

        [XmlElement("statusCode")]
        public string StatusCode { get; set; }

        [XmlElement("statusMessage")]
        public string StatusMessage { get; set; }

        [XmlArray("errors", IsNullable = true), XmlArrayItem("error", typeof(string))]
        public List<string> Errors { get; set; }

        public static EgeResult CreateError(string error)
        {
            var result = new EgeResult();
            if (string.IsNullOrEmpty(error)) throw new ArgumentNullException("error");
            result.Errors.Add(error);
            return result;
        }

        public static EgeResult Create(EgeCertificate certificate)
        {
            if (certificate == null) throw new ArgumentNullException("certificate");
            var result = new EgeResult();
            result.Certificates.Add(certificate);
            return result;
        }

        public static EgeResult Create(string xml)
        {
            if (string.IsNullOrEmpty(xml)) throw new ArgumentNullException("xml");

            var result =
                (EgeResult)Serializer.Deserialize(XmlReader.Create(new StringReader(xml), ReaderSettings));
            return result;
        }

        public override string ToString()
        {
            return XmlSerializerHelper.SerializeToString(this, Serializer);
        }

        /// <summary>
        ///     Returns <see cref="EgeCertificate" /> with best mark for the specified subject or null if not found.
        /// </summary>
        public EgeCertificate FindBestMark(string subject,int? year, out decimal bestMark)
        {
            if (subject == null)
                throw new ArgumentNullException("subject");

            bestMark = 0;
            EgeCertificate result = null;

            foreach (EgeCertificate certificate in Certificates
                .Where(x => ((year.HasValue)&&(x.Year == year.ToString()))||(!year.HasValue))
                .Where(x => x.Status == ActualCertificate))//39141 При проверке свидетельств ЕГЭ не проставлять баллы из просроченных свидетельств
            {
                Mark mark = certificate.Marks
                    .Where(
                    x =>
                    (subject == LanguageSubjects.ForeignLanguage &&
                     LanguageSubjects.EntranceTestBySubject.ContainsKey(x.SubjectName))
                     ||
                    (subject.StartsWith(LanguageSubjects.ForeignLanguagePrefix) &&
                     LanguageSubjects.SubjectByEntranceTest[subject].ToLower() == x.SubjectName.ToLower())
                     ||
                    (!subject.StartsWith(LanguageSubjects.ForeignLanguage) &&
                     x.SubjectName.ToLower() == subject.ToLower()))
                    .OrderByDescending(x => x.Value).FirstOrDefault();

                if (mark == null || mark.Value < bestMark)
                    continue;

                if (mark.Value == bestMark && result != null)
                {
                    //берём самое свежее свидетельство
                    if (certificate.Year.To(0) > result.Year.To(0))
                    {
                        result = certificate;
                    }
                }
                else
                {
                    bestMark = mark.Value;
                    result = certificate;
                }
            }

            return result;
        }

        /// <summary>
        ///     Returns <see cref="EgeCertificate" /> with the specified mark for the specified subject or null if not found.
        /// </summary>
        public EgeCertificate FindByMark(string subject,int? year, decimal specifiedMark)
        {
            if (subject == null)
                throw new ArgumentNullException("subject");

            IEnumerable<EgeCertificate> foundByMarks = Certificates
                .Where(x => ((year.HasValue) && (x.Year == year.ToString())) || (!year.HasValue))
                .Where(c => c.Marks
                     .Where(
                     x =>
                     (subject == LanguageSubjects.ForeignLanguage &&
                      LanguageSubjects.EntranceTestBySubject.ContainsKey(x.SubjectName))
                     ||
                     (subject.StartsWith(LanguageSubjects.ForeignLanguagePrefix) &&
                      LanguageSubjects.SubjectByEntranceTest[subject].ToLower() == x.SubjectName.ToLower())
                     ||
                     (!subject.StartsWith(LanguageSubjects.ForeignLanguage) &&
                      x.SubjectName.ToLower() == subject.ToLower()))
                     .Where(x => x.Value == specifiedMark)
                     .Any());

            EgeCertificate result = foundByMarks.FirstOrDefault(x => x.Status == ActualCertificate);
            if (result == null)
            {
                result = foundByMarks.FirstOrDefault();
            }

            return result;
        }
    }
}