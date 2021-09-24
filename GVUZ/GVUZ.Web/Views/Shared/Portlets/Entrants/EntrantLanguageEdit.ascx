<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.Portlets.Entrants.EntrantLanguageViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<div id="content">
    <% if (Model.ShowDenyMessage) { %> 
      <div>Невозможно редактировать данное заявление</div>
      <script type="text/javascript">     function doSubmit() { return false; }</script>  
    <% }else{ %>
        <table class="gvuzDataGrid" style="width: 400px">
            <colgroup><col width="355" /><col width="45" /></colgroup>
            <thead><tr><th class="caption" align="center" colspan="2">Иностранные Языки</th></tr></thead>
            <tbody>
                <tr id="trAddNewLang"><td colspan="2"><a href="#" id="btnAddNewLang" class="add">Добавить язык</a></td></tr>
            </tbody>
        </table>
        <br />
        <% if (Model.ApplicationStep == 0){ %>
            <div>
                <a id="btnSubmit" href="<%= Url.Generate<EntrantController>(c => c.Languages()) %>">Сохранить</a>
                <a id="btnCancel" href="<%= Url.Generate<EntrantController>(c => c.Languages()) %>">Отмена</a>
            </div>
        <% } %>
        <script type="text/javascript">
            var existingLangs = JSON.parse('<%= Html.Serialize(Model.LanguageData) %>');
            var allLangs = JSON.parse('<%= Html.Serialize(Model.LanguageList) %>');

            function doAddViewRow($trBefore, item) {
                var itemName = '';
                jQuery.each(allLangs, function(i, n) {
                    if (n.ID == item) {
                        itemName = n.Name;
                        return false;
                    } else return true;
                });
                $trBefore.before('<tr><td class="itemData" itemID="' + item + '">' + escapeHtml(itemName) + '</td>'
                    + '<td align="center" nowrap="nowrap">'
                    + '<a href="#" title="Редактировать" class="btnEdit" onclick="doEditRow(this);return false;"></a>&nbsp;'
                    + '<a href="#" class="btnDelete" title="Удалить" onclick="doDeleteRow(this);return false;"></a></td></tr>');
            }

            function fillExistingLangs() {
                jQuery.each(existingLangs, function(i, item) { doAddViewRow(jQuery('#trAddNewLang'), item); });
                if (jQuery('.gvuzDataGrid tr td.itemData').length == allLangs.length)
                    jQuery('#trAddNewLang').hide();
            }

            function doSaveRow(el) {
                var $tr = jQuery(el).parents('tr:first');
                var selVar = $tr.find('.addNewLangSelect').val();
                var selID = $tr.find('.addNewLangSelect > option:selected').val();
                doAddViewRow($tr, selID);
                $tr.remove().detach();
                if (jQuery('.gvuzDataGrid tr td.itemData').length < allLangs.length)
                    jQuery('#trAddNewLang').show();
            }

            function doDeleteRow(el) {
                jQuery(el).parents('tr:first').remove().detach();
                if (jQuery('.addNewLangSelect').length == 0)
                    jQuery('#trAddNewLang').show();
            }

            function doEditRow(el) {
                var $sel = jQuery('.addNewLangSelect');
                if ($sel.length > 0)
                    $sel.parents('tr:first').find('.btnSave').click();
                $tr = jQuery(el).parents('tr:first');
                addNewLangRow($tr);
                $tr.remove().detach();
            }

            function addNewLangRow($trBefore) {
                var itemID = $trBefore.children('td:first').attr('itemID');
                $trBefore.before('<tr><td align="center">'
                    + '<select class="addNewLangSelect" /></td><td align="center"><a href="#" title="Сохранить" class="btnSave" onclick="doSaveRow(this);return false;"></a> '
                    + '<a href="#" title="Удалить" class="btnDeleteU" onclick="doDeleteRow(this);return false;"></a></td></tr>');
                var lData = [];
                jQuery('.gvuzDataGrid tr td.itemData').each(function() {
                    var ii = jQuery(this).attr('itemID');
                    if (ii != itemID) lData.push(ii);
                });
                jQuery('.addNewLangSelect').html(jQuery.map(allLangs, function(n) {
                    if (jQuery.inArray(n.ID.toString(), lData) >= 0) return '';
                    return '<option value="' + n.ID + '">' + n.Name + '</option>';
                }).join(''));
                if (itemID != undefined) {
                    jQuery('.addNewLangSelect').val(itemID);
                    jQuery('.addNewLangSelect option:selected').css('background-color', '#f3f3f3');
                }
                if (jQuery('.addNewLangSelect option').length == 0)
                    jQuery('.addNewLangSelect').attr('disabled', 'disabled');
                else
                    jQuery('.addNewLangSelect').focus();
                jQuery('#trAddNewLang').hide();
            }

            function doSubmit() {
                if (jQuery('.addNewLangSelect').length > 0 && !confirm('На странице есть несохранённый язык. Он не будет добавлен. Продолжить?'))
                    return false;
                var model = {
                    EntrantID: <%= Model.EntrantID %>,
                    ApplicationID: <%= Model.ApplicationID.HasValue ? Model.ApplicationID.Value.ToString() : "null" %>,
                    LanguageData: []
                };
                var isError = false;
                jQuery('.gvuzDataGrid tr td.itemData').each(function() {
                    var itemID = jQuery(this).attr('itemID');
                    if (jQuery.inArray(itemID, model.LanguageData) >= 0) {
                        alert('Повторяется язык: ' + jQuery(this).html());
                        isError = true;
                        return false;
                    }
                    model.LanguageData.push(itemID);
                });
                if (isError == true)
                    return false;
                doPostAjax("<%= Url.Generate<EntrantController>(x => x.SaveEntrantLanguages(null)) %>", 'model=' + JSON.stringify(model), function(data) {
                    if (!addValidationErrorsFromServerResponse(data)) {
                        if (typeof doAppNavigate != "undefined")
                            doAppNavigate();
                        else
                            jQuery('#btnCancel').click();
                    }
                }, "application/x-www-form-urlencoded", null);
                return false;
            }


            jQuery(document).ready(function() {

                jQuery('#btnCancel, #btnSubmit').button();

                jQuery('#postFile').change(function() { fileID = 0; });
                jQuery('#btnSubmit').click(function() {
                    doSubmit();
                    return false;
                });
                jQuery('#btnCancel').click(function() { window.location.href = jQuery(this).attr('href'); });
                jQuery('#btnAddNewLang').click(function() {
                    addNewLangRow(jQuery('#trAddNewLang'));
                    return false;
                });
                fillExistingLangs();
            });
        </script>
    <% } %>
</div>