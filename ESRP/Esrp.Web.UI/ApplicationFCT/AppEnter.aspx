<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppEnter.aspx.cs" Inherits="Esrp.Web.ApplicationFCT.AppEnter" 
MasterPageFile="~/Common/Templates/Main.master" ValidateRequest="False" MaintainScrollPositionOnPostback="true" %>
<%@ Import Namespace="Esrp.Core.Systems" %>
<%@ Import Namespace="Esrp.Web" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphContent">
    <script type="text/javascript" src="/Common/Scripts/jquery-1.6.1.min.js"> </script>
    <script type="text/javascript" src="/Common/Scripts/cusel-min-2.5.js"></script>
    <script type="text/javascript" src="/Common/Scripts/js.js"></script>
    <script language="javascript" type="text/javascript">
    </script>

    <form id="form1" runat="server">
        <div class="content">			
            <p><strong>Здравствуйте!</strong> Данный сервис помогает корректно создать заявку для согласования подключения к ЗКСПД ФГБУ «ФЦТ» по схеме защищенного взаимодействия №1.</p><br/>
            <p>Согласно порядку согласования схем подключения к ЗКСПД ФГБУ «ФЦТ» требуется согласовать схему подключения в электронном виде. При этом необходимо направить в адрес ФГБУ «ФЦТ» электронные версии следующих документов:</p>
        
			 <div  style="margin-left: 40px; font-size:15px;" >
				<p style="padding:0px;">• карточка подключаемого объекта (Форма 1);</p>
				<p style="padding:0px;">• схема подключения (Форма 2);</p>
				<p style="padding:0px;">• приказ о назначении ответственного лица, отвечающего за вопросы реализации и поддержки работоспособности и корректности эксплуатации схемы подключения к ФГБУ «ФЦТ».</p>
			</div>
        
            <p>После оформления заявки, в адрес ОУ (на e-mail ответственного лица) будет направлен ответ с описанием дальнейших действий.</p>
			<p>В информационных системах Рособрнадзора осуществляется регистрация только сотрудников образовательных учреждений и организаций, участвующих и/или обеспечивающих проведение единого государственного экзамена и приема граждан в образовательные учреждения среднего и высшего профессионального образования.</p><br/>
			<p>Перед началом работы с сервисом, необходимо ознакомиться со следующими нормативными документами:</p>
            <div style="list-style-type:decimal; margin-left: 40px; font-size:15px;">
				<p style="padding:0px;">Технические условия для подключения к ЗКСПД ФГБУ «ФЦТ» (опубликованы на сайте <a href="http://priem.edu.ru">http://priem.edu.ru</a>, в разделе «документы»);</p>
				<p style="padding:0px;">Порядок согласования схем подключения к ЗКСПД ФГБУ «ФЦТ» (опубликованы на сайте <a href="http://priem.edu.ru">http://priem.edu.ru</a> в разделе «инструкции»);</p>
				<p style="padding:0px;">Методические рекомендации по реализации ТУ на подключение  (опубликованы на сайте <a href="http://priem.edu.ru">http://priem.edu.ru</a>, в разделе «документы»);</p>
				<p style="padding:0px;">Ответы на наиболее популярные вопросы по подключению к ЗКСПД ФЦТ (опубликованы на сайте <a href="http://priem.edu.ru">http://priem.edu.ru</a> в разделе «инструкции»).</p>
			</div>
			<p>
             <asp:checkbox runat="server" Text="<i>Подтверждаем, что ознакомились с документами</i>"  id="agree" />
            </p>
         <div class="clear"></div>			
        </div>
        <asp:Panel runat="server" ID="errpanel" Visible="false">
         <asp:label ID="error_lab" style="color:red;padding:0px;" runat="server" Text="Пожалуйста, ознакомьтесь с методическими документами перед оформлением заявки для согласования схемы подключения."  CssClass="error_block" />
        </asp:Panel>
        <br />
        <asp:Button runat="server" ID="btnGotoStep2" Text="Далее >>" CssClass="bt" OnClick="ValidateEnter" Width="100px" />        
    </form>
</asp:Content>
