using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Fbs.Web
{
    /// <summary>
    /// Данный веб-сервис сделан как заглушка для демонстрации портала госуслуг
    /// </summary>
    [WebService(Namespace = "urn:fbd:v1")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class informationservice : System.Web.Services.WebService
    {
        [WebMethod]
        public string GetEgeInformation(string AppId, string AppXml)
        {
            return
                "<StatusCode>003</StatusCode>" +
                "<ResultXml>" +
                "<certificate>" +
                    "<lastName>Иванов</lastName>" +
                    "<firstName>Иван</firstName>" +
                    "<patronymicName>Иванович</patronymicName>" +
                    "<passportSeria>1111</passportSeria>" +
                    "<passportNumber>123123</passportNumber>" +
                    "<certificateNumber>01-000043475-10</certificateNumber>" +
                    "<typographicNumber/>" +
                    "<year>2010</year>" +
                    "<status>Действительно</status>" +
                    "<uniqueIHEaFCheck>0</uniqueIHEaFCheck>" +
                    "<marks>" +
                        "<mark>" +
                            "<subjectName>Русский язык</subjectName>" +
                            "<subjectMark>69,0</subjectMark>" +
                        "</mark>" +
                        "<mark>" +
                            "<subjectName>МАТЕМАТИКА</subjectName>" +
                            "<subjectMark>88,0</subjectMark>" +
                        "</mark>" +
                    "</marks>" +
                "</certificate>" +
                "<errors/>" +
                "</ResultXml>";
        }
    }
}
