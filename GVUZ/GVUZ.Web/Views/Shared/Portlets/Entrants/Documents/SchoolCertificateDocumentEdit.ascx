<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Model.Entrants.Documents.SchoolCertificateDocumentViewModel>" %>
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
                <td class="caption"><%= Html.TableLabelReqFor(m => m.DocumentNumber) %></td>
                <td><%= Html.TextBoxExFor(m => m.DocumentSeries, new {style = "width:40px"}) %>
                <%= Html.TextBoxExFor(m => m.DocumentNumber, new {style = "width:100px"}) %></td>
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
                <td class="caption"><%= Html.TableLabelReqFor(m => m.GPA) %></td>
                <td><%= Html.TextBoxExFor(m => m.GPA, new {style = "width: 500px"}) %></td>
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

        jQuery(function() {
            $('#DocumentSeries,#DocumentNumber').blur(function() {
                checkDocumentOnExisting(jQuery('#DocumentSeries').val(), jQuery('#DocumentNumber').val());
            });
        });
/*jQuery(function(){
           $("input#GPA").mask("?999,9999" ,{placeholder:" "});
        });*/

        jQuery(function() {
            $('#GPA').blur(function() {
                if ($('input#GPA').val() == '')
                    return false;
                var tempvar = parseFloat($('input#GPA').val().replace(',', '.'));

                if (tempvar > 100 || tempvar < 0) {
                    alert('Значение среднего балла не может быть больше 100 или меньше 0.');
                    jQuery('input#GPA').val('');
                    return;
                }

                tempvar = (tempvar.toFixed(4)).replace('.', ',');
                if (tempvar == 'NaN') {
                    alert('Неправильный формат ввода. Средний балл - вещественное число о 0 до 100.');
                    jQuery('input#GPA').val('');
                    return;
                }

                jQuery('input#GPA').val(tempvar);

            });
        });

        function fillModel() {
            var model =
            {
                DocumentTypeID: <%= Model.DocumentTypeID %>,
                UID: jQuery("#UID").val(),
                DocumentSeries: jQuery('#DocumentSeries').val(),
                DocumentNumber: jQuery('#DocumentNumber').val(),
                DocumentDate: jQuery('#DocumentDate').val(),
                DocumentOrganization: jQuery('#DocumentOrganization').val(),
                GPA: jQuery('#GPA').val()
            };
            return model;
        }
    </script>
</div>