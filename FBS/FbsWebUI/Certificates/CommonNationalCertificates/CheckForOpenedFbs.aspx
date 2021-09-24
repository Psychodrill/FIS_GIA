<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckForOpenedFbs.aspx.cs" 
    Inherits="Fbs.Web.Certificates.CommonNationalCertificates.CheckForOpenedFbs"
    MasterPageFile="~/Common/Templates/Certificates.Master" %>
<%@ Register TagPrefix="fbs" TagName="HistoryCheck" Src="~/Controls/CommonNationalCertificates/HistoryCheckList.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphCertificateHead" runat="server">
    <script type="text/javascript" src="/Common/Scripts/Utils.js"></script>
    <script type="text/javascript" src="/Common/Scripts/SessVars.js"></script>
    <script type="text/javascript" src="/Common/Scripts/Notice.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphCertificateContent" runat="server">

<form id="Form1" runat="server" defaultbutton="btnCheck">

    <asp:ValidationSummary runat="server" DisplayMode="BulletList" EnableClientScript="false"
        HeaderText="<p>Произошли следующие ошибки:</p>"/>
        
     <table style="width:620px;">
        <tr>
           <td colspan="2">
                <div class="notice" id="CheckTitleNotice">
                    <div class="top">
                    <div class="l"></div>
                    <div class="r"></div>
                    <div class="m"></div>
                </div>
                <div class="cont">
                <div dir="ltr" class="hide" title="Свернуть" onclick="ToggleNoticeState(this);">x<span></span></div>
                    <div class="txt-in">
                        <p>Поиск и проверка свидетельств осуществляются по строгому соответствию параметров запроса «Регистрационный номер свидетельства» и баллам как минимум по двум предметам ЕГЭ хранящимся в Подсистеме ФИС &laquo;Результаты ЕГЭ&raquo;.
                           Поле «Регистрационный номер свидетельства» и баллы как минимум по двум предметам ЕГЭ обязательны для заполнения.
                    </div>
                </div>
                    <div class="bottom">
                        <div class="l"></div>
                        <div class="r"></div>
                        <div class="m"></div>
                    </div>
                </div>   
            </td>
        </tr>
        <tr>
            <td>
                <div class="form-l">
                    <asp:TextBox runat="server" ID="txtNumber" CssClass="txt" />
                    <input id="cNumber" value="Номер свидетельства" class ="txt h" style="display:none" />

                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ControlToValidate="txtNumber" EnableClientScript="false" Display="None"
                        ErrorMessage='Поле "Номер свидетельства" обязательно для заполнения' /> 
                    
                     <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                        ControlToValidate="txtNumber" EnableClientScript="false" Display="None"
                        ErrorMessage='Номер свидетельства должен быть в формате XX-XXXXXXXXX-XX' 
                        ValidationExpression="\d{2}-\d{9}-\d{2}" />  
                
                    <script type="text/javascript">
                        IntiInputWithDefaultValue("<%= txtNumber.ClientID %>", "cNumber");
                    </script>
                </div> 

                <div class="PaddingContainer5">
                    <asp:Repeater runat="server"  ID="rpSubjects" DataSourceID="dsSubjects">
                        <HeaderTemplate>
                            <table class="form-r">
                                <tr><th colspan="2"><div><div>Баллы</div></div></th></tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="odd"><td class="left-td">
                                <%#Eval("Name").ToString()%>
                            </td><td>
                                <asp:HiddenField ID="hfId" runat="server" Value='<%#Eval("Id")%>' />
                                <asp:TextBox ID="txtValue" runat="server" CssClass="txt-sm" MaxLength="4" />
                            </td></tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr><td class="left-td">
                                <%#Eval("Name").ToString()%>
                            </td><td>
                                <asp:HiddenField ID="hfId" runat="server" Value='<%#Eval("Id")%>' />
                                <asp:TextBox ID="txtValue" runat="server" CssClass="txt-sm" MaxLength="4" />
                            </td></tr>
                        </AlternatingItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
            
                    <asp:CustomValidator runat="server" ID="cvSubjectMarks" EnableClientScript="false" Display="None" ErrorMessage="Проверьте правильность заполнения баллов" />
                    <asp:CustomValidator runat="server" ID="cvSubjectMarksEmpty" EnableClientScript="false" Display="None" ErrorMessage="Укажите баллы хотя бы по двум предметам" />  
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="t-line">
                <asp:Button runat="server" ID="btnReset" OnClick="BtnResetClick" 
                    Text="Очистить" CssClass="bt" />
                <asp:Button runat="server" ID="btnCheck" OnClick="BtnCheckClick"
                    Text="Проверить" CssClass="bt" />
            </td>
        </tr>
    </table>

    <fbs:HistoryCheck runat="server" ID="historyCheck" 
        DataGridSelectCommandName="SelectCheckHystory" 
        PagerSelectCommandName="GetNEWebUICheckLog"
        Type="CNENumberOpen"/>

    <script type="text/javascript">
        InitNotice();
    </script>   

    <%--  
        Для восстановления состояния контролов необходим DataBind репитера. Второй раз DataBind 
        вызывается самим asp.net при окончательной отрисовкой страницы.
        Кэшированием датасорса я предотвращаю выполнение повторного запроса к базе при DataBind.
    --%>     
    <asp:SqlDataSource runat="server" ID="dsSubjects" 
        EnableCaching="true" CacheDuration="1"
        ConnectionString="<%$ ConnectionStrings:Fbs.Core.Properties.Settings.FbsConnectionString %>"
        SelectCommand="dbo.GetSubject"  CancelSelectOnNullParameter="false"
        SelectCommandType="StoredProcedure"> 
    </asp:SqlDataSource>
    
    </form>
</asp:Content>
