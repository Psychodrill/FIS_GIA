<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditInformationSystems.aspx.cs"
    Inherits="Esrp.Web.Administration.InformationSystems.EditInformationSystems" MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Import Namespace="Esrp.Web" %>

<%@ Register TagPrefix="esrp" TagName="GroupList" Src="~/Controls/GroupList.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript" src="/Common/Scripts/jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="/Common/Scripts/jquery-ui-1.8.18.min.js"></script>
    <script src="/Common/Scripts/Filter.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphLeftMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphActions" runat="server">
</asp:Content>
<asp:Content ID="Content5" runat="server" ContentPlaceHolderID="cphContentModalDialog">
    <div id="dialogDiv" class="pop" style="display: none;">
        <div class="pop_bg">
        </div>
        <div class="pop_rel">
            <div class="block">
                <div class="statement_table statement" style="border-bottom-color: rgb(235,243,246);
                    padding-bottom: 0px;">
                    <table width="100%">
                        <tr>
                            <th style="border-top-color: rgb(235,243,246);">
                                Наименование группы
                            </th>
                            <td width="1"  style="border-top-color: rgb(235,243,246);">
                                <input type="text" name="groupName" id="fgroupName" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Обозначение группы
                            </th>
                            <td>
                                <input type="text" name="groupCode" id="fgroupCode" />
                            </td>
                        </tr>
                        <tr style="border-bottom-color: rgb(235,243,246); padding-bottom: 0px;">
                            <td style="border-bottom-color: rgb(235,243,246); padding-bottom: 0px;">
                            </td>
                            <td style="border-bottom-color: rgb(235,243,246); padding-bottom: 0px;">
                                <input type="button" value="Отмена" class="closed un_dott" onclick="closeDlg();" />
                                <input type="submit" id="addGroupBtn" value="Добавить" onclick="addGroup();" />
                                <input type="submit" id="updateGroupBtn" value="Изменить" onclick="updateGroup();" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <!--block-->
        </div>
    </div>
    <script type="text/javascript">

        function closeDlg() {
            var dlgDiv = document.getElementById('dialogDiv');
            dlgDiv.style.display = 'none';
        }

        // Отображение диалога добавления
        function showAddGroupDlg() {
            document.getElementById('dialogDiv').style.display = '';
            document.getElementById('updateGroupBtn').style.display = 'none';
            document.getElementById('addGroupBtn').style.display = '';

        }

        // Отображение диалога редактировани
        function showUpdateGroupDlg(grouId) {
            document.getElementById('dialogDiv').style.display = '';
            document.getElementById('updateGroupBtn').style.display = '';
            document.getElementById('addGroupBtn').style.display = 'none';
            $('#<%=hfGroupId.ClientID %>').val(grouId);
        }

        function addGroup() {
            var name = $('#fgroupName').val(),
            code = $('#fgroupCode').val(),
            systemId = '<%= this.Id %>';

            $.ajax({
                type: "POST",
                url: "SaveGroup.asmx/CreateGroup",
                data: { 'name': name, 'code': code, 'systemId': systemId },
                success: function () {
                    window.location.reload();
                },
                error: function () {
                    $('#ErrorMsg').css('display', '').html("<table width='100%' border='0'><tr><td valign='middle' width='100%' style='text-align:left;'>&nbsp;&nbsp;•&nbsp;При добавлении группы возникли ошибки!<br></td></tr></table>");
                }
            });
            closeDlg();
        }

        function updateGroup() {
            var name = $('#fgroupName').val(),
            code = $('#fgroupCode').val(),
            groupId = $('#<%=hfGroupId.ClientID %>').val();
            $.ajax({
                type: "POST",
                url: "SaveGroup.asmx/UpdateGroup",
                data: { 'name': name, 'code': code, 'groupId': groupId },
                success: function () {
                    window.location.reload();
                }
            });
            closeDlg();
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphContent" runat="server">
    <div class="left_col">
        <form runat="server">
        <asp:ValidationSummary CssClass="error_block" runat="server" ID="vsErrors" DisplayMode="BulletList"
            EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />
        <br/>
        <div class="col_in">
            <div class="statement edit">
                <p class="title">
                    Редактировать ИС №
                    <%= this.Id %></p>
                <p class="back">
                    <a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
                <p class="statement_menu">
                    <a href="#" class="active" onclick="$('#<%= fvInformationSystem.FindControl("bSave").ClientID %>').click();"
                        title="Сохранить">Сохранить</a> <a id="linkGroup" href="#" class="gray" onclick="showAddGroupDlg(true); return false;"
                            title="Добавить группу">Добавить группу</a>
                </p>
                <div class="clear">
                </div>
                <asp:FormView ID="fvInformationSystem" runat="server" CellPadding="8" Width="100%"
                    RenderOuterTable="false" DataSourceID="odsInformationSystem" DataKeyNames="SystemId"
                    BorderWidth="0" DefaultMode="Edit">
                    <EditItemTemplate>
                        <div class="statement_in">
                            <p>
                                <strong>
                                    <asp:CheckBox runat="server" ID="cbAvailableRegistration" Text="&nbsp;Доступна для регистрации"
                                        Checked='<%# Bind("AvailableRegistration") %>' Enabled='<%# Eval("AvailableRegistrationEnable") %>' /></strong></p>
                        </div>
                        <div class="statement_table">
                            <table width="100%">
                                <tr>
                                    <th style="border-top-color: #fff;">
                                        Краткое наименование<span style="color: rgb(215, 5, 5) !important;"><%=this.Page.Required()%></span>
                                    </th>
                                    <td width="1" style="border-top-color: #fff;">
                                        <asp:TextBox runat="server" ID="tbShortName" MaxLength="500" Text='<%# Bind("ShortName") %>' />
                                    </td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="tbShortName"
                                        EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "Краткое наименование" обязательно для заполнения' />
                                </tr>
                                <tr>
                                    <th>
                                        Обозначение системы<span style="color: rgb(215, 5, 5) !important;"><%=this.Page.Required()%></span>
                                    </th>
                                    <td>
                                        <asp:TextBox runat="server" ID="tbNotationSystem" MaxLength="20" Text='<%# Bind("Code") %>' />
                                    </td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbNotationSystem"
                                        EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "Обозначение системы" обязательно для заполнения' />
                                </tr>
                                <tr>
                                    <th>
                                        Полное наименование<span style="color: rgb(215, 5, 5) !important;"><%=this.Page.Required()%></span>
                                    </th>
                                    <td>
                                        <asp:TextBox runat="server" ID="tbFullName" MaxLength="500" Text='<%# Bind("FullName") %>' />
                                    </td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbFullName"
                                        EnableClientScript="false" Text="*" Display="None" ErrorMessage='Поле "Полное наименование" обязательно для заполнения' />
                                </tr>
                            </table>
                        </div>
                        <div class="statement_table">
                            <%--Контрол с группами для ИС--%>
                            <esrp:GroupList runat="server" ID="GroupList" SystemId='<%# Eval("SystemId") %>' />
                            <table>
                                <tr>
                                    <td style="text-align: right; border-color: #fff;">
                                        <asp:Button runat="server" ID="bSave" CausesValidation="True" CommandName="Update"
                                            CssClass="bt" Text="Сохранить" Enabled='<%#Eval("IsExistDefautlGroup") %>'></asp:Button>
                                        <asp:Button ID="Button2" runat="server" CausesValidation="False" CommandName="Cancel"
                                            CssClass="bt" Text="Отменить"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </EditItemTemplate>
                </asp:FormView>
            </div>
        </div>
        <asp:ObjectDataSource ID="odsInformationSystem" runat="server" DataObjectTypeName="Esrp.Web.ViewModel.InformationSystems.InformationSystemEntity"
            TypeName="Esrp.Services.InformationSystemsService" SelectMethod="SelectInformationSystemById"
            OnSelecting="OdsInformationSystemsOnSelecting" OnSelected="OdsInformationSystemsOnSelected"
            UpdateMethod="UpdateOrCreateInformationSystem" OnUpdating="OdsInformationSystemOnUpdating" />
        <asp:HiddenField runat="server" ID="hfSystemId" />
        <asp:HiddenField runat="server" ID="hfGroupId" />
        </form>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#linkGroup").click(function () {
                var sytemPrefix = $('input[id*="tbNotationSystem"]');
                if (sytemPrefix.length == 0) {
                    return;
                }
                sytemPrefix = sytemPrefix.val() + "_";
                $('#fgroupCode').val(sytemPrefix);
                showAddGroupDlg();
            });
        });
    </script>
</asp:Content>
