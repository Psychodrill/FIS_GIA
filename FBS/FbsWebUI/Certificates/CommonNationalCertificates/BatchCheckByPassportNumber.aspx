<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchCheckByPassportNumber.aspx.cs"
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.BatchCheckByPassportNumber"
    MasterPageFile="~/Common/Templates/Certificates.Master" %>

<%@ Import Namespace="Fbs.Core.Organizations" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Controls" Assembly="FbsWebUI" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server">
    <script type="text/javascript" src="/Common/Scripts/Utils.js"> </script>
    <script type="text/javascript" src="/Common/Scripts/SessVars.js"> </script>
    <script type="text/javascript" src="/Common/Scripts/Notice.js"> </script>
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
                <li><a href="<%=this.Request.Url.PathAndQuery%>" title="Обновить страницу">Обновить
                    результаты</a></li>
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
<asp:Content runat="server" ContentPlaceHolderID="cphCertificateContent">
    <form runat="server" enctype="multipart/form-data">
    <asp:ValidationSummary runat="server" DisplayMode="BulletList" EnableClientScript="false"
        HeaderText="<p>Произошли следующие ошибки:</p>" />
    <table class="form f600">
        <tr>
            <td>
                <div class="notice" id="BatchCheckSampleNotice">
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
                                <b>Пример файла:</b> (<a href="/Shared/BatchCheckFileFormatByPassportSample.csv"
                                    title="скачать файл в формате csv">скачать файл</a>)
                                <pre>
<fbs:FileView ID="FileView1" runat="server" FilePath="/Shared/BatchCheckFileFormatByPassportSample.csv" /></pre>
                            </p>
                            <p>
                                <b>Формат файла:</b><br />
                                <pre>
                        &lt;Серия документа&gt;%&lt;Номер документа&gt;%&lt;Русский язык&gt;%&lt;Математика&gt;%&lt;Физика&gt;%&lt;Химия&gt;%&lt;Биология&gt;%&lt;История России&gt;%&lt;География&gt;%&lt;Английский язык&gt;%&lt;Немецкий язык&gt;%&lt;Французский язык&gt;%&lt;Обществознание&gt;%&lt;Литература&gt;%&lt;Испанский язык&gt;%&lt;Информатика&gt;
                        </pre>
                            </p>
                            <p>
                                Поиск и проверка свидетельств осуществляются по строгому соответствию параметров
                                запроса «Серия документа», «Номер документа» и баллов по предметам, хранящимся в
                                Подсистеме ФИС &laquo;Результаты ЕГЭ&raquo;. Поле «Номер документа» обязательно для заполнения.</p>
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
                <p>
                    Укажите путь к файлу с данными для проверки</p>
                <asp:FileUpload ID="fuData" runat="server" CssClass="content-in wlong" />
                <br />
                <asp:DropDownList ID="ddlYear" runat="server">
                    <asp:ListItem Selected="True" Value="">Поиск по всем годам</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="box-submit">
                <asp:Button ID="StartParseBtn" runat="server" Text="Проверить" OnClick="StartParseBtn_Click" />
            </td>
        </tr>
        <asp:PlaceHolder runat="server" ID="FileErrorMsg" Visible="False">
            <tr>
                <td>
                    <asp:Label ID="FileErrorMsg1" Visible="false" runat="server" Text="" Font-Size="Smaller"
                        ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </asp:PlaceHolder>
    </table>
    <asp:RequiredFieldValidator runat="server" ControlToValidate="fuData" EnableClientScript="false"
        Display="None" ErrorMessage="Не выбран файл для загрузки" />
    <asp:CustomValidator ID="cvFileEmpty" runat="server" ControlToValidate="fuData" ValidateEmptyText="false"
        EnableClientScript="false" Display="None" OnServerValidate="cvFileEmpty_ServerValidate"
        ErrorMessage="Вы загрузили пустой файл. Пожалуйста, укажите сведения хотя бы об одном свидетельстве о результатах ЕГЭ!" />
    <asp:CustomValidator ID="cvFileTooLarge" runat="server" ControlToValidate="fuData"
        ValidateEmptyText="false" EnableClientScript="false" Display="None" OnServerValidate="cvFileTooLarge_ServerValidate"
        ErrorMessage="Вы пытаетесь загрузить слишком большой файл. Максимальный размер файла {0} мегабайт." />
    <asp:CustomValidator ID="cvFileFormat" runat="server" ControlToValidate="fuData"
        ValidateEmptyText="false" EnableClientScript="false" Display="None" OnServerValidate="cvFileFormat_ServerValidate"
        ErrorMessage="Ошибка в данных файла. Строка {0}." EnableViewState="false" />
    <style>
        #ResultContainer
        {
            overflow-x: auto;
        }
        html:first-child #ResultContainer
        {
            overflow: auto;
        }
        /* только для Opera */
        #ResultContainer td
        {
            white-space: nowrap;
        }
    </style>
    <div id="ResultContainer" style="width: 100%; height: auto; margin-bottom: 10px;">
        <asp:PlaceHolder runat="server" ID="plhParseFileError">
        <div style="margin-bottom: 8px; color: red;">При проверке файла обнаружены ошибки:</div>
        </asp:PlaceHolder>
        <asp:DataGrid runat="server" ID="dgResultsList" OnPageIndexChanged="dgResultsList_PageIndexChanged"
            AutoGenerateColumns="false" EnableViewState="true" ShowHeader="True" GridLines="None"
            CssClass="table-th-border table-th" Visible="false" Width="99%">
            <HeaderStyle CssClass="th" />
            <Columns>
                <asp:TemplateColumn>
                    <HeaderStyle CssClass="left-th" />
                    <HeaderTemplate>
                        <div title="Номер строки">
                            №</div>
                    </HeaderTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("RowIndex"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <div title="Номер свидетельства">
                            Cв-во</div>
                    </HeaderTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <div title="Типографский номер">
                            ТН</div>
                    </HeaderTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <span title="Серия паспорта">СП</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Серия паспорта"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <span title="Номер паспорта">НП</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Номер паспорта"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <span title="Регион">Регион</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <span title="Год выдачи свидетельства">Год</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderTemplate>
                        <span title="Статус">Статус</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Русский язык">РЯ</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Русский язык"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по русскому языку">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Апелляция по русскому языку"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Математика">М</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Математика"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по математике">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Апелляция по математике"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Физика">Ф</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Физика"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по физике">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Апелляция по физике"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Химия">Х</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Химия"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по химии">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Апелляция по химии"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Биология">Б</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Биология"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по биологии">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Апелляция по биологии"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="История России">ИР</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("История России"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по истории России">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Апелляция по истории России"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="География">Г</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("География"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по географии">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Апелляция по географии"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Английский язык">АЯ</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Английский язык"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по английскому языку">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Апелляция по английскому языку"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Немецкий язык">НЯ</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Немецкий язык"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по немецкому языку">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Апелляция по немецкому языку"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Французский язык">ФЯ</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Французский язык"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по французскому языку">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Апелляция по французскому языку"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Обществознание">О</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Обществознание"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по обществознанию">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Апелляция по обществознанию"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Литература">Л</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Литература"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по литературе">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Апелляция по литературе"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Испанский язык">ИЯ</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Испанский язык"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Апелляция по испанскому языку">Ап</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Апелляция по испанскому языку"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <span title="Информатика">И</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Информатика"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <div title="Апелляция по информатике">
                            Ап</div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Апелляция по информатике"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle CssClass="right-th" />
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderTemplate>
                        <div title="Комментарий">
                            Комментарий</div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# HighlightValue(this.Eval("Комментарий"))%>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </div>
    <h1>
        История проверок</h1>
    <table class="pager">
        <tr>
            <td>
                <web:DataSourcePager runat="server" ID="DataSourcePagerHead" DataSourceId="dsUserListCount"
                    StartRowIndexParamName="start" MaxRowCountParamName="count" HideDefaultTemplates="true"
                    AlwaysShow="true" DefaultMaxRowCount="10" DefaultMaxRowCountSource="Cookies">
                    <Header>
                        Результаты проверок #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                        <web:DataSourceMaxRowCount runat="server" Variants="10,20" DataSourcePagerId="DataSourcePagerHead">
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
    <asp:DataGrid runat="server" ID="dgUserList" DataSourceID="dsUserList" AutoGenerateColumns="false"
        EnableViewState="false" ShowHeader="True" GridLines="None" CssClass="table-th f600"
        OnItemDataBound="DataGridOnItemDataBound">
        <HeaderStyle CssClass="th" />
        <Columns>
            <asp:TemplateColumn>
                <HeaderStyle Width="15%" CssClass="left-th" />
                <HeaderTemplate>
                    <div>
                        Дата</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Convert.ToString(this.Eval("CreateDate"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="20%" />
                <HeaderTemplate>
                    <div>
                        Пользователь</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <% if (this.User.IsInRole("EditAdministratorAccount"))
                       { %>
                    <a href="/Administration/Accounts/Users/View.aspx?login=<%# this.Eval("Login")%>"
                        title="Посмотреть профиль">
                        <%# this.Eval("Login")%></a>
                    <% }
                       else
                       { %>
                    <%# this.Eval("Login") %>
                    <% } %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="30%" />
                <HeaderTemplate>
                    <div>
                        Статус</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%#
    Convert.ToBoolean(this.Eval("IsProcess"))
        ? "на обработке"
        : string.Format("найдено {0} из {1}", this.Eval("Found"), this.Eval("Total")) %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="15%" />
                <ItemStyle HorizontalAlign="Center" />
                <HeaderTemplate>
                    <div>
                        Результат</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%#GetResult(Convert.ToBoolean(this.Eval("IsProcess")), this.Eval("Id")) %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <%-- Удалить элемент--%>
            <asp:TemplateColumn>
                <HeaderStyle CssClass="right-th" HorizontalAlign="Center" Width="20%" />
                <HeaderTemplate>
                    <div>
                        Удалить</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="lbDeleteCheck" runat="server" CommandArgument='<%# this.Eval("Id") %>'
                        OnCommand="DeleteCheck"><%# GetText(this.Eval("Login").ToString())%></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    <web:NoRecordsText runat="server" ControlId="dgUserList">
        <Message>
            <div class="notfound w600">
                <img src="/Common/Images/spacer.gif" width="32" height="31" style="background-image: url('/Common/Images/important.gif')" />
                Не найдено ни одной записи</div>
        </Message>
    </web:NoRecordsText>
    <table class="pager">
        <tr>
            <td>
                <web:DataSourcePager runat="server" DataSourceId="DataSourcePagerHead" StartRowIndexParamName="start"
                    MaxRowCountParamName="count" HideDefaultTemplates="true" AlwaysShow="true" DefaultMaxRowCount="10"
                    DefaultMaxRowCountSource="Cookies">
                    <Header>
                        Результаты проверок #StartRowIndex#-#LastRowIndex# из #TotalCount#.
                        <web:DataSourceMaxRowCount runat="server" Variants="10,20" DataSourcePagerId="DataSourcePagerHead">
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
    <script type="text/javascript">
        InitNotice();
    </script>
    <asp:SqlDataSource runat="server" ID="dsUserList" ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="SearchCommonNationalExamCertificateByNumberCheckBatch" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure">
        <SelectParameters>
            <fbs:CurrentUserParameter Name="login" Type="String" />
            <asp:QueryStringParameter DefaultValue="0" Name="startRowIndex" Type="String" QueryStringField="start" />
            <web:MaxRowCountParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count"
                CheckParamName="count" CheckParamSource="Cookies" />
            <asp:Parameter DefaultValue="false" Name="showCount" Type="Boolean" />
            <asp:Parameter DefaultValue="2" Name="type" Type="Int16" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource runat="server" ID="dsUserListCount" ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="SearchCommonNationalExamCertificateByNumberCheckBatch" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure">
        <SelectParameters>
            <fbs:CurrentUserParameter Name="login" Type="String" />
            <asp:QueryStringParameter DefaultValue="0" Name="startRowIndex" Type="String" QueryStringField="start" />
            <asp:QueryStringParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count" />
            <asp:Parameter DefaultValue="true" Name="showCount" Type="Boolean" />
            <asp:Parameter DefaultValue="2" Name="type" Type="Int16" />
        </SelectParameters>
    </asp:SqlDataSource>
    </form>
</asp:Content>
<script runat="server" type="text/C#">

    private static string GetText(string login)
    {
        string res = string.Empty;

        Organization org = OrganizationDataAccessor.GetByLogin(login);

        if (org != null && org.DisableLog)
        {
            res = "удалить";
        }

        return res;
    }


    static string HighlightValue(object valueObj)
    {
        string value = Convert.ToString(valueObj);
        if (value.Contains("[НЕВЕРЕН ФОРМАТ]"))
            return String.Format(
                "<span \" style=\"color:Red\">{0}</span>", value);

        return value;
    }

</script>
