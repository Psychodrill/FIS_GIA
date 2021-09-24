<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/Certificates.Master" AutoEventWireup="true" CodeBehind="BatchRequestByPassportResultObsolete.aspx.cs" Inherits="Fbs.Web.Certificates.CommonNationalCertificates.BatchRequestByPassportResultObsolete" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>

<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server">
    <% if (BatchWait) { %>
     <meta http-equiv="refresh" content="10" />
<% } else { %>

     <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>
    

<% } %> 

</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphCertificateActions">
    <asp:Panel runat="server" ID="exportPanel">
    
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
                    <a href="/Certificates/CommonNationalCertificates/BatchCheckResultExportCsvObsolete.aspx?id=<%= CheckId %>"
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
    </asp:Panel>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphCertificateContent" runat="server">
    <% if (BatchWait) { %>
<p><i>Идет обработка пакета, пожалуйста подождите...</i></p>
    
<% } else { %>

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
                <p>ТН - Типографский номер; Год - Год получения результата; РЯ - русский язык; М - математика; Ф - физика; Х - химия; Б - биология; ИР - история России; Г - география; АЯ - английский язык; НЯ - немецкий язык; ФЯ - французский язык; О - обществознание; Л - литература; ИЯ - испанский язык; И - информатика.
                <br />
Если не удается найти результаты участника ЕГЭ или свидетельство аннулировано без объяснения причины, то обращайтесь за информацией в Региональный Центр Обработки Информации, находящийся на территории региона, в котором было выдано данное свидетельство.
<br />
Срок действия результатов ЕГЭ определяется в соответствии с действующими нормативными документами.
<br />
В 2015 году действительны результаты ЕГЭ 2012, 2013, 2014 и 2015 годов.
<br />
Результаты 2011 года действительны только для лиц, проходивших военную службу по призыву и уволенных с военной службы.
<br />
Специальным знаком &laquo;!&raquo; перед баллом выделены баллы, которые меньше минимальных установленных Рособрнадзором значений.
<br />
Аннулированные и недействительные результаты ЕГЭ отображаются как 0 баллов.
                    </p>
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

<style>
        #ResultContainer {overflow-x:scroll;}
        html:first-child #ResultContainer {overflow:scroll;} /* только для Opera */
        #ResultContainer td {white-space:nowrap;}
    </style>
    
    <form id="Form1" runat="server">
        <table class="pager">
            <tr><td>
    
                <web:DataSourcePager runat="server"
                    id="DataSourcePagerHead" 
                    DataSourceId="resultCountDataSource"
	                StartRowIndexParamName="start" 
	                MaxRowCountParamName="count"
	                HideDefaultTemplates="true"
	                AlwaysShow="true"
	                DefaultMaxRowCount="10"
	                DefaultMaxRowCountSource="Cookies">
                    <Header>
                        Участники #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
                        <web:DataSourceMaxRowCount ID="DataSourceMaxRowCount1" runat="server"
                            Variants="10,20,50"
                            DataSourcePagerId="DataSourcePagerHead">
                        <Header>Участников на странице: </Header>
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

                <web:DataSourcePager ID="DataSourcePager1" runat="server"
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
    <div id="ResultContainer">
    <asp:GridView runat="server" ID="dsMaster" AllowPaging="False" 
        AutoGenerateColumns="False" CssClass="table-th" GridLines="None" EnableViewState="False">
        <HeaderStyle CssClass="th"></HeaderStyle>
        <Columns>
            <%--Свидетельство--%>
            <asp:TemplateField>
                <HeaderStyle CssClass="left-th"></HeaderStyle>
                <HeaderTemplate>
                    <div>Свидетельство</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# RenderLicenseNumber(Container) %>
                </ItemTemplate>
            </asp:TemplateField>
            <%--Типографский номер--%>
            <asp:TemplateField HeaderText="ТН">
                <ItemTemplate>
                    <%# RenderTypographicNumber(Container) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Фамилия">
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# GetDetailsPageUrl(Eval("GroupId")) %>' Text='<%# Eval("Surname") %>'></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Имя">
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# GetDetailsPageUrl(Eval("GroupId")) %>' Text='<%# Eval("Name") %>'></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Отчество">
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# GetDetailsPageUrl(Eval("GroupId")) %>' Text='<%# Eval("SecondName") %>'></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Документ">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container, "DataItem.DocumentSeries") %>&nbsp;&nbsp;<%# DataBinder.Eval(Container, "DataItem.DocumentNumber") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="UseYear" HeaderText="Год" />
            <asp:BoundField DataField="StatusName" HeaderText="Статус" />
            <%--Предметы--%>
            <%--1	Русский язык--%>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Русский язык">РЯ</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectMark(Container, "sbj1") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Апелляция по Русскому языку">Ап</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectAppeal(Container, "sbj1ap") %>
                </ItemTemplate>
            </asp:TemplateField>
            <%--2	Математика--%>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Математика">М</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectMark(Container, "sbj2") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Апелляция по Математике">Ап</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectAppeal(Container, "sbj2ap") %>
                </ItemTemplate>
            </asp:TemplateField>
            <%--3	Физика--%>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Физика">Ф</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectMark(Container, "sbj3") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Апелляция по Физике">Ап</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectAppeal(Container, "sbj3ap") %>
                </ItemTemplate>
            </asp:TemplateField>
            <%--4	Химия--%>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Химия">Х</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectMark(Container, "sbj4") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Апелляция по Химии">Ап</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectAppeal(Container, "sbj4ap") %>
                </ItemTemplate>
            </asp:TemplateField>
            <%--6	Биология--%>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Биология">Б</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectMark(Container, "sbj6") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Апелляция по Биологии">Ап</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectAppeal(Container, "sbj6ap") %>
                </ItemTemplate>
            </asp:TemplateField>
            <%--7	История--%>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="История">И</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectMark(Container, "sbj7") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Апелляция по Истории">Ап</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectAppeal(Container, "sbj7ap") %>
                </ItemTemplate>
            </asp:TemplateField>
            <%--8	География--%>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="География">Г</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# RenderSubjectMark(Container, "sbj8") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Апелляция по Географии">Ап</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectAppeal(Container, "sbj8ap") %>
                </ItemTemplate>
            </asp:TemplateField>
            <%--9	Английский язык--%>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Английский язык">АЯ</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectMark(Container, "sbj9") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Апелляция по Английскому языку">Ап</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectAppeal(Container, "sbj9ap") %>
                </ItemTemplate>
            </asp:TemplateField>
            <%--10	Немецкий язык--%>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Немецкий язык">НЯ</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectMark(Container, "sbj10") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Апелляция по Немецкому языку">Ап</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectAppeal(Container, "sbj10ap") %>
                </ItemTemplate>
            </asp:TemplateField>
            <%--11	Французский язык--%>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Французский язык">ФЯ</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectMark(Container, "sbj11") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Апелляция по Французскому языку">Ап</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectAppeal(Container, "sbj11ap") %>
                </ItemTemplate>
            </asp:TemplateField>
            <%--12	Обществознание--%>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Обществознание">О</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectMark(Container, "sbj12") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Апелляция по Обществознанию">Ап</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectAppeal(Container, "sbj12ap") %>
                </ItemTemplate>
            </asp:TemplateField>
            <%--18	Литература--%>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Литература">Л</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectMark(Container, "sbj18") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Апелляция по Литературе">Ап</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectAppeal(Container, "sbj18ap") %>
                </ItemTemplate>
            </asp:TemplateField>
            <%--13	Испанский язык--%>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Испанский язык">ИЯ</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectMark(Container, "sbj13") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Апелляция по Испанскому языку">Ап</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectAppeal(Container, "sbj13ap") %>
                </ItemTemplate>
            </asp:TemplateField>
            <%--5	Информатика и ИКТ--%>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Информатика и ИКТ">ИКТ</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectMark(Container, "sbj5") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Апелляция по Информатике и ИКТ">Ап</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectAppeal(Container, "sbj5ap") %>
                </ItemTemplate>
            </asp:TemplateField>

            <%--20	Сочинение--%>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Сочинение">СЧ</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectMark(Container, "sbj20") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Апелляция по Сочинению">Ап</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectAppeal(Container, "sbj20ap") %>
                </ItemTemplate>
            </asp:TemplateField>
            <%--21	Изложение--%>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Изложение">ИЗ</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectMark(Container, "sbj21") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Апелляция по Изложению">Ап</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectAppeal(Container, "sbj21ap") %>
                </ItemTemplate>
            </asp:TemplateField>
            <%--22	Математика базовая--%>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Математика базовая">МБ</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectMark(Container, "sbj22") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Апелляция по Математике базовой">Ап</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectAppeal(Container, "sbj22ap") %>
                </ItemTemplate>
            </asp:TemplateField>
            <%--29	Английский язык (устный)--%>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Английский язык (устный)">АЯУ</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectMark(Container, "sbj29") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Апелляция по Английскому языку (устному)">Ап</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectAppeal(Container, "sbj29ap") %>
                </ItemTemplate>
            </asp:TemplateField>
            <%--30	Немецкий язык (устный)--%>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Немецкий язык (устный)">НЯУ</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectMark(Container, "sbj30")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Апелляция по Немецкому языку (устному)">Ап</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectAppeal(Container, "sbj30ap")%>
                </ItemTemplate>
            </asp:TemplateField>
            <%--31	Французский язык (устный)--%>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Французcкий язык (устный)">ФЯУ</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectMark(Container, "sbj31")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Апелляция по Французcкому языку (устному)">Ап</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectAppeal(Container, "sbj31ap")%>
                </ItemTemplate>
            </asp:TemplateField>
            <%--33	Испанский язык (устный)--%>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Испанский язык (устный)">ИЯУ</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectMark(Container, "sbj33")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Апелляция по Испанскому языку (устному)">Ап</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# RenderSubjectAppeal(Container, "sbj33ap")%>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField>
                <HeaderStyle CssClass="right-th" HorizontalAlign="Center"></HeaderStyle>
                <HeaderTemplate>
                    <div title="Число проверок ВУЗами и их филиалами">П</div>
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <%# DataBinder.Eval(Container, "DataItem.UniqueChecks") %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        
    </asp:GridView>
    </div>
    
    <table class="pager">
        <tr><td>
    
            <web:DataSourcePager ID="DataSourcePager2" runat="server"
                DataSourceId="DataSourcePagerHead"
	            StartRowIndexParamName="start" 
	            MaxRowCountParamName="count"
	            HideDefaultTemplates="true"
	            AlwaysShow="true"
	            DefaultMaxRowCount="10"
	            DefaultMaxRowCountSource="Cookies">
                <Header>
                    Участники #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                
                    <web:DataSourceMaxRowCount ID="DataSourceMaxRowCount2" runat="server"
                        Variants="10,20,50"
                        DataSourcePagerId="DataSourcePagerHead">
                    <Header>Участников на странице: </Header>
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

            <web:DataSourcePager ID="DataSourcePager3" runat="server"
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
    
    <web:NoRecordsText runat="server" ControlId="dsMaster">
        <asp:Panel runat="server">
            <p id="noresulttext" align="center">
                <img src="/Common/Images/spacer.gif" width="32" height="31" style="background-image: url('/Common/Images/important.gif')" />
                По Вашему запросу ничего не найдено
             </p>
             <script type="text/javascript">
                 if ($('#noresulttext').is(':visible'))
                 $('#ResultContainer').hide();
             </script>
           <%--<asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" NavigateUrl='<%# GenerateNotFoundPrintLink() %>' Visible='<%# Convert.ToBoolean(ConfigurationManager.AppSettings["EnableNotFoundNote"])%>' Text="Распечатать" />--%>
        </asp:Panel>
    </web:NoRecordsText>
    
    <asp:ObjectDataSource runat="server" ID="resultCountDataSource" />

    </form>

<% } %>

</asp:Content>