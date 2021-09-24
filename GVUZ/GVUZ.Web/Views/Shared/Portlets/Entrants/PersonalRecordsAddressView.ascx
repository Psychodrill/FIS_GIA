<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Entrants.PersonalRecordsAddressViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<div id="content" style="margin-right: 10px;">
    <table class="data tableApp2">
        <thead>
            <tr>
                <th class="caption" style="width: 10%">Прописка:</th><th></th>
                <th class="caption" style="width: 20%">Фактическое место жительства:</th><th></th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.RegistrationAddress.PostalCode) %></td>
                <td class="registrationData"><%= Html.CommonInputReadOnly(Model.RegistrationAddress.PostalCode) %></td>
                <td class="caption"><%= Html.TableLabelFor(m => m.IsOnlyRegistration) %></td>
                <td class="commonData"><%= Html.CheckBoxFor(m => m.IsOnlyRegistration, new Dictionary<string, object> {{"disabled", "disabled"}}) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.RegistrationAddress.CountryID) %></td>
                <td class="registrationData"><%= Html.CommonInputReadOnly(Model.RegistrationAddress.CountryName) %></td>
                <td class="caption"><%= Html.TableLabelFor(m => m.FactAddress.PostalCode) %></td>
                <td class="factData"><%= Html.CommonInputReadOnly(Model.FactAddress.PostalCode) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.RegistrationAddress.RegionID) %></td>
                <td class="registrationData"><%= Html.CommonInputReadOnly(Model.RegistrationAddress.RegionName) %></td>
                <td class="caption"><%= Html.TableLabelFor(m => m.FactAddress.CountryID) %></td>
                <td class="factData"><%= Html.CommonInputReadOnly(Model.FactAddress.CountryName) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.RegistrationAddress.CityName) %></td>
                <td class="registrationData"><%= Html.CommonInputReadOnly(Model.RegistrationAddress.CityName) %></td>
                <td class="caption"><%= Html.TableLabelFor(m => m.FactAddress.RegionID) %></td>
                <td class="factData"><%= Html.CommonInputReadOnly(Model.FactAddress.RegionName) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.RegistrationAddress.Street) %></td>
                <td class="registrationData"><%= Html.CommonInputReadOnly(Model.RegistrationAddress.Street) %></td>
                <td class="caption"><%= Html.TableLabelFor(m => m.FactAddress.CityName) %></td>
                <td class="factData"><%= Html.CommonInputReadOnly(Model.FactAddress.CityName) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.RegistrationAddress.BuildingPart) %></td>
                <td class="registrationData"><%= Html.CommonInputReadOnly(Model.RegistrationAddress.Building + " / " + Model.RegistrationAddress.BuildingPart) %></td>
                <td class="caption"><%= Html.TableLabelFor(m => m.FactAddress.Street) %></td>
                <td class="factData"><%= Html.CommonInputReadOnly(Model.FactAddress.Street) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.RegistrationAddress.Room) %></td>
                <td class="registrationData"><%= Html.CommonInputReadOnly(Model.RegistrationAddress.Room) %></td>
                <td class="caption"><%= Html.TableLabelFor(m => m.FactAddress.BuildingPart) %></td>
                <td class="factData"><%= Html.CommonInputReadOnly(Model.FactAddress.Building + " / " + Model.FactAddress.BuildingPart) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.RegistrationAddress.Phone) %></td>
                <td class="registrationData"><%= Html.CommonInputReadOnly(Model.RegistrationAddress.Phone) %></td>
                <td class="caption"><%= Html.TableLabelFor(m => m.FactAddress.Room) %></td>
                <td class="factData"><%= Html.CommonInputReadOnly(Model.FactAddress.Room) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.Mobile) %></td>
                <td class="commonData"><%= Html.CommonInputReadOnly(Model.Mobile) %></td>
                <td class="caption"><%= Html.TableLabelFor(m => m.FactAddress.Phone) %></td>
                <td class="factData"><%= Html.CommonInputReadOnly(Model.FactAddress.Phone) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.Email) %></td>
                <td class="commonData"><%= Html.CommonInputReadOnly(Model.Email) %></td>
                <td class="caption">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </tbody>
    </table>
    <% if (Model.ApplicationStep == 0)
       { %>
        <div>
            <a id="btnEdit" href="<%= Url.Generate<EntrantController>(c => c.AddressEdit()) %>">Редактировать</a>
        </div>
        <script type="text/javascript">
            jQuery(document).ready(function() {
                jQuery('#btnEdit').button();
            })
        </script>
    <% } %>
</div>