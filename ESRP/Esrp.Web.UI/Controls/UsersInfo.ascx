<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UsersInfo.ascx.cs" Inherits="Esrp.Web.Controls.UsersInfo" %>
<%@ Import Namespace="Esrp.Web" %>

<asp:ListView ID="fvUsersInfo" runat="server" CellPadding="8" DataSourceID="odsUsersInfo">
    <LayoutTemplate>
        <table width="100%">
            <tr runat="server" id="itemPlaceholder"></tr>
        </table>
    </LayoutTemplate>
    <ItemTemplate>
            <tr>
                <th>
                    Логин             
                </th>
                <td>
                    <a href='../Accounts/Users/View.aspx?login=<%#Eval("Login")%>'><%#Eval("Login")%></a>
                </td>
                <td width="10%">
                <input type="submit" value="Изменить" title="Изменить регистрационные данные"
                        onclick="correctRedirectToURLfromJS('/Administration/Accounts/Users/Edit.aspx?login=<%#Eval("Login")%>&req_id=<%= RequestID %>'); return false"/>
                </td>
            </tr>
		    <tr>
			    <th>
				    Ф. И. О.
			    </th>
			    <td colspan="2">
                    <asp:Label runat="server" ID="lFIO" Text='<%#Eval("FIO")%>' />
			    </td>
		    </tr>
		    <tr>
			    <th>
				    Система
			    </th>
			    <td colspan="2">
                    <asp:Label runat="server" ID="lblSystems" Text='<%#Eval("SystemNames")%>' />
			    </td>
		    </tr>
		    <tr>
			    <th>
				    Состояние
			    </th>
			    <td colspan="2">
				    <%#Eval("StatusText")%>
			    </td>
		    </tr>
		    <tr>
			    <th>
				    Скан заявки на регистрацию
			    </th>
			    <td colspan="2">
				    <%#Eval("RegDocument")%>
			    </td>
		    </tr>
            <tr>
                <td colspan="3" style="border-bottom-color:#fff;"></td>
            </tr>
    </ItemTemplate>
</asp:ListView>

<asp:ObjectDataSource ID="odsUsersInfo" runat="server" 
    DataObjectTypeName="Esrp.Web.ViewModel.Users.UserViewForRequest" TypeName="Esrp.Services.UsersService" SelectMethod="SelectUsersByRequest" OnSelecting="UsersByRequestOnSelecting" OnSelected="UsersByRequestOnSelected" />
