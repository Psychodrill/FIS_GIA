using System.Collections.Generic;
using System.Text;

namespace GVUZ.Helper.ExternalValidation
{
    public class EmulatorEgeInformationProvider : IEgeInformationProvider
    {
        private readonly Dictionary<string, EgeResult> _results;

        public EmulatorEgeInformationProvider()
        {
            var multiLangMarks = new StringBuilder();
            multiLangMarks
                .Append(
                    "<mark><subjectName>АНГЛИЙСКИЙ ЯЗЫК</subjectName><subjectMark>61,0</subjectMark><subjectAppeal>0</subjectAppeal></mark>")
                .Append(
                    "<mark><subjectName>НЕМЕЦКИЙ ЯЗЫК</subjectName><subjectMark>62,0</subjectMark><subjectAppeal>0</subjectAppeal></mark>")
                .Append(
                    "<mark><subjectName>ФРАНЦУЗСКИЙ ЯЗЫК</subjectName><subjectMark>64,0</subjectMark><subjectAppeal>0</subjectAppeal></mark>")
                .Append(
                    "<mark><subjectName>ИСПАНСКИЙ ЯЗЫК</subjectName><subjectMark>65,0</subjectMark><subjectAppeal>0</subjectAppeal></mark>");
            _results = new Dictionary<string, EgeResult>
                {
                    {
                        @"<items><query><firstName>Николай</firstName><lastName>Мишутин</lastName><patronymicName /><passportSeria>5408</passportSeria><passportNumber>555210</passportNumber><certificateNumber /><typographicNumber /></query></items>"
                        ,
                        EgeResult.Create(
                            @"<?xml version=""1.0"" encoding=""utf-8""?><checkResults><certificate><lastName>Мишутин</lastName><firstName>Николай</firstName><patronymicName>Алексеевич</patronymicName><passportSeria>5408</passportSeria><passportNumber>555210</passportNumber><certificateNumber>01-001001001-09</certificateNumber><typographicNumber>1221222</typographicNumber><year>2009</year><status>Истек срок</status><uniqueIHEaFCheck>0</uniqueIHEaFCheck><certificateDeny>0</certificateDeny><marks><mark><subjectName>РУССКИЙ ЯЗЫК</subjectName><subjectMark>67,0</subjectMark><subjectAppeal>0</subjectAppeal></mark><mark><subjectName>МАТЕМАТИКА</subjectName><subjectMark>55,0</subjectMark><subjectAppeal>0</subjectAppeal></mark></marks></certificate><certificate><lastName>Мишутин</lastName><firstName>Николай</firstName><patronymicName>Алексеевич</patronymicName><passportSeria>5408</passportSeria><passportNumber>555210</passportNumber><certificateNumber>01-001001002-11</certificateNumber><typographicNumber>1234567</typographicNumber><year>2011</year><status>Действительно</status><uniqueIHEaFCheck>0</uniqueIHEaFCheck><certificateDeny>0</certificateDeny><marks><mark><subjectName>РУССКИЙ ЯЗЫК</subjectName><subjectMark>66,0</subjectMark><subjectAppeal>0</subjectAppeal></mark><mark><subjectName>МАТЕМАТИКА</subjectName><subjectMark>78,0</subjectMark><subjectAppeal>0</subjectAppeal></mark><mark><subjectName>БИОЛОГИЯ</subjectName><subjectMark>80,0</subjectMark><subjectAppeal>0</subjectAppeal></mark></marks></certificate><certificate><lastName>Мишутин</lastName><firstName>Николай</firstName><patronymicName>Алексеевич</patronymicName><passportSeria>5408</passportSeria><passportNumber>555210</passportNumber><certificateNumber>01-001001003-12</certificateNumber><typographicNumber>8765432</typographicNumber><year>2012</year><status>Действительно</status><uniqueIHEaFCheck>0</uniqueIHEaFCheck><certificateDeny>0</certificateDeny><marks><mark><subjectName>РУССКИЙ ЯЗЫК</subjectName><subjectMark>90,0</subjectMark><subjectAppeal>0</subjectAppeal></mark><mark><subjectName>МАТЕМАТИКА</subjectName><subjectMark>57,0</subjectMark><subjectAppeal>0</subjectAppeal></mark><mark><subjectName>БИОЛОГИЯ</subjectName><subjectMark>90,0</subjectMark><subjectAppeal>0</subjectAppeal></mark></marks></certificate></checkResults>")
                    },
                    {
                        @"<items><query><firstName>Виктория</firstName><lastName>Будникова</lastName><patronymicName /><passportSeria>7907</passportSeria><passportNumber>509122</passportNumber><certificateNumber /><typographicNumber /></query></items>"
                        ,
                        EgeResult.Create(
                            @"<?xml version=""1.0"" encoding=""utf-8""?><checkResults><certificate><lastName>Будникова</lastName><firstName>Виктория</firstName><patronymicName>Александровна</patronymicName><passportSeria>7907</passportSeria><passportNumber>509122</passportNumber><certificateNumber>01-000043476-08</certificateNumber><typographicNumber></typographicNumber><year>2010</year><status>Истек срок</status><uniqueIHEaFCheck>2</uniqueIHEaFCheck><certificateDeny>0</certificateDeny><marks><mark><subjectName>РУССКИЙ ЯЗЫК</subjectName><subjectMark>80,0</subjectMark><subjectAppeal>0</subjectAppeal></mark><mark><subjectName>МАТЕМАТИКА</subjectName><subjectMark>65,0</subjectMark><subjectAppeal>0</subjectAppeal></mark><mark><subjectName>БИОЛОГИЯ</subjectName><subjectMark>56,0</subjectMark><subjectAppeal>0</subjectAppeal></mark></marks></certificate></checkResults>")
                    },
                    {
                        @"<items><query><firstName>Василий</firstName><lastName>Олейников</lastName><patronymicName /><passportSeria>7903</passportSeria><passportNumber>412060</passportNumber><certificateNumber /><typographicNumber /></query></items>"
                        ,
                        EgeResult.Create(
                            @"<?xml version=""1.0"" encoding=""utf-8""?><checkResults><certificate><lastName>Олейников</lastName><firstName>Василий</firstName><patronymicName>Владимирович</patronymicName><passportSeria>7903</passportSeria><passportNumber>412060</passportNumber><certificateNumber>01-000043475-08</certificateNumber><typographicNumber>2143233</typographicNumber><year>2011</year><status>Действительно</status><uniqueIHEaFCheck>3</uniqueIHEaFCheck><certificateDeny>0</certificateDeny><marks><mark><subjectName>РУССКИЙ ЯЗЫК</subjectName><subjectMark>72,0</subjectMark><subjectAppeal>0</subjectAppeal></mark><mark><subjectName>МАТЕМАТИКА</subjectName><subjectMark>60,0</subjectMark><subjectAppeal>0</subjectAppeal></mark><mark><subjectName>ХИМИЯ</subjectName><subjectMark>81,0</subjectMark><subjectAppeal>0</subjectAppeal></mark><mark><subjectName>БИОЛОГИЯ</subjectName><subjectMark>94,0</subjectMark><subjectAppeal>0</subjectAppeal></mark></marks></certificate><certificate><lastName>Олейников</lastName><firstName>Василий</firstName><patronymicName>Владимирович</patronymicName><passportSeria>7903</passportSeria><passportNumber>412060</passportNumber><certificateNumber>01-000043500-08</certificateNumber><typographicNumber>2142368</typographicNumber><year>2011</year><status>Аннулировано</status><uniqueIHEaFCheck>0</uniqueIHEaFCheck><certificateDeny>1</certificateDeny><certificateNewNumber>01-000043475-08</certificateNewNumber><certificateDenyComment>Аннулировано просто так</certificateDenyComment><marks><mark><subjectName>МАТЕМАТИКА</subjectName><subjectMark>55,0</subjectMark><subjectAppeal>0</subjectAppeal></mark></marks></certificate></checkResults>")
                    },
                    {
                        @"<items><query><firstName>Иван</firstName><lastName>Иванов</lastName><patronymicName /><passportSeria>1111</passportSeria><passportNumber>123123</passportNumber><certificateNumber /><typographicNumber /></query></items>"
                        ,
                        EgeResult.Create(
                            @"<?xml version=""1.0"" encoding=""utf-8""?><checkResults><certificate><lastName>Иванов</lastName><firstName>Иван</firstName><patronymicName>Иванович</patronymicName><passportSeria>1111</passportSeria><passportNumber>123123</passportNumber><certificateNumber>01-000043475-10</certificateNumber><typographicNumber>8049558</typographicNumber><year>2011</year><status>Аннулировано</status><uniqueIHEaFCheck>0</uniqueIHEaFCheck><certificateDeny>1</certificateDeny><certificateNewNumber></certificateNewNumber><certificateDenyComment>Аннулировано для теста</certificateDenyComment><marks><mark><subjectName>РУССКИЙ ЯЗЫК</subjectName><subjectMark>64,0</subjectMark><subjectAppeal>0</subjectAppeal></mark><mark><subjectName>МАТЕМАТИКА</subjectName><subjectMark>46,0</subjectMark><subjectAppeal>0</subjectAppeal></mark></marks></certificate></checkResults>")
                    },
                    {
                        @"<items><query><firstName>Егор</firstName><lastName>Бородин</lastName><patronymicName /><passportSeria>3076</passportSeria><passportNumber>656852</passportNumber><certificateNumber /><typographicNumber /></query></items>"
                        ,
                        EgeResult.Create(
                            @"<?xml version=""1.0"" encoding=""utf-8""?><checkResults><certificate><lastName>Бородин</lastName><firstName>Егор</firstName><patronymicName>Кириллович</patronymicName><passportSeria>3076</passportSeria><passportNumber>656852</passportNumber><certificateNumber>42-567891027-11</certificateNumber><typographicNumber>73834523</typographicNumber><year>2011</year><status>Действительно</status><uniqueIHEaFCheck>2</uniqueIHEaFCheck><certificateDeny>0</certificateDeny><marks><mark><subjectName>РУССКИЙ ЯЗЫК</subjectName><subjectMark>70,0</subjectMark><subjectAppeal>0</subjectAppeal></mark><mark><subjectName>МАТЕМАТИКА</subjectName><subjectMark>77,0</subjectMark><subjectAppeal>0</subjectAppeal></mark><mark><subjectName>ГЕОГРАФИЯ</subjectName><subjectMark>74,0</subjectMark><subjectAppeal>0</subjectAppeal></mark>" +
                            multiLangMarks +
                            "<mark><subjectName>ИНФОРМАТИКА И ИКТ</subjectName><subjectMark>97,0</subjectMark><subjectAppeal>0</subjectAppeal></mark></marks></certificate></checkResults>")
                    },

                    {
                        @"<items><query><firstName>Подачи</firstName><lastName>Проверка</lastName><patronymicName /><passportSeria /><passportNumber /><certificateNumber>33-444444442-22</certificateNumber><typographicNumber /></query></items>"
                        ,
                        EgeResult.Create(
                            @"<?xml version=""1.0"" encoding=""utf-8""?><checkResults><certificate><lastName>Проверка</lastName><firstName>Подачи</firstName><patronymicName>Кириллович</patronymicName><passportSeria>3076</passportSeria><passportNumber>656852</passportNumber><certificateNumber>33-444444442-22</certificateNumber><typographicNumber>73834523</typographicNumber><year>2011</year><status>Действительно</status><uniqueIHEaFCheck>2</uniqueIHEaFCheck><certificateDeny>0</certificateDeny><marks><mark><subjectName>РУССКИЙ ЯЗЫК</subjectName><subjectMark>70,0</subjectMark><subjectAppeal>0</subjectAppeal></mark><mark><subjectName>МАТЕМАТИКА</subjectName><subjectMark>77,0</subjectMark><subjectAppeal>0</subjectAppeal></mark><mark><subjectName>ГЕОГРАФИЯ</subjectName><subjectMark>74,0</subjectMark><subjectAppeal>0</subjectAppeal></mark>" +
                            multiLangMarks +
                            "<mark><subjectName>ИНФОРМАТИКА И ИКТ</subjectName><subjectMark>97,0</subjectMark><subjectAppeal>0</subjectAppeal></mark></marks></certificate></checkResults>")
                    },

                    {
                        @"<items><query><firstName>Подачи</firstName><lastName>Проверка</lastName><patronymicName /><passportSeria>3313</passportSeria><passportNumber>337123</passportNumber><certificateNumber /><typographicNumber /></query></items>"
                        ,
                        EgeResult.Create(
                            @"<?xml version=""1.0"" encoding=""utf-8""?><checkResults><certificate><lastName>Иванов</lastName><firstName>Иван</firstName><patronymicName>Иванович</patronymicName><passportSeria>1111</passportSeria><passportNumber>123123</passportNumber><certificateNumber>01-000043475-10</certificateNumber><typographicNumber>8049558</typographicNumber><year>2011</year><status>Действительно</status><uniqueIHEaFCheck>0</uniqueIHEaFCheck><certificateDeny>1</certificateDeny><certificateNewNumber></certificateNewNumber><certificateDenyComment>Аннулировано для теста</certificateDenyComment><marks><mark><subjectName>РУССКИЙ ЯЗЫК</subjectName><subjectMark>64,0</subjectMark><subjectAppeal>0</subjectAppeal></mark><mark><subjectName>МАТЕМАТИКА</subjectName><subjectMark>46,0</subjectMark><subjectAppeal>0</subjectAppeal></mark></marks></certificate></checkResults>")
                    },

                    {
                        @"<items><query><firstName>Петр</firstName><lastName>Пятёркин</lastName><patronymicName>Павлович</patronymicName><passportSeria>11</passportSeria><passportNumber>22</passportNumber><certificateNumber /><typographicNumber /></query></items>"
                        ,
                        EgeResult.Create(
                            @"<?xml version=""1.0"" encoding=""utf-8""?><checkResults><certificate><lastName>Пятёркин</lastName><firstName>Петр</firstName><patronymicName>Павлович</patronymicName><passportSeria>11</passportSeria><passportNumber>22</passportNumber><certificateNumber>88-888888888-81</certificateNumber><typographicNumber>8049550</typographicNumber><year>2012</year><status>Действительно</status><uniqueIHEaFCheck>0</uniqueIHEaFCheck><certificateDeny>0</certificateDeny><certificateNewNumber></certificateNewNumber><marks><mark><subjectName>РУССКИЙ ЯЗЫК</subjectName><subjectMark>56,0</subjectMark><subjectAppeal>0</subjectAppeal></mark><mark><subjectName>АНГЛИЙСКИЙ ЯЗЫК</subjectName><subjectMark>44,0</subjectMark><subjectAppeal>0</subjectAppeal></mark></marks></certificate></checkResults>")
                    },
                };
            //
        }

        public EgeResultAndStatus GetEgeInformation(EgePacket packet)
        {
            EgeResult result;
            if (_results.TryGetValue(packet.ToString(), out result))
                return new EgeResultAndStatus(result, EgeResultAndStatus.Succeded);

            return new EgeResultAndStatus(EgeResult.CreateError("Not found"), EgeResultAndStatus.TransferError);
        }
    }
}