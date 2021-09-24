<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common/Templates/Main.Master" %>

<%@ Register TagPrefix="esrp" TagName="RssNewsList" Src="~/Controls/RssNewsList.ascx" %>
<%@ Register TagPrefix="esrp" TagName="RssDocumentList" Src="~/Controls/RssDocumentList.ascx" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphHead">

    <script type="text/javascript" src="/Common/Scripts/Notice.js"> </script>

</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    <div class="col_in">
        <div class="news">
            <p class="title"><strong><a href="/Documents.aspx"><span class="un_solid">Последние документы</span><a href="/RssHandler.ashx?type=docs">&nbsp;<img src="Common/images/bg/rss.png" alt="" /></a></strong></p>
            <esrp:RssDocumentList runat="server" ID="docsList"></esrp:RssDocumentList>
            
        </div>
        <!--news-->
        <div class="page content">
            <h3>Единая система регистрации пользователей</h3>
            <p>Единая система регистрации пользователей (ЕСРП) предназначена для регистрации пользователей в следующих информационных системах Федеральной службы по надзору в сфере образования и науки (Рособрнадзор):<br></br>
Федеральная информационная система обеспечения проведения государственной итоговой аттестации обучающихся, освоивших основные образовательные программы основного общего и среднего общего образования, и приема граждан в образовательные организации для получения среднего профессионального и высшего образования и региональных информационных системах обеспечения проведения государственной итоговой аттестации обучающихся, освоивших основные образовательные программы основного общего и среднего общего образования (ФИС ГИА и приема);<br>
</br>Федеральная база свидетельств о результатах единого государственного экзамена (ФБС), которая включена в подсистему ФИС ГИА и приема  "Результаты ЕГЭ".</p>
            <p>&nbsp;</p>
            <h3>Вниманию частных лиц</h3>
            <p>В информационных системах Рособрнадзора осуществляется регистрация только сотрудников образовательных учреждений и организаций, участвующих и/или обеспечивающих проведение единого государственного экзамена и приема граждан в образовательные учреждения среднего и высшего профессионального образования </p>
            <p>Процедура регистрации включает в себя обязательное предоставление письменного 
                подтверждения регистрации пользователя руководителем организации.</p>
            <div class="clear"></div>
        </div>

        <div class="clear"></div>

        <div class="news_list">
            <p class="title"><strong><a href="/NewsArchive.aspx"><span class="un_solid">Новости</span><a href="/RssHandler.ashx?type=news">&nbsp;<img src="Common/images/bg/rss.png" alt="" /></a></strong></p>
            <esrp:RssNewsList runat="server" ID="newsList"></esrp:RssNewsList>
            <div class="clear"></div>
        </div>
    </div>
</asp:Content>
