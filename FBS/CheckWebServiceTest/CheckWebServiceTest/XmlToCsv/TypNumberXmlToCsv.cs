using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;


namespace CheckWebService.XmlToCsv
{
    public class TypNumberXmlToCsv : XmlToCsv
    {
        protected override string HeaderPattern
        {
            get
            {
                return "Номер свидетельства%Типографский номер%Фамилия%Имя%Отчество%Серия документа%Номер документа%Регион%Год выдачи свидетельства%Статус свидетельства%Русский язык%Апелляция%Математика%Апелляция%Физика%Апелляция%Химия%Апелляция%Биология%Апелляция%История россии%Апелляция%География%Апелляция%Английский язык%Апелляция%Немецкий язык%Апелляция%Французский язык%Апелляция%Обществознание%Апелляция%Литература%Апелляция%Испанский язык%Апелляция%Информатика%Апелляция%Проверок ВУЗами и их филиалами";
            }
        }
        protected override string FooterPattern
        {
            get
            {
                return Environment.NewLine + "Комментарий: Специальным знаком «!» перед баллом выделены баллы, которые меньше минимальных значений, установленных Федеральной службой по надзору в сфере образования и науки (Рособрнадзор). С Приказами Рособрнадзора о применении шкалы минимальных баллов за соответствующие годы можно ознакомиться в разделе «Документы» Подсистемы ФИС Результаты ЕГЭ. Срок действия свидетельств ЕГЭ определяется в соответствии с действующими нормативными документами. С нормативными документами можно ознакомиться в разделе «Документы» Подсистемы ФИС Результаты ЕГЭ. В данном файле приведены сведения о свидетельствах за все год(ы). " +
                    Environment.NewLine + Environment.NewLine;
            }
        }

        protected override void AppendCertificateRow(XmlNode certificateNode)
        {
            string status = certificateNode.SelectSingleNode("status").InnerText;
            string lastName = certificateNode.SelectSingleNode("lastName").InnerText;
            string firstName = certificateNode.SelectSingleNode("firstName").InnerText;
            string patronymicName = certificateNode.SelectSingleNode("patronymicName").InnerText;
            string passportSeria = certificateNode.SelectSingleNode("passportSeria").InnerText;
            string passportNumber = certificateNode.SelectSingleNode("passportNumber").InnerText;
            string certificateNumber = certificateNode.SelectSingleNode("certificateNumber").InnerText;
            string typographicNumber = certificateNode.SelectSingleNode("typographicNumber").InnerText;
            string year = certificateNode.SelectSingleNode("year").InnerText;
            string region = String.IsNullOrEmpty(certificateNumber) ? "" : certificateNumber.Substring(0, 2);

            string isDeny = certificateNode.SelectSingleNode("certificateDeny").InnerText;
            string certificateNewNumber = isDeny != "0"
                ? certificateNode.SelectSingleNode("certificateNewNumber").InnerText
                : string.Empty;
            string certificateDenyComment = isDeny != "0"
                ? certificateNode.SelectSingleNode("certificateDenyComment").InnerText
                : string.Empty;

            string rusLang = ExtractSubjectMark(certificateNode, "Русский язык");
            string rusLangApp = ExtractSubjectAppeal(certificateNode, "Русский язык");

            string math = ExtractSubjectMark(certificateNode, "Математика");
            string mathApp = ExtractSubjectAppeal(certificateNode, "Математика");

            string phis = ExtractSubjectMark(certificateNode, "Физика");
            string phisApp = ExtractSubjectAppeal(certificateNode, "Физика");

            string chem = ExtractSubjectMark(certificateNode, "Химия");
            string chemApp = ExtractSubjectAppeal(certificateNode, "Химия");

            string biology = ExtractSubjectMark(certificateNode, "Биология");
            string biologyApp = ExtractSubjectAppeal(certificateNode, "Биология");

            string rusHist = ExtractSubjectMark(certificateNode, "История России");
            string rusHistApp = ExtractSubjectAppeal(certificateNode, "История России");

            string geo = ExtractSubjectMark(certificateNode, "География");
            string geoApp = ExtractSubjectAppeal(certificateNode, "География");

            string engLang = ExtractSubjectMark(certificateNode, "Английский язык");
            string engLangApp = ExtractSubjectAppeal(certificateNode, "Английский язык");

            string gerLang = ExtractSubjectMark(certificateNode, "Немецкий язык");
            string gerLangApp = ExtractSubjectAppeal(certificateNode, "Немецкий язык");

            string frLang = ExtractSubjectMark(certificateNode, "Французский язык");
            string frLangApp = ExtractSubjectAppeal(certificateNode, "Французский язык");

            string social = ExtractSubjectMark(certificateNode, "Обществознание");
            string socialApp = ExtractSubjectAppeal(certificateNode, "Обществознание");

            string lit = ExtractSubjectMark(certificateNode, "Литература");
            string litApp = ExtractSubjectAppeal(certificateNode, "Литература");

            string esLang = ExtractSubjectMark(certificateNode, "Испанский язык");
            string esLangApp = ExtractSubjectAppeal(certificateNode, "Испанский язык");

            string inf = ExtractSubjectMark(certificateNode, "Информатика");
            string infApp = ExtractSubjectAppeal(certificateNode, "Информатика");

            string uniqueIHEaFCheck = certificateNode.SelectSingleNode("uniqueIHEaFCheck").InnerText;

            //НЕ НАЙДЕНО 01-987654321-09%%Петров%Петр%Петрович%%%%%Не найдено%Ошибка 35,0 ()%0%Ошибка 58,0 ()%0%%0%%0%%0%Ошибка 38,0 ()%0%%0%%0%%0%%0%%0%%0%%0%%0
            string lineFormat;
            if (status.ToUpper() == "НЕ НАЙДЕНО")
            {
                lineFormat = "{0}{1}%{2}%{3}%{4}%{5}%{6}%{7}%{8}%{9}%{10}";
                csv.AppendLine(String.Format(lineFormat,
           (status.ToUpper() == "НЕ НАЙДЕНО") ? status.ToUpper() + " " : "",
           certificateNumber,
           typographicNumber,
           lastName,
           firstName,
           patronymicName,
           passportSeria,
           passportNumber,
           region,
           year,
           status));
            }
            else
            {
                lineFormat = "{0}{1}%{2}%{3}%{4}%{5}%{6}%{7}%{8}%{9}%{10}%{11}%{12}%{13}%{14}%{15}%{16}%{17}%{18}%{19}%{20}%{21}%{22}%{23}%{24}%{25}%{26}%{27}%{28}%{29}%{30}%{31}%{32}%{33}%{34}%{35}%{36}%{37}%{38}%{39}";
                csv.AppendLine(String.Format(lineFormat,
             (status.ToUpper() == "НЕ НАЙДЕНО") 
                ? status.ToUpper() + " "
                : isDeny != "0"
                    ? string.Format("АННУЛИРОВАНО {0}: {1}. Актуальное свидетельство: ", certificateNumber, certificateDenyComment)
                    : "",
             isDeny != "0" ? certificateNewNumber : certificateNumber,
             typographicNumber,
             lastName,
             firstName,
             patronymicName,
             passportSeria,
             passportNumber,
             region,
             year,
             status,
             rusLang, rusLangApp,
             math, mathApp,
             phis, phisApp,
             chem, chemApp,
             biology, biologyApp,
             rusHist, rusHistApp,
             geo, geoApp,
             engLang, engLangApp,
             gerLang, gerLangApp,
             frLang, frLangApp,
             social, socialApp,
             lit, litApp,
             esLang, esLangApp,
             inf, infApp,
             uniqueIHEaFCheck));
            }


        }
    }
}
