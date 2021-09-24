<%@ Page Language="C#" MasterPageFile="~/Common/Templates/Administration.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Esrp.Web.Administration.Reports.Default" %>

<%@ Register TagPrefix="esrp" Namespace="Esrp.Web.Controls" Assembly="Esrp.Web.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="server">
    <link media="all" href="/Common/Styles/ReportStyles.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <form id="Form1" runat="server">
        <asp:ScriptManager ID="AjaxScriptManager" AsyncPostBackTimeout="0" runat="server" />
        <div class="main_table">
		  
		    <div class="clear"></div>
        <asp:ListView runat="server" ID="reportsList">
            <EmptyDataTemplate>
                <p class="warning">
                    Отчеты в системе не найдены
                </p>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <table class="table-th" cellspacing="0" border="0"  style="width:100%;border-collapse:collapse;">
                    <thead>
                        <tr class="actions">
                            <th>
                                №
                            </th>
                            <th>
                                Название отчета
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server"/>
                    </tbody>
	            </table>
                
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%#Container.DataItemIndex +1 %>
                    </td>
                    <td>
                        <a href='<%#DataBinder.Eval(Container.DataItem,"Url")%>' target="_blank"><%#DataBinder.Eval(Container.DataItem,"Title") %></a>
                    </tr>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        </div>
    </form>
    
</asp:Content>