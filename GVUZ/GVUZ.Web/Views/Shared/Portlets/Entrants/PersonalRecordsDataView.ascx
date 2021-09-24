<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Entrants.PersonalRecordsDataViewModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.Web.Models" %>
<div id="content" style="margin-right: 10px;">
    <table class="data tableApp2">
        <thead>
            <tr>
                <th class="caption" style="width: 20%"></th><th></th>
                <th class="caption" style="width: 20%"></th><th></th>
            </tr>
        </thead>
        <tbody>
            <% if (Model.ApplicationStep != ApplicationStepType.ParentData)
               { %>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Entrant.LastName) %></td>
                    <td><%= Html.CommonInputReadOnly(Model.Entrant.LastName) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Entrant.GenderID) %></td>
                    <td><%= Html.CommonInputReadOnly(Model.GenderName) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Entrant.FirstName) %></td>
                    <td><%= Html.CommonInputReadOnly(Model.Entrant.FirstName) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.NationalityID) %></td>
                    <td><%= Html.CommonInputReadOnly(Model.NationalityName) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Entrant.MiddleName) %></td>
                    <td><%= Html.CommonInputReadOnly(Model.Entrant.MiddleName) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.BirthPlace) %></td>
                    <td><%= Html.CommonInputReadOnly(Model.BirthPlace) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.BirthDate) %></td>
                    <td><%= Html.CommonInputReadOnly(Model.BirthDate.ToLocalTime().ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)) %></td>
                    <td class="caption" rowspan="4"><%= Html.TableLabelFor(m => m.CustomInformation) %></td>
                    <td rowspan="4"><%= Html.CommonTextAreaReadOnly(Model.CustomInformation) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.DocumentTypeID) %></td>
                    <td><%= Html.CommonInputReadOnly(Model.IdentityDocumentName) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.DocumentNumber) %></td>
                    <td><%= Html.CommonInputReadOnly(Model.DocumentSeries + " " + Model.DocumentNumber) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.DocumentOrganization) %></td>
                    <td><%= Html.CommonInputReadOnly(Model.DocumentOrganization) %></td>
                </tr>
                <tr>
                    <td class="caption">
                        <div style="padding-bottom: 5px;">
                            <%= Html.TableLabelFor(m => m.DocumentDate) %>
                        </div>
                    </td>
                    <td><%= Html.CommonInputReadOnly(
                                (Model.DocumentDate.HasValue ? Model.DocumentDate.Value.ToLocalTime().ToString("dd.MM.yyyy", CultureInfo.InvariantCulture) : "")
                                + " / " + (Model.SubdivisionCode ?? "")) %></td>
                    <% if (true)
                       { %>
                        <td class="caption"><%= Html.TableLabelFor(m => m.NeedHostel) %></td>
                        <td><%= Html.CheckBoxFor(m => m.NeedHostel, new Dictionary<string, object> {{"disabled", "disabled"}}) %></td>
                    <% } %>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.DocumentAttachmentID) %></td>
                    <td><%= Url.GenerateLinkIf<EntrantController>(x => x.GetFile1(Model.DocumentAttachmentID), Model.DocumentAttachmentName, Model.DocumentAttachmentID != Guid.Empty) %>
                    <td class="caption"></td>
                    <td></td>
                </tr>
            <% } %>
            <tr>
                <td colspan="4" style="height: 40px"></td>
            </tr>
            <% if (Model.ApplicationStep == ApplicationStepType.NotApplication)
               { %>
                <tr>
                    <td colspan="4">Сведения о родителях</td>
                </tr>
            <% } %>
            <% if (Model.ApplicationStep != ApplicationStepType.PersonalData && false)
               { %>
                <tr>
                    <th colspan="2">отец</th>
                    <th colspan="2">мать</th>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Father.PersonData.LastName) %></td>
                    <td class="fatherData"><%= Html.CommonInputReadOnly(Model.Father.PersonData.LastName) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Mother.PersonData.LastName) %></td>
                    <td class="motherData"><%= Html.CommonInputReadOnly(Model.Mother.PersonData.LastName) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Father.PersonData.FirstName) %></td>
                    <td class="fatherData"><%= Html.CommonInputReadOnly(Model.Father.PersonData.FirstName) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Mother.PersonData.FirstName) %></td>
                    <td class="motherData"><%= Html.CommonInputReadOnly(Model.Mother.PersonData.FirstName) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Father.PersonData.MiddleName) %></td>
                    <td class="fatherData"><%= Html.CommonInputReadOnly(Model.Father.PersonData.MiddleName) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Mother.PersonData.MiddleName) %></td>
                    <td class="motherData"><%= Html.CommonInputReadOnly(Model.Mother.PersonData.MiddleName) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Father.WorkPlace) %></td>
                    <td class="fatherData"><%= Html.CommonInputReadOnly(Model.Father.WorkPlace) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Mother.WorkPlace) %></td>
                    <td class="motherData"><%= Html.CommonInputReadOnly(Model.Mother.WorkPlace) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Father.Position) %></td>
                    <td class="fatherData"><%= Html.CommonInputReadOnly(Model.Father.Position) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Mother.Position) %></td>
                    <td class="motherData"><%= Html.CommonInputReadOnly(Model.Mother.Position) %></td>
                </tr>
                <tr>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Father.WorkPhone) %></td>
                    <td class="fatherData"><%= Html.CommonInputReadOnly(Model.Father.WorkPhone) %></td>
                    <td class="caption"><%= Html.TableLabelFor(m => m.Mother.WorkPhone) %></td>
                    <td class="motherData"><%= Html.CommonInputReadOnly(Model.Mother.WorkPhone) %></td>
                </tr><% } %>
        </tbody>
    </table>
    <% if (Model.ApplicationStep == 0)
       { %>
        <div>
            <a id="btnEdit" href="<%= Url.Generate<EntrantController>(c => c.Edit()) %>">Редактировать</a>
        </div>

        <script type="text/javascript">
            jQuery(document).ready(
                function() {
                    jQuery('#btnEdit').button();
                });
        </script>
    <% } %>
</div>