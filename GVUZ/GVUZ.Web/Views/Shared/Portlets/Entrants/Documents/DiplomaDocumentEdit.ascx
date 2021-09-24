<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Model.Entrants.Documents.DiplomaDocumentViewModel>" %>
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
                <td><%= Html.TextBoxExFor(m => m.DocumentSeries, new {style = "width:40px"}) %><%= Html.TextBoxExFor(m => m.DocumentNumber, new {style = "width:100px"}) %></td>
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
                <%--<td><%= Html.DropDownListFor(m => m.QualificationTypeID, new SelectList(Model.QualificationList, "ID", "Name"))%></td>--%>
                <td><%= Html.TextBoxExFor(m => m.QualificationTypeName, new {style = "width:500px"}) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.SpecialityTypeID) %></td>
                <%--<td><%= Html.DropDownListFor(m => m.SpecialityTypeID, new SelectList(Model.SpecialityList, "ID", "Name"))%></td>--%>
                <td><%= Html.TextBoxExFor(m => m.SpecialityTypeName, new {style = "width:500px"}) %></td>
            </tr>
            <%--<tr>
				<td class="caption"><input type="checkbox" id="specializationCb" <%= Model.SpecializationTypeID.HasValue ? "checked=\"checked\"" : "" %> />
				<%= Html.TableLabelReqFor(m => m.SpecializationTypeID) %></td>
				<td><%= Html.DropDownListFor(m => m.SpecializationTypeID, new SelectList(Model.SpecializationList, "ID", "Name"), new { style = "width:512px" })%></td>
			</tr>--%>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.InstitutionName) %></td>
                <td><%= Html.TextBoxExFor(m => m.InstitutionName, new {style = "width:500px"}) %></td>
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

        var institutionData = JSON.parse('<%= Html.Serialize(Model.InstitutionList) %>');
        var qualificationData = JSON.parse('<%= Html.Serialize(Model.QualificationList) %>');
        var specialityData = JSON.parse('<%= Html.Serialize(Model.SpecialityList) %>');
        jQuery(function() {
            $('#DocumentSeries,#DocumentNumber').blur(function() {
                checkDocumentOnExisting(jQuery('#DocumentSeries').val(), jQuery('#DocumentNumber').val());
            });

            $('#QualificationTypeName').focus(function() {
                var $this = $(this);
                $this.data('originalValue', $this.val());
            });
            
            $('#QualificationTypeName').blur(function() {
                var $this = $(this);
                
                var selectedQualification = $this.val();
                var originalValue = $this.data('originalValue');

                if (selectedQualification != originalValue) {
                 
                    specialityData = [];
                    var $specialityName = $('#SpecialityTypeName');
                    $specialityName.val('');
                    
                    if (selectedQualification && selectedQualification.length > 0) {
                        //console.log('Load speciality for ', selectedQualification);
                        $specialityName.attr('disabled', 'disabled');

                        $.ajax({
                            url: '<%=Url.Action("FindSpecialityByQualification", "Entrant") %>',
                            contentType: 'application/json; charset=utf-8',
                            type: 'POST',
                            dataType: 'json',
                            data: JSON.stringify({ qualification: selectedQualification }),
                            success: function(data) {
                                if (isArrayNotEmpty(data)) {
                                    specialityData = data;
                                    if (specialityData.length == 1) {
                                        $('#SpecialityTypeName').val(specialityData[0].Name);
                                    }
                                }
                            },
                            error: showAsyncError,
                            complete: function() {
                                $specialityName.removeAttr('disabled');
                            }
                        });
                    }
                }
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
                RegistrationNumber: jQuery('#RegistrationNumber').val(),
                QualificationTypeName: jQuery('#QualificationTypeName').val(),
                SpecialityTypeName: jQuery('#SpecialityTypeName').val(),
                //SpecializationTypeID: jQuery('#specializationCb').attr('checked') ? jQuery('#SpecializationTypeID').val() : null,
                InstitutionName: jQuery('#InstitutionName').val(),
                GPA: jQuery('#GPA').val()
            };
            return model;
        }

        /*autocompleteDropdown(jQuery('#InstitutionName'),
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
        autocompleteDropdown(jQuery('#QualificationTypeName'),
            {
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
            }
        );
        autocompleteDropdown(jQuery('#SpecialityTypeName'),
            {
                source: function(ui, response) {
                    var res = [];
                    var x = ui.term.toUpperCase();
                    for (var i = 0; i < specialityData.length; i++)
                        if (specialityData[i].Name.toUpperCase().indexOf(x) >= 0) {
                            res.push(specialityData[i].Name);
                            if (res.length > 10) break;
                        }
                    response(res);
                },
                minLength: 3
            }
        );
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


        function setSpecializationVisibility() {
            if (jQuery('#specializationCb').attr('checked'))
                jQuery('#SpecializationTypeID').removeAttr('disabled');
            else
                jQuery('#SpecializationTypeID').attr('disabled', 'disabled');
        }

        jQuery('#specializationCb').change(setSpecializationVisibility);
        setSpecializationVisibility();
    </script>
</div>