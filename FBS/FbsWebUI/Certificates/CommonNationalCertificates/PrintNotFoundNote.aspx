<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintNotFoundNote.aspx.cs" Inherits="Fbs.Web.Certificates.CommonNationalCertificates.PrintNotFoundNote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title></title>
    </head>
    <body>
        <form id="form1" runat="server">
            <!-- Форма справки -->
            <asp:PlaceHolder ID="phNote" runat="server">
                <table cellpadding="5" cellspacing="0" border="0" style="width: 590px; font-family: Arial; font-size: .9em; text-align: center;">
                    <tbody>
                        <tr>
                            <td style="font-size: 1.2em; font-weight: bold;">
                                Справка
                            </td>
                        </tr>

                        <tr>
                            <td style="font-weight: bold;">
                                об отсутствии результатов единого государственного экзамена
                            </td>
                        </tr>
                        <tr>
                            <td >
                                По запросу с параметрами:
                            </td>
                        </tr>
                        <tr>
                            <td style="">
                                <asp:PlaceHolder ID="requestPanel" runat="server">
                                </asp:PlaceHolder>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 20px;">
                                В ФИС ГИА и приема 
                                на <%=DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") %> сведения о результатах ЕГЭ
                                за период 2012 – <%=DateTime.Now.ToString("yyyy") %> годы не найдены:
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 20px; text-align: left;">
                                Справка для личного дела абитуриента сформирована из ФИС ГИА и приема для образовательной организации:
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
                            <td style="padding-top: 40px; text-align: left;white-space: nowrap">
                                Дата выдачи «____»&nbsp;&nbsp;____________&nbsp;&nbsp;______ г.&nbsp;&nbsp;Регистрационный&nbsp;№&nbsp;____________________________
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:PlaceHolder>
        </form>
    </body>
</html>
