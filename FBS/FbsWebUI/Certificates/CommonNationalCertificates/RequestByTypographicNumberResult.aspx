<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestByTypographicNumberResult.aspx.cs"
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.RequestByTypographicNumberResult"
    MasterPageFile="~/Common/Templates/Certificates.Master" %>

<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<%@ Import Namespace="Fbs.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCertificateHead" runat="server">

    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>

    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>

</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphCertificateActions" ID="cphActions">
    <asp:Panel runat="server" ID="pActions" Visible="false">
        <div class="h10">
        </div>
        <div class="border-block">
            <div class="tr">
                <div class="tt">
                    <div>
                    </div>
                </div>
            </div>
            <div class="content">
                <ul>
                    <% if (User.IsInRole("CheckSchoolCompetitionCertificate"))
                       { %>
                    <li>
                        <asp:HyperLink ID="hlCompetitionCertificates" runat="server" CssClass="gray" NavigateUrl="/Certificates/CompetitionCertificates/RequestResult.aspx?LastName={0}&FirstName={1}&PatronymicName={2}&Region={3}">
                    Проверить участие в олимпиадах
                        </asp:HyperLink>
                    </li>
                    <% } %>
                    <% if (User.IsInRole("CheckEntrant"))
                       { %>
                    <li><a class="gray" href="/Certificates/Entrants/CheckResult.aspx?Number=<%= Request.QueryString["Number"] %>">
                        Проверить поступление</a> </li>
                    <% } %>
                </ul>
            </div>
            <div class="br">
                <div class="tt">
                    <div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphCertificateContent" runat="server">
    <form id="Form1" runat="server">
    <asp:Repeater runat="server" ID="rpSearch" DataSourceID="dsSearch" OnItemDataBound="rpSearch_ItemDataBound">
        <ItemTemplate>
            <asp:Panel runat="server" ID="pNotExist" Visible="false">
                <div class="notice" id="CheckResultNoticeAbsentee">
                    <div class="top">
                        <div class="l">
                        </div>
                        <div class="r">
                        </div>
                        <div class="m">
                        </div>
                    </div>
                    <div class="cont">
                        <div dir="ltr" class="hide" title="Свернуть" onclick="ToggleNoticeState(this);">
                            x<span></span></div>
                        <div class="txt-in">
                            <p>
                                За дополнительной информацией о причине отсутствия свидетельства обращайтесь в Региональный
                                Центр Обработки Информации, находящийся на территории региона, в котором было выдано
                                данное свидетельство.</p>
                        </div>
                    </div>
                    <div class="bottom">
                        <div class="l">
                        </div>
                        <div class="r">
                        </div>
                        <div class="m">
                        </div>
                    </div>
                </div>
                <p align="center">
                    <img src="/Common/Images/spacer.gif" width="32" height="31" style="background-image: url('/Common/Images/important.gif')" />
                    Свидетельство с заданными параметрами не найдено.</p>
                      <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" NavigateUrl='<%# GenerateNotFoundPrintLink() %>' Visible='<%# Convert.ToBoolean(ConfigurationManager.AppSettings["EnableNotFoundNote"])%>' Text="Распечатать" />
                <script type="text/javascript">
                    InitNotice();
                </script>

            </asp:Panel>
            <asp:Panel runat="server" ID="pDeny" Visible="false">
                <p style="color: Red">
                    Свидетельство №<%# Eval("Number") %>
                    аннулировано
                    <%--<%# 
                    Convert.IsDBNull(Eval("DenyNewCertificateNumber")) ? String.Empty :
                        String.Format("(новое свидетельство №<a href=\"/Certificates/CommonNationalCertificates/CheckResult.aspx?number={0}\">{0}</a>)",
                        Eval("DenyNewCertificateNumber"))
                %>--%>
                    по следующей причине:<br />
                    <%# FormLinkToNewCertificate(Eval("DenyComment").ToString(), Eval("DenyNewCertificateNumber").ToString(), Eval("LastName").ToString())%></p>
                <div class="notice" id="CheckResultNoticeDeny">
                    <div class="top">
                        <div class="l">
                        </div>
                        <div class="r">
                        </div>
                        <div class="m">
                        </div>
                    </div>
                    <div class="cont">
                        <div dir="ltr" class="hide" title="Свернуть" onclick="ToggleNoticeState(this);">
                            x<span></span></div>
                        <div class="txt-in">
                            <p>
                                За дополнительной информацией о причине аннулирования свидетельства обращайтесь
                                в Региональный Центр Обработки Информации, находящийся на территории региона, в
                                котором было выдано данное свидетельство.</p>
                        </div>
                    </div>
                    <div class="bottom">
                        <div class="l">
                        </div>
                        <div class="r">
                        </div>
                        <div class="m">
                        </div>
                    </div>
                </div>

                <script type="text/javascript">
                    InitNotice();
                </script>
                
            </asp:Panel>
            <asp:Panel runat="server" ID="pCertificate">
                <table class="form" style="width: 100%">
                    <tr>
                        <td style="width: 20%">
                            Номер свидетельства
                        </td>
                        <td colspan="2">
                            <%# Eval("Number") %>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%">
                            Типографский номер
                        </td>
                        <td>
                            <%# Eval("TypographicNumber") %>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%">
                            Фамилия
                        </td>
                        <td>
                            <%# HighlightValues(Eval("LastName"), Eval("CheckLastName"), Eval("LastNameIsCorrect"))%>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%">
                            Имя
                        </td>
                        <td>
                            <%# HighlightValues(Eval("FirstName"), Eval("CheckFirstName"), Eval("FirstNameIsCorrect"))%>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%">
                            Отчество
                        </td>
                        <td>
                            <%# HighlightValues(Eval("PatronymicName"), Eval("CheckPatronymicName"), Eval("PatronymicNameIsCorrect"))%>
                        </td>
                    </tr>
                    <% if (User.IsInRole("ViewCommonNationalCertificateDocument"))
                       {%>
                    <tr>
                        <td style="width: 20%">
                            Документ
                        </td>
                        <td>
                            <span title="Серия">
                                <%# Eval("PassportSeria")%></span> <span title="Номер">
                                    <%# Eval("PassportNumber")%></span>
                        </td>
                    </tr>
                    <%} %>
                    </tr>
                    <tr>
                        <td style="width: 20%">
                            Регион
                        </td>
                        <td>
                            <%# Eval("RegionName")%>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%">
                            Год выдачи свидетельства
                        </td>
                        <td>
                            <%# Eval("Year") %>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%">
                            Статус свидетельства
                        </td>
                        <td>
                            <%# HighlightValue(Eval("Status")) %>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%" title="Количество уникальных проверок ВУЗами и их филиалами">
                            Проверки <span style="color:Red;">*</span>
                        </td>
                        <td>
                            <%# Eval("UniqueIHEaFCheck")%>
                        </td>
                    </tr>
                </table>
                <asp:DataGrid runat="server" ID="dgSubjects" DataSourceID="dsSearch" AutoGenerateColumns="false"
        EnableViewState="false" ShowHeader="True" GridLines="None" CssClass="table-th"
        Width="60%">
        <HeaderStyle CssClass="th" />
        <Columns>
            <asp:TemplateColumn>
                <HeaderStyle Width="25%" CssClass="left-th" />
                <HeaderTemplate>
                    <div>
                        Предмет</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# HighlightValue(Eval("SubjectName"), 
                    Convert.ToBoolean(Eval("SubjectMarkIsCorrect")) || string.IsNullOrEmpty(Convert.ToString(Eval("CheckSubjectMark"))))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="15%" />
                <HeaderTemplate>
                    <div>
                        Заявлено</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <%# HighlightValue(Eval("CheckSubjectMark"), Eval("SubjectMarkIsCorrect"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="15%" />
                <HeaderTemplate>
                    <div>
                        В&nbsp;базе</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <%# HighlightValue(Eval("SubjectMark")) %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="10%" CssClass="right-th" />
                <HeaderTemplate>
                    <div>
                        Апелляция&nbsp;</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <%# (Convert.IsDBNull(Eval("HasAppeal")) ? string.Empty : (!Convert.IsDBNull(Eval("HasAppeal")) && Convert.ToBoolean(Eval("HasAppeal"))) ? "<span style=\"color:Red\">Да</span>" : "Нет") %>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
            </asp:Panel>
            <asp:HiddenField runat="server" ID="hfCNEId" Value='<%#Eval("CertificateId")%>' />
            <asp:HiddenField runat="server" ID="hfIsExist" Value='<%#Eval("IsExist")%>' />
            <asp:HiddenField runat="server" ID="hfIsDeny" Value='<%#Eval("IsDeny")%>' />
            <asp:HiddenField runat="server" ID="hfLastName" Value='<%#Eval("LastName") %>' />
            <asp:HiddenField runat="server" ID="hfFirstName" Value='<%#Eval("FirstName") %>' />
            <asp:HiddenField runat="server" ID="hfPatronymicName" Value='<%#Eval("PatronymicName") %>' />
            <asp:HiddenField runat="server" ID="hfRegion" Value='<%#Eval("RegionId") %>' />
            <asp:HiddenField runat="server" ID="hfLastNameIsCorrect" Value='<%#Eval("LastNameIsCorrect") %>' />
            <asp:HiddenField runat="server" ID="hfFirstNameIsCorrect" Value='<%#Eval("FirstNameIsCorrect") %>' />
            <asp:HiddenField runat="server" ID="hfPatronymicNameIsCorrect" Value='<%#Eval("PatronymicNameIsCorrect") %>' />
            
        </ItemTemplate>
    </asp:Repeater>

    
    
    <web:NoRecordsText runat="server" ControlId="rpSearch" ID="nrtSubjects" >
        <asp:Panel runat="server">
            <p align="center">
                <img src="/Common/Images/spacer.gif" width="32" height="31" style="background-image: url('/Common/Images/important.gif')" />
                По Вашему запросу ничего не найдено
             </p>
             <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" NavigateUrl='<%# GenerateNotFoundPrintLink() %>' Visible='<%# Convert.ToBoolean(ConfigurationManager.AppSettings["EnableNotFoundNote"])%>' Text="Распечатать" />
        </asp:Panel>
    </web:NoRecordsText>
   
    
    <asp:SqlDataSource runat="server" ID="dsSearch" ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="dbo.CheckCommonNationalExamCertificateByNumber" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure" EnableCaching="True" CacheDuration="2">
        <SelectParameters>
            <fbs:CurrentUserParameter Name="login" Type="String" />
            <fbs:CurrentUserParameter Name="ip" Type="String" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="number" Type="String"
                QueryStringField="number" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="checkTypographicNumber"
                Type="String" QueryStringField="TypographicNumber" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="checkLastName" Type="String"
                QueryStringField="LastName" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="checkFirstName" Type="String"
                QueryStringField="FirstName" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="checkPatronymicName"
                Type="String" QueryStringField="PatronymicName" />
            <asp:QueryStringParameter ConvertEmptyStringToNull="true" Name="checkSubjectMarks "
                Type="String" QueryStringField="SubjectMarks" />
        </SelectParameters>
    </asp:SqlDataSource>
    
    <asp:PlaceHolder ID="phUniqueChecks" runat="server" Visible="true">
        <span style="color: Red">* </span><span>Количество уникальных проверок свидетельства ВУЗами и их филиалами. Для пользователей ССУЗов данное поле носит справочный характер.</span>
    </asp:PlaceHolder>
    
    <asp:Panel runat="server" ID="pNoticeMarks" Visible="true">
        <div class="notice">
            <div class="top">
                <div class="l">
                </div>
                <div class="r">
                </div>
                <div class="m">
                </div>
            </div>
            <div class="cont">
                <div dir="ltr" class="hide" title="Свернуть" onclick="ToggleNoticeState(this);">
                    x<span></span></div>
                <div class="txt-in">
                    <p>
                    Красным цветом в столбце «В базе» выделяются баллы, которые меньше минимальных установленных Рособрнадзором значений за 2013 год.
                    <br /><span style="color:Red;">В 2011-2013 годах в свидетельство о результатах ЕГЭ баллы ниже установленного Рособрнадзором минимального, не выставляются.</span>
                    <br />С Приказами Рособрнадзора о применении шкалы минимальных баллов за соответствующие годы можно ознакомиться в разделе «Документы».
                        </p>
                    <p>
                        Срок действия свидетельств ЕГЭ определяется в соответствии с действующими нормативными
                        документами. 
                        <br />В 2013 году действительны свидетельства 2012 и 2013 годов. 
    <br /><span style="color:Red;">Свидетельства 2011 года действительны только для лиц, проходивших военную службу по призыву и уволенных с военной службы.</span>
С нормативными документами можно ознакомиться в разделе «Документы» Подсистемы ФИС &laquo;Результаты ЕГЭ&raquo;.</p>
                </div>
            </div>
            <div class="bottom">
                <div class="l">
                </div>
                <div class="r">
                </div>
                <div class="m">
                </div>
            </div>
        </div>
    </asp:Panel>
    </form>
</asp:Content>

<script runat="server">

    private static string HighlightValue(object valueObj)
    {
        string value = Convert.ToString(valueObj);
        if (value.StartsWith("!") || value.StartsWith("Истек"))
            return String.Format("<font color=\"red\">{0}</font>", value);
        return value;
    }

    private static string HighlightValue(object valueObj, object isCorrectObj)
    {
        string value = Convert.ToString(valueObj);
        if (!Convert.IsDBNull(isCorrectObj) && Convert.ToBoolean(isCorrectObj))
            return value;
        return String.Format("<font color=\"red\">{0}</font>", value);
    }

    private static string HighlightValues(object valueObj, object checkValueObj, object isCorrectObj)
    {
        string value = Convert.ToString(valueObj);
        string checkValue = Convert.ToString(checkValueObj);
        if ((!Convert.IsDBNull(isCorrectObj) && Convert.ToBoolean(isCorrectObj)) ||
                string.IsNullOrEmpty(checkValue))
            return value;

        if (Convert.IsDBNull(valueObj) && Convert.IsDBNull(checkValueObj))
            return string.Empty;

        checkValue = String.IsNullOrEmpty(checkValue) ? "не&nbsp;задано" : checkValue;
        value = String.IsNullOrEmpty(value) ? "не&nbsp;найдено" : value;

        return String.Format("<span title=\"Ошибка: заявленое {0} (в базе {1})\"><span style=\"color:Red\">{0}</span> ({1})</span>",
            checkValue, value);
    }

    private static string FormLinkToNewCertificate(string comment, string newCertificate, string lastName)
    {
        if (string.IsNullOrEmpty(newCertificate))
            return comment;
        return string.Format("<a href=\"/Certificates/CommonNationalCertificates/CheckResult.aspx?number={1}&LastName={2}\">{0}</a>", comment, newCertificate, lastName);
    }        
</script>

