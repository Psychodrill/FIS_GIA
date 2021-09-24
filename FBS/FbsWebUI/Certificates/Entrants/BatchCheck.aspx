<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchCheck.aspx.cs" 
    Inherits="Fbs.Web.Certificates.Entrants.BatchCheck" 
    MasterPageFile="~/Common/Templates/Certificates.Master" %>
    
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Controls" Assembly="FbsWebUI" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>

<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server">
    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphCertificateActions">
    <div class="h10"></div>
    <div class="border-block">
        <div class="tr"><div class="tt"><div></div></div></div>
        <div class="content">
        <ul>
            <li><a href="<%= Request.Url.PathAndQuery %>" 
                title="Обновить страницу">Обновить результаты</a></li>
        </ul>
        </div>
        <div class="br"><div class="tt"><div></div></div></div>
    </div>    
</asp:Content>

<asp:Content ContentPlaceHolderID="cphCertificateContent" runat="server">

    <form runat="server" enctype="multipart/form-data">
    
        <asp:ValidationSummary runat="server" DisplayMode="BulletList" EnableClientScript="false"
            HeaderText="<p>Произошли следующие ошибки:</p>"/>
            
        <table class="form f600">
            <tr><td>
                <div class="notice" id="EntrantsBatchCheckSampleNotice">
                <div class="top"><div class="l"></div><div class="r"></div><div class="m"></div></div>
                <div class="cont">
                <div dir="ltr" class="hide" title="Свернуть" onclick="ToggleNoticeState(this);">x<span></span></div>
                <div class="txt-in">
                    <p>
                        <b>Пример файла:</b> 
                        (<a href="/Shared/EntrantsBatchCheckFileSample.txt" 
                            title="скачать файл в формате csv">скачать файл</a>)
                        <pre><fbs:FileView runat="server" 
                            FilePath="/Shared/EntrantsBatchCheckFileSample.txt" /></pre>
                    </p>
                    <p>
                        <b>Формат файла:</b><br />
                        <pre>
                        &lt;номер свидетельства&gt;
                        </pre>
                    </p>
                </div>
                </div>
                <div class="bottom"><div class="l"></div><div class="r"></div><div class="m"></div></div>
                </div>

                <p>Укажите путь к файлу с данными для проверки</p>
                <asp:FileUpload ID="fuData" runat="server" CssClass="content-in wlong" />
            </td></tr>
            <tr><td class="box-submit">
		        <input type="submit" value="Проверить" class="bt" />
		    </td></tr>
        </table> 
        
        <script type="text/javascript">
            InitNotice();
        </script>
        
        <asp:RequiredFieldValidator runat="server" ControlToValidate="fuData" 
            EnableClientScript="false" Display="None"
            ErrorMessage="Не выбран файл для загрузки" /> 
            
        <asp:CustomValidator ID="cvFileEmpty" runat="server"
            ControlToValidate="fuData" ValidateEmptyText="false"
            EnableClientScript="false" Display="None"
            OnServerValidate="cvFileEmpty_ServerValidate"
            ErrorMessage="Вы пытаетесь загрузить пустой файл" />

        <asp:CustomValidator ID="cvFileFormat" runat="server"
            ControlToValidate="fuData" ValidateEmptyText="false"        
            EnableClientScript="false" Display="None"
            OnServerValidate="cvFileFormat_ServerValidate"
            ErrorMessage="Ошибка в данных файла. Строка {0}." EnableViewState="false" />
            
        <table class="pager">
        <tr><td>
    
            <web:DataSourcePager runat="server"
                id="DataSourcePagerHead" 
                DataSourceId="dsBatchListCount"
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

        <asp:DataGrid runat="server" id="dgBatchList"
            DataSourceID="dsBatchList"
            AutoGenerateColumns="false" 
            EnableViewState="false"
            ShowHeader="True" 
            GridLines="None"
            CssClass="table-th f600">
            <HeaderStyle CssClass="th" />
            <Columns>
                <asp:TemplateColumn>
                <HeaderStyle Width="25%" CssClass="left-th" />
                <HeaderTemplate>
                    <div>Дата</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Convert.ToString(Eval("CreateDate")) %>
                </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn>
                <HeaderStyle Width="25%" />
                <HeaderTemplate>
                    <div>Статус</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# GetStatus(Convert.ToBoolean(Eval("IsProcess")), Eval("IsCorrect")) %>
                </ItemTemplate>
                </asp:TemplateColumn>
                
                <asp:TemplateColumn>
                <HeaderStyle Width="15%" CssClass="right-th" />
                <ItemStyle HorizontalAlign="Center" />
                <HeaderTemplate>
                    <div>Результат</div>
                </HeaderTemplate>
                <ItemTemplate>
                   <%# GetResult(Convert.ToBoolean(Eval("IsProcess")), Eval("Id")) %>
                </ItemTemplate>
                </asp:TemplateColumn>
                
            </Columns>
        </asp:DataGrid>

        <web:NoRecordsText runat="server" ControlId="dgBatchList">
            <Message><div class="notfound w600">Не найдено ни одной записи</div></Message>
        </web:NoRecordsText>

        <web:DataSourcePager runat="server" 
            DataSourceId="dsBatchListCount"
	        StartRowIndexParamName="start" 
	        MaxRowCountParamName="count"
	        DefaultMaxRowCount="20">
            <Header>
                <table class="pager f600"><tr>
                    <td>Результаты проверок #StartRowIndex#-#LastRowIndex# из #TotalCount#</td>
                    <td align="right">
            </Header>
            <Footer></td></tr></table></Footer>
			<PrevGroupTemplate></PrevGroupTemplate>
            <NextGroupTemplate></NextGroupTemplate>
            <FirstPageTemplate><a href="#PageUrl#"><<</a>&nbsp;</FirstPageTemplate>
            <LastPageTemplate>&nbsp;<a href="#PageUrl#">>></a></LastPageTemplate>
            <ActivePrevPageTemplate><a href="#PageUrl#"><</a>&nbsp;</ActivePrevPageTemplate>
            <ActivePageTemplate><span>#PageNo#</span> </ActivePageTemplate>
            <ActiveNextPageTemplate>&nbsp;<a href="#PageUrl#">></a></ActiveNextPageTemplate>
        </web:DataSourcePager>
        
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

        <script type="text/javascript">
            InitNotice();
        </script>

        <asp:SqlDataSource runat="server" ID="dsBatchList" 
            ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
            SelectCommand="dbo.SearchEntrantCheckBatch" CancelSelectOnNullParameter="false"
            SelectCommandType="StoredProcedure"> 
            <SelectParameters>
                <fbs:CurrentUserParameter Name="login" Type="String" />
                <asp:QueryStringParameter DefaultValue="0" Name="startRowIndex" Type="String" QueryStringField="start" />
                <web:MaxRowCountParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count"
                    CheckParamName="count" CheckParamSource="Cookies" />
                <asp:Parameter DefaultValue="false" Name="showCount" Type="Boolean" />
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource runat="server" ID="dsBatchListCount" 
            ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
            SelectCommand="dbo.SearchEntrantCheckBatch" CancelSelectOnNullParameter="false"
            SelectCommandType="StoredProcedure"> 
            <SelectParameters>
                <fbs:CurrentUserParameter Name="login" Type="String" />
                <asp:QueryStringParameter DefaultValue="0" Name="startRowIndex" Type="String" QueryStringField="start" />
                <asp:QueryStringParameter DefaultValue="20" Name="maxRowCount" Type="String" QueryStringField="count" />
                <asp:Parameter DefaultValue="true" Name="showCount" Type="Boolean" />
            </SelectParameters>
        </asp:SqlDataSource>
    </form>
</asp:Content>

<script runat="server" type="text/C#">
    string GetStatus(bool isProcess, object isCorrectObj)
    {
        bool isCorrect = false;

        if (Convert.IsDBNull(isCorrectObj)) 
            isCorrect = false;
        else
            isCorrect = Convert.ToBoolean(isCorrectObj);

        return isProcess ? "на обработке" : (isCorrect ? "выполнен" : "выполнен с ошибкой");
    }

    string GetResult(bool isProcess, object id)
    {
        string enable = String.Format(
                "<a href=\"/Certificates/Entrants/BatchCheckResult.aspx?id={0}\">результат</a>", id);
        string disable = string.Empty;
        
        return isProcess ? disable : enable;
    }
</script>