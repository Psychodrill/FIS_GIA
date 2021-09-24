<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RssDocumentList.ascx.cs" Inherits="Esrp.Web.Controls.RssDocumentList" %>

<asp:ListView ID="lvDocuments" runat="server">
    <LayoutTemplate>
        <div runat="server" id="itemPlaceholder"></div>
    </LayoutTemplate>
    <ItemTemplate>
        <p><a href='../Document.aspx?id=<%#Eval("Id")%>' class="un"><%#Eval("Name")%></a></p>
    </ItemTemplate>
</asp:ListView>
