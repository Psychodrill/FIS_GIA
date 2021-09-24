<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckResult.aspx.cs" 
    Inherits="Fbs.Web.Certificates.SchoolLeavingCertificates.CheckResult" 
    MasterPageFile="~/Common/Templates/Certificates.Master" %>
    
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Parameters" Assembly="FbsWebUI" %>
<%@ Import Namespace="Fbs.Web" %>

<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="cphCertificateContent" runat="server">
    <form runat="server">
    <asp:Repeater runat="server" ID="rpSearch" DataSourceID="dsSearch">
        <ItemTemplate>
            <%# Convert.ToBoolean(Eval("IsDeny")) ? 
                    String.Format("<span style='color:red'>Аттестат №{0} аннулирован по следующей причине: {1}.</span>", 
                        Eval("CertificateNumber"), Eval("DenyComment")) :
                    String.Format("Информация об аннулировании аттестата №{0} отсутствует.", 
                        Eval("CertificateNumber"))  %>
        </ItemTemplate>
    </asp:Repeater>
    
    <asp:SqlDataSource  runat="server" ID="dsSearch" 
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="dbo.CheckSchoolLeavingCertificate"  CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
        <SelectParameters>
            <fbs:CurrentUserParameter Name="login" Type="String" />
            <fbs:CurrentUserParameter Name="ip" Type="String" />
            <asp:QueryStringParameter Name="certificateNumber" ConvertEmptyStringToNull="true" Type="String" QueryStringField="Number" />
        </SelectParameters>
    </asp:SqlDataSource> 
    </form>
</asp:Content>
