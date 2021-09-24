<%@ Page Title="" Language="C#" MasterPageFile="~/Common/Templates/Loginless.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="Esrp.Web.WebUserAccount.ResetPassword" %>
<%@ Register Assembly="Esrp.Web.UI" Namespace="Esrp.Web.Controls" TagPrefix="esrp" %>
<%@ Import Namespace="Esrp.Web" %>
<%@ Import Namespace="Esrp.Core.Systems" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphContentModalDialog" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphSecondLevelMenu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphThirdLevelMenu" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContent" runat="server">
<form runat="server">
<asp:Panel runat="server" ID="resetPasswordPanel">
     <asp:ValidationSummary ID="ValidationSummary1" CssClass="error_block"  runat="server" DisplayMode="BulletList" EnableClientScript="true"
        HeaderText='<p class="l8">Произошли следующие ошибки:</p>' />
     <div class="col_in">
	    <div class="statement edit">
            <p class="title">Задать пароль для пользователя <asp:Label runat="server" ID="userName" /></p>
            <div class="clear"></div>
            <div class="statement_table">
                <table >
                    <tr runat="server" id="oldPwdRow">
                        <th style="border: 0px;"><b>Текущий пароль</b><span style="color:  rgb(215, 5, 5) !important;"><%=this.Page.Required()%></span>:</th>
                        <td width="1" style="border: 0px;">
                            <asp:TextBox runat="server" ID="password" TextMode="Password" CssClass="txt short" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="password" 
                            EnableClientScript="true" Display="Dynamic" Text="*"
                            ErrorMessage='Поле "Текущий пароль" обязательно для заполнения' />
                        </td>
                    </tr>
       
                    <tr>
                        <th>
                            
                            <asp:Label runat="server" AssociatedControlID="newPassword">
                                <b>Новый пароль</b><span style="color:  rgb(215, 5, 5) !important;"><%=this.Page.Required()%></span>:
                            </asp:Label>
                        </th>
                        <td>
                            <asp:TextBox runat="server" ID="newPassword" TextMode="Password" CssClass="txt short"/>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="newPassword" ErrorMessage="Введите новый пароль" Text="*" Display="None" />
                            <esrp:MinLengthValidator MinLength="6"  runat="server" ID="rvPassword" ControlToValidate="newPassword"  ErrorMessage="Длина пароля должна быть не менее 6 символов" Display="None" Text="*" />
                            <asp:RegularExpressionValidator runat="server" Text="*" ErrorMessage='Поле "Новый пароль" должно содержать цифры, строчные, заглавные буквы' ControlToValidate="newPassword" ValidationExpression="^.*(?=.{6,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*$" Display="None" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="Label1" runat="server" AssociatedControlID="repeatPassword">
                                <b>Повторите новый пароль</b><span style="color:  rgb(215, 5, 5) !important;"><%=this.Page.Required()%></span>:
                            </asp:Label>
                        </th>
                        <td>
                            <asp:TextBox runat="server" ID="repeatPassword" TextMode="Password" CssClass="txt short"/>
                            <asp:RequiredFieldValidator runat="server" ErrorMessage="Повторите пароль" ControlToValidate="repeatPassword" Display="None" Text="*" />
                            <asp:CompareValidator runat="server" ControlToValidate="repeatPassword" ControlToCompare="newPassword" ErrorMessage="Введённые пароли не совпадают" Display="None" Text="*" />
                        </td>
                    </tr>
                    
                </table>
                <asp:CustomValidator ID="validPwdValidator" runat="server" ErrorMessage="Пароль введен неверно" Display="None" ControlToValidate="password" />
                  <p class="save">
                    <asp:Button runat="server" id="btnReset" Text="Задать пароль" CssClass="bt" onclick="btnReset_Click" />
		        </p>
            </div>
        </div>
    </div>
    </asp:Panel>
    <asp:Panel runat="server"  ID="resetSuccessPanel" Visible="false">
        <p>
            Пароль был успешно изменен. Вы можете <asp:Hyperlink ID="Hyperlink1" runat="server" NavigateUrl="~/Login.aspx" Text="войти в систему"/> используя новый пароль
        </p>
    </asp:Panel>
    <asp:Panel runat="server"  ID="registrationSuccessPanel" Visible="false">
        <div class="statement_table" style="margin-top: 0px;">
            <p>Шаг 1 из 3 успешно пройден. Для продолжения регистрации вам необходимо <asp:Hyperlink runat="server" NavigateUrl="~/Login.aspx" Text="войти в систему"/>, используя ваш новый логин и пароль.
            </p>         
        </div>
    </asp:Panel>
</form>
</asp:Content>
