<%@ Page Language="C#" MasterPageFile="~/Common/Templates/Administration.Master"
    AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="Fbs.Web.Administration.Deliveries.Create"
    Title="Untitled Page" ValidateRequest="false" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphHead">

    <script type="text/javascript" src="/Common/Scripts/CalendarPopup.js"></script>

    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>

</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
    <form id="Form1" runat="server">
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList"
        EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />
    <table class="form">
        <tr>
            <td class="left">
                Тема
            </td>
            <td class="text">
                <asp:TextBox runat="server" ID="TbTitle" CssClass="txt small" />
            </td>
        </tr>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TbTitle"
            EnableClientScript="false" Display="None" ErrorMessage='Поле "Тема рассылки" обязательно для заполнения' />
        <tr>
            <td class="left">
                Сообщение
            </td>
            <td class="text">
                <asp:TextBox runat="server" ID="TbMessage" CssClass="txt long" TextMode="MultiLine"
                    Rows="6" Height="100px"  />
            </td>
        </tr>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TbMessage"
            EnableClientScript="false" Display="None" ErrorMessage='Поле "Текст рассылки" обязательно для заполнения' />
        <tr>
            <td class="left">
                Дата отправки
            </td>
            <td class="text">
                <asp:TextBox runat="server" ID="TbDate" CssClass="txt date" />
                <img src="/Common/Images/ico-datepicker-fbs.gif" id="BtDate" onclick='return PickDate("<%=TbDate.ClientID%>", "BtDate", "CalendarContainer");' />
            </td>
        </tr>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TbDate"
            EnableClientScript="false" Display="None" ErrorMessage='Поле "Дата рассылки" обязательно для заполнения' />
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Неверный формат даты"
            ControlToValidate="TbDate" EnableClientScript="false" Display="None" ValidationExpression="^\d{1,2}\.\d{1,2}\.(?:\d{4}|\d{2})$" />
        <tr>
            <td class="left">
                Адресаты
            </td>
            <td>
                <asp:CheckBoxList runat="server" DataTextField="Name" 
                    DataValueField="Id" ID="ChBRecipients" >
                </asp:CheckBoxList>
            </td>
        </tr>
        <asp:CustomValidator ID="CustomValidator" runat="server" ErrorMessage="Не выбран ни одни адресат" Display="None"
            OnServerValidate="CustomValidator_ServerValidate" EnableClientScript="False"></asp:CustomValidator>
        <tr>
            <td colspan="2" class="box-submit">
                <asp:Button runat="server" ID="btnUpdate" Text="Сохранить" CssClass="bt" OnClick="btnUpdate_Click" />
            </td>
        </tr>
    </table>
    </form>
    <div id="CalendarContainer" style="position: absolute; visibility: hidden; background-color: white;
        layer-background-color: white;">
    </div>
    <div id="Div1" style="position: absolute; visibility: hidden; background-color: white;
        layer-background-color: white;">
    </div>
  
</asp:Content>
