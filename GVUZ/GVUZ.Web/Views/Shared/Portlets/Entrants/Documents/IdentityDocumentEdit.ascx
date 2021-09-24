<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Model.Entrants.Documents.IdentityDocumentViewModel>" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Register TagPrefix="gv" TagName="BaseDocumentEditCommonPart" Src="~/Views/Shared/Portlets/Entrants/Documents/BaseDocumentEditCommonPart.ascx" %>
<div id="content">
    <table class="data">
        <thead>
            <tr>
                <th class="caption"></th><th></th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.DocumentTypeID) %></td>
                <td><b><%: Model.DocumentTypeName %></b></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.UID) %></td>
                <td><%= Html.TextBoxExFor(m => m.UID) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.IdentityDocumentTypeID) %></td>
                <td><%= Html.DropDownListExFor(m => m.IdentityDocumentTypeID, Model.IdentityDocumentList, new {onchange = "identityDocumentTypeChanged()"}) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.DocumentNumber) %></td>
                <td><%= Html.TextBoxExFor(m => m.DocumentSeries, new {@class = "passSeries"}) %><%= Html.TextBoxExFor(m => m.DocumentNumber, new {@class = "passNumber"}) %>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.DocumentDate) %></td>
                <td><%= Html.DatePickerFor(m => m.DocumentDate, new {@class = "passSeries"}) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.SubdivisionCode) %></td>
                <td><%= Html.TextBoxExFor(m => m.SubdivisionCode, new {@class = "passSeries"}) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.DocumentOrganization) %></td>
                <td><%= Html.TextBoxExFor(m => m.DocumentOrganization) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.GenderTypeID) %></td>
                <td><%= Html.DropDownListFor(m => m.GenderTypeID, new SelectList(Model.GenderList, "ID", "Name")) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.NationalityTypeID) %></td>
                <td><%= Html.DropDownListFor(m => m.NationalityTypeID, new SelectList(Model.NationalityList, "ID", "Name")) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.BirthDate) %></td>
                <td><%= Html.DatePickerFor(m => m.BirthDate) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.BirthPlace) %></td>
                <td><%= Html.TextBoxExFor(m => m.BirthPlace) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.DocumentAttachmentID) %></td>
                <td><% if (Model.DocumentAttachmentID != Guid.Empty) { %>
                <div><a fileID="<%= Model.DocumentAttachmentID %>" class="getFileLink"><%: Model.DocumentAttachmentName %></a><button class="fileDelete" onclick=" detachAttachedDocument(this);return false; ">&nbsp;</button></div>
                <% } %>
                <%= Html.FileForm("") %></td>
            </tr>
        </tbody>
    </table>
    <gv:BaseDocumentEditCommonPart ID="commonPart" runat="server" />
    <script type="text/javascript">
        var russianDocs = JSON.parse('<%= Html.Serialize(Model.RussianDocs) %>');
        validateDocumentSeriesAndNumber = false;

        function identityDocumentTypeChanged() {
            var docTypeID = jQuery('#IdentityDocumentTypeID').val();
            if (russianDocs.indexOf(docTypeID) >= 0)
                $('#NationalityTypeID').val('1').attr('disabled', 'disabled');
            else
                $('#NationalityTypeID').removeAttr('disabled');
        }

        $(document).ready(function() {
            $("input#SubdivisionCode").mask("999-999");
            identityDocumentTypeChanged();
        });

        function fillModel() {
            var model = {
                DocumentTypeID: <%= Model.DocumentTypeID %>,
                UID: $("#UID").val(),
                DocumentSeries: $('#DocumentSeries').val(),
                DocumentNumber: $('#DocumentNumber').val(),
                DocumentDate: $('#DocumentDate').val(),
                SubdivisionCode: $('#SubdivisionCode').val(),
                DocumentOrganization: $('#DocumentOrganization').val(),
                IdentityDocumentTypeID: $('#IdentityDocumentTypeID').val(),
                GenderTypeID: $('#GenderTypeID').val(),
                NationalityTypeID: $('#NationalityTypeID').val(),
                BirthPlace: $('#BirthPlace').val(),
                BirthDate: $('#BirthDate').val()
            };
            return model;
        }

        jQuery(function() {
            $('#DocumentSeries,#DocumentNumber').blur(function() {
                checkDocumentOnExisting(jQuery('#DocumentSeries').val(), $('#DocumentNumber').val());
            });
        });
    </script>
</div>