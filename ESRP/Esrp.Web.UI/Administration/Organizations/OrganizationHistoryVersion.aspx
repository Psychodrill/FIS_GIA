<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrganizationHistoryVersion.aspx.cs"
    Inherits="Esrp.Web.Administration.Organizations.OrganizationHistoryVersion" MasterPageFile="~/Common/Templates/Administration.Master" %>

<%@ Register src="~/Controls/OrganizationView.ascx" TagName="OrganizationView" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphLeftMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphActions" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphContent" runat="server">
<div class="left_col">
                    <div class="col_in">
						<div class="statement edit">
							
							<p class="title">Организация&nbsp;<%= this.currentOrg.FullName %></p>
							<p class="back"><a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
							<p class="statement_menu">
                                <a href="/Administration/Organizations/Administrators/OrgCard_Edit.aspx?OrgID=<%=this.currentOrg.Id%>" 
                                    class="gray"><span>Изменить</span></a>
                                <a href="/Administration/Organizations/OrganizationHistory.aspx?OrgId=<%=this.currentOrg.Id%>" 
                                    title="История изменений" class="active"><span>История изменений</span></a>
							</p>
							<div class="clear"></div>
                            <div class="statement edit">
							    <p class="title">Редакция № <%= this.GetParamInt("Version")%></p>
                            </div>
                            <div class="statement_table">
                                <form runat="server">
                                    <uc:OrganizationView ID="OrganizationView" runat="server" />
                                </form>
                            </div>
                        </div>
                </div> 
</div>
</asp:Content>
