<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Support.aspx.cs" Inherits="Esrp.Web.Support"
    MasterPageFile="~/Common/Templates/Main.Master" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
<div class="col_in">
    <div class="content">
        <p>Если у вас возникли вопросы по работе с ФИС ГИА и приема, рекомендуем сначала обратиться к разделу 
        <a href="/Instructions.aspx" title="Инструкции">Инструкции</a>, возможно там уже есть ответ на Ваш вопрос.</p>
        <p>
        Контактная информация Горячей линии ФИС ГИА и приема:
	    </p>
        <div class="list_link">
        <ul style="margin-left:3em;">
            <li>Электронная почта: <a href="mailto:<%=Esrp.Web.Config.SupportMail %>"><%=Esrp.Web.Config.SupportMail %></a></li>
            <li>Телефон: <%=Esrp.Web.Config.SupportPhone %></li>
            <li>Горячая линия ФИС ГИА и приема осуществляет работу по московскому времени<br></br>
        </ul>
        <p>График работы горячей линии: </p>
        <ul style="margin-left:3em;">
            <li>С 15 мая по 30 сентября по будням с 9:00 до 20:00 по московскому времени;</li>
            <li>С 1 октября по 14 мая по будням с 09:00 до 18:00 по московскому времени.</li>
        </ul>
        </div>
    </div>
</div>

</asp:Content>