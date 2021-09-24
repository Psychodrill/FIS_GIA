<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckResult.aspx.cs" 
    Inherits="Fbs.Web.Certificates.Entrants.CheckResult" 
    MasterPageFile="~/Common/Templates/Certificates.Master" %>

<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<%@ Import Namespace="Fbs.Web" %>

<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server">
    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphCertificateContent" runat="server">
    <form runat="server">
    <asp:Repeater runat="server" ID="rpSearch" DataSourceID="dsSearch" 
        onitemdatabound="rpSearch_ItemDataBound">
        <ItemTemplate>
            <asp:Panel runat="server" ID="pNotExist" Visible="false">
                <p style="color:Red" >Поступившего абитурента по свидетельству ЕГЭ №<%# String.Format("<a href=\"/Certificates/CommonNationalCertificates/CheckResult.aspx?number={0}&isOriginal=False\">{0}</a>", Eval("CertificateNumber")) %> не найдено.</p>
            </asp:Panel>

            <asp:Panel runat="server" ID="pResult">
                <table class="form" style="width:100%" ><tr>
                    <td style="width:20%">Образовательное учреждение</td>
                    <td><%# Convert.IsDBNull(Eval("OrganizationName")) ? "Не найдено" : Eval("OrganizationName") %></td>                    
                </tr><tr>
                    <td style="width:20%">Свидетельство ЕГЭ</td>
                    <td><%# String.Format("<a href=\"/Certificates/CommonNationalCertificates/CheckResult.aspx?number={0}&isOriginal=False\">{0}</a>", Eval("CertificateNumber")) %></td>
                </tr><tr>
                </tr><tr>
                    <td style="width:20%">Фамилия</td>
                    <td><%# Eval("LastName") %></td>
                </tr><tr>
                    <td style="width:20%">Имя</td>
                    <td><%# Eval("FirstName") %></td>
                </tr><tr>
                    <td style="width:20%">Отчество</td>
                    <td><%# Eval("PatronymicName") %></td>
                </tr><tr>
                    <td style="width:20%">Дата поступления</td>
                    <td><%# Convert.IsDBNull(Eval("EntrantCreateDate")) ? String.Empty : Convert.ToDateTime(Eval("EntrantCreateDate")).ToShortDateString() %></td>
                </tr></table>
            </asp:Panel>
            <asp:HiddenField runat="server" ID="hfIsExist" Value='<%#Convert.ToBoolean(Eval("IsExist"))%>' />
        </ItemTemplate>
    </asp:Repeater>
    
    <asp:SqlDataSource  runat="server" ID="dsSearch" 
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="dbo.CheckEntrant"  CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <fbs:CurrentUserParameter Name="login" Type="String" />
            <fbs:CurrentUserParameter Name="ip" Type="String" />
            <asp:QueryStringParameter Name="certificateNumber" ConvertEmptyStringToNull="true" Type="String" QueryStringField="Number" />
        </SelectParameters>
    </asp:SqlDataSource> 
    </form>
</asp:Content>