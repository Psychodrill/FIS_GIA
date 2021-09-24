<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchCheckByNumberResult.aspx.cs"
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.BatchCheckByNumberResult"
    MasterPageFile="~/Common/Templates/Certificates.Master" %>

<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Fbs.Core" %>
<%@ Import Namespace="Fbs.Web" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server">
    <% if (!HasResults)
       {%>
    <meta http-equiv="refresh" content="10" />
    <%} %>
    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphCertificateActions">
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
                <li>
                    <img alt="" src="/Common/Images/csv-icon.gif" />
                    <a href="/Certificates/CommonNationalCertificates/BatchCheckResultExportCsv.aspx?id=<%= Request.QueryString["id"] %>"
                        title="Экспорт в CSV" class="gray">Экспорт в CSV</a></li>
                <!-- Функциональность отключена до второго этапа -->
                <!--<li><a href="/Certificates/CommonNationalCertificates/BatchCheckResultExport.aspx?id=<%= Request.QueryString["id"] %>"
                    title="Экспорт в XLS">Экспорт в XLS</a></li>-->
            </ul>
        </div>
        <div class="br">
            <div class="tt">
                <div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphCertificateContent" runat="server">
<form runat="server" target="_blank" >
    <div class="notice" id="BatchCheckResultSubjectNotice">
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
                    ТН - Типографский номер; Год - Год выдачи свидетельства; РЯ - русский язык; М -
                    математика; Ф - физика; Х - химия; Б - биология; ИР - история России; Г - география;
                    АЯ - английский язык; НЯ - немецкий язык; ФЯ - французский язык; О - обществознание;
                    Л - литература; ИЯ - испанский язык; И - информатика.
                </p>
                <p>
                    Если не удается найти результаты участника ЕГЭ или свидетельство аннулировано без
                    объяснения причины, то обращайтесь за информацией в Региональный Центр Обработки
                    Информации, находящийся на территории региона, в котором было выдано данное свидетельство.</p>
                <p>
                    Специальным знаком «!» перед баллом выделены баллы, которые меньше минимальных установленных
                    Рособрнадзором значений.
                    <br>
                    Срок действия свидетельств ЕГЭ определяется в соответствии с действующими нормативными
                    документами.
                    <br />
                    В 2013 году действительны свидетельства 2012 и 2013 годов. 
                    <br />
                    <span style="color: Red;">Свидетельства 2011 года действительны только для лиц, 
                        проходивших военную службу по призыву и уволенных с военной службы.</span>
                    <br>
                    С нормативными документами можно ознакомиться в разделе «Документы»</p>
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
    <table class="pager">
        <tr>
            <td>
                <web:DataSourcePager runat="server" ID="DataSourcePagerHead" DataSourceId="dsResultsListCount"
                    StartRowIndexParamName="start" MaxRowCountParamName="count" HideDefaultTemplates="true"
                    AlwaysShow="true" DefaultMaxRowCount="20" DefaultMaxRowCountSource="Cookies">
                    <Header>
                        Результаты проверок #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                        <web:DataSourceMaxRowCount runat="server" Variants="20,50,100" DataSourcePagerId="DataSourcePagerHead">
                            <Header>
                                Записей на странице:
                            </Header>
                            <Footer>
                                .</Footer>
                            <Separator>
                                ,
                            </Separator>
                            <ActiveTemplate>
                                <span>#count#</span></ActiveTemplate>
                            <InactiveTemplate>
                                <a href="/SetMaxRowCount.aspx?count=#count#" title="Отображать #count# записей на странице">
                                    #count#</a></InactiveTemplate>
                        </web:DataSourceMaxRowCount>
                    </Header>
                </web:DataSourcePager>
            </td>
            <td align="right">
                <web:DataSourcePager runat="server" DataSourceId="DataSourcePagerHead" StartRowIndexParamName="start"
                    MaxRowCountParamName="count">
                    <PrevGroupTemplate>
                    </PrevGroupTemplate>
                    <NextGroupTemplate>
                    </NextGroupTemplate>
                    <ActivePrevPageTemplate>
                        <a href="#PageUrl#">Предыдущая</a>&nbsp;</ActivePrevPageTemplate>
                    <ActivePageTemplate>
                        <span>#PageNo#</span>
                    </ActivePageTemplate>
                    <ActiveNextPageTemplate>
                        &nbsp;<a href="#PageUrl#">Следующая</a></ActiveNextPageTemplate>
                </web:DataSourcePager>
            </td>
        </tr>
    </table>
    <style>
        #ResultContainer
        {
            overflow-x: scroll;
            padding-right: 2px!important;
        }
        html:first-child #ResultContainer
        {
            overflow: scroll;
        }
        /* только для Opera */#ResultContainer td
        {
            white-space: nowrap;
        }
    </style>
    <div id="ResultContainer" style="width: 100%; height: auto;">
        <asp:DataGrid runat="server" ID="dgResultsList" DataSourceID="dsResultsList" AutoGenerateColumns="false"
            EnableViewState="true" ShowHeader="True" GridLines="None" CssClass="table-th-border table-th">
            <HeaderStyle CssClass="th" />
            <Columns>
                <asp:TemplateColumn>
                    <HeaderStyle CssClass="left-th" />
                    <HeaderTemplate>
                        <div title="Номер свидетельства">
                            Свидетельство</div>
                    </HeaderTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:LinkButton title='<%# "Свидетельство № "+ Eval("CertificateNumber")+ " не найдено"%>' style="color:Red" Text="не найдено" runat="server" CommandName="PrintNotFound" Visible='<%#!Convert.ToBoolean(Eval("IsExist")) && Convert.ToBoolean(ConfigurationManager.AppSettings["EnableNotFoundNote"])%>' />
                        <asp:PlaceHolder Visible='<%# Convert.ToBoolean(Eval("IsExist")) || !Convert.ToBoolean(ConfigurationManager.AppSettings["EnableNotFoundNote"])%>' runat="server">
                            
                            <%#this.ShowCertificateNumber(Container.DataItem) %>
                        </asp:PlaceHolder>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <div>
                            ТН</div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("TypographicNumber") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="20%" />
                    <HeaderTemplate>
                        <div>
                            Документ</div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <span title="Серия документа">
                            <%# Eval("PassportSeria")%></span> <span title="Номер документа">
                                <%# Eval("PassportNumber")%></span>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="30%" />
                    <HeaderTemplate>
                        <div>
                            Регион</div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.ToString(Eval("RegionName")) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="10%" />
                    <HeaderTemplate>
                        <div>
                            Год</div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("SourceCertificateYear")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle Width="10%" />
                    <HeaderTemplate>
                        <div>
                            Статус</div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Highlight(Eval("Status")) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <div title="Количество уникальных проверок ВУЗами и их филиалами">
                            Пр</div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.IsDBNull(Eval("UniqueIHEaFCheck")) ? 0 : Eval("UniqueIHEaFCheck") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Русский язык">Р Я</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("RussianMark"), Eval("RussianCheckMark"), Eval("RussianMarkIsCorrect"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Апелляция по русскому языку">А п</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.IsDBNull(Eval("RussianHasAppeal")) ? string.Empty : 
                    (!Convert.IsDBNull(Eval("RussianHasAppeal")) && Convert.ToBoolean(Eval("RussianHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Математика">М</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("MathematicsMark"), Eval("MathematicsCheckMark"), Eval("MathematicsMarkIsCorrect"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Апелляция по математике">А п</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.IsDBNull(Eval("MathematicsHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("MathematicsHasAppeal")) && Convert.ToBoolean(Eval("MathematicsHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Физика">Ф</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("PhysicsMark"), Eval("PhysicsCheckMark"), Eval("PhysicsMarkIsCorrect"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Апелляция по физике">А п</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.IsDBNull(Eval("PhysicsHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("PhysicsHasAppeal")) && Convert.ToBoolean(Eval("PhysicsHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Химия">Х</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("ChemistryMark"), Eval("ChemistryCheckMark"), Eval("ChemistryMarkIsCorrect"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Апелляция по химии">А п</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.IsDBNull(Eval("ChemistryHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("ChemistryHasAppeal")) && Convert.ToBoolean(Eval("ChemistryHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Биология">Б</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("BiologyMark"), Eval("BiologyCheckMark"), Eval("BiologyIsCorrect"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Апелляция по биологии">А п</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.IsDBNull(Eval("BiologyHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("BiologyHasAppeal")) && Convert.ToBoolean(Eval("BiologyHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="История России">И Р</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("RussiaHistoryMark"), Eval("RussiaHistoryCheckMark"), Eval("RussiaHistoryMarkIsCorrect"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Апелляция по истории России">А п</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.IsDBNull(Eval("RussiaHistoryHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("RussiaHistoryHasAppeal")) && Convert.ToBoolean(Eval("RussiaHistoryHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="География">Г</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("GeographyMark"), Eval("GeographyCheckMark"), Eval("GeographyMarkIsCorrect"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Апелляция по географии">А п</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.IsDBNull(Eval("GeographyHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("GeographyHasAppeal")) && Convert.ToBoolean(Eval("GeographyHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Английский язык">А Я</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("EnglishMark"), Eval("EnglishCheckMark"), Eval("EnglishMarkIsCorrect"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Апелляция по английскому языку">А п</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.IsDBNull(Eval("EnglishHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("EnglishHasAppeal")) && Convert.ToBoolean(Eval("EnglishHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Немецкий язык">Н Я</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("GermanMark"), Eval("GermanCheckMark"), Eval("GermanMarkIsCorrect"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Апелляция по немецкому языку">А п</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.IsDBNull(Eval("GermanHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("GermanHasAppeal")) && Convert.ToBoolean(Eval("GermanHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Французский язык">Ф Я</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("FranchMark"), Eval("FranchCheckMark"), Eval("FranchMarkIsCorrect"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Апелляция по французскому языку">А п</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.IsDBNull(Eval("FranchHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("FranchHasAppeal")) && Convert.ToBoolean(Eval("FranchHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Обществознание">О</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("SocialScienceMark"), Eval("SocialScienceCheckMark"), Eval("SocialScienceMarkIsCorrect"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Апелляция по обществознанию">А п</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.IsDBNull(Eval("SocialScienceHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("SocialScienceHasAppeal")) && Convert.ToBoolean(Eval("SocialScienceHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Литература">Л</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("LiteratureMark"), Eval("LiteratureCheckMark"), Eval("LiteratureMarkIsCorrect"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Апелляция по литературе">А п</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.IsDBNull(Eval("LiteratureHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("LiteratureHasAppeal")) && Convert.ToBoolean(Eval("LiteratureHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Испанский язык">И Я</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("SpanishMark"), Eval("SpanishCheckMark"), Eval("SpanishMarkIsCorrect"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Апелляция по испанскому языку">А п</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.IsDBNull(Eval("SpanishHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("SpanishHasAppeal")) && Convert.ToBoolean(Eval("SpanishHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <span title="Информатика и ИКТ">И</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(Eval("InformationScienceMark"), Eval("InformationScienceCheckMark"), Eval("InformationScienceMarkIsCorrect"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle CssClass="right-th" />
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderTemplate>
                        <div title="Апелляция по информатике">
                            А п</div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.IsDBNull(Eval("InformationScienceHasAppeal")) ? string.Empty :
                    (!Convert.IsDBNull(Eval("InformationScienceHasAppeal")) && Convert.ToBoolean(Eval("InformationScienceHasAppeal")) ?
                        "<span style=\"color:Red\">Да</span>" : "Нет") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </div>
    <web:NoRecordsText runat="server" ControlId="dgResultsList">
        <Message>
            <style type="text/css">
                #ExportContainer, #ResultContainer
                {
                    display: none;
                }
            </style>
            <p class="norecords">
                Осуществляется обработка файла</p>
        </Message>
    </web:NoRecordsText>
    <table class="pager">
        <tr>
            <td>
                <web:DataSourcePager runat="server" DataSourceId="DataSourcePagerHead" StartRowIndexParamName="start"
                    MaxRowCountParamName="count" HideDefaultTemplates="true" AlwaysShow="true" DefaultMaxRowCount="20"
                    DefaultMaxRowCountSource="Cookies">
                    <Header>
                        Результаты проверок #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                        <web:DataSourceMaxRowCount runat="server" Variants="20,50,100" DataSourcePagerId="DataSourcePagerHead">
                            <Header>
                                Записей на странице:
                            </Header>
                            <Footer>
                                .</Footer>
                            <Separator>
                                ,
                            </Separator>
                            <ActiveTemplate>
                                <span>#count#</span></ActiveTemplate>
                            <InactiveTemplate>
                                <a href="/SetMaxRowCount.aspx?count=#count#" title="Отображать #count# записей на странице">
                                    #count#</a></InactiveTemplate>
                        </web:DataSourceMaxRowCount>
                    </Header>
                </web:DataSourcePager>
            </td>
            <td align="right">
                <web:DataSourcePager runat="server" DataSourceId="DataSourcePagerHead" StartRowIndexParamName="start"
                    MaxRowCountParamName="count">
                    <PrevGroupTemplate>
                    </PrevGroupTemplate>
                    <NextGroupTemplate>
                    </NextGroupTemplate>
                    <ActivePrevPageTemplate>
                        <a href="#PageUrl#">Предыдущая</a>&nbsp;</ActivePrevPageTemplate>
                    <ActivePageTemplate>
                        <span>#PageNo#</span>
                    </ActivePageTemplate>
                    <ActiveNextPageTemplate>
                        &nbsp;<a href="#PageUrl#">Следующая</a></ActiveNextPageTemplate>
                </web:DataSourcePager>
            </td>
        </tr>
    </table>
    <asp:SqlDataSource runat="server" ID="dsResultsList" ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="dbo.SearchCommonNationalExamCertificateCheck" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure" OnSelecting="dsResultsList_Selecting">
        <SelectParameters>
            <fbs:CurrentUserParameter Name="login" Type="String" />
            <asp:QueryStringParameter Name="batchId" QueryStringField="id" Type="Int64" />
            <asp:QueryStringParameter DefaultValue="1" Name="startRowIndex" Type="String" QueryStringField="start" />
            <web:MaxRowCountParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count"
                CheckParamName="count" CheckParamSource="Cookies" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource runat="server" ID="dsResultsListCount" ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="dbo.SearchCommonNationalExamCertificateCheck" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure">
        <SelectParameters>
            <fbs:CurrentUserParameter Name="login" Type="String" />
            <asp:QueryStringParameter Name="batchId" QueryStringField="id" Type="Int64" />
            <asp:Parameter Name="showCount" Type="Boolean" DefaultValue="True" />
        </SelectParameters>
    </asp:SqlDataSource>
    <script type="text/javascript">
        InitNotice();
    </script>
</form>
</asp:Content>
<script runat="server">

    private static string HighlightValue(object valueObj, object checkValueObj, object isCorrectObj)
    {
        string value = Convert.ToString(valueObj);
        if (!Convert.IsDBNull(isCorrectObj) && Convert.ToBoolean(isCorrectObj))
            return value;

        if (Convert.IsDBNull(valueObj) && Convert.IsDBNull(checkValueObj))
            return string.Empty;

        string checkValue = Convert.ToString(checkValueObj);
        string descriptionCheckValue = String.IsNullOrEmpty(checkValue) ? "не&nbsp;задано" : checkValue;
        string descriptionValue = String.IsNullOrEmpty(value) ? "не&nbsp;найдено" : value;

        return String.Format("<span title=\"Ошибка: заявленное {2} (в базе {3})\" style=\"color:Red\">{0} ({1})</span>",
            Highlight(checkValue), Highlight(value), descriptionCheckValue, descriptionValue);
    }

    private static string Highlight(object value)
    {
        string sValue = value.ToString();
        return sValue.StartsWith("!") || sValue.StartsWith("Истек") ? string.Format("<span style=\"color:red\">{0}</span>", sValue) : sValue;
    }
</script>
