<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InformationSystemsRegistration.ascx.cs"
    Inherits="Esrp.Web.Controls.InformationSystemsRegistration" %>
<%@ Import Namespace="Esrp.Web" %>
<%@ Import Namespace="Esrp.Core.Systems" %>
<%@ Register Assembly="Esrp.Web.UI" Namespace="Esrp.Web.Controls" TagPrefix="esrp" %>
<esrp:EsrpListView ID="listView" runat="server" DataKeyNames="SystemID" DataSourceID="dataSource"
    AutoChange="false" OnItemDataBound="lvOnItemDataBound" OnItemUpdated="LvOnItemUpdated">
    <ItemTemplate>
        <div class="statement_table">
            <h2>
                Доступ к <%#Eval("ShortName")%></h2>
            <br />
            <asp:HiddenField runat="server" ID="hfId" Value='<%#Eval("SystemId") %>' />
            <asp:HiddenField runat="server" ID="IdSystemName" Value='<%#Eval("ShortName")%>' />
            <asp:CheckBox runat="server" ID="cbAllowSystem" Checked='<%#Bind("AccessToSystem")%>' Enabled='<%#Eval("AccessToSystemEnable")%>' />
            <asp:Label ID="lblAllowSystem" runat="server" AssociatedControlID="cbAllowSystem" CssClass="chkboxLabel">Доступ к <%#Eval("ShortName")%></asp:Label>
            <div style="margin: 15px 0px">
                <asp:Label ID="lFbdInfo" runat="server"><%#Eval("lFbdInfo")%></asp:Label>
            </div>
            
            <table width="100%">
                <%-- Часть только для ФБС. Начало--%>
                <tr id="trStatActive1" style="display: <%#this.CheckVisibility((bool)Eval("trStatActive1Visible"))%>">
                    <td colspan="2" class="left" style="padding: 10px 0px 30px 0px">
                        Сведения о <b>текущем</b> уполномоченном сотруднике по работе с <%=GeneralSystemManager.GetSystemName(3)%>:
                    </td>
                </tr>
                <tr id="trStatActive2" style="display: <%#this.CheckVisibility((bool)Eval("trStatActive2Visible"))%>">
                    <th>
                        Ф.И.О.:
                    </th>
                    <td width="1">
                        <asp:TextBox runat="server" ID="TBCurFbdAdmissionUserFullName" CssClass="txt long"
                            Text='<%#Bind("CurFullName") %>' ReadOnly='<%#Eval("ReadOnlyCurFullName") %>'
                            BackColor='<%#Eval("BackColorCurFullName") %>' />
                    </td>
                </tr>
                <tr id="trStatActive3" style="display: <%#this.CheckVisibility((bool)Eval("trStatActive3Visible"))%>">
                    <th>
                        Должность:
                    </th>
                    <td>
                        <asp:TextBox runat="server" ID="TBCurFbdAdmissionUserPosition" CssClass="txt long"
                            Text='<%#Bind("CurPosition") %>' ReadOnly='<%#Eval("ReadOnlyCurPosition") %>'
                            BackColor='<%#Eval("BackColorCurPosition") %>' />
                    </td>
                </tr>
                <tr id="trStatActive4" style="display: <%#this.CheckVisibility((bool)Eval("trStatActive4Visible"))%>">
                    <th>
                        <b>Логин (E-mail, указанный при регистрации)</b>:
                    </th>
                    <td>
                        <asp:TextBox runat="server" ID="TBCurFbdAdmissionUserEmail" CssClass="txt long"
                            Text='<%#Bind("CurEmail") %>' ReadOnly='<%#Eval("ReadOnlyCurEmail") %>' BackColor='<%#Eval("BackColorCurEmail") %>' />
                    </td>
                </tr>
                <tr id="trSameDataAsFbs" style="display: <%# this.CheckVisibility((bool)Eval("SameDataAsFbsVisible"))%>">
                    <td class="left" style="padding-bottom: 20px;">
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="CBSameDataAsFbs">Совпадает с <%=GeneralSystemManager.GetSystemName(2)%> </asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="CBSameDataAsFbs" Checked='<%#Bind("SameDataAsFbs")%>'
                            Enabled='<%#Eval("SameDataAsFbsEnable")%>' />
                    </td>
                </tr>
                <tr id="trStatActive5" style="display: <%#this.CheckVisibility((bool)Eval("trStatActive5Visible"))%>">
                    <td class="left" colspan="2" style="padding: 10px 0px">
                        Сведения о <b>новом</b> уполномоченном сотруднике по работе с
                        <%=GeneralSystemManager.GetSystemName(3)%>:
                    </td>
                </tr>
                <%-- Часть только для ФБС. Конец--%>
                <tr>
                    <th>
                        Ф.И.О.<span style="color:  rgb(215, 5, 5) !important;"><%=this.Page.Required()%></span>:
                    </th>
                    <td width="1">
                        <asp:HiddenField runat="server" ID="tbUserFullName_hidden" Value="" />
                        <asp:TextBox runat="server" ID="tbUserFullName" CssClass="txt long" MaxLength="255"
                            Text='<%#Bind("FullName") %>' Enabled='<%#Eval("FullNameEnable")%>' />
                        <asp:RequiredFieldValidator ID="rfvUserFullName" runat="server" ControlToValidate="tbUserFullName"
                            Text="*" EnableClientScript="false" Display="None" ErrorMessage='Поле "Ф.И.О. лица, ответственного за работу с {0}" обязательно для заполнения' />
                    </td>
                </tr>
                <tr>
                    <th>
                        Должность:
                    </th>
                    <td>
                        <asp:HiddenField runat="server" ID="tbUserPosition_hidden" Value="" />
                        <asp:TextBox runat="server" ID="tbUserPosition" CssClass="txt long" MaxLength="255"
                            Text='<%#Bind("Position") %>' Enabled='<%#Eval("PositionEnable")%>' />
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label runat="server" ID="lblTBFbdAdmissionUserEmail">
                            <b>E-mail (будет являться логином)</b><span style="color:  rgb(215, 5, 5) !important;"><%=this.Page.Required()%></span>:
                        </asp:Label>
                    </th>
                    <td>
                        <asp:HiddenField runat="server" ID="tbUserEmail_hidden" Value="" />
                        <asp:TextBox runat="server" ID="tbUserEmail" CssClass="txt long" MaxLength="255" autocomplete="off"
                            Text='<%#Bind("Email") %>' Enabled='<%#Eval("EmailEnable")%>' />
                        <asp:RequiredFieldValidator ID="rfvUserEmail" runat="server" ControlToValidate="tbUserEmail"
                            EnableClientScript="false" Display="None" Text="*" ErrorMessage='Поле "E-mail лица, ответственного за работу с {0}" обязательно для заполнения' />
                        <asp:RegularExpressionValidator runat="server" ID="revUserEmail" ControlToValidate="tbUserEmail"
                            EnableClientScript="false" Display="None" Text="*" ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$"
                            ErrorMessage='Поле "E-mail лица, ответственного за работу с {0}" заполнено неверно' />
                        <asp:CustomValidator runat="server" ControlToValidate="tbUserEmail" Display="None"
                            EnableClientScript="false" ID="cvFbdAdmissionUserEmail" Text="*" />
                    </td>
                </tr>
               
                <tr id="trFbdAdmissionUserPhone" style="display: <%#this.CheckVisibility(!(bool)Eval("UserPhoneVisible"))%>">
                    <th>
                        Телефон:
                    </th>
                    <td>
                        <asp:HiddenField runat="server" ID="tbUserPhone_hidden" Value="" />
                        <asp:TextBox runat="server" ID="tbUserPhone" CssClass="txt long" MaxLength="255"
                            Text='<%#Bind("Phone") %>' Enabled='<%#Eval("PhoneEnable")%>' />
                    </td>
                </tr>
                <tr id="trFbdChangeReadonlyAuthorizedStaff" style="display: <%#this.CheckVisibility((bool)Eval("trFbdChangeReadonlyAuthorizedStaffVisible"))%>">
                    <td colspan="2" style="padding-top: 20px;">
                        <a href="#" id="btnFbdChange" runat="server" onclick="Do(); return false;"><b>Сменить</b>
                            уполномоченного сотрудника по работе с <b>
                                <%#Eval("ShortName")%></b></a>
                    </td>
                </tr>
            </table>
        </div>
    </ItemTemplate>
    <LayoutTemplate>
        <div id="listview">
            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
        </div>
    </LayoutTemplate>
</esrp:EsrpListView>
<asp:ObjectDataSource runat="server" ID="dataSource" TypeName="Esrp.Services.InformationSystemsService"
    DataObjectTypeName="Esrp.Web.ViewModel.InformationSystems.InformationSystemsRegistrationView"
    SelectMethod="SelectInformationSystems" OnSelecting="OdsOnSelecting" UpdateMethod="UpdateInformationSystems"
    OnUpdating="OnUpdatingInformationSystem"></asp:ObjectDataSource>
<asp:HiddenField runat="server" ID="ReadOnlyUser" Value="false" />
<script type="text/javascript">

    $(document).ready(function () {

        // Если ФБС выбрана, то enable галочки, иначе disable и снятие галочки
        if (!$('#<%=this.AllowFbs%>').is(':checked')) {
            $('#<%=this.SameFbs%>').prop('checked', false);
            $('#<%=this.SameFbs%>').attr('disabled', 'disabled');
        }
        else {
            $('#<%=this.SameFbs%>').attr('disabled', false);
        }

        if (!(!$('#<%=this.AllowFbd%>').is(':disabled') && $('#<%=this.AllowFbd%>').is(':checked'))) {
            $('#<%=this.SameFbs%>').attr('disabled', 'disabled');
            var parent = $('#<%=this.AllowFbd%>').closest("div");
            parent.find('input[id*="tbUserFullName"]').prop('readonly', true).css('background-color', 'LightGrey');
            parent.find('input[id*="tbUserFullName_hidden"]').val("disabled");
            parent.find('input[id*="tbUserPosition"]').prop('readonly', true).css('background-color', 'LightGrey');
            parent.find('input[id*="tbUserPosition_hidden"]').val("disabled");
            parent.find('input[id*="tbUserEmail"]').prop('readonly', true).css('background-color', 'LightGrey');
            parent.find('input[id*="tbUserEmail_hidden"]').val("disabled");
            parent.find('input[id*="tbUserPhone"]').prop('readonly', true).css('background-color', 'LightGrey');
            parent.find('input[id*="tbUserPhone_hidden"]').val("disabled");
        }

        // Для каждого элемента с именем cbAllowSystem
        // Если checkbox выбран, то открываются соответствующие текстовые поля, иначе закрываются
        // Также для каждого текстового поля есть hidden, если поле закрыто, то выставляется value='disabled' 
        // нужно для серверного подключения или отключения валидации
        $('input[id*="cbAllowSystem"]').each(function () {
            var result = $(this).is(':checked');
            var parent = $(this).closest("div");
            var id = parent.find('input[id*="hfId"]').val();
           
            if (id != 3) {
                if (result) {
                    parent.find('input[id*="tbUserFullName"]').prop('readonly', false).css('background-color', 'rgb(235,243,246)');
                    parent.find('input[id*="tbUserFullName_hidden"]').val("");
                    parent.find('input[id*="tbUserPosition"]').prop('readonly', false).css('background-color', 'rgb(235,243,246)');
                    parent.find('input[id*="tbUserPosition_hidden"]').val("");
                    parent.find('input[id*="tbUserEmail"]').prop('readonly', false).css('background-color', 'rgb(235,243,246)');
                    parent.find('input[id*="tbUserEmail_hidden"]').val("");
                    parent.find('input[id*="tbUserPhone"]').prop('readonly', false).css('background-color', 'rgb(235,243,246)');
                    parent.find('input[id*="tbUserPhone_hidden"]').val("");
                }
                else {
                    parent.find($('input[id*="tbUserFullName"]')).prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserFullName_hidden"]').val("disabled");
                    parent.find($('input[id*="tbUserPosition"]')).prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserPosition_hidden"]').val("disabled");
                    parent.find($('input[id*="tbUserEmail"]')).prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserEmail_hidden"]').val("disabled");
                    parent.find($('input[id*="tbUserPhone"]')).prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserPhone_hidden"]').val("disabled");
                }
            }
        });

        // Для системы с ид = 3 для чекбокса "Совпадает с"
        $('input[id*="CBSameDataAsFbs"]').each(function () {
            var parent = $(this).closest("div");
            var id = parent.find($('input[id*="hfId"]')).val();
            if (id == 3) {
                // Если выбрана организация и она имеет тип отличный от ВУЗ или ССУЗ, то этот блок блокируем и выходим
                if ("<%=this.EnabledFbdBlock%>" == "True") {

                    parent.find('input[id*="CBSameDataAsFbs"]').attr('disabled', 'disabled');
                    parent.find('input[id*="cbAllowSystem"]').attr('disabled', 'disabled').prop('checked', false);

                    parent.find('input[id*="tbUserFullName"]').prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserFullName_hidden"]').val("disabled");
                    parent.find('input[id*="tbUserPosition"]').prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserPosition_hidden"]').val("disabled");
                    parent.find('input[id*="tbUserEmail"]').prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserEmail_hidden"]').val("disabled");
                    parent.find('input[id*="tbUserPhone"]').prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserPhone_hidden"]').val("disabled");

                    return;
                }

                // Если существует пользователь и у него статус "на регистрации" или "на согласовании" или "на доработке", то блокируем 
                if ("<%=this.fbdAuthorizedStaffStatus%>" != "" && ("<%=this.fbdAuthorizedStaffStatus%>" == "Registration" || "<%=this.fbdAuthorizedStaffStatus%>" == "Consideration" || "<%=this.fbdAuthorizedStaffStatus%>" == "Revision")) {
                    $('#<%=this.AllowFbd%>').attr('checked', false).attr('disabled', 'disabled');
                    $('#<%=this.SameFbs%>').attr('checked', false).attr('disabled', 'disabled');
                    parent.find('span[id*="lFbdInfo"]').html("Для получения доступа к " + parent.find('input[id*="IdSystemName"]').val() + " завершите регистрацию учетной записи следующего уполномоченного сотрудника:");

                    parent.find('input[id*="tbUserFullName"]').val("<%=this.fbdAuthorizedStaffLastName%>").attr('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserPosition"]').val("<%=this.fbdAuthorizedStaffPosition%>").attr('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserEmail"]').val("<%=this.fbdAuthorizedStaffLogin%>").attr('readonly', true).css('background-color', 'LightGrey');
                    parent.find('tr[id*="trSameDataAsFbs"]').css('display', 'none');
                    parent.find('tr[id*="trFbdAdmissionUserPhone"]').css('display', 'none');
                }
                else
                // Если пользователь активирован, то его можно сменить
                    if ("<%=this.fbdAuthorizedStaffStatus%>" != "" && "<%=this.fbdAuthorizedStaffStatus%>" == "Activated") {
                        parent.find('span[id*="lFbdInfo"]').html("<b>Смена</b> уполномоченного сотрудника по работе с <b>" + parent.find('input[id*="IdSystemName"]').val() + "</b>:");
                        parent.find('input[id*="TBCurFbdAdmissionUserFullName"]').val("<%=this.fbdAuthorizedStaffLastName%>").prop('readonly', true).css('background-color', 'LightGrey');
                        parent.find('input[id*="TBCurFbdAdmissionUserPosition"]').val("<%=this.fbdAuthorizedStaffPosition%>").prop('readonly', true).css('background-color', 'LightGrey');
                        parent.find('input[id*="TBCurFbdAdmissionUserEmail"]').val("<%=this.fbdAuthorizedStaffLogin%>").prop('readonly', true).css('background-color', 'LightGrey');

                        parent.find('tr[id*="trStatActive1"]').css('display', '');
                        parent.find('tr[id*="trStatActive2"]').css('display', '');
                        parent.find('tr[id*="trStatActive3"]').css('display', '');
                        parent.find('tr[id*="trStatActive4"]').css('display', '');
                        parent.find('tr[id*="trStatActive5"]').css('display', '');
                        parent.find('tr[id*="trSameDataAsFbs"]').css('display', '');
                    }

                if (parent.find('span[id*="lFbdInfo"]').html() == "") {
                    parent.find('span[id*="lFbdInfo"]').html("<b>Регистрация</b> уполномоченного сотрудника по работе с <b>" + parent.find('input[id*="IdSystemName"]').val() + "</b>:");
                }

                // Если ФБД не выбрана или галочка совпадает выбрана, то дизайбл полей, иначе их можно изменить
                var emptyAndReadonly = !$('#<%=this.AllowFbd%>').is(':checked') || $(this).is(':checked');
                if (emptyAndReadonly) {
                    parent.find('input[id*="tbUserFullName"]').prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserFullName_hidden"]').val("disabled");
                    parent.find('input[id*="tbUserPosition"]').prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserPosition_hidden"]').val("disabled");
                    parent.find('input[id*="tbUserEmail"]').prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserEmail_hidden"]').val("disabled");
                    parent.find('input[id*="tbUserPhone"]').prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserPhone_hidden"]').val("disabled");
                }
                else {
                    parent.find('input[id*="tbUserFullName"]').prop('readonly', false).css('background-color', 'rgb(235,243,246)');
                    parent.find('input[id*="tbUserFullName_hidden"]').val("");
                    parent.find('input[id*="tbUserPosition"]').prop('readonly', false).css('background-color', 'rgb(235,243,246)');
                    parent.find('input[id*="tbUserPosition_hidden"]').val("");
                    parent.find('input[id*="tbUserEmail"]').prop('readonly', false).css('background-color', 'rgb(235,243,246)');
                    parent.find('input[id*="tbUserEmail_hidden"]').val("");
                    parent.find('input[id*="tbUserPhone"]').prop('readonly', false).css('background-color', 'rgb(235,243,246)');
                    parent.find('input[id*="tbUserPhone_hidden"]').val("");
                }
            }
        });

        $('input[id*="cbAllowSystem"]').change(function () {
            var result = $(this).is(':checked');
            var parent = $(this).closest("div");
            var id = parent.find('input[id*="hfId"]').val();
            if (id == 1) {
                if (result) {
                    parent.find('input[id*="newPassword"]').prop('readonly', false).css('background-color', 'rgb(235,243,246)');
                    parent.find('input[id*="repeatPassword"]').prop('readonly', false).css('background-color', 'rgb(235,243,246)');
                }
                else {
                    parent.find('input[id*="newPassword"]').prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="repeatPassword"]').prop('readonly', true).css('background-color', 'LightGrey');
                }
            }
            if (id != 3) {
                if (result) {
                    parent.find('input[id*="tbUserFullName"]').prop('readonly', false).css('background-color', 'rgb(235,243,246)');
                    parent.find('input[id*="tbUserFullName_hidden"]').val("");
                    parent.find('input[id*="tbUserPosition"]').prop('readonly', false).css('background-color', 'rgb(235,243,246)');
                    parent.find('input[id*="tbUserPosition_hidden"]').val("");
                    parent.find('input[id*="tbUserEmail"]').prop('readonly', false).css('background-color', 'rgb(235,243,246)');
                    parent.find('input[id*="tbUserEmail_hidden"]').val("");
                    parent.find('input[id*="tbUserPhone"]').prop('readonly', false).css('background-color', 'rgb(235,243,246)');
                    parent.find('input[id*="tbUserPhone_hidden"]').val("");
                }
                else {
                    parent.find($('input[id*="tbUserFullName"]')).prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserFullName_hidden"]').val("disabled");
                    parent.find($('input[id*="tbUserPosition"]')).prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserPosition_hidden"]').val("disabled");
                    parent.find($('input[id*="tbUserEmail"]')).prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserEmail_hidden"]').val("disabled");
                    parent.find($('input[id*="tbUserPhone"]')).prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserPhone_hidden"]').val("disabled");
                }
            }

            // Если ФБС выбрана, то enable галочки, иначе disable и снятие галочки
            if (!$('#<%=this.AllowFbs%>').is(':checked')) {
                $('#<%=this.SameFbs%>').prop('checked', false);
                $('#<%=this.SameFbs%>').attr('disabled', 'disabled');
            }
            else {
                $('#<%=this.SameFbs%>').attr('disabled', false);
            }

            result = $('#<%=this.AllowFbd%>').is(':checked');
            parent = $('#<%=this.AllowFbd%>').closest("div");
            if (!result) {
                $('#<%=this.SameFbs%>').attr('disabled', 'disabled');

                parent.find('input[id*="tbUserFullName"]').prop('readonly', true).css('background-color', 'LightGrey');
                parent.find('input[id*="tbUserFullName_hidden"]').val("disabled");
                parent.find('input[id*="tbUserPosition"]').prop('readonly', true).css('background-color', 'LightGrey');
                parent.find('input[id*="tbUserPosition_hidden"]').val("disabled");
                parent.find('input[id*="tbUserEmail"]').prop('readonly', true).css('background-color', 'LightGrey');
                parent.find('input[id*="tbUserEmail_hidden"]').val("disabled");
                parent.find('input[id*="tbUserPhone"]').prop('readonly', true).css('background-color', 'LightGrey');
                parent.find('input[id*="tbUserPhone_hidden"]').val("disabled");
            }
            else {
                if ($('#<%=this.SameFbs%>').is(":checked")) {
                    parent.find('input[id*="tbUserFullName"]').prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserFullName_hidden"]').val("disabled");
                    parent.find('input[id*="tbUserPosition"]').prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserPosition_hidden"]').val("disabled");
                    parent.find('input[id*="tbUserEmail"]').prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserEmail_hidden"]').val("disabled");
                    parent.find('input[id*="tbUserPhone"]').prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserPhone_hidden"]').val("disabled");
                }
                else {
                    parent.find('input[id*="tbUserFullName"]').prop('readonly', false).css('background-color', 'rgb(235,243,246)');
                    parent.find('input[id*="tbUserFullName_hidden"]').val("");
                    parent.find('input[id*="tbUserPosition"]').prop('readonly', false).css('background-color', 'rgb(235,243,246)');
                    parent.find('input[id*="tbUserPosition_hidden"]').val("");
                    parent.find('input[id*="tbUserEmail"]').prop('readonly', false).css('background-color', 'rgb(235,243,246)');
                    parent.find('input[id*="tbUserEmail_hidden"]').val("");
                    parent.find('input[id*="tbUserPhone"]').prop('readonly', false).css('background-color', 'rgb(235,243,246)');
                    parent.find('input[id*="tbUserPhone_hidden"]').val("");
                }
            }
        });

        $('input[id*="CBSameDataAsFbs"]').change(function () {
            var parent = $(this).closest("div");
            var id = parent.find($('input[id*="hfId"]')).val();

            if (id == 3) {
                var emptyAndReadonly = !$('#<%=this.AllowFbd%>').is(':checked') || $(this).is(':checked');
                if (emptyAndReadonly) {
                    parent.find('input[id*="tbUserFullName"]').prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserFullName_hidden"]').val("disabled");
                    parent.find('input[id*="tbUserPosition"]').prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserPosition_hidden"]').val("disabled");
                    parent.find('input[id*="tbUserEmail"]').prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserEmail_hidden"]').val("disabled");
                    parent.find('input[id*="tbUserPhone"]').prop('readonly', true).css('background-color', 'LightGrey');
                    parent.find('input[id*="tbUserPhone_hidden"]').val("disabled");
                }
                else {
                    parent.find('input[id*="tbUserFullName"]').prop('readonly', false).css('background-color', 'rgb(235,243,246)');
                    parent.find('input[id*="tbUserFullName_hidden"]').val("");
                    parent.find('input[id*="tbUserPosition"]').prop('readonly', false).css('background-color', 'rgb(235,243,246)');
                    parent.find('input[id*="tbUserPosition_hidden"]').val("");
                    parent.find('input[id*="tbUserEmail"]').prop('readonly', false).css('background-color', 'rgb(235,243,246)');
                    parent.find('input[id*="tbUserEmail_hidden"]').val("");
                    parent.find('input[id*="tbUserPhone"]').prop('readonly', false).css('background-color', 'rgb(235,243,246)');
                    parent.find('input[id*="tbUserPhone_hidden"]').val("");
                }
            }
        });
    });
</script>
