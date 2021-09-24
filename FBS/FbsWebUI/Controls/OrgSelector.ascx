<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrgSelector.ascx.cs" Inherits="Fbs.Web.Controls.OrgSelector" %>
<div class="org-selector" style="width:300px;position:relative">
    <asp:HiddenField runat="server" ID="selectedOrg" />
    <asp:TextBox Text="Выберите образовательное учреждение" style="width:100%;padding:5px;margin-left:5px" TextMode="MultiLine" Rows="5" Columns="36" Enabled="false" runat="server" ID="selectedOrgName" /> 
    <br />
    <asp:Button CssClass="immutable" style="margin-left:5px" ID="SelectOrganization" runat="server" Text="Выбрать" CausesValidation="False"
		OnClick="SelectOrganization_Click" />
       
</div>

