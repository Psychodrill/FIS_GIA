<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.AddStructureItemViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<div id="dialogCaption" style="display: none"><%: Model.StructureItemName %></div>
<div id="content">
    <table class="gvuzData">
        <thead>
            <tr>
                <th class="caption">
                </th>
                <th>
                </th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="caption">
                    <%= Html.LabelFor(m => m.StructureType) %>:
                </td>
                <td>
                    <%= Html.DropDownListFor(m => m.StructureType, new SelectList(Model.StructureTypeList, "ID", "Description")) %>
                </td>
            </tr>
            <tr>
                <td class="caption" id="tdFullName">
                    <%= Html.LabelFor(m => m.FullName) %>:
                </td>
                <td>
                    <%= Html.TextBoxExFor(m => m.FullName) %> <%= Html.DropDownListFor(m => m.Directions, new SelectList(Model.Directions, "Code", "Name")) %>
                </td>
            </tr>
            <tr id="trBriefName">
                <td class="caption">
                    <%: Html.LabelFor(m => m.BriefName) %>:
                </td>
                <td>
                    <%= Html.TextBoxExFor(m => m.BriefName) %>
                </td>
            </tr>
            <tr id="trDirectionCode">
                <td class="caption">
                    <%: Html.LabelFor(m => m.DirectionCode) %>:
                </td>
                <td>
                    <%= Html.TextBoxExFor(m => m.DirectionCode) %>
                </td>
            </tr>
            <tr id="trSite">
                <td class="caption">
                    <%: Html.LabelFor(m => m.Site) %>:
                </td>
                <td>
                    <%= Html.TextBoxExFor(m => m.Site) %>
                </td>
            </tr>
        </tbody>
    </table>
    <div style="display: none">
        <input id="btnSubmit" type="button" value="Сохранить" />
        <input id="btnCancel" type="button" value="Отмена" />
    </div>
</div>
<script type="text/javascript">
    function setFieldsVisibility() {
        var v = jQuery('#StructureType').val();
        if (v == 3) {
            jQuery('#FullName').hide();
            jQuery('#Directions').show();
            jQuery('#trBriefName').hide();
            jQuery('#trDirectionCode').show();
            jQuery('#trSite').hide();

            jQuery('#DirectionCode').attr('disabled', 'disabled');
            setFieldValues();
            jQuery('#tdFullName').html('Наименование');
        } else {
            jQuery('#FullName').show();
            jQuery('#Directions').hide();
            jQuery('#trBriefName').show();
            jQuery('#trDirectionCode').hide();
            jQuery('#trSite').show();

            jQuery('#DirectionCode').attr('disabled', 'disabled');
            jQuery('#DirectionCode').val('');
            jQuery('#tdFullName').html('Полное наименование');
        }
        if (document.getElementById('StructureType').options.length == 0)
            jQuery('#btnSubmit').attr('disabled', 'disabled');
    }

    function setFieldValues() {
        var d = document.getElementById('Directions');
        if (d.selectedIndex >= 0) {
            jQuery('#DirectionCode').val(d.value);
            jQuery('#FullName').val(d.options[d.selectedIndex].text);
        } else if (d.options.length == 0) {
            jQuery('#FullName').val('');
            jQuery('#DirectionCode').val('');
        }
    }

    jQuery('#btnCancel').click(function() { closeDialog(jQuery('#treeControlDialog')); });
    jQuery('#btnSubmit').click(function() {
        if (revalidatePage(jQuery('.gvuzData'))) return;
        var model = {
            StructureItemID: <%= Model.StructureItemID %>,
            FullName: jQuery('#FullName').val(),
            BriefName: jQuery('#BriefName').val(),
            DirectionCode: jQuery('#DirectionCode').val(),
            StructureType: jQuery('#StructureType').val(),
            Site: jQuery('#Site').val()
        };
        doPostAjax(
            '<%= Model.IsAdd ? Url.Generate<StructureController>(x => x.CreateItem(null)) : Url.Generate<StructureController>(x => x.SaveItem(null)) %>',
            JSON.stringify(model),
            function(data, status) {
                if (!addValidationErrorsFromServerResponse(data)) {
                    createdItem = data.Data;
                    jQuery('#btnCancel').click();
                }
            });
    });
    <% if (!Model.IsAdd)
       { %>;
    jQuery('#StructureType').attr('disabled', 'disabled');
    jQuery('#Directions').val('<%= Model.DirectionCode %>');
    <% } %>;
    jQuery('#StructureType').change(setFieldsVisibility);
    jQuery('#Directions').change(setFieldValues);
    setFieldsVisibility();
</script>