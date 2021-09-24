<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Check.aspx.cs" 
    Inherits="Fbs.Web.Certificates.Entrants.Check" 
    MasterPageFile="~/Common/Templates/Certificates.Master" %>
    
<%@ Register TagPrefix="fbs" Namespace="Fbs.Web.Controls" Assembly="FbsWebUI" %>

<asp:Content ContentPlaceHolderID="cphCertificateHead" runat="server">
    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphCertificateContent" runat="server">

<form runat="server" defaultbutton="btnCheck">
    <asp:ValidationSummary runat="server" DisplayMode="BulletList" EnableClientScript="false"
        HeaderText="<p>Произошли следующие ошибки:</p>"/>
        
    <table style="width:380px;">
        <tr><td><div class="form-l"> 
            <asp:TextBox runat="server" ID="txtNumber" CssClass="txt" />
            <input id="cNumber" value="Номер свидетельства" class ="txt" style="display:none" /><br />
            <p class="example">Пример: 12-123456789-08</p>
            
            <script type="text/javascript">
                IntiInputWithDefaultValue("<%= txtNumber.ClientID %>", "cNumber");
            </script>
            
            <asp:RequiredFieldValidator runat="server" 
                ControlToValidate="txtNumber" EnableClientScript="false" Display="None"
                ErrorMessage='Поле "Номер свидетельства" обязательно для заполнения' />
                
             <asp:RegularExpressionValidator runat="server"
                ControlToValidate="txtNumber" EnableClientScript="false" Display="None"
                ErrorMessage='Номер свидетельства должен быть в формате XX-XXXXXXXXX-XX' 
                ValidationExpression="\d{2}-\d{9}-\d{2}" />
        </div></td></tr>
        <tr><td class="t-line">
            <asp:Button runat="server" ID="btnReset" OnClick="btnReset_Click" 
                Text="Очистить" CssClass="bt" />
            <asp:Button runat="server" ID="btnCheck" OnClick="btnCheck_Click"
                Text="Проверить" CssClass="bt" />
        </td></tr>
    </table>
</form>

</asp:Content>
