<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperatorPanel.ascx.cs" Inherits="Fbs.Web.Controls.OperatorPanel" %>
<table style="border: 1px solid #C0C0C0;">
    <tr>
        <td align=left colspan=2>
        <asp:Button ID="btnGetNewUser" runat="server" Text="Следующий пользователь" 
    ToolTip="Проверка следующего &quot;свободного&quot; пользователя" 
    Width="100%" onclick="btnGetNewUser_Click" />
        </td>
    </tr>
    <tr>
        <td style="background:lightgrey; width:200px;" colspan=2>
            Оператор:
            &nbsp;<asp:Label ID="lblOperatorName" runat="server"/>
            &nbsp;<asp:Image ID="imgEditStatus" runat="server" ImageUrl="../Common/Images/Warning.png" Visible=false/>
        </td>
    </tr>
    <tr>
        <td colspan=2>
Комментарий:<br/>            
<asp:TextBox ID="txtComments" runat="server" MaxLength="1024" Rows="5"  EnableViewState=false
    TextMode="MultiLine" Width="100%"></asp:TextBox>
            <br />
<asp:Button ID="btnEditComment" runat="server" Text="Сохранить" 
                onclick="btnEditComment_Click" EnableViewState=false />
        </td>
    </tr>
    <tr>
        <td>
            Мои:
            <div style="height:100px; width:100px; overflow:auto;padding-left:15px;">
                <ol>
                    <asp:Repeater ID="rptMyUsers" runat=server>
                        <ItemTemplate>
                            <li><a href="./View.aspx?login=<%# Eval("login") %>" Title="<%# Eval("FIO") %>"><%# Eval("login") %></a></li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ol>
            </div>
        </td>
        <td >
        С комментами:
            <div style="height:100px; width:100px; overflow:auto;padding-left:15px;">
                <ol>
                    <asp:Repeater ID="rptMyUsersWithComment" runat=server>
                        <ItemTemplate>
                            <li><a href="./View.aspx?login=<%# Eval("login") %>" Title="<%# Eval("FIO") %>"><%# Eval("login") %></a></li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ol>
            </div>
        </td>
    </tr>
</table>
