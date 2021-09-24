<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Model.Entrants.Documents.OlympicDocumentViewModel>" %>
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
                <td class="caption"><%= Html.TableLabelReqFor(m => m.DocumentSeries) %></td>
                <td><%= Html.TextBoxExFor(m => m.DocumentSeries) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.DocumentNumber) %></td>
                <td><%= Html.TextBoxExFor(m => m.DocumentNumber) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.DiplomaTypeID) %></td>
                <td><%= Html.DropDownListFor(m => m.DiplomaTypeID, new SelectList(Model.DiplomaList, "ID", "Name")) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.OlympicID) %></td>
                <td><%= Html.DropDownListFor(m => m.OlympicID, new SelectList(new string[0])) %></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.OlympicDetails.SubjectNames) %></td>
                <td id="tdSubjectNames"></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.OlympicDetails.LevelName) %></td>
                <td id="tdLevelName"></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.OlympicDetails.OlympicYear) %></td>
                <td id="tdOlympicYear"></td>
            </tr>
            <tr>
                <td class="caption"><%= Html.TableLabelReqFor(m => m.OlympicDetails.OrganizerName) %></td>
                <td id="tdOrganizerName"></td>
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

        var DocumentSeries = null;

        jQuery(function() {
            $('#DocumentNumber').blur(function() {
                checkDocumentOnExisting(null, jQuery('#DocumentNumber').val());
            });

            $('#DocumentSeries').blur(function() {
                //alert('s'+$('#DocumentSeries').val()+'s');
                if ((($('#DocumentSeries').val() + '') == '') || ($('#DocumentSeries').val() == ' ')) {
                    DocumentSeries = null;
                    $('input#DocumentSeries').val(' ');
                } else {
                    DocumentSeries = $('#DocumentSeries').val();
                }
                ;
            });
        });
        var olympicDatas = JSON.parse('<%= Html.Serialize(Model.OlympicDatas) %>');

        function fillModel() {
            var model =
            {
                DocumentTypeID: <%= Model.DocumentTypeID %>,
                UID: jQuery("#UID").val(),
                DocumentSeries: DocumentSeries,
                DocumentNumber: jQuery('#DocumentNumber').val(),
                DiplomaTypeID: jQuery('#DiplomaTypeID').val(),
                OlympicID: jQuery('#OlympicID').val()
            };
            return model;
        }

        function fillList() {
            var res = '';
            for (var i = 0; i < olympicDatas.length; i++)
                res += '<option value="' + olympicDatas[i].OlympicID + '">' + olympicDatas[i].OlympicName + '</option>';
            jQuery('#OlympicID').html(res);
            if (<%= Model.OlympicID %> > 0) jQuery('#OlympicID').val(<%= Model.OlympicID %>);
            fillData();
            if (jQuery('#OlympicID')[0].setAttribute)
                jQuery('#OlympicID')[0].setAttribute('onchange', "fillData()");
            else
                jQuery('#OlympicID').change(fillData);
        }

        function fillData() {
            var sel = jQuery('#OlympicID').val();
            for (var i = 0; i < olympicDatas.length; i++)                
                if (olympicDatas[i].OlympicID == sel) {
                    jQuery('#tdOrganizerName').html(olympicDatas[i].OrganizerName);
                    jQuery('#tdOlympicYear').html(olympicDatas[i].OlympicYear);

                    var lName = '';
                    if (olympicDatas[i].LevelName != null)
                        for (var k = 0; k < olympicDatas[i].LevelName.length; k++)
                            lName += olympicDatas[i].LevelName[k] + '<br/>';
                    jQuery('#tdLevelName').html(lName);
                    
                    var sNames = '';
                    if (olympicDatas[i].SubjectNames != null)
                        for (var k = 0; k < olympicDatas[i].SubjectNames.length; k++)
                            sNames += olympicDatas[i].SubjectNames[k] + '<br/>';
                    jQuery('#tdSubjectNames').html(sNames);
                }
        }

        fillList();
    </script>
</div>