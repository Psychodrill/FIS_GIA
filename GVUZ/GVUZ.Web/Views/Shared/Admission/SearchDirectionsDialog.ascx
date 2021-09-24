<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GVUZ.Web.ViewModels.SearchDirectionsDialogViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers.Admission" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="System.Collections.Generic" %>

<% 
    const string NotSelected = "[Не выбрано]";
    var eduLevelAttr = new Dictionary<string, object> { { "data-bind", "value: eduLevelId" } };
    var ugsAttr = new Dictionary<string, object> { { "data-bind", "value: ugsId" } };
    var instId = Guid.NewGuid().ToString("N");
%>
<div id="dlg_search_directions" style="display: none">
    <table class="data">
	    <tbody>
		    <tr>
			    <td class="caption"><%= Html.TableLabelFor(x => x.EducationLevelId) %></td>
			    <td><%= Html.DropDownListFor(x => x.EducationLevelId, Model.EducationLevels.ToSelectList(NotSelected), eduLevelAttr) %></td>
		    </tr>
		    <tr>
			    <td class="caption"><%= Html.TableLabelFor(x => x.UgsId) %></td>
			    <td><%= Html.DropDownListFor(x => x.UgsId, Model.Ugs.ToSelectList(NotSelected), ugsAttr) %></td>
		    </tr>
		    <tr>
			    <td colspan="2">
				    <div style="padding-top: 10px;padding-left: 10px;" data-bind="visible: !hasDirs()">Отсутствуют подходящие специальности</div>
                    <div style="width: auto;max-height: 100px;overflow-x: hidden;overflow-y: auto">
                    <ol data-bind="visible: hasDirs(),foreach: dirList">
                        <li><input type="checkbox" data-bind="attr: {'id': $root.getDirId($data)}, checkedValue: $data, checked: $root.checkedDirs"/><label data-bind="attr: {'for': $root.getDirId($data)}, text: Name"></label></li>
                    </ol>
                    </div>
			    </td>
		    </tr>
            <!-- ko if: showComment -->
            <tr>
                <td colspan="2">Комментарий:</td>
            </tr>
            <tr>
                <td colspan="2">
                    <textarea style="width: 640px; height: 150px;" cols="91" rows="9" data-bind="textInput: comment, attr: { placeholder: commentHint }"></textarea>
                </td>
            </tr>
            <!-- /ko -->
	    </tbody>
    </table>
</div>

<script type="text/javascript">
    function SearchDirectionsDialogViewModel(options) {

        this._searchCommand = {
            SearchType: options.searchType,
            TempDirectionsId: options.tempId || [],
            Year: options.year
        };

        this.showComment = options.showComment === true;
        this.commentHint = options.commentHint;

        this.eduLevelId = ko.observable('');
        this.ugsId = ko.observable('');
        this.comment = ko.observable('');
        this.dirList = ko.observableArray([]);
        this.checkedDirs = ko.observableArray([]);

        this.hasDirs = ko.pureComputed(this._getHasDirs, this);
        this.checkedId = ko.pureComputed(this._getCheckedId, this);

        this.ugsIdHandler = this.ugsId.subscribe(this._search, this);
        this.eduLevelIdHandler = this.eduLevelId.subscribe(this._search, this);
    }

    SearchDirectionsDialogViewModel.prototype = {
        _getHasDirs: function () {
            return this.dirList().length > 0;
        },
        getDirId: function (item) {
            return 'dir<%= instId %>_' + item.Id;
        },
        _getCheckedId: function () {
            return ko.utils.arrayMap(this.checkedDirs(), function (item) {
                return item.Id;
            });
        },
        _search: function(){
            this._searchCommand.UgsId = parseInt(this.ugsId());
            this._searchCommand.EducationLevelId = parseInt(this.eduLevelId());

            if (isNaN(this._searchCommand.UgsId) || this._searchCommand.UgsId <= 0 ||
                isNaN(this._searchCommand.EducationLevelId) || this._searchCommand.EducationLevelId <= 0) {
                this.dirList.removeAll();
                return;
            };

            doPostAjax('<%= Url.Generate<AdmissionController>(c => c.SearchDirections(null)) %>', JSON.stringify(this._searchCommand), this.dirList, null, 'json');
        },
        dispose: function () {
            this.hasDirs.dispose();
            this.checkedId.dispose();
            this.ugsIdHandler.dispose();
            this.eduLevelIdHandler.dispose();
        }
    }
</script>

