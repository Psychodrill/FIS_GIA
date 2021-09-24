<%@ Control Language="C#" Inherits="ViewUserControl<FindPersonResultModel>" %>

<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Data.Model" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.OlympicDiplomant" %>
<%@ Import Namespace="GVUZ.Data.Helpers" %>

<div class="container">
    <div>
        <h3>Победитель/призер олимпиады не найден в базе данных участников ЕГЭ. Уточните параметры поиска по неполным данным:</h3>
    </div>
</div>

<table>
    <tr>
        <td>
            <input type="checkbox" checked="checked" id="filCheckLastName" /></td>
        <td>Фамилия</td>
        <td>
            <input type="text" id="filLastName" /></td>
    </tr>
    <tr>
        <td>
            <input type="checkbox" checked="checked" id="filCheckFirstName" /></td>
        <td>Имя</td>
        <td>
            <input type="text" id="filFirstName" /></td>
    </tr>
    <tr>
        <td>
            <input type="checkbox" id="filCheckMiddleName" /></td>
        <td>Отчество</td>
        <td>
            <input type="text" id="filMiddleName" /></td>
    </tr>
    <tr>
        <td>
            <input type="checkbox" id="filCheckBirthDate" /></td>
        <td>Дата рождения</td>
        <td>
            <input type="text" id="filBirthDate" maxlength="10" class="shortInput datePicker" /></td>
    </tr>
    <tr>
        <td>
            <input type="checkbox" id="filCheckIdentityDocumentTypeID" /></td>
        <td>Тип документа</td>
        <td>
            <select id="filIdentityDocumentTypeID" /></td>
    </tr>
    <tr>
        <td>
            <input type="checkbox" id="filCheckDocumentSeries" /></td>
        <td>Серия документа</td>
        <td>
            <input type="text" id="filDocumentSeries" /></td>
    </tr>
    <tr>
        <td>
            <input type="checkbox" id="filCheckDocumentNumber" /></td>
        <td>Номер документа</td>
        <td>
            <input type="text" id="filDocumentNumber" /></td>
    </tr>
</table>

<hr />
<div>
    <button type="button" id="filFind">Поиск</button>
</div>

<div id="findresultsrows">
</div>

<hr />

<div>
    <input name="filsel" type="radio" checked="checked" id="filNotFind" itemid="0">Участник не найден</input>
    <input id="filComment" type="text" placeholder="Обязательно введите причину..." style="margin-left:20px; width:500px;" />
</div>

<script type="text/javascript">
    StatusId = '<%= Model.Status %>';

    $("#filBirthDate").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '/Resources/Images/calendar.jpg', buttonImageOnly: true, yearRange: '-40:+0' });

    $("#filLastName").val($("#editLastName").val());
    $("#filFirstName").val($("#editFirstName").val());
    $("#filMiddleName").val($("#editMiddleName").val());
    $("#filBirthDate").val($("#editBirthDate").val());

    var $options = $("#editIdentityDocumentTypeID > option").clone();
    $('#filIdentityDocumentTypeID').append($options);
    $("#filIdentityDocumentTypeID").val($("#editIdentityDocumentTypeID").val());

    $("#filDocumentNumber").val($("#editDocumentNumber").val());
    $("#filDocumentSeries").val($("#editDocumentSeries").val()); 
</script>



