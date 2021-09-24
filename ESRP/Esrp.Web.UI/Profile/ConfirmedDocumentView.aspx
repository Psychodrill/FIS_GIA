<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfirmedDocumentView.aspx.cs" Inherits="Esrp.Web.Profile.ConfirmedDocumentView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel runat=server ID="pnlError" Visible="false">
            <asp:Label runat=server ID="lblErrorMsg" Font-Bold=true ForeColor=DarkRed></asp:Label>
        </asp:Panel>
        
        <asp:Panel runat=server ID="pnlNotImage" Visible="false">
         Регистрационный документ имеет тип '<asp:Label runat=server ID="lblDocType" Text="" />'  и не может быть отображён как картинка. <br/>
         Вы можете <a href='ConfirmedDocument.aspx?login=<%=GetLogin() %>'>скачать документ</a>. 
         Размер: <asp:Label runat=server ID="lblDocSize_notImg" Text="" /> Кбайт.
        </asp:Panel>
        
        
        <asp:Panel runat=server ID="pnlImage" Visible="false">
           Масштаб: 
           <asp:DropDownList runat=server id="ddlChangeSize" AutoPostBack="true" OnSelectedIndexChanged="ddlChangeSize_SelectedIndexChanged">
                <asp:ListItem>50%</asp:ListItem>
                <asp:ListItem>75%</asp:ListItem>
                <asp:ListItem>100%</asp:ListItem>
                <asp:ListItem>150%</asp:ListItem>
                <asp:ListItem>200%</asp:ListItem>
                <asp:ListItem>300%</asp:ListItem>
                <asp:ListItem>500%</asp:ListItem>
            </asp:DropDownList>
           <a href='ConfirmedDocument.aspx?login=<%=GetLogin() %>'>Cкачать</a>
           (<asp:Label runat=server ID="lblDocSize_img" Text="" /> Кб)<BR/>
            <asp:Image runat=server ID="img" AlternateText="Скан регистрационного документа" />
        </asp:Panel>
    </form>
</body>
</html>
