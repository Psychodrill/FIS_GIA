using System;
using System.Xml.Serialization;
using FogSoft.Helpers;

namespace GVUZ.Helper.ExternalValidation
{
    [XmlRoot("query", Namespace = "")]
    public class EgeQuery : ICloneable
    {
        private static readonly XmlSerializer Serializer;

        static EgeQuery()
        {
            Serializer = new XmlSerializer(typeof (EgeQuery));
        }

        public EgeQuery()
        {
            TypographicNumber = "";
            PatronymicName = "";
        }

        /// <summary>
        ///     Создание запроса для проверки подлинности данных о свидетельствах ЕГЭ по ФИО, регистрационному номеру св-ва и паспорту.
        /// </summary>
        public EgeQuery(string lastName, string firstName, string patronymicName, string certificateNumber,
                        string passportSeries, string passportNumber) : this()
        {
            FirstName = firstName;
            LastName = lastName;
            PatronymicName = patronymicName ?? "";

            PassportSeries = passportSeries ?? "";
            PassportNumber = passportNumber;
            CertificateNumber = certificateNumber;
        }

        [XmlElement("firstName", IsNullable = false)]
        public string FirstName { get; set; }

        [XmlElement("lastName", IsNullable = false)]
        public string LastName { get; set; }

        [XmlElement("patronymicName", IsNullable = false)]
        public string PatronymicName { get; set; }

        [XmlElement("passportSeria", IsNullable = false)]
        public string PassportSeries { get; set; }

        [XmlElement("passportNumber", IsNullable = false)]
        public string PassportNumber { get; set; }

        [XmlElement("certificateNumber", IsNullable = false)]
        public string CertificateNumber { get; set; }

        [XmlElement("typographicNumber", IsNullable = false)]
        public string TypographicNumber { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override string ToString()
        {
            //ФБС так хочет пустую серию
            return XmlSerializerHelper.SerializeToString(this, Serializer)
                                      .Replace("<passportSeria />", "<passportSeria></passportSeria>");
        }
    }
}