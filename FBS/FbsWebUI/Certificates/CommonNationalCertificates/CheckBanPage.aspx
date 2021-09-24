<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckBanPage.aspx.cs"
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.CheckBanPage"
    MasterPageFile="~/Common/Templates/Certificates.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphCertificateHead" runat="server">
    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphCertificateActions" ID="cphActions">
    <asp:Panel runat="server" ID="pActions" Visible="false">
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
    <form id="Form1" runat="server">
        <p align="center">
            <img src="/Common/Images/spacer.gif" width="32" height="31" style="background-image: url('/Common/Images/important.gif')" />
            Ваш доступ к проверкам свидетельств временно заблокирован. Обратитесь к администратору
        </p>
    <div class="SpanContainer30">
    </div>
    </form>
</asp:Content>
