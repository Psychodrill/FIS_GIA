<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/Administration.Master"
	AutoEventWireup="true" CodeBehind="OrgCard_Edit_Success.aspx.cs" Inherits="Fbs.Web.Administration.Organizations.Administrators.OrgCard_Edit_Success" %>
<%@ Register src="~/Controls/OrganizationView.ascx" TagName="OrganizationView" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphLeftMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphActions" runat="server">
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
			<ul>
				<li><a href="/Administration/Organizations/OrganizationHistory.aspx?OrgId=<%= GetParamInt("OrgID") %>"
					title="История изменений" class="gray">История изменений</a></li>
                <li>
                    <a href="/Administration/Organizations/CertificateList.aspx?OrgId=<%= GetParamInt("OrgID") %>"
					title="История проверок свидетельств организацией" class="gray">История проверок свидетельств организацией</a>
                </li>
			</ul>
		</div>
		<div class="br">
			<div class="tt">
				<div>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphContent" runat="server">
	<uc:OrganizationView ID="OrganizationView" runat="server" />    
</asp:Content>
