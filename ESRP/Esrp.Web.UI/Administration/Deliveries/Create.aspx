<%@ Page Language="C#" MasterPageFile="~/Common/Templates/Administration.Master"
    AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="Esrp.Web.Administration.Deliveries.Create"
    Title="Untitled Page" ValidateRequest="false" %>
    <%@ Register Src="~/Controls/DatePickerControl.ascx" TagPrefix="uc" TagName="DatePicker" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphHead">
    <script type="text/javascript" src="/Common/Scripts/CalendarPopup.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>
   
</asp:Content>
<asp:Content ContentPlaceHolderID="cphContent" runat="server">
    <div class="left_col">
        <form id="Form1" runat="server">
        <asp:ValidationSummary CssClass="error_block" ID="ValidationSummary1" runat="server"
            DisplayMode="BulletList" EnableClientScript="true" HeaderText="<p>Произошли следующие ошибки:</p>" />
        <div class="col_in">
            <div class="statement edit">
                <p class="title">
                  <asp:Label runat="server"><%= this.Title %></asp:Label>  </p>
                <p class="back">
                    <a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
                <p class="statement_menu">
                    <a href="#" onclick="$('#<%= btnUpdate.ClientID %>').click();" class="active"><span>
                        Сохранить</span></a>
                </p>
                <div class="clear">
                </div>
                <div class="statement_table">
                    <table width="700">
                        <tr>
                            <th style="width: 200px;">
                                Тема
                            </th>
                            <td width="1">
                                <asp:TextBox runat="server" ID="TbTitle" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TbTitle"
                            EnableClientScript="false" Display="None" ErrorMessage='Поле "Тема" обязательно для заполнения' />
                        <tr>
                            <th>
                                Дата отправки
                            </th>
                            <td nowrap="nowrap">

                                <uc:DatePicker runat="server" ID="TbDate" MinDate="01.01.2000" />
                               <asp:CustomValidator runat="server" OnServerValidate="validateDate" ErrorMessage='Поле "Дата отправки" обязательно для заполнения' Display="None" />
                            </td>
                        </tr>
                        
                        <tr>
                            <th style="vertical-align: top;">
                                Сообщение
                            </th>
                            <td nowrap="nowrap">
                                <p>
                                    <asp:TextBox runat="server" ID="TbMessage" TextMode="MultiLine" Rows="6" Height="100px"
                                        CssClass="textareaoverflowauto" Width="520" /></p>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TbMessage"
                                    EnableClientScript="false" Display="None" ErrorMessage='Поле "Сообщение" обязательно для заполнения' />
                                <p>
                                    &nbsp;</p>
                                <p>
                                    <strong></strong>
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <th style="vertical-align: top;">
                                Адресаты
                            </th>
                            <td nowrap>
                                <asp:CheckBoxList runat="server" DataTextField="Name" DataValueField="Id" ID="ChBRecipients"
                                    CssClass="checkbox-box-list" />
                                <asp:CustomValidator ID="CustomValidator5" runat="server" ErrorMessage="Не выбран ни одни адресат"
                                    Display="None" OnServerValidate="CustomValidator_ServerValidate" EnableClientScript="False"></asp:CustomValidator>
                            </td>
                        </tr>
                    </table>
                    <p class="save">
                        <asp:Button runat="server" ID="btnUpdate" Text="Сохранить" OnClick="btnUpdate_Click" /></p>
                </div>
            </div>
        </div>
        </form>
    </div>
    <div id="CalendarContainer" style="position: absolute; visibility: hidden; background-color: white;
        layer-background-color: white;">
    </div>
    <div id="Div1" style="position: absolute; visibility: hidden; background-color: white;
        layer-background-color: white;">
    </div>
</asp:Content>
