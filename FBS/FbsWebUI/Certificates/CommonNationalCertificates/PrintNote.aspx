<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintNote.aspx.cs" Inherits="Fbs.Web.Certificates.CommonNationalCertificates.PrintNote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">


    <!-- Форма справки -->
    
    <asp:PlaceHolder ID="phNote" runat="server">
    
        <table cellpadding="5" cellspacing="0" border="0" style="width: 590px; font-family: Arial; font-size: .9em; text-align: center;">
        
        <tr>
            <td style="font-size: 1.2em; font-weight: bold;">
                СПРАВКА
            </td>
        </tr>

        <tr>
            <td style="font-weight: bold;">
                о результатах единого государственного экзамена
            </td>
        </tr>

        <tr>
            <td style="padding-top: 20px;">
                В соответствии со свидетельством
            </td>
        </tr>

        <tr>
            <td style="">
                № <%= Cert.CertificateNumber %> статус: <%= Cert.Status %>
            </td>
        </tr>
        <%if (!this.EnableOpenedFbs)
        { %>
            <tr>
                <td style="">
                    <%= 
                        (string.IsNullOrEmpty(Cert.LastName) ? string.Empty : Cert.LastName + "&nbsp;&nbsp;&nbsp;") +
                        (string.IsNullOrEmpty(Cert.FirstName) ? string.Empty : Cert.FirstName + "&nbsp;&nbsp;&nbsp;") +
                        (string.IsNullOrEmpty(Cert.PatronymicName) ? string.Empty : Cert.PatronymicName)
                    %>
                </td>
            </tr>
        <% } 
           else
          { %>
             <tr>
                <td style="padding-top: 35px; text-align: left;">
                    <table cellpadding="0" cellspacing="0" border="0" style="text-align: center;">
                        <tr>
                            <td style="padding-right: 10px;">____________________________________________________________________________</td>                           
                        </tr>
                        <tr>
                            <td style="padding-right: 10px;"><sup>(Фамилия Имя Отчество)</sup></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="">
                    Паспорт серия <%=(string.IsNullOrEmpty(Cert.PassportSeria) ? string.Empty : Cert.PassportSeria)%> номер <%=(string.IsNullOrEmpty(Cert.PassportNumber) ? string.Empty : Cert.PassportNumber)%>
                </td>
            </tr>
          <% } %>      

        <tr>
            <td style="">
                по результатам сдачи единого государственного экзамена в <%= Cert.Year %> году обнаружил(а) следующие знания по общеобразовательным предметам
            </td>
        </tr>
        
        <tr>
            <td style="padding-top: 20px;">
            
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; border: solid 1px #777">
                    <tr>
                        <td style="width: 50%; padding-top: 10px; padding-bottom: 10px; border-right: solid 1px #777;">Наименование<br />общеобразовательных предметов</td>
                        <td style="width: 50%; padding-top: 10px; padding-bottom: 10px;">Баллы</td>
                    </tr>

                    <% foreach (Fbs.Core.CNEChecks.MarkItem item in Cert.Marks) { %>
                
                        <tr>
                            <td style="text-align: left; padding: 7px 0 7px 20px; border-top: solid 1px #777; border-right: solid 1px #777; "><%= item.SubjectName %></td>
                            <td style="padding: 7px 0; border-top: solid 1px #777;"><%= item.SubjectMark %></td>
                        </tr>

                    <% } %>
                
                </table>
            
            </td>
        </tr>

        <tr>
            <td style="padding-top: 20px; text-align: left;">
                Справка для личного дела абитуриента сформирована из Подсистемы ФИС &laquo;Результаты ЕГЭ&raquo; для образовательного учреждения:
<br><br><%= string.IsNullOrEmpty(OrganizationName) ? "<название не задано>" : OrganizationName %>
            </td>
        </tr>
   <tr>
            <td style="padding-top: 35px; text-align: left;">
                <table cellpadding="0" cellspacing="0" border="0" style="text-align: left;">
                    <tr>
                        <td style="padding-right: 10px;">Лицо, сформировавшее справку:<br><br></td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" border="0" style="text-align: center;">
                    <tr>
                        <td style="padding-right: 10px;">___________________</td>
                        <td style="padding-right: 10px;">_________________</td>
                        <td>_________________________________</td>
                    </tr>
                    <tr>
                        <td style="padding-right: 10px;"><sup>(должность)</sup></td>
                        <td style="padding-right: 10px;"><sup>(подпись)</sup></td>
                        <td><sup>(Фамилия И. О.)</sup></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding-top: 35px; text-align: left;">
                   <table cellpadding="0" cellspacing="0" border="0" style="text-align: left;">
                    <tr>
                        <td style="padding-right: 10px;">Ответственное лицо приемной комиссии:<br><br></td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" border="0" style="text-align: center;">
                    <tr>
                        <td style="padding-right: 10px;">___________________</td>
                        <td style="padding-right: 10px;">_________________</td>
                        <td>_________________________________</td>
                    </tr>
                    <tr>
                        <td style="padding-right: 10px;"><sup>(должность)</sup></td>
                        <td style="padding-right: 10px;"><sup>(подпись)</sup></td>
                        <td><sup>(Фамилия И. О.)</sup></td>
                    </tr>
                    <tr>
                        <td style="padding-right: 10px;">&nbsp;</td>
                        <td style="padding-right: 10px; padding-top: 25px;">М. П.</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
        
        <tr>
            <td style="padding-top: 40px; text-align: left;">
                Дата выдачи «____»&nbsp;&nbsp;____________&nbsp;&nbsp;______ г. регистрационный № ______________
            </td>
        </tr>

        </table>
    
    </asp:PlaceHolder>
    




    <!-- Справка не найдена -->
    <asp:PlaceHolder ID="phEmpty" runat="server">
        <table cellpadding="5" cellspacing="0" border="0" style="width: 590px; font-family: Arial; font-size: .9em; text-align: left;">
        
            <tr>
                <td style="font-size: 1.2em; font-weight: bold;">
                    Справка не найдена <br />
                </td>
            </tr>
            
            <tr>
                <td style="font-size: 1.2em;">
                    Перейдите на предыдущую страницу и повторите поиск свидетельства
                </td>
            </tr>
        
        </table>
    </asp:PlaceHolder>
    
    </form>
</body>
</html>
