<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestByPassportResult.aspx.cs"
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.RequestByPassportResult"
    MasterPageFile="~/Common/Templates/Certificates.Master" %>

<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<%@ Import Namespace="Fbs.Web" %>

<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server">
    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphCertificateContent" runat="server">
    <div class="notice" id="RequestByPassportResult">
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
                    Если не удается найти результаты участника ЕГЭ или свидетельство аннулировано без
                    объяснения причины, то обращайтесь за информацией в Региональный Центр Обработки
                    Информации, находящийся на территории региона, в котором было выдано данное свидетельство.</p>
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

    <form runat="server">
    <style>
        #ResultContainer { overflow-x: auto; }
        html:first-child #ResultContainer { overflow: auto; } /* только для Opera */
        #ResultContainer td { white-space: nowrap; }
    </style>    
    
    <div id="ResultContainer" style="width:100%; height:auto;">
    <asp:DataGrid runat="server" ID="dgSearch" DataSourceID="dsSearch" AutoGenerateColumns="false"
        EnableViewState="false" ShowHeader="True" GridLines="None" CssClass="table-th"
        OnInit="dgSearch_Init" OnPreRender="dgSearch_PreRender">
        <HeaderStyle CssClass="th" />
        <Columns>
            <asp:TemplateColumn>
                <HeaderStyle Width="15%" CssClass="left-th" />
                <HeaderTemplate>
                    <div>
                        <nobr>Свидетельство</nobr>
                    </div>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:HiddenField runat="server" ID="hfCNEId" Value='<%#Eval("CertificateId")%>' />
                    <%# this.GetCertificateLink(Container.DataItem) %>		
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="10%" />
                <HeaderTemplate>
                    <div>
                        ТН</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Eval("TypographicNumber") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="10%" />
                <HeaderTemplate>
                    <div>
                        Фамилия</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Eval("LastName") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="10%" />
                <HeaderTemplate>
                    <div>
                        Имя</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Eval("FirstName") %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="10%" />
                <HeaderTemplate>
                    <div>
                        Отчество</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Eval("PatronymicName")%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="10%" />
                <HeaderTemplate>
                    <div>
                        Документ</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <nobr>
                <span title="Серия"><%# Eval("PassportSeria")%></span>
                <span title="Номер"><%# Eval("PassportNumber")%></span>
                </nobr>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="10%" />
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
                    <%# Convert.ToString(Eval("Year")) %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="10%" />
                <HeaderTemplate>
                    <div>
                        Статус</div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Convert.ToString(Eval("Status")) %>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <HeaderStyle Width="10%" CssClass="right-th" />
                <HeaderTemplate>
                    <div title="Количество уникальных проверок ВУЗами и их филиалами">Проверки<span style="color: Red">&nbsp;*</span></div>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# Convert.ToString(Eval("UniqueIHEaFCheck"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    </div>
  
    <web:NoRecordsText runat="server" ControlId="dgSearch">
        <asp:Panel runat="server">
            <p align="center">
                <img src="/Common/Images/spacer.gif" width="32" height="31" style="background-image: url('/Common/Images/important.gif')" />
                По Вашему запросу ничего не найдено
            </p>
              <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" NavigateUrl='<%# GenerateNotFoundPrintLink() %>' Visible='<%# Convert.ToBoolean(ConfigurationManager.AppSettings["EnableNotFoundNote"])%>' Text="Распечатать" />
        </asp:Panel>
    </web:NoRecordsText>
    
    
    <asp:PlaceHolder ID="phUniqueChecks" runat="server" Visible="true">
        <span style="color: Red">* </span><span>Количество уникальных проверок свидетельства ВУЗами и их филиалами. Для пользователей ССУЗов данное поле носит справочный характер.</span>
    </asp:PlaceHolder>
    
    <asp:SqlDataSource runat="server" ID="dsSearch" ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="SearchCommonNationalExamCertificatePassport" CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure">
        <SelectParameters>
            <fbs:CurrentUserParameter Name="login" Type="String" />
            <fbs:CurrentUserParameter Name="ip" Type="String" />
            <asp:QueryStringParameter Name="lastName" ConvertEmptyStringToNull="true" Type="String"
                QueryStringField="LastName" />
            <asp:QueryStringParameter Name="firstName" ConvertEmptyStringToNull="true" Type="String"
                QueryStringField="FirstName" />
            <asp:QueryStringParameter Name="patronymicName" ConvertEmptyStringToNull="true" Type="String"
                QueryStringField="PatronymicName" />
            <asp:QueryStringParameter Name="passportSeria" ConvertEmptyStringToNull="true" Type="String"
                QueryStringField="Series" />
            <asp:QueryStringParameter Name="passportNumber" ConvertEmptyStringToNull="true" Type="String"
                QueryStringField="Number" />
            <asp:QueryStringParameter Name="year" ConvertEmptyStringToNull="true" Type="String"
                QueryStringField="Year" />
        </SelectParameters>
    </asp:SqlDataSource>
</form>
</asp:Content>
