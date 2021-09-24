<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckResultByPassportForOpenedFbs.aspx.cs"
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.CheckResultByPassportForOpenedFbs"
    MasterPageFile="~/Common/Templates/Certificates.Master" %>

<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<%@ Register TagPrefix="fbs" TagName="HistotyCertificate" Src="~/Controls/CommonNationalCertificates/HistoryCertificate.ascx" %>
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
                    <% if (User.IsInRole("CheckEntrant"))
                       { %>
                    <li><a class="gray" href="/Certificates/Entrants/CheckResultForOpenedFbs.aspx?Number=<%=Request.QueryString["Number"]%>">
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
    <asp:Repeater runat="server" ID="rpSearch" OnItemDataBound="rpSearch_ItemDataBound">
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
                        <div dir="ltr" class="hide" title="Свернуть" onclick=" ToggleNoticeState(this); ">
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
                <script type="text/javascript">
                    InitNotice();
                </script>
            </asp:Panel>
            <asp:Panel runat="server" ID="pDeny" Visible="false">
                <p style="color: Red">
                    Свидетельство №<%#Eval("Number")%>
                    аннулировано по следующей причине:<br />
                    <%#FormLinkToNewCertificate(Eval("DenyComment").ToString(),
                                                          Eval("DenyNewCertificateNumber").ToString())%></p>
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
                        <div dir="ltr" class="hide" title="Свернуть" onclick=" ToggleNoticeState(this); ">
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
                            <%#Eval("Number")%>
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="width: 20%">
                            Типографский номер
                        </td>
                        <td>
                            <%#Eval("TypographicNumber")%>
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
                                <%#Eval("PassportSeria")%></span> <span title="Номер">
                                    <%#Eval("PassportNumber")%></span>
                        </td>
                    </tr>
                    <% } %>
                    </tr>
                    <tr>
                        <td style="width: 20%">
                            Регион
                        </td>
                        <td>
                            <%#Eval("RegionName")%>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%">
                            Год выдачи свидетельства
                        </td>
                        <td>
                            <%#Eval("Year")%>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%" title="Количество уникальных проверок ВУЗами и их филиалами">
                            Проверки <span style="color: Red">*</span>
                        </td>
                        <td>
                            <%# Eval("UniqueIHEaFCheck") is DBNull ? "0" : Eval("UniqueIHEaFCheck")%>
                            <% if (User.IsInRole("ViewCertificateCheckHistory"))
                               {%>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:HyperLink ID="lnkCheckHystory" Enabled='<%# !(Eval("UniqueIHEaFCheck") is DBNull) && Convert.ToInt32(Eval("UniqueIHEaFCheck")) > 0 %>'
                                NavigateUrl='<%# string.Format(
                                                    "~/Certificates/CommonNationalCertificates/CheckHistoryByPassportForOpenedFbs.aspx?certificateId={0}&certificateNumber={1} ", Eval("CertificateId"), Eval("Number")) %>'
                                runat="server">Просмотреть историю проверок</asp:HyperLink>
                            <%} %>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%">
                            Статус свидетельства
                        </td>
                        <td>
                            <%#HighlightValue(Eval("Status"))%>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <a target="_blank"
                                href="PrintNote.aspx?id=<%=NoteId%>">Распечатать справку</a>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:HiddenField runat="server" ID="hfCNEId" Value='<%#Eval("CertificateId")%>' />
            <asp:HiddenField runat="server" ID="hfIsExist" Value='<%#Eval("IsExist")%>' />
            <asp:HiddenField runat="server" ID="hfIsDeny" Value='<%#Eval("IsDeny")%>' />
            <asp:HiddenField runat="server" ID="hfRegion" Value='<%#Eval("RegionId")%>' />
            <asp:DataGrid runat="server" ID="dgSubjects" DataSource="<%#this.rpSearch.DataSource %>" AutoGenerateColumns="false" EnableViewState="false"
        ShowHeader="True" GridLines="None" CssClass="table-th" Width="60%">
        <HeaderStyle CssClass="th" />
        <Columns>
            <asp:TemplateColumn>
                <HeaderStyle Width="25%" CssClass="left-th" />
                <HeaderTemplate>
                    <div>
                        Предмет</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%#HighlightValue(Eval("SubjectName"),
                                                Convert.ToBoolean(Convert.ToInt32(Eval("SubjectMarkIsCorrect"))) ||
                                                string.IsNullOrEmpty(Convert.ToString(Eval("CheckSubjectMark"))))%>
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
                    <%#HighlightValue(Eval("CheckSubjectMark"), Eval("SubjectMarkIsCorrect"))%>
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
                    <%#HighlightValue(Eval("SubjectMark"))%>
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
                    <%#(Convert.IsDBNull(Eval("HasAppeal"))
                                             ? string.Empty
                                             : (!Convert.IsDBNull(Eval("HasAppeal")) && Convert.ToBoolean(Convert.ToInt32(Eval("HasAppeal"))))
                                                   ? "<span style=\"color:Red\">Да</span>"
                                                   : "Нет")%>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
        </ItemTemplate>
    </asp:Repeater>
    
    <web:NoRecordsText runat="server" ControlId="rpSearch" ID="nrtSubjects" >
        <asp:Panel ID="Panel1" runat="server">
            <p align="center">
                <img src="/Common/Images/spacer.gif" width="32" height="31" style="background-image: url('/Common/Images/important.gif')" />
                По Вашему запросу ничего не найдено
             </p>
             <%--<asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" NavigateUrl='<%# GenerateNotFoundPrintLink() %>' Visible='<%# Convert.ToBoolean(ConfigurationManager.AppSettings["EnableNotFoundNote"])%>' Text="Распечатать" />--%>
        </asp:Panel>
    </web:NoRecordsText>
    <div class="SpanContainer30">
    </div>
    <fbs:HistotyCertificate runat="server" ID="historyCertificate" />
    <asp:PlaceHolder ID="phUniqueChecks" runat="server" Visible="true"><span style="color: Red">
        * </span><span>Количество уникальных проверок свидетельства ВУЗами и их филиалами. Для
            пользователей ССУЗов данное поле носит справочный характер.</span> </asp:PlaceHolder>
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
                <div dir="ltr" class="hide" title="Свернуть" onclick=" ToggleNoticeState(this); ">
                    x<span></span></div>
                <div class="txt-in">
                    <p>
                        <span style="color: Red;">В 2011-2013 годах в свидетельства о результатах ЕГЭ баллы
                            ниже установленного Рособрнадзором минимального, не выставляются.</span>
                        <br />
                        С Приказами Рособрнадзора о применении шкалы минимальных баллов за соответствующие
                        годы можно ознакомиться в разделе «Документы».
                    </p>
                    <p>
                        Срок действия свидетельств ЕГЭ определяется в соответствии с действующими нормативными
                        документами.
                        <br />
                        В 2013 году действительны свидетельства 2012 и 2013 годов. 
                        <br />
                        <span style="color: Red;">Свидетельства 2011 года действительны только для лиц, проходивших 
                            военную службу по призыву и уволенных с военной службы.</span>
                        <br>
                        С нормативными документами можно ознакомиться в разделе «Документы».
                        <br />
                        <br />
                        <span id="SRusHist" runat="server" visible="false">* В 2008 году наименование предмета
                            "История" было "История России".</span></p>
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
