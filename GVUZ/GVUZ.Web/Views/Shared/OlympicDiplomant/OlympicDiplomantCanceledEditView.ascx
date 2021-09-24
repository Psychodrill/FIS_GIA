<%@ Control Language="C#" Inherits="ViewUserControl<OlympicDiplomantCanceledEditViewModel>" %>

<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="GVUZ.DAL.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.ViewModels.OlympicDiplomant" %>


<%= Html.TextBoxExFor(m => m.Data.OlympicDiplomantDocumentID, new { @id="editOlympicDiplomantDocumentID", @type="hidden" })%>

<div id="containerCanceled" class="TableInLine">
    <table>
        <tr>
            <td>
                <div>
                    <table>
                        <tr>
                            <td><%=Html.LabelFor(m => m.Data.LastName)%>: <span style="color: crimson; font-size: large;">*</span></td>
                            <td><%= Html.TextBoxExFor(m => m.Data.LastName, new { @id="editCanceledLastName" })%></td>
                        </tr>

                        <tr>
                            <td><%=Html.LabelFor(m => m.Data.FirstName)%>: <span style="color: crimson; font-size: large;">*</span></td>
                            <td><%= Html.TextBoxExFor(m => m.Data.FirstName, new { @id="editCanceledFirstName" })%></td>
                        </tr>

                        <tr>
                            <td><%=Html.LabelFor(m => m.Data.MiddleName)%>: </td>
                            <td><%= Html.TextBoxExFor(m => m.Data.MiddleName, new { @id="editCanceledMiddleName" })%></td>
                        </tr>

                        <tr>
                            <td><%=Html.LabelFor(m => m.Data.BirthDate)%>: <span style="color: crimson; font-size: large;">*</span></td>
                            <td><%= Html.DatePickerFor(m => m.Data.BirthDate, new { @id="editCanceledBirthDate" })%></td>
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
                            <td><%=Html.LabelFor(m => m.Data.IdentityDocumentTypeID)%>: <span style="color: crimson; font-size: large;">*</span></td>
                            <td><%= Html.DropDownListExFor(x=>x.Data.IdentityDocumentTypeID, Model.IdentityDocumentType, new { @id="editCanceledIdentityDocumentTypeID", @class="chosen" })%></td>
                        </tr>

                        <tr>
                            <td><%=Html.LabelFor(m => m.Data.DocumentSeries)%>:</td>
                            <td><%= Html.TextBoxExFor(m => m.Data.DocumentSeries, new { @id="editCanceledDocumentSeries",style="width: 326px;" })%></td>
                        </tr>

                        <tr>
                            <td><%=Html.LabelFor(m => m.Data.DocumentNumber)%>: <span style="color: crimson; font-size: large;">*</span></td>
                            <td><%= Html.TextBoxExFor(m => m.Data.DocumentNumber, new { @id="editCanceledDocumentNumber",style="width: 326px;" })%></td>
                        </tr>

                        <tr>
                            <td><%=Html.LabelFor(m => m.Data.OrganizationIssue)%>:</td>
                            <td><%= Html.TextBoxExFor(m => m.Data.OrganizationIssue, new { @id="editCanceledOrganizationIssue",style="width: 326px;" })%></td>
                        </tr>

                        <tr>
                            <td><%=Html.LabelFor(m => m.Data.DateIssue)%>:</td>
                            <td><%= Html.DatePickerFor(m => m.Data.DateIssue, new { @id="editCanceledDateIssue" })%></td>
                        </tr>

                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>



<div>
    <span id="errorMessageCanceled" style="color: crimson; font-size: medium"></span>
</div>

<script type="text/javascript">
    SetDatepicker();
</script>
