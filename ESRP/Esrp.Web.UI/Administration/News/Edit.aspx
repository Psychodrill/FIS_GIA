<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Esrp.Web.Administration.News.Edit"
    ValidateRequest="false" MasterPageFile="~/Common/Templates/Administration.master" %>
    <%@ Register Src="~/Controls/DatePickerControl.ascx" TagPrefix="uc" TagName="DatePicker" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>
<%@ Register TagPrefix="esrp" Namespace="Esrp.Web.Controls" Assembly="Esrp.Web.UI" %>
<asp:Content runat="server" ContentPlaceHolderID="cphHead">
    <script type="text/javascript" src="/Common/Scripts/tiny_mce/tiny_mce.js"></script>
    <script type="text/javascript" src="/Common/Scripts/CalendarPopup.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            var params = {
                changedEl: 'select',
                visRows: 7,
                scrollArrows: true
            }
            cuSel(params);
        });
    </script>
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphThirdLevelMenu">
    <div class="third_line">
        <div class="max_width">
            <esrp:TopMenu ID="SecondLevelMenu1" runat="server" RootResourceKey="press-center"
                HeaderTemplate="<ul>" FooterTemplate="</ul>" />
            <div class="clear">
            </div>
        </div>
    </div>
    <!--bottom_line-->
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <div class="left_col">
        <form runat="server">
        <asp:ValidationSummary CssClass="error_block" runat="server" DisplayMode="BulletList"
            EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />
        <div class="col_in">
            <div class="statement edit">
                <p class="title">
                    Новость
                    <%= this.txtName.Text %></p>
                <p class="back">
                    <a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
                <p class="statement_menu">
                    <a href="#" onclick="$('#<%= btnUpdate.ClientID %>').click();" class="active"><span>
                        Сохранить</span></a>
                </p>
                <div class="clear">
                </div>
                <div class="statement_table">
                    <table width="100%">
                        <tr>
                            <th>
                                Дата
                            </th>
                            <td >
                                 <uc:DatePicker runat="server" ID="txtDate" MinDate="01.01.2000" />
                                 <asp:CustomValidator runat="server" OnServerValidate="validateDate" ErrorMessage="Необходимо указать дату" Display="None" />
                            </td>
                        </tr>
                       
                        <tr>
                            <th>
                                Название
                            </th>
                            <td class="text">
                                <asp:TextBox runat="server" ID="txtName" CssClass="txt small" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" EnableClientScript="false"
                            Display="None" ErrorMessage='Поле "Название" обязательно для заполнения' />
              
                        <tr>
                            <th>
                                Краткое описание
                            </th>
                            <td colspan="3">
                                <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Rows="5" Columns="35"
                                    CssClass="textareaoverflowauto" Width="520" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDescription" EnableClientScript="false"
                                    Display="None" ErrorMessage='Поле "Краткое писание" обязательно для заполнения' />
                            </td>
                        </tr>          
                        <tr>
                            <th>
                                Опубликовать новость
                            </th>
                            <td width="1">
                                <asp:DropDownList ID="ddlIsActive" runat="server">
                                    <asp:ListItem Text="Да" Value="True" />
                                    <asp:ListItem Text="Нет" Value="False" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th colspan="2" style="border-bottom-color: #fff;">
                                Текст новости
                            </th>
                        </tr>
                        <tr>
                            <td colspan="2" class="text" style="border-top-color: #fff;">
                                <asp:TextBox runat="server" ID="txtNews" TextMode="MultiLine" Columns="60" Rows="15"
                                    CssClass="txt-area" />
                                <script type="text/javascript">InitTinyMCE("<%= txtNews.ClientID %>");</script>
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNews" EnableClientScript="false"
                            Display="None" ErrorMessage='Поле "Текст" обязательно для заполнения' />
                        <tr>
                            <td colspan="2" class="box-submit" style="border-bottom-color:#fff;">
                                <asp:Button runat="server" ID="btnUpdate" Text="Сохранить" CssClass="bt" OnClick="btnUpdate_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        </form>
    </div>
    <div id="CalendarContainer" style="position: absolute; visibility: hidden; background-color: white;
        layer-background-color: white;">
    </div>
</asp:Content>
