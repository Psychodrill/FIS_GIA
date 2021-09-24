<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Model.Entrants.Documents.BasicDiplomaDocumentViewModel>" %>
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
                <td class="caption"><%= Html.TableLabelFor(m => m.RegistrationNumber) %></td>
                <td><%= Html.TextBoxExFor(m => m.RegistrationNumber) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.QualificationTypeID) %></td>
                <td><%= Html.TextBoxExFor(m => m.QualificationTypeName, new {style = "width: 500px"}) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.ProfessionTypeID) %></td>
                <td><%= Html.TextBoxExFor(m => m.ProfessionTypeName, new {style = "width: 500px"}) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.InstitutionName) %></td>
                <td><%= Html.TextBoxExFor(m => m.InstitutionName, new {style = "width: 500px"}) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.GPA) %></td>
                <td><%= Html.TextBoxExFor(m => m.GPA, new {style = "width: 500px"}) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelFor(m => m.DocumentAttachmentID) %></td>
                <td><div><% if (Model.DocumentAttachmentID != Guid.Empty)
                            { %><a fileID="<%= Model.DocumentAttachmentID %>" class="getFileLink"><%: Model.DocumentAttachmentName
    %></a><button class="fileDelete" onclick=" detachAttachedDocument(this);return false; ">&nbsp;</button></div><% } %>
                    <%= Html.FileForm("") %></td>
            </tr>
        </tbody>
    </table>
    <gv:BaseDocumentEditCommonPart ID="commonPart" runat="server" />
    <script type="text/javascript">

        var institutionData = JSON.parse('<%= Html.Serialize(Model.InstitutionList) %>');
        var professionData = JSON.parse('<%= Html.Serialize(Model.ProfessionList) %>');
        var qualificationData = JSON.parse('<%= Html.Serialize(Model.QualificationList) %>');
        jQuery(function() {
            $('#DocumentSeries, #DocumentNumber').blur(function() {
                checkDocumentOnExisting(jQuery('#DocumentSeries').val(), jQuery('#DocumentNumber').val());
            });
        });

        function fillModel() {
            var model =  {
                DocumentTypeID: <%= Model.DocumentTypeID %>,
                UID: jQuery('#UID').val(),
                DocumentSeries: jQuery('#DocumentSeries').val(),
                DocumentNumber: jQuery('#DocumentNumber').val(),
                DocumentDate: jQuery('#DocumentDate').val(),
                RegistrationNumber: jQuery('#RegistrationNumber').val(),
                QualificationTypeName: jQuery('#QualificationTypeName').val(),
                ProfessionTypeName: jQuery('#ProfessionTypeName').val(),
                InstitutionName: jQuery('#InstitutionName').val(),
                GPA: jQuery('#GPA').val()
            };
            return model;
        }

        /*autocompleteDropdown(jQuery('#InstitutionName'), {
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
        });*/

        autocompleteDropdown(jQuery('#ProfessionTypeName'), {
            source: function(ui, response) {
                var res = [];
                var x = ui.term.toUpperCase();
                for (var i = 0; i < professionData.length; i++)
                    if (professionData[i].Name.toUpperCase().indexOf(x) >= 0) {
                        res.push(professionData[i].Name);
                        if (res.length > 10) break;
                    }
                response(res);
            },
            minLength: 3
        });

        autocompleteDropdown(jQuery('#QualificationTypeName'), {
            source: function(ui, response) {
                var res = [];
                var x = ui.term.toUpperCase();
                for (var i = 0; i < qualificationData.length; i++)
                    if (qualificationData[i].Name.toUpperCase().indexOf(x) >= 0) {
                        res.push(qualificationData[i].Name);
                        if (res.length > 10) break;
                    }
                response(res);
            },
            minLength: 3
        });

        function setProfessionByID(prID) {
            jQuery("#ProfessionTypeID").val('');
            jQuery(professionData).each(function(i, e) {
                if (e.ID == prID) {
                    jQuery("#ProfessionTypeID").val(e.Name);
                    return false;
                }
            });
        }

        setProfessionByID(jQuery("#ProfessionTypeID").val());

        function setQualificationByID(prID) {
            jQuery("#QualificationTypeID").val('');
            jQuery(qualificationData).each(function(i, e) {
                if (e.ID == prID) {
                    jQuery("#QualificationTypeID").val(e.Name);
                    return false;
                }
            });
        }

        setQualificationByID(jQuery("#QualificationTypeID").val());
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
/*jQuery(function(){
           $("input#GPA").mask("?999,9999" ,{placeholder:" "});
        });*/

        function applyTitles() {
            jQuery('#ProfessionTypeID option').each(function() { jQuery(this).attr('title', jQuery(this).text()); });
        }

        applyTitles();
    </script>
</div>