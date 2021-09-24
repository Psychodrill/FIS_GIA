<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/Main.Master"
    AutoEventWireup="true" CodeBehind="OrgCardInfo.aspx.cs" Inherits="Esrp.Web.Administration.Organizations.UserDepartments.OrgCardInfo" %>
<%@ Import Namespace="Esrp.Core.Systems" %>

<%@ Register Src="~/Controls/OrganizationView.ascx" TagName="OrganizationView" TagPrefix="uc" %>
<%@ Register TagPrefix="esrp" Namespace="Esrp.Web.Controls" Assembly="Esrp.Web.UI" %>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cphSecondLevelMenu">
			<div class="bottom_line">
				<div class="max_width">
					<esrp:TopMenu ID="SecondLevelMenu1" runat="server" RootResourceKey="organizationUserDepartments" 
                        HeaderTemplate="<ul>" FooterTemplate="</ul>"/>
					<div class="clear"></div>
				</div>
			</div><!--bottom_line-->
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphContent" runat="server">
    <div class="left_col">
        <form id="Form1" runat="server">
            <asp:ValidationSummary CssClass="error_block" ID="ValidationSummary2" runat="server"
                DisplayMode="BulletList" EnableClientScript="false" HeaderText="<p>Произошли следующие ошибки:</p>" />
            <asp:Label ID="lblError" Visible="False" runat="server" ForeColor="Red" />
            <div class="col_in">
                <div class="statement edit">
                    <p class="back">
                        <a id="BackLink" runat="server" href="#">
                            <span class="un">Вернуться</span>
                        </a>
                    </p>
                    <p class="statement_menu">
                        <%if (GeneralSystemManager.CanChangeRCModel(this.User.Identity.Name))
                            { %>
                            <a href="#" class="active" onclick="$('#<%= btnUpdate.ClientID %>').click();">
                                <span>Сохранить</span>
                            </a>
                        <%} else { %>
                            <a href="/Administration/Organizations/OrganizationHistory.aspx?OrgId=<%= GetParamInt("OrgID") %>"
                                title="История изменений" class="gray">История изменений</a>
                        <%} %>
                    </p>
                    <div class="clear">
                    </div>
                    <div class="statement_table">
                            <uc:OrganizationView ID="OrganizationView" CanEdit="False" CanChangeRCModel="True" runat="server" />
                            <%if (GeneralSystemManager.CanChangeRCModel(this.User.Identity.Name))
                                { %>
                                <table width="100%">
                                    <tr>
                                        <%--<td id="noteTd" class="box-submit" style="text-align: left; font-size: .8em;">
                                            <%--(1) Сведения об объеме и структуре приема заполняются для ОУ в целом<br />
                                            (2) Заполняется только для федеральных ОУ<br />
                                            (3) Заполняется только для региональных ОУ
                                        </td>--%>
                                        <td style="width: 560px;">
                                            <asp:Label ID="ForConfirmation" style="color: rgb(215, 5, 5) !important;" runat="server"></asp:Label>
                                        </td>
                                        <td class="box-submit" colspan="2">
                                            <div style="float: right; margin-right: 5px;">
                                                <asp:CheckBox ID="CBXConfirm" runat="server"/>
                                            </div>
                                            <label for="<%=CBXConfirm.ClientID%>" style="float: right;">Подтверждаю введенные сведения</label>
                                        </td>
                                        <td class="box-submit" width="100">                                            
                                            <div style="float: right; margin-right: 5px;">
                                                <asp:Button runat="server" ID="btnUpdate" Text="Сохранить" CssClass="bt" OnClick="BtnUpdateClick" Enabled="False" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            <%} %>
                            <script type="text/javascript">
                                $('#<%=CBXConfirm.ClientID%>').change(function() {
                                    if ($('#<%=CBXConfirm.ClientID%>').is(':checked')) {
                                        $('#<%=btnUpdate.ClientID%>').removeAttr('disabled');
                                    } else {
                                        $('#<%=btnUpdate.ClientID%>').attr('disabled', 'disabled');
                                    }
                                });
                            </script>
                    </div>
                </div>
            </div>
        </form>
    </div>
</asp:Content>
