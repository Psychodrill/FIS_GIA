<%@ Control Language="C#" Inherits="ViewUserControl<OlympicDiplomantEditViewModel>" %>
<%@ Import Namespace="System" %>
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
    <div id="dialogInfo">
    </div>

    <div id="dialogCanceled">
    </div>

    <div>
        <h4 class="text-info">Сведения о победителе/призере олимпиады</h4>
    </div>

    <div class="TableInLine" id="TableInLineSmall">
        <table>
            <tr>
                <td>
                    <div>
                        <table>
                            <tr>
                                <td><%=Html.LabelFor(m => m.Data.OlympicDiplomantDocument.LastName)%>: <span style="color:crimson; font-size:large;">*</span></td>
                                <td><%= Html.TextBoxExFor(m => m.Data.OlympicDiplomantDocument.LastName, new { @id="editLastName", @class = "classChange", style="width: 326px" })%></td>
                            </tr>

                            <tr>
                                <td><%=Html.LabelFor(m => m.Data.OlympicDiplomantDocument.FirstName)%>: <span style="color:crimson; font-size:large;">*</span></td>
                                <td><%= Html.TextBoxExFor(m => m.Data.OlympicDiplomantDocument.FirstName, new { @id="editFirstName", @class = "classChange", style="width: 326px" })%></td>
                            </tr>

                            <tr>
                                <td><%=Html.LabelFor(m => m.Data.OlympicDiplomantDocument.MiddleName)%>:</td>
                                <td><%= Html.TextBoxExFor(m => m.Data.OlympicDiplomantDocument.MiddleName, new { @id="editMiddleName", @class = "classChange", style="width: 326px"})%></td>
                            </tr>

                            <tr>
                                <td><%=Html.LabelFor(m => m.Data.OlympicDiplomantDocument.BirthDate)%>: <span style="color:crimson; font-size:large;">*</span></td>
                                <td><%= Html.DatePickerFor(m => m.Data.OlympicDiplomantDocument.BirthDate, new { @id="editBirthDate", @class = "classChange" })%></td>
                            </tr>
                        </table>
                    </div>
                </td>
                        </tr>
        </table>
    </div>
    <div class="TableInLine">
        <table>
            <tr>
                <td>
                    <div>
                        <table>

                            <tr>
                                <td><%=Html.LabelFor(m => m.Data.OlympicDiplomantDocument.IdentityDocumentTypeID)%>: <span style="color:crimson; font-size:large;">*</span></td>
                                <td><%= Html.DropDownListExFor(x=>x.Data.OlympicDiplomantDocument.IdentityDocumentTypeID, Model.IdentityDocumentType, new { @id="editIdentityDocumentTypeID", @class="chosen classChange" })%></td>
                            </tr>

                            <tr>
                                <td><%=Html.LabelFor(m => m.Data.OlympicDiplomantDocument.DocumentSeries)%>:</td>
                                <td><%= Html.TextBoxExFor(m => m.Data.OlympicDiplomantDocument.DocumentSeries, new { @id="editDocumentSeries" , @class = "classChange", style="width: 326px"})%></td>
                            </tr>

                            <tr>
                                <td><%=Html.LabelFor(m => m.Data.OlympicDiplomantDocument.DocumentNumber)%>: <span style="color:crimson; font-size:large;">*</span></td>
                                <td><%= Html.TextBoxExFor(m => m.Data.OlympicDiplomantDocument.DocumentNumber, new { @id="editDocumentNumber" , @class = "classChange", style="width: 326px"})%></td>
                            </tr>

                            <tr>
                                <td><%=Html.LabelFor(m => m.Data.OlympicDiplomantDocument.OrganizationIssue)%>:</td>
                                <td><%= Html.TextBoxExFor(m => m.Data.OlympicDiplomantDocument.OrganizationIssue, new { @id="editOrganizationIssue", style="width: 326px"})%></td>
                            </tr>

                            <tr>
                                <td><%=Html.LabelFor(m => m.Data.OlympicDiplomantDocument.DateIssue)%>:</td>
                                <td><%= Html.DatePickerFor(m => m.Data.OlympicDiplomantDocument.DateIssue, new { @id="editDateIssue" })%></td>
                            </tr>

                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <div id="canceledArea">
        <div>
            <h4 class="text-info">Сведения о недействующих документах, удостоверяющих личность</h4>
        </div>

        <div id="canceledrows">
            <% Html.RenderPartial("OlympicDiplomant/OlympicDiplomantCanceledTableView", Model.Data.OlympicDiplomantDocumentCanceled); %>
        </div>

        <div>
            <button type="button" class="btnAddS button primary">Добавить документ</button>
        </div>
    </div>


    <div>
        <h4 class="text-info">Сведения об образовательной организации, в которой обучается участник</h4>
    </div>

    <div class="TableInLine">
        <table>
            <tr>
                <td>
                    <div>
                        <table>
                            <tr>
                                <td><%=Html.LabelFor(m => m.Data.SchoolRegionID)%>:</td>
                                <td><%= Html.DropDownListExFor(x=>x.Data.SchoolRegionID, Model.RegionType, new { @id="editSchoolRegionID", @class="chosen" })%></td>
                            </tr>
                            <tr>
                                <td><%=Html.LabelFor(m => m.Data.FormNumber)%>: <span style="color:crimson; font-size:large;">*</span></td>
                                <td><%= Html.DropDownListExFor(x=>x.Data.FormNumber, Model.FormNumber, new { @id="editFormNumber", @class="chosen" })%></td>
                            </tr>
                            <tr>
                                <td><%=Html.LabelFor(m => m.Data.EndingDate)%>: <span style="color:crimson; font-size:large;">*</span></td>
                                <td><%= Html.TextBoxExFor(m => m.Data.EndingDate, new { @id="editEndingDate", style="width: 326px"})%></td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div class="TableInLine">
        <table>
            <tr>
                  <td>
                    <div>
                        <table>
                            <tr>
                                <td><%=Html.LabelFor(m => m.Data.SchoolEgeCode)%>:</td>
                                <td><%= Html.TextBoxExFor(m => m.Data.SchoolEgeCode, new { @id="editSchoolEgeCode"})%></td>
                            </tr>
                            <tr>
                                <td><%=Html.LabelFor(m => m.Data.SchoolEgeName)%>:</td>
                                <td><%= Html.TextBoxExFor(m => m.Data.SchoolEgeName, new { @id="editSchoolEgeName"} )%></td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>




    <div>
        <h4 class="text-info">Сведения о дипломе победителя/призера олимпиады школьников</h4>
    </div>

    <div class="TableInLine">
        <table>
            <tr>
                <td>
                    <div>
                        <table>
                            <tr>
                                <td><%=Html.LabelFor(m => m.Data.ResultLevelID)%>:  <span style="color:crimson; font-size:large;">*</span></td>
                                <td><%= Html.DropDownListExFor(x=>x.Data.ResultLevelID, Model.ResultLevel, new { @id="editResultLevelID", @class="chosen" })%></td>
                            </tr>
                            <tr>
                                <td><%=Html.LabelFor(m => m.Data.DiplomaSeries )%>:</td>
                                <td><%= Html.TextBoxExFor(m => m.Data.DiplomaSeries , new { @id="editDiplomaSeries", style="width: 326px"})%></td>
                            </tr>
                            <tr>
                                <td><%=Html.LabelFor(m => m.Data.DiplomaNumber)%>:</td>
                                <td><%= Html.TextBoxExFor(m => m.Data.DiplomaNumber, new { @id="editDiplomaNumber", style="width: 326px"})%></td>
                            </tr>
                            <tr>
                                <td><%=Html.LabelFor(m => m.Data.DiplomaDateIssue)%>:</td>
                                <td><%= Html.DatePickerFor(m => m.Data.DiplomaDateIssue, new { @id="editDiplomaDateIssue" })%></td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div class="TableInLine">
        <table>
            <tr>
                <td>
                    <div>
                        <table>
                            <tr>
                                <td><%=Html.LabelFor(m => m.OlympicYearID)%>:</td>
                                <td><%= Html.DropDownListExFor(x=>x.OlympicYearID, Model.OlympicYear, new { @id="editOlympicYearID", @class="chosen" })%></td>
                            </tr>
                            <tr>
                                <td><%=Html.LabelFor(m => m.OlympicTypeID)%>:</td>
                                <td><%= Html.DropDownListExFor(x=>x.OlympicTypeID, Model.OlympicType, new { @id="editOlympicTypeID", @class="chosen" })%></td>
                            </tr>
                            <tr>
                                <td><%=Html.LabelFor(m => m.Data.OlympicTypeProfileID)%>: <span style="color:crimson; font-size:large;">*</span></td>
                                <td><%= Html.DropDownListExFor(x=>x.Data.OlympicTypeProfileID, Model.OlympicProfile, new { @id="editOlympicTypeProfileID", @class="chosen" })%></td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>




    <div id="calcArea" style="display: none">
        <% Html.RenderPartial("OlympicDiplomant/OlympicDiplomantInfoView", Model); %>
    </div>

    <%= Html.TextBoxExFor(m => m.Data.OlympicDiplomantID, new { @id="editID", @type="hidden" })%>
    <%= Html.TextBoxExFor(m => m.Data.OlympicDiplomantIdentityDocumentID, new { @id="editOlympicDiplomantIdentityDocumentID", @type="hidden" })%>
    <%= Html.TextBoxExFor(m => m.Data.StatusID, new { @id="editStatusID", @type="hidden" })%>
    <%= Html.TextBoxExFor(m => m.Data.PersonId, new { @id="editPersonID", @type="hidden" })%>
    <%= Html.TextBoxExFor(m => m.Data.PersonLinkDate, new { @id="editPersonLinkDate", @type="hidden" })%>
    <%= Html.TextBoxExFor(m => m.Data.Comment, new { @id="editComment", @type="hidden" })%>

    <div>
        <span id="errorMessage" style="color:crimson; font-size:medium"></span>
    </div>

</div>

<script type="text/javascript">
    var OlympicTypeID = '<%= Model.OlympicTypeID%>';
    var OlympicTypeProfileID = '<%= Model.Data.OlympicTypeProfileID%>';

    $(document).ready(function ()
    {
        if ($("#editID").val() > 0)
            $("#calcArea").show();
    })

    SetDatepicker();

</script>
