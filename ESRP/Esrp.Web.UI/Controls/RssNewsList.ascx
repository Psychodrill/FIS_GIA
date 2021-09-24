<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RssNewsList.ascx.cs" Inherits="Esrp.Web.Controls.RssNewsList" %>

<asp:ListView ID="lvNews" runat="server">
    <LayoutTemplate>
        <p></p>
        <table width="100%">
            <tr runat="server" id="itemPlaceholder">
            </tr>
	    </table>
    </LayoutTemplate>
    <ItemTemplate>
			    <td width="33%">
				    <p><a href='../News.aspx?id=<%#Eval("Id")%>' class="un"><%#Eval("Name")%></a></p>
				    <p><%#((DateTime)Eval("CreateDate")).ToString("d MMMM yyyy")%></p>
			    </td>
    </ItemTemplate>
</asp:ListView>