<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Model.Entrants.Documents.StudentDocumentViewModel>" %>
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
                <td class="caption"><%= Html.TableLabelReqFor(m => m.UID) %></td>
                <td><%= Html.TextBoxExFor(m => m.UID) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.DocumentNumber) %></td>
                <td><%= Html.TextBoxExFor(m => m.DocumentNumber, new {style = "width:100px"}) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.DocumentDate) %></td>
                <td><%= Html.DatePickerFor(m => m.DocumentDate) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.DocumentOrganization) %></td>
                <td><%= Html.TextBoxExFor(m => m.DocumentOrganization) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.DocumentAttachmentID) %></td>
                <td><% if (Model.DocumentAttachmentID != Guid.Empty)
                       { %><div><a fileID="<%= Model.DocumentAttachmentID %>" class="getFileLink"><%: Model.DocumentAttachmentName
    %></a><button class="fileDelete" onclick=" detachAttachedDocument(this);return false; ">&nbsp;</button></div><% } %>
                    <%= Html.FileForm("") %></td>
            </tr>
        </tbody>
    </table>
    <gv:BaseDocumentEditCommonPart ID="commonPart" runat="server" />
    <script type="text/javascript">

        var institutionData = JSON.parse('<%= Html.Serialize(Model.InstitutionList) %>');
        jQuery(function() {
            validateDocumentSeriesAndNumber = false;
            $('#DocumentSeries,#DocumentNumber').blur(function() {
                checkDocumentOnExisting(jQuery('#DocumentSeries').val(), jQuery('#DocumentNumber').val());
            });
        });

        function fillModel() {

            var model =
            {
                DocumentTypeID: <%= Model.DocumentTypeID %>,
                UID: jQuery("#UID").val(),
                DocumentNumber: jQuery('#DocumentNumber').val(),
                DocumentOrganization: jQuery('#DocumentOrganization').val(),
                DocumentDate: jQuery('#DocumentDate').val(),
            };

            return model;
        }

        /*autocompleteDropdown(jQuery('#DocumentOrganization'),
            {
                source: function(ui, response) {
                    var res = [];
                    var x = ui.term.toUpperCase();
                    for (var i = 0; i < institutionData.length; i++)
                        if (institutionData[i].toUpperCase().indexOf(x) >= 0) {
                            res.push(institutionData[i]);
                            if (res.length > 10) break;
                        }
                    response(res);
                },
                minLength: 3
            }
        );*/
    </script>
</div>