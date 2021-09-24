<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.RequestProfDirectionsDataViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers.Admission" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="GVUZ.DAL.Dto" %>

<% 
   var yearListAttr = new Dictionary<string, object>
   {
       { "style", "width: 100px;" },
       { "data-bind", "value: year" }
   };
%>

<div id="dlg_manage_profdirections" style="display: none">
    <div style="margin-bottom: 8px;white-space: nowrap">
        <%= Html.LabelFor(m => m.Year) %>:&nbsp;
        <%= Html.DropDownListFor(m => m.Year, Model.YearList, yearListAttr) %>
    </div>
    <fieldset style="padding: 8px;margin-bottom: 8px;">
        <legend>Список направлений на добавление</legend>
        <div data-bind="visible: hasAddedItems()" style="width: auto;max-height: 100px;overflow-x: hidden;overflow-y: auto;">
            <ol data-bind="foreach: addedItems">
                <li>
                    <span data-bind="text: DisplayName"></span>
                    <a class="btnDelete" href="javascript:void(0)" data-bind="click: function () { $root.deleteRequest.apply($root, arguments); }" title="Удалить заявку">&nbsp;</a></li>
            </ol>
        </div>
        <div style="text-align: right">
            <a class="button3" href="javascript:void(0)" data-bind="click: addDirections">Добавить направления</a>  
        </div>
    </fieldset>
    <fieldset style="padding: 8px">
        <legend>Отклоненные</legend>
        <div data-bind="visible: hasDeniedItems()" style="width: auto;max-height: 100px;overflow-x: hidden;overflow-y: auto;">
            <ol data-bind="foreach: deniedItems">
                <li><span data-bind="text: DisplayName"></span></li>
            </ol>
        </div>
        <div data-bind="visible: hasDeniedItems()" style="text-align: right">
            <a class="button3" href="javascript:void(0)" data-bind="click: clearDenied">Очистить отклоненные</a>
        </div>
    </fieldset>
</div>


<script type="text/javascript">
    function addProfDirectionsRequest() {
        var $dialog = $('#dlg_manage_profdirections');
        
        if (!$dialog.hasClass('ui-content-dialog')) {
            $dialog.dialog({
                modal: true,
                title: 'Управление списком направлений c профильными вступительными испытаниями',
                width: 700,
                buttons: {
                    'Оставить заявку': function () {
                        var model = ko.contextFor(this).$rawData;
                        model.submitPendingRequests(function () {
                            $(this).dialog('close');
                        }, this);
                    }
                }
            });
        };

        var model = ko.contextFor($dialog[0]);
        
        if (!model) {
            model = new RequestProfDirectionsDialogViewModel();
            ko.applyBindings(model, $dialog[0]);
        }
        else{
            model = model.$rawData;
        }

        model.refresh(function () {
            $dialog.dialog('open');
        });
        
    }

    function RequestProfDirectionsDialogViewModel() {
        this.addedItems = ko.observableArray([]);
        this.deniedItems = ko.observableArray([]);
        this.year = ko.observable(<%= DateTime.Now.Year %>);
        this.hasAddedItems = ko.pureComputed(this._getHasAddedItems, this);
        this.hasDeniedItems = ko.pureComputed(this._getHasDeniedItem, this);
    }

    RequestProfDirectionsDialogViewModel.prototype = {
        _getHasAddedItems: function(){
            return this.addedItems().length > 0;
        },
        _getHasDeniedItem: function(){
            return this.deniedItems().length > 0
        },
        refresh: function(cb, scope){
            var url = '<%= Url.Generate<AdmissionController>(c => c.RequestProfDirectionsData()) %>';
            var me = this;
            doPostAjax(url, '', function (data) {
                me.addedItems(data.AddedItems);
                me.deniedItems(data.DeniedItems);
                me.year(data.Year);

                if (cb) {
                    cb.call(scope || window, me);
                }

            });
        },
        submitPendingRequests: function(cb, scope){
            var model = {
                AddedItems: ko.utils.arrayFilter(this.addedItems(), this._isTemp),
                Year: this.year()
            }
            
            doPostAjax('<%= Url.Generate<AdmissionController>(c => c.SubmitProfDirectionRequest(null)) %>', JSON.stringify(model), function(){ if (cb) { cb.call(scope || window); }});
        },
        _mapSelected: function(dir, comment){
            return {RequestId: 0, DirectionId: dir.Id, DisplayName: dir.Name, Comment: comment};
        },
        _appendAdded: function(selectedDirs, comment){
            var me = this;
            var added = ko.utils.arrayMap(selectedDirs, function(dir) { return me._mapSelected(dir, comment);});
            this.addedItems(this.addedItems().concat(added));
        },
        addDirections: function () {
            var me = this;

            var dialogOptions = {
                modal: true,
                width: 700,
                title: 'Поиск специальностей для добавления',
                buttons: {
                    "Выбрать": function () {
                        var model = ko.contextFor(this).$rawData();
                        $(this).dialog('close');
                        me._appendAdded(model.checkedDirs(), model.comment());
                    },
                    "Отмена": function () {
                        $(this).dialog('close');
                    }
                }
            };

            var modelOptions = {searchType: <%= InstitutionDirectionSearchType.IncludeProfDirection.ToString("d") %>, year: this.year(), tempId: this._getTempId(), showComment: true, commentHint: 'Укажите причину добавления специальностей...' };

            searchDirectionsDialog(dialogOptions, modelOptions);
        },
        _isTemp: function(item){
            return item.RequestId <= 0;
        },
        _getTempId: function(){
            return ko.utils.arrayMap(ko.utils.arrayFilter(this.addedItems(), this._isTemp), function(item){ return item.DirectionId;});
        },
        clearDenied: function () {
            var me = this;
            var url = '<%= Url.Generate<AdmissionController>(c => c.ClearDeniedRequests(null)) %>';
            doPostAjax(url, {isProf: true}, function(){ me.deniedItems.removeAll();});
        },
        deleteRequest: function (item) {
            if (item.RequestId <= 0){
                this.addedItems.remove(item);
            }
            else{
                var me = this;
                doPostAjax('<%= Url.Generate<AdmissionController>(c => c.DeleteRequest(null)) %>', JSON.stringify({ requestId: item.RequestId }), function(){
                    me.addedItems.remove(item);
                });
            }
        }
    }

</script>

