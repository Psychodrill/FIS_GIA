<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestResult.aspx.cs" 
    Inherits="Fbs.Web.Certificates.CompetitionCertificates.RequestResult" 
    MasterPageFile="~/Common/Templates/Certificates.Master" %>
    
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>

<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server">
    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphCertificateContent" runat="server">

    <asp:DataGrid runat="server" id="dgResultsList"
            DataSourceID="dsResultsList"
            AutoGenerateColumns="false" 
            EnableViewState="false"
            ShowHeader="True" 
            GridLines="None"
            CssClass="table-th">
        <HeaderStyle CssClass="th" />
        <Columns>
            <asp:TemplateColumn>
            <HeaderStyle CssClass="left-th" />
            <HeaderTemplate>
                <div>Степень</div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.ToBoolean(Eval("IsExist")) ? Convert.ToString(Eval("Degree")) : "<span style='color:red'>Не найдено</span>" %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderTemplate>
                Олимпиада
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("CompetitionTypeName") %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderTemplate>
                Фамилия
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("LastName")%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderTemplate>
                Имя
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("FirstName")%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderTemplate>
                Отчество
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("PatronymicName")%>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <HeaderTemplate>
                Регион
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("RegionName")%>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <HeaderTemplate>
                Город
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("City")%>
            </ItemTemplate>
            </asp:TemplateColumn>
              
            <asp:TemplateColumn>
            <HeaderTemplate>
                Школа
            </HeaderTemplate>
            <ItemTemplate>
                <%# Eval("School")%>
            </ItemTemplate>
            </asp:TemplateColumn> 
                     
            <asp:TemplateColumn>
            <HeaderStyle CssClass="right-th" />
            <HeaderTemplate><div>Класс &nbsp;</div></HeaderTemplate>
            <ItemTemplate>
                <%# Eval("Class")%>
            </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

    <asp:SqlDataSource runat="server" ID="dsResultsList" 
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="dbo.SearchCompetitionCertificate"  CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure">
        <SelectParameters>
            <fbs:CurrentUserParameter Name="login" Type="String" />
            <fbs:CurrentUserParameter Name="ip" Type="String" />
            <asp:QueryStringParameter Name="competitionTypeId" Type="Int32" QueryStringField="CompetitionType" />
            <asp:QueryStringParameter Name="lastName" Type="String" QueryStringField="LastName" />
            <asp:QueryStringParameter Name="firstName" Type="String" QueryStringField="FirstName" />
            <asp:QueryStringParameter Name="patronymicName" ConvertEmptyStringToNull="true" Type="String" QueryStringField="PatronymicName" />
            <asp:QueryStringParameter Name="regionId" Type="Int32" QueryStringField="Region" />
        </SelectParameters>
    </asp:SqlDataSource>
    
</asp:Content>