<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchRequestByPassportResult.aspx.cs" 
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.BatchRequestByPassportResult" 
    MasterPageFile="~/Common/Templates/Certificates.Master" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Fbs.Core" %>
<%@ Import Namespace="Fbs.Web" %>
<%@ Import Namespace="Fbs.Web.Helpers" %>
<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server">
<% if (!HasResults) {%>
    <meta http-equiv="refresh" content="10" />
<%} %>
    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphCertificateActions">
    <div class="h10"></div>
    <div class="border-block">
        <div class="tr"><div class="tt"><div></div></div></div>
        <div class="content">
        <ul>
            <!--li><a href="/Certificates/CommonNationalCertificates/BatchRequestByPassportResultExportCsv.aspx?id=<%= Request.QueryString["id"] %>" 
                title="Экспорт в CSV (без баллов)" class="gray">Экспорт в CSV (без баллов)</a></li-->
            <li>
            <img alt="" src="/Common/Images/csv-icon.gif"/>
            <a href="/Certificates/CommonNationalCertificates/BatchRequestByPassportResultExportCsvExtended.aspx?id=<%= Request.QueryString["id"] %>&year=<%= Request.QueryString["year"] %>" 
                title="Экспорт в CSV" class="gray">Экспорт в CSV</a></li>
            <!--li>
                <div class="notice">
              <div class="cleaner"></div>
                    <div class="top"><div class="l"></div><div class="r"></div><div class="m"></div></div>
                    <div class="cont">
                        <div class="txt-in">
                            <p><img src="/Common/Images/notice-block-notice.gif" width="22" height="22" 
                                alt="!" class="ico-notice" /> Внимание! Специальный формат, подробнее см. 
                                <a href="/Instr" title="Инструкция">инструкцию</a>.</p>
                        </div>
                    </div>
                    <div class="bottom"><div class="l"></div><div class="r"></div><div class="m"></div></div>
                </div>
                <a href="/Certificates/CommonNationalCertificates/BatchRequestByPassportResultExportCsvExtendedSpecial.aspx?id=<%= Request.QueryString["id"] %>" 
                    title="Экспорт в CSV (с актуальными баллами)" class="gray">Экспорт в CSV (с актуальными баллами)</a></li-->
        <% if (User.IsInRole("CheckCommonNationalCertificateExam")) { %>
            <!--li>
                <div class="notice">
              <div class="cleaner"></div>
                    <div class="top"><div class="l"></div><div class="r"></div><div class="m"></div></div>
                    <div class="cont">
                        <div class="txt-in">
                            <p><img src="/Common/Images/notice-block-notice.gif" width="22" height="22" 
                                alt="!" class="ico-notice" />Специальный формат для АИС Экзамен.</p>
                        </div>
                    </div>
                    <div class="bottom"><div class="l"></div><div class="r"></div><div class="m"></div></div>
                </div>
                <a href="/Certificates/CommonNationalCertificates/BatchRequestByPassportResultExportCsvExtendedByExam.aspx?id=<%= Request.QueryString["id"] %>" 
                    title="Экспорт в CSV (сдача экзаменов)" class="gray">Экспорт в CSV (сдача экзаменов)</a></li-->
        <% } %>
        <!-- Функциональность отключена до второго этапа -->
        <!--<li><a href="/Certificates/CommonNationalCertificates/BatchRequestByPassportResultExport.aspx?id=<%= Request.QueryString["id"] %>"
                title="Экспорт в XLS">Экспорт в XLS</a></li>-->
        </ul>
        </div>
        <div class="br"><div class="tt"><div></div></div></div>
    </div>    
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphCertificateContent">
<form runat="server" target="_blank" >
    <div class="notice" id="BatchRequestResultSubjectNotice">
    <div class="top"><div class="l"></div><div class="r"></div><div class="m"></div></div>
    <div class="cont">
    <div dir="ltr" class="hide" title="Свернуть" onclick="ToggleNoticeState(this);">x<span></span></div>
    <div class="txt-in">
    
    <p>
    ТН - Типографский номер; Год - Год выдачи свидетельства; 
    РЯ - русский язык; М - математика; Ф - физика; Х - химия; Б - биология; 
    ИР - история России; Г - география; АЯ - английский язык; НЯ - немецкий язык; ФЯ - французский язык; О - обществознание; 
    Л - литература; ИЯ - испанский язык; И - информатика.
    </p>
    <p>Если не удается найти результаты участника ЕГЭ или свидетельство аннулировано без объяснения причины, то обращайтесь за информацией в Региональный Центр Обработки Информации, находящийся на территории региона, в котором было выдано данное свидетельство.</p>
    <p>Специальным знаком «!» перед баллом выделены баллы, которые меньше минимальных установленных Рособрнадзором значений.
    <br>Срок действия свидетельств ЕГЭ определяется в соответствии с действующими нормативными документами.
    <br />В 2013 году действительны свидетельства 2012 и 2013 годов. 
    <br /><span style="color:Red;">Свидетельства 2011 года действительны только для лиц, проходивших военную службу по призыву и уволенных с военной службы. </span>
    <br>С нормативными документами можно ознакомиться в разделе «Документы»</p>
   
    </div>
    </div>
    <div class="bottom"><div class="l"></div><div class="r"></div><div class="m"></div></div>
    </div>
    
    <table class="pager">
    <tr><td>
    
        <web:DataSourcePager runat="server"
            id="DataSourcePagerHead" 
            DataSourceId="dsResultsCount"
          StartRowIndexParamName="start" 
          MaxRowCountParamName="count"
          HideDefaultTemplates="true"
          AlwaysShow="true"
          DefaultMaxRowCount="20"
          DefaultMaxRowCountSource="Cookies">
            <Header>
                Результаты проверок #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
                <web:DataSourceMaxRowCount runat="server"
                    Variants="20,50,100"
                    DataSourcePagerId="DataSourcePagerHead">
                <Header>Записей на странице: </Header>
                <Footer>.</Footer>
                <Separator>, </Separator>
                <ActiveTemplate><span>#count#</span></ActiveTemplate> 
                <InactiveTemplate><a href="/SetMaxRowCount.aspx?count=#count#" 
                    title="Отображать #count# записей на странице">#count#</a></InactiveTemplate> 
                </web:DataSourceMaxRowCount>                 
            </Header>
        </web:DataSourcePager>
        
    </td>    
    <td align="right">

        <web:DataSourcePager runat="server"
            DataSourceId="DataSourcePagerHead"
          StartRowIndexParamName="start"
          MaxRowCountParamName="count">
      <PrevGroupTemplate></PrevGroupTemplate>
            <NextGroupTemplate></NextGroupTemplate>
            <FirstPageTemplate><a href="#PageUrl#"><<</a>&nbsp;</FirstPageTemplate>
            <LastPageTemplate>&nbsp;<a href="#PageUrl#">>></a></LastPageTemplate>
            <ActivePrevPageTemplate><a href="#PageUrl#"><</a>&nbsp;</ActivePrevPageTemplate>
            <ActivePageTemplate><span>#PageNo#</span> </ActivePageTemplate>
            <ActiveNextPageTemplate>&nbsp;<a href="#PageUrl#">></a></ActiveNextPageTemplate>
        </web:DataSourcePager>    

    </td></tr>
    </table>

    <style>
        #ResultContainer {overflow-x:scroll;}
        html:first-child #ResultContainer {overflow:scroll;} /* только для Opera */
        #ResultContainer td {white-space:nowrap;}
    </style>

    <div id="ResultContainer" style="width:100%; height:auto;">
    <asp:DataGrid runat="server" id="dgResultsList"
        DataSourceID="dsResultsList"
        AutoGenerateColumns="false" 
        EnableViewState="false"
        ShowHeader="True" 
        GridLines="None"
        CssClass="table-th-border table-th">
        <HeaderStyle CssClass="th" />
        <Columns>

            <asp:TemplateColumn>
            <HeaderStyle Width="20%" CssClass="left-th" />
            <HeaderTemplate>
                <div title="Номер свидетельства">Свидетельство</div>
            </HeaderTemplate>
            <ItemTemplate>
                     <asp:LinkButton ID="LinkButton1" title='<%# "Свидетельство № "+ Eval("CertificateNumber")+ " не найдено"%>' style="color:Red" Text="не найдено" runat="server" CommandName="PrintNotFound" Visible='<%#!Convert.ToBoolean(Eval("IsExist")) && Convert.ToBoolean(ConfigurationManager.AppSettings["EnableNotFoundNote"])%>' />
                        <asp:PlaceHolder ID="PlaceHolder1" Visible='<%# Convert.ToBoolean(Eval("IsExist")) || !Convert.ToBoolean(ConfigurationManager.AppSettings["EnableNotFoundNote"])%>' runat="server">
                            
                            <%#this.ShowCertificateNumber(Container.DataItem) %>
                        </asp:PlaceHolder>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <HeaderStyle Width="10%" />
            <HeaderTemplate>
                <div>ТН</div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("TypographicNumber") %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderStyle Width="10%" />
            <HeaderTemplate>
                <div>Фамилия</div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("LastName") %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderStyle Width="10%" />
            <HeaderTemplate>
                <div>Имя</div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("FirstName") %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderStyle Width="10%" />
            <HeaderTemplate>
                <div>Отчество</div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("PatronymicName")%>
            </ItemTemplate>
            </asp:TemplateColumn> 
            
            <asp:TemplateColumn>
            <HeaderStyle Width="20%" />
            <HeaderTemplate>
                <div>Документ</div>
            </HeaderTemplate>
            <ItemTemplate>
                <span title="Серия документа"><%# Eval("PassportSeria")%></span> 
                <span title="Номер документа"><%# Eval("PassportNumber")%></span> 
            </ItemTemplate>
            </asp:TemplateColumn> 
            
            <asp:TemplateColumn>
            <HeaderStyle Width="30%" />
            <HeaderTemplate>
                <div>Регион</div>
            </HeaderTemplate>
            <ItemTemplate>
               <%# Convert.ToString(Eval("RegionName")) %>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <HeaderStyle Width="10%" />
            <HeaderTemplate>
                <div>Год</div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("SourceCertificateYear") %>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <HeaderStyle Width="10%" />
            <HeaderTemplate>
                <div>Статус</div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("Status") %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                <div title="Количество уникальных проверок свидетельства ВУЗами и их филиалами">Пр</div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.IsDBNull(Eval("UniqueIHEaFCheck")) ? string.Empty : Eval("UniqueIHEaFCheck") %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                <span title="Русский язык">Р Я</span>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("RussianMark").ToString().Contains("!") ?
                        string.Format("<span style=\"color:Red\">{0}</span>", Eval("RussianMark")) 
                                        : Eval("RussianMark")%>
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
                <%# Eval("MathematicsMark").ToString().Contains("!") ?
                        string.Format("<span style=\"color:Red\">{0}</span>", Eval("MathematicsMark")) 
                                        : Eval("MathematicsMark") %>
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
                <%# Eval("PhysicsMark").ToString().Contains("!") ?
                        string.Format("<span style=\"color:Red\">{0}</span>", Eval("PhysicsMark"))
                                                            : Eval("PhysicsMark")%>
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
                <%# Eval("ChemistryMark").ToString().Contains("!") ?
                        string.Format("<span style=\"color:Red\">{0}</span>", Eval("ChemistryMark")) 
                                        : Eval("ChemistryMark")%>
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
                <%# Eval("BiologyMark").ToString().Contains("!") ?
                        string.Format("<span style=\"color:Red\">{0}</span>", Eval("BiologyMark")) 
                                        : Eval("BiologyMark") %>
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
                <%# Eval("RussiaHistoryMark").ToString().Contains("!") ?
                        string.Format("<span style=\"color:Red\">{0}</span>", Eval("RussiaHistoryMark")) 
                                        : Eval("RussiaHistoryMark") %>
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
                <%# Eval("GeographyMark").ToString().Contains("!") ?
                        string.Format("<span style=\"color:Red\">{0}</span>", Eval("GeographyMark")) 
                                        : Eval("GeographyMark") %>
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
                <%# Eval("EnglishMark").ToString().Contains("!") ?
                        string.Format("<span style=\"color:Red\">{0}</span>", Eval("EnglishMark")) 
                                        : Eval("EnglishMark") %>
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
                <%# Eval("GermanMark").ToString().Contains("!") ?
                        string.Format("<span style=\"color:Red\">{0}</span>", Eval("GermanMark")) 
                                        : Eval("GermanMark") %>
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
                <%# Eval("FranchMark").ToString().Contains("!") ?
                        string.Format("<span style=\"color:Red\">{0}</span>", Eval("FranchMark")) 
                                        : Eval("FranchMark") %>
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
                <%# Eval("SocialScienceMark").ToString().Contains("!") ?
                        string.Format("<span style=\"color:Red\">{0}</span>", Eval("SocialScienceMark")) 
                                        : Eval("SocialScienceMark") %>
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
                <%# Eval("LiteratureMark").ToString().Contains("!") ?
                        string.Format("<span style=\"color:Red\">{0}</span>", Eval("LiteratureMark")) 
                                        : Eval("LiteratureMark") %>
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
                <%# Eval("SpanishMark").ToString().Contains("!") ?
                        string.Format("<span style=\"color:Red\">{0}</span>", Eval("SpanishMark")) 
                                        : Eval("SpanishMark") %>
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
               <span title="Информатика">И</span>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("InformationScienceMark").ToString().Contains("!") ?
                        string.Format("<span style=\"color:Red\">{0}</span>", Eval("InformationScienceMark")) 
                                        : Eval("InformationScienceMark") %>
            </ItemTemplate>
            </asp:TemplateColumn>   
            
            <asp:TemplateColumn>
            <HeaderStyle CssClass="right-th" />
            <ItemStyle HorizontalAlign="Center" />
            <HeaderTemplate>
                <div title="Апелляция по информатике">А п</div>
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
        <Message><style type="text/css">#ResultContainer, #ExportContainer{display:none;}</style> 
        <p class="norecords">Осуществляется обработка файла</p></Message>
    </web:NoRecordsText>
    
    <table class="pager">
    <tr><td>
    
        <web:DataSourcePager runat="server"
            DataSourceId="DataSourcePagerHead"
          StartRowIndexParamName="start" 
          MaxRowCountParamName="count"
          HideDefaultTemplates="true"
          AlwaysShow="true"
          DefaultMaxRowCount="20"
          DefaultMaxRowCountSource="Cookies">
            <Header>
                Результаты проверок #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
                <web:DataSourceMaxRowCount runat="server"
                    Variants="20,50,100"
                    DataSourcePagerId="DataSourcePagerHead">
                <Header>Записей на странице: </Header>
                <Footer>.</Footer>
                <Separator>, </Separator>
                <ActiveTemplate><span>#count#</span></ActiveTemplate> 
                <InactiveTemplate><a href="/SetMaxRowCount.aspx?count=#count#" 
                    title="Отображать #count# записей на странице">#count#</a></InactiveTemplate> 
                </web:DataSourceMaxRowCount>                 
            </Header>
        </web:DataSourcePager>
        
    </td>    
    <td align="right">

        <web:DataSourcePager runat="server"
            DataSourceId="DataSourcePagerHead"
          StartRowIndexParamName="start"
          MaxRowCountParamName="count">
      <PrevGroupTemplate></PrevGroupTemplate>
            <NextGroupTemplate></NextGroupTemplate>
            <FirstPageTemplate><a href="#PageUrl#"><<</a>&nbsp;</FirstPageTemplate>
            <LastPageTemplate>&nbsp;<a href="#PageUrl#">>></a></LastPageTemplate>
            <ActivePrevPageTemplate><a href="#PageUrl#"><</a>&nbsp;</ActivePrevPageTemplate>
            <ActivePageTemplate><span>#PageNo#</span> </ActivePageTemplate>
            <ActiveNextPageTemplate>&nbsp;<a href="#PageUrl#">></a></ActiveNextPageTemplate>
        </web:DataSourcePager>    

    </td></tr>
    </table>

    <asp:SqlDataSource  runat="server" ID="dsResultsList" 
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="dbo.SearchCommonNationalExamCertificateRequest"  CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <fbs:CurrentUserParameter Name="login" Type="String" />
            <asp:QueryStringParameter Name="batchId" QueryStringField="id" Type="Int64" />
            <asp:Parameter Name="isExtended" Type="Boolean" DefaultValue="true" />
            <asp:QueryStringParameter DefaultValue="0" Name="startRowIndex" Type="String" QueryStringField="start" />
            <web:MaxRowCountParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count"
                CheckParamName="count" CheckParamSource="Cookies" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource  runat="server" ID="dsResultsCount" 
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="dbo.SearchCommonNationalExamCertificateRequest"  CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <fbs:CurrentUserParameter Name="login" Type="String" />
            <asp:QueryStringParameter Name="batchId" QueryStringField="id" Type="Int64" />
            <asp:Parameter Name="isExtended" Type="Boolean" DefaultValue="true" />
            <asp:QueryStringParameter DefaultValue="1" Name="startRowIndex" Type="String" QueryStringField="start" />
            <asp:QueryStringParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count" />
            <asp:Parameter DefaultValue="true" Name="showCount" Type="Boolean" />
        </SelectParameters>
    </asp:SqlDataSource>
    
    <script type="text/javascript">
        InitNotice();
    </script>
    
    <script runat="server">
        private static string GetCheckLink(string number, string lastName, string participantId, string year)
        {
            return
                string.Format(
                    "<a href=\"/Certificates/CommonNationalCertificates/CheckResult.aspx?number={0}&LastName={1}&participantID={2}&year={4}\">{3}</a>",
                    number,
                    lastName,
                    participantId,
                    string.IsNullOrEmpty(number) ? "Нет свидетельства" : number,
                    year
                );
        }

        private string ShowCertificateNumber(object dataItem)
        {
            var dataRow = (System.Data.DataRowView)dataItem;
            string lastName = string.IsNullOrEmpty(dataRow["LastName"].ToString()) ? dataRow["CheckLastName"].ToString() : dataRow["LastName"].ToString();
            string rawNumber = dataRow["CertificateNumber"].ToString();
            string number = rawNumber.Equals("Нет свидетества") || rawNumber.Equals("Нет свидетельства") ? "" : rawNumber;
            string denyComment = dataRow["DenyComment"].ToString();
            string newNumber = dataRow["DenyNewCertificateNumber"].ToString();
            string participantId = dataRow["ParticipantID"].ToString();
            string year = dataRow["SourceCertificateYear"].ToString();
            
            string numberString = GetCheckLink(number, lastName, participantId, year);
            if (!Convert.ToBoolean(dataRow["IsExist"]))
            {
                //string notFoundLink = String.Format("PrintNotFoundNote.aspx?Series={0}&Number={1}&check=byPassportName&GivenName={2}&FirstName={3}&LastName={4}&Year={5}", dataRow["PassportSeria"], dataRow["PassportNumber"],  dataRow["PatronymicName"], dataRow["FirstName"], lastName, this.Request.QueryString["year"]);
                return
                         string.Format(
                             "<span title='Свидетельство №{0} не найдено' style=\"color:Red\">не&nbsp;найдено</span><br/>{1}",
                             number,
                             number);
            }
            string newNumberString = GetCheckLink(newNumber, lastName, participantId, year);
            if (Convert.ToBoolean(dataRow["IsDeny"]))
                return string.Format("<span title='Свидетельство №{0} аннулировано по следующей причине:\n{1}' style=\"color:Red\">аннулировано<br/>{3}{2}</span>",
                    number,
                    denyComment,
                    string.IsNullOrEmpty(newNumber) ? "" : string.Format("<br/>актуальное<br/>{0}", newNumberString),
                    numberString
                );
            return numberString;
        }    
    </script>
</form>
</asp:Content>