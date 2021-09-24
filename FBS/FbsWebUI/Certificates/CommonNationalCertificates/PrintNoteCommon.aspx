<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintNoteCommon.aspx.cs" Inherits="Fbs.Web.Certificates.CommonNationalCertificates.PrintNoteCommon" EnableViewState="false" %>
<%@ Import Namespace="Fbs.Web.Certificates.CommonNationalCertificates" %>

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
            <td style="font-weight: bold">
                <%= 
                    (string.IsNullOrEmpty(Cert.LastName) ? string.Empty : Cert.LastName + "&nbsp;&nbsp;&nbsp;") +
                    (string.IsNullOrEmpty(Cert.FirstName) ? string.Empty : Cert.FirstName + "&nbsp;&nbsp;&nbsp;") +
                    (string.IsNullOrEmpty(Cert.MiddleName) ? string.Empty : Cert.MiddleName)
                %>
            </td>
        </tr>
        
        <tr>
            <td style="white-space: nowrap">
                документ, удостоверяющий личность: <%= Cert.DocumentSeries %>&nbsp;&nbsp;<%= Cert.DocumentNumber %>
            </td>
        </tr>

        <tr>
            <td style="">
                по результатам сдачи единого государственного экзамена обнаружил(а) следующие знания по общеобразовательным предметам:
            </td>
        </tr>
        
        <tr>
            <td style="padding-top: 20px;">
            
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; border: solid 1px #777">
                    <tr>
                        <td style="width: 30%;padding-top: 10px; padding-bottom: 10px; border-right: solid 1px #777;">Наименование<br />предмета</td>
                        <td style="width: 10%;padding-top: 10px; padding-bottom: 10px;border-right: solid 1px #777;">Балл</td>
                        <td style="width: 10%;padding-top: 10px; padding-bottom: 10px;border-right: solid 1px #777;">Год<br />сдачи</td>
                        <td style="width: 25%;padding-top: 10px; padding-bottom: 10px;border-right: solid 1px #777;">Статус результата</td>
                        <td style="width: 25%;padding-top: 10px; padding-bottom: 10px;">Номер<br />свидетельства</td>   

                    </tr>

                    <% foreach (PrintNoteMarkItem item in Cert.Marks) { %>
                
                        <tr>
                            <td style="text-align: left; padding: 7px 0 7px 20px; border-top: solid 1px #777; border-right: solid 1px #777; "><%= item.SubjectName %></td>
                            <td style="padding: 7px 0; border-top: solid 1px #777; border-right: solid 1px #777;"><%= item.Mark %></td>
                            <td style="padding: 7px 0; border-top: solid 1px #777; border-right: solid 1px #777;"><%= item.Year %></td>
                            <td style="padding: 7px 0; border-top: solid 1px #777; border-right: solid 1px #777;"><%= item.Status %></td>
                            <td style="padding: 7px 0; border-top: solid 1px #777;"><%= (string.IsNullOrEmpty(item.CertificateNumber) ? "&mdash;" : item.CertificateNumber) %></td>

                        </tr>

                    <% } %>
                
                </table>
            
            </td>
        </tr>

        <tr>
            <td style="padding-top: 20px; text-align: left;">
                Справка для личного дела абитуриента сформирована из ФИС ГИА и приема для образовательной организации:
<br><br><%= string.IsNullOrEmpty(OrganizationName) ? "<название не задано>" : OrganizationName %>
            </td>
        </tr>
        <tr>
            <td style="padding-top: 20px; text-align: left;font-size: 9pt;white-space: nowrap">Дата и время формирования справки: <%= DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") %></td>
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
