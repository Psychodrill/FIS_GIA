using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using FogSoft.Helpers;

namespace GVUZ.Helper.ExternalValidation
{
    public class EgeCertificate
    {
        public EgeCertificate()
        {
            Marks = new List<Mark>();
        }

        [XmlElement("lastName", IsNullable = true)]
        public string LastName { get; set; }

        [XmlElement("firstName", IsNullable = true)]
        public string FirstName { get; set; }

        [XmlElement("patronymicName", IsNullable = true)]
        public string PatronymicName { get; set; }

        [XmlElement("passportSeria", IsNullable = true)]
        public string PassportSeries { get; set; }

        [XmlElement("passportNumber", IsNullable = true)]
        public string PassportNumber { get; set; }

        [XmlElement("certificateNumber", IsNullable = true)]
        public string CertificateNumber { get; set; }

        [XmlElement("typographicNumber", IsNullable = true)]
        public string TypographicNumber { get; set; }

        [XmlElement("year", IsNullable = true)]
        public string Year { get; set; }

        [XmlElement("status", IsNullable = false)]
        public string Status { get; set; }

        [XmlElement("uniqueIHEaFCheck", IsNullable = false)]
        public string UniqueCheckReserved { get; set; }

        [XmlElement("certificateDeny", IsNullable = false)]
        public string СertificateDeny { get; set; }

        [XmlElement("certificateNewNumber", IsNullable = false)]
        public string CertificateNewNumber { get; set; }

        [XmlElement("certificateDenyComment", IsNullable = false)]
        public string CertificateDenyComment { get; set; }

        [XmlArray("marks"), XmlArrayItem("mark", typeof (Mark))]
        public List<Mark> Marks { get; set; }

        /// <summary>
        ///     Возвращает пустой список, если переданный (актулаьный) сертификат соответствует
        ///     текущему (созданному для валидации) или строки с ошибками.
        /// </summary>
        public List<EgeSubjectValidateError> Validate(EgeCertificate actual, out bool isStatusError)
        {
            isStatusError = false;
            if (actual == null) throw new ArgumentNullException("actual");
            var errors = new List<EgeSubjectValidateError>();

            if (ReferenceEquals(this, actual)) return errors;

            if (actual.Year != Year)
                errors.Add(new EgeSubjectValidateError
                    {
                        Error = Messages.EgeValidator_InvalidYear.FormatWith(actual.Year, Year)
                    });
            if (!actual.Status.Equals(Status, StringComparison.CurrentCultureIgnoreCase))
            {
                errors.Add(new EgeSubjectValidateError
                    {
                        Error = Messages.EgeValidator_InvalidStatus.FormatWith(actual.Status, Status)
                    });
                isStatusError = true;
            }

            if (!Marks.IsNullOrEmpty())
            {
                foreach (Mark item in Marks)
                {
                    Mark mark = item;
                    Mark markBySubject;
                    if (mark.SubjectName.StartsWith(LanguageSubjects.ForeignLanguage))
                    {
                        markBySubject =
                            actual.Marks.FirstOrDefault(
                                x => LanguageSubjects.EntranceTestBySubject.ContainsKey(x.SubjectName)
                                     && x.Value == mark.Value);
                        if (markBySubject == null)
                        {
                            markBySubject =
                                actual.Marks.Where(
                                    x => LanguageSubjects.EntranceTestBySubject.ContainsKey(x.SubjectName))
                                      .OrderByDescending(x => x.Value)
                                      .Take(1)
                                      .FirstOrDefault();
                        }
                    }
                    else
                    {
                        string subjectName;
                        if (!LanguageSubjects.SubjectByEntranceTest.TryGetValue(mark.SubjectName, out subjectName))
                            subjectName = mark.SubjectName;

                        subjectName = subjectName.ToLower();
                        markBySubject = actual.Marks.FirstOrDefault(x => x.SubjectName.ToLower() == subjectName);
                    }

                    if (markBySubject == null)
                    {
                        errors.Add(new EgeSubjectValidateError
                            {
                                Error = Messages.EgeValidator_SubjectMarkNotFound.FormatWith(mark.SubjectName),
                                SubjectName = mark.SubjectName
                            });
                    }
                    else if (markBySubject.Value != mark.Value)
                    {
                        errors.Add(new EgeSubjectValidateError
                            {
                                Error = Messages.EgeValidator_InvalidMark
                                                .FormatWith(mark.SubjectName, markBySubject.SubjectMark,
                                                            mark.SubjectMark),
                                SubjectName = mark.SubjectName,
                                ResultValue = markBySubject.Value
                            });
                    }
                }
            }
            return errors;
        }
    }
}