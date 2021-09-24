<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OlympicsCatalogEditViewModel>" %>

<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.DAL.Dapper.ViewModel.Olympics" %>
<%@ Import Namespace="GVUZ.Data.Model" %>

<style type="text/css">
    span.ui-tool select {
        margin: 0px;
        padding: 0px;
    }

    .width1 {
        width: 630px;
    }
</style>

<div id="content">
    <table class="gvuzData">
        <tbody>
            <tr>
                <td class="caption">
                    <h3>Сведения об олимпиаде</h3>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%=Html.LabelFor(m => m.SelectedYear)%>:
                </td>
                <td>
                    <%= Html.DropDownListExFor(x=>x.SelectedYear, Model.Years, new { @id="editYear", @class="width1" })%>
                </td>
            </tr>  
            <tr>
                <td class="caption">
                    <%=Html.LabelFor(m => m.Data.OlympicType.Name)%>: <span style="color:crimson; font-size:large;">*</span>
                </td>
                <td>
                    <%= Html.DropDownListExFor(x=>x.Data.OlympicTypeID, Model.Olympics, new { @id="editName", @class="width1" })%>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%=Html.LabelFor(m => m.Data.OlympicProfile.ProfileName)%>: <span style="color:crimson; font-size:large;">*</span>
                </td>
                <td>
                    <%= Html.DropDownListExFor(x=>x.Data.OlympicProfileID, Model.Profiles, new { @id="editProfile", @class="width1" })%>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%=Html.LabelFor(m => m.Data.OlympicLevel.Name)%>: <span style="color:crimson; font-size:large;">*</span>
                </td>
                <td>
                    <%= Html.DropDownListExFor(x=>x.Data.OlympicLevelID, Model.Levels, new { @id="editLevel", @class="width1" })%>
                </td>
            </tr>
            <tr>
                <td class="caption">Профильные предметы:
                </td>
                <td>
                    <%= Html.ListBoxFor(m => m.SelectedSubjects, Model.Subjects, new { @class = "chosen", multiple = "multiple", style = "width: 630px;" })%>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <h3>Сведения об организаторе</h3>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%=Html.LabelFor(m => m.Data.OrganizerName)%>: <span style="color:crimson; font-size:large;">*</span>
                </td>
                <td>
                    <%= Html.TextAreaFor(m => m.Data.OrganizerName, new { @id="editOrganizerName", @style="width: 630px;height:45px;", @placeholder="введите несколько символов и начнется поиск..." })%>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%=Html.LabelFor(m => m.Data.OrganizerAddress)%>:
                </td>
                <td>
                    <%= Html.TextBoxExFor(m => m.Data.OrganizerAddress, new { @id="editOrganizerAddress", @style="width: 617px;" })%>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    Подключение к ЗКСПД:
                </td>
                <td>
                    <%= Html.DropDownListFor(m => m.Connected, new SelectList(new[] { new { Value = "0", Text = "Не выбрано" }, new { Value = "1", Text = "Да" }, new { Value = "2", Text = "Нет" } }, "Value", "Text"), new { @id="editOrganizerConnected", @class="width1" })%>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td class="caption" style="text-align:left">
                    <h4>Сведения об ответственном лице от Организатора олимпиады</h4>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%=Html.LabelFor(m => m.Data.FirstName)%>:
                </td>
                <td>
                    <%= Html.TextBoxExFor(m => m.Data.FirstName, new { @id="editFirstName", @style="width: 617px;" })%>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%=Html.LabelFor(m => m.Data.LastName)%>:
                </td>
                <td>
                    <%= Html.TextBoxExFor(m => m.Data.LastName, new { @id="editLastName", @style="width: 617px;" })%>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%=Html.LabelFor(m => m.Data.MiddleName)%>:
                </td>
                <td>
                    <%= Html.TextBoxExFor(m => m.Data.MiddleName, new { @id="editMiddleName", @style="width: 617px;" })%>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%=Html.LabelFor(m => m.Data.Email)%>:
                </td>
                <td>
                    <%= Html.TextBoxExFor(m => m.Data.Email, new { @id="editEmail", @style="width: 617px;" })%>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%=Html.LabelFor(m => m.Data.PhoneNumber)%>:
                </td>
                <td>
                    <%= Html.TextBoxExFor(m => m.Data.PhoneNumber, new { @id="editPhoneNumber", @style="width: 617px;" })%>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%=Html.LabelFor(m => m.Data.Position)%>:
                </td>
                <td>
                    <%= Html.TextBoxExFor(m => m.Data.Position, new { @id="editPosition", @style="width: 617px;" })%>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <h3>Сведения о соорганизаторе</h3>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    <%=Html.LabelFor(m => m.CoOrganizer)%>:
                </td>
                <td>
                    <%= Html.TextAreaFor(m => m.CoOrganizer, new { @id="editCoOrganizerName", @style="width: 630px;height:45px;", @placeholder="введите несколько символов и начнется поиск..." })%>
                </td>
            </tr>
            <tr>
                <td class="caption">
                    Подключение к ЗКСПД:
                </td>
                <td>
                    <%= Html.DropDownListFor(m => m.CoConnected, new SelectList(new[] { new { Value = "0", Text = "Не выбрано" }, new { Value = "1", Text = "Да" }, new { Value = "2", Text = "Нет" } }, "Value", "Text"), new { @id="editCoOrganizerConnected", @class="width1" })%>
                </td>
            </tr>

            <tr>
                 <th colspan="2" style="text-align:left;"><h3>ВУЗ, осуществляющий внесение сведений о победителях/призерах ОШ</h3></th>
            </tr>
            
            <tr>
                <td></td>
                <td>
                    <fieldset>
                        <div class="some-class">
                            <input checked type="radio" class="radio" name="x" value="0" id="x" />
                            <label for="x">Не выбрано</label>
                            <input type="radio" class="radio" name="x" value="1" id="y" />
                            <label for="y">Организатор</label>
                            <input type="radio" class="radio" name="x" value="2" id="z" />
                            <label for="z">Соорганизатор</label>
                        </div>
                    </fieldset>
                </td>
            </tr>

        </tbody>
    </table>
    <div style="display: none">
        <input id="btnSubmit" type="button" value="Сохранить" />
        <input id="btnCancel" type="button" value="Отмена" />
    </div>
    <div>
        <span id="errorMessage" style="color:crimson; font-size:medium"></span>
    </div>
</div>

<%= Html.TextBoxExFor(m => m.Data.OrganizerID, new { @id="editOrganizerID", @type="hidden" })%>
<%= Html.TextBoxExFor(m => m.Data.CoOrganizerID, new { @id="editCoOrganizerID", @type="hidden" })%>
<%= Html.TextBoxExFor(m => m.Data.OrgOlympicEnterID, new { @id="editOrgOlympicEnterID", @type="hidden" })%>

<script type="text/javascript">

    $("#editYear").change(function () {
        var year = 'year=' + $("#editYear").val();
        doPostAjax('<%= Url.Generate<AdministrationController>(x => x.GetOlympicsForYear(null)) %>', year, function (data)
        {
            var ddl = $('#editName').empty();
            $.each(data, function (index, item) {
                ddl.append("<option value='" + item.Id + "'>" + item.Name + "</option>");
            });
        }, "application/x-www-form-urlencoded")
    })

    var isError = false

    $('#btnCancel').click(function () { closeDialog($('#dialog')); })

    $('#btnSubmit').click(function ()
    {
        if (!isError) {
            clearValidationErrors($('.gvuzData'))
            submitData();
        }
    })

    function submitData() {
        var model = fillModel();

        doPostAjax("<%= Url.Generate<AdministrationController>(x => x.OlympicsCatalogModify(null)) %>",
            JSON.stringify(model), function (data) {
                if (!addValidationErrorsFromServerResponse(data)) {
                    createdItem = data.Data
                    $('#btnCancel').click()
                } else {
                    var control = "[name=" + data.Data[0].ControlID.replace("_", ".") + "]";
                    $(control).addClass("input-validation-error");
                    $('#errorMessage').text(data.Data[0].ErrorMessage);
                }
                unblockUI()
            }, null, null, false)
    }

    function fillModel() {
        var e1 = $('#editOrganizerConnected').val();
        var e2 = $('#editCoOrganizerConnected').val();

        var o1 = $('#editOrganizerID').val();
        var o2 = $('#editCoOrganizerID').val();
        var o3 = $('#editOrgOlympicEnterID').val();

        var c1 = $('#x').attr("checked");
        var c2 = $('#y').attr("checked");
        var c3 = $('#z').attr("checked");

        if (c1)
            o3 = null;
        if (c2)
            o3 = o1;
        if (c3)
            o3 = o2;

        var model = {
            Data: {
                OlympicTypeProfileID: '<%= Model.Data.OlympicTypeProfileID%>',
                OrganizerName: $('#editOrganizerName').val(),
                FirstName: $('#editFirstName').val(),
                LastName: $('#editLastName').val(),
                MiddleName: $('#editMiddleName').val(),
                Email: $('#editEmail').val(),
                Position: $('#editPosition').val(),
                OrganizerAddress: $('#editOrganizerAddress').val(),
                PhoneNumber: $('#editPhoneNumber').val(),
                OlympicLevelID: $('#editLevel').val(),
                OlympicProfileID: $('#editProfile').val(),
                OlympicTypeID: $('#editName').val(),
                OrganizerConnected: e1 == "0" ? null : e1 == "1" ? true : false,
                CoOrganizerConnected: e2 == "0" ? null : e2 == "1" ? true : false,
                OrganizerID: o1,
                CoOrganizerID: o2,
                OrgOlympicEnterID: o3,
                OlympicSubject: []
            }
        }

        $('#SelectedSubjects option:selected').each(function () {
            model.Data.OlympicSubject.push(
                {
                    OlympicSubjectID: 0,
                    OlympicTypeProfileID: '<%= Model.Data.OlympicTypeProfileID%>',
                    SubjectID: $(this).val(),
                })
        });

        return model
    }
</script>