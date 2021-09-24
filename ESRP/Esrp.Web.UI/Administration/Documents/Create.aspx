<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="Esrp.Web.Administration.Documents.Create"
    MasterPageFile="~/Common/Templates/Administration.master" %>

<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="Esrp.Core" %>
<%@ Import Namespace="Esrp.Web" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %>
<%@ Register TagPrefix="esrp" Namespace="Esrp.Web.Controls" Assembly="Esrp.Web.UI" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphHead">
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
                    Создать документ</p>
                <p class="back">
                    <a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
                <p class="statement_menu">
                    <a href="#" onclick="$('#<%= btnUpdate.ClientID %>').click();" class="active"><span>
                        Сохранить</span></a>
                </p>
                <div class="clear">
                </div>
                <div class="statement_table" >
                    <table width="100%">
                        <tr>
                            <th>
                                Название
                            </th>
                            <td width="1">
                                <asp:TextBox runat="server" ID="txtName" CssClass="txt small" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" EnableClientScript="false"
                            Display="None" ErrorMessage='Поле "Название" обязательно для заполнения' />
                        <tr>
                            <th colspan="2" style="border-bottom-color: #fff;">
                                Описание
                            </th>
                        </tr>
                        <tr>
                            <td colspan="2" style="border-top-color: #fff;">
                                <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Rows="5" Columns="35"
                                    CssClass="textareaoverflowauto" Width="99%" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Контекст
                            </th>
                            <td width="1">
                                <asp:DropDownList runat="server" ID="DDLContext" DataSourceID="dsContext" DataTextField="Name"
                                    DataValueField="Code">
                                </asp:DropDownList>
                                <asp:SqlDataSource runat="server" ID="dsContext" ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
                                    SelectCommand="SearchContext" CancelSelectOnNullParameter="false" SelectCommandType="StoredProcedure">
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtRelativeUrl"
                            EnableClientScript="false" Display="None" ValidationExpression="^(/[a-zA-Z0-9-_./]+)$"
                            ErrorMessage='Поле "Относительный Url" заполнено некорректно.' />
                        <tr>
                            <th>
                                Относительный Url
                            </th>
                            <td>
                                <asp:TextBox runat="server" ID="txtRelativeUrl" CssClass="txt small" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDescription" EnableClientScript="false"
                            Display="None" ErrorMessage='Поле "Описание" обязательно для заполнения' />
                        <tr>
                            <th>
                                Опубликован
                            </th>
                            <td class="text">
                                <asp:DropDownList ID="ddlIsActive" runat="server">
                                    <asp:ListItem Text="Да" Value="True" />
                                    <asp:ListItem Text="Нет" Value="False" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Файл
                            </th>
                            <td class="text">
                                <asp:FileUpload ID="fuDocument" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="fuDocument" EnableClientScript="false"
                            Display="None" ErrorMessage='Не выбран файл для загрузки' />
                        <asp:CustomValidator runat="server" ID="validFileSize" EnableClientScript="false"
                            ControlToValidate="fuDocument" Display="None" OnServerValidate="validFileSize_ServerValidate"
                            EnableViewState="false" ErrorMessage='Вы пытаетесь загрузить пустой файл' />
                        <tr>
                            <td colspan="2" class="box-submit" style="border-bottom-color: #fff;">
                                <asp:Button runat="server" ID="btnUpdate" Text="Сохранить" CssClass="bt" OnClick="btnUpdate_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        </form>
    </div>
</asp:Content>
