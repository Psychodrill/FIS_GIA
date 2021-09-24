<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountKeyList.aspx.cs" 
    Inherits="Esrp.Web.Administration.Accounts.Users.AccountKeyList"
    MasterPageFile="~/Common/Templates/Administration.Master" %>
<%@ Register TagPrefix="web" Namespace="WebControls" Assembly="WebControls" %> 


<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<div class="left_col">
    <form id="Form2" runat="server">
                <div class="col_in">
					<div class="statement edit">							
							<p class="title">Логин/Е-mail&nbsp;<%= CurrentUserLogin %></p>
							<p class="back"><a id="BackLink" runat="server" href="#"><span class="un">Вернуться</span></a></p>
							<p class="statement_menu">
								<a href="/Administration/Accounts/Users/Edit<%= GetUserKeyCode() %>.aspx?Login=<%= Login %>&UserKey=<%=GetUserKeyCode() %>" 
                                    class="gray"><span>Изменить</span></a>
                                <a href="/Administration/Accounts/Users/ChangePassword.aspx?Login=<%= Login %>&UserKey=<%=GetUserKeyCode() %>" 
                                    title="Изменить пароль" class="gray"><span>Изменить пароль</span></a>
                                <a href="/Administration/Accounts/Users/History.aspx?Login=<%= Login %>&UserKey=<%=GetUserKeyCode() %>" 
                                    title="История изменений" class="gray"><span>История изменений</span></a>
                                <a href="/Administration/Accounts/Users/AuthenticationHistory.aspx?Login=<%= Login %>&UserKey=<%=GetUserKeyCode() %>" 
                                    title="История аутентификаци" class="gray"><span>История аутентификаций</span></a>
                                <a href="#" title="Ключи доступа" class="active"><span>Ключи доступа</span></a>
							</p>
							<div class="clear"></div>
                        </div>
                <div class="main_table">
					<div class="sort table_header">
                        <div class="f_left">
                            <a href="/Administration/Accounts/Users/AccountKeyCreate.aspx?login=<%= Login %>&UserKey=<%=GetUserKeyCode() %>" 
                                title="Создать ключ доступа" class="create_user">Создать ключ доступа</a> 
						</div>
                        <div class="sorted">
                            <div class="clear"></div>
                        </div>
                    </div>
					<div class="clear"></div>

    <asp:DataGrid runat="server" id="dgAccountKeyList"
        DataSourceID="dsAccountKeyList"
        AutoGenerateColumns="false" 
        EnableViewState="false"
        ShowHeader="True" 
        GridLines="None"
        UseAccessibleHeader="true"
        CssClass="table-th">
        <HeaderStyle CssClass="actions" />
        <Columns>
            <asp:TemplateColumn>
            <HeaderStyle Width="20%"/>
            <HeaderTemplate>
                <div>Ключ</div>
            </HeaderTemplate>
            <ItemTemplate>
                <a href="/Administration/Accounts/Users/AccountKeyEdit.aspx?login=<%= Login %>&key=<%# Eval("Key") %>&UserKey=<%=GetUserKeyCode() %>" title="Редактировать ключ доступа" ><%# Eval("Key") %></a>
            </ItemTemplate>
            </asp:TemplateColumn>

            <asp:TemplateColumn>
            <HeaderStyle Width="15%" />
            <HeaderTemplate>
                Действителен с
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.IsDBNull(Eval("DateFrom")) ? String.Empty : Convert.ToDateTime(Eval("DateFrom")).ToShortDateString()%>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderStyle Width="15%" />
            <HeaderTemplate>
                Действителен по
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.IsDBNull(Eval("DateTo")) ? String.Empty : Convert.ToDateTime(Eval("DateTo")).ToShortDateString() %>
            </ItemTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn>
            <HeaderStyle Width="10%"/>
            <HeaderTemplate>
                <div> Активен </div>
            </HeaderTemplate>
            <ItemTemplate>
                <%# Convert.ToBoolean(Eval("IsActive")) ? "Да" : "Нет" %>
            </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

    <web:NoRecordsText runat="server" ControlId="dgAccountKeyList">
        <Message><p class="notfound">Ключи доступа отсутствуют</p></Message>
    </web:NoRecordsText>
    
    <asp:SqlDataSource runat="server" ID="dsAccountKeyList" CancelSelectOnNullParameter="false"
        ConnectionString="<%$ ConnectionStrings:Esrp.Core.Properties.Settings.EsrpConnectionString %>"
        SelectCommand="dbo.SearchAccountKey"  SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="login" Name="login" ConvertEmptyStringToNull="true" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>

    </div>
    </div>
    </form>

    </div>   
</asp:Content>

