<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<GVUZ.DAL.Dapper.ViewModel.Olympics.OlympicsListViewModel>" %>

<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Import Namespace="GVUZ.DAL.Dapper.ViewModel.Olympics" %>

<asp:Content ContentPlaceHolderID="TitleContent" runat="server">Администрирование - Справочники системы</asp:Content>

<asp:Content ContentPlaceHolderID="PageTitle" runat="server">Олимпиады</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">

    <% 
        var ssClass = new { style = "width: 300px" };
        var labelsInsideClass = new { @class = "labelsInside" };
    %>

    <div id="dialog">
    </div>

    <div class="navigation">
        <span class="back">
            <%=Url.GenerateLink<AdministrationController>(c=>c.CatalogsList(),"Назад") %>
        </span>
    </div>
    <p class="left10 navigation" style="margin-bottom: 0px;"><a href="#" id="btnAddNewTop" class="add">Добавить олимпиаду</a></p>

    <div style="white-space: nowrap; padding: 8px; padding-left: 0">
        <label for="changeYear"><%= Html.DisplayNameFor(x => x.Filter.Year) %>:</label>
        &nbsp;
    <%= Html.DropDownListExFor(x=>x.Filter.Year, Model.Years, new { @id="changeYear",  @class = "ss", style="width: 100px;" })%>
    &nbsp;
    <label for="changeName"><%= Html.DisplayNameFor(x => x.Filter.Name) %>:</label>
        &nbsp;
    <%= Html.DropDownListExFor(x=>x.Filter.Name, Model.Names, new { @id="changeName", @class = "ss", style="width: 500px;" })%>
        <label for="changeProfile"><%= Html.DisplayNameFor(x => x.Filter.Profile) %>:</label>
        &nbsp;
    <%= Html.DropDownListExFor(x=>x.Filter.Profile, Model.Profiles, new { @id="changeProfile", @class = "ss", style="width: 300px;" })%>
    </div>

    <div class="tableHeader2l tableHeaderCollapsed" style="height: 100%">
        <div id="divFilterPlace">
            <div class="hideTable" onclick="ToggleFilter()" style="float: left"><span id="btnShowFilter">Отобразить фильтр</span> </div>
        </div>

        <div class="appCount">Записей:&nbsp;<span id="totalcount" style="color: crimson">0</span></div>

        <div id="messageFilter" style="text-align: right;">
            <span id="btnClear" class="linkSumulator" style="color: salmon; font-size: 11px; font-weight: bold; padding-right: 5px;">[Внимание! Применен фильтр] (сбросить)</span>
        </div>

        <div id="divFilter" style="display: none; clear: both;">
            <table class="tableForm">
                <tbody>
                    <tr>
                        <td class="labelsInside">
                            <%= Html.TableLabelFor(x => x.Filter.Level)%>
                        </td>
                        <td>
                            <%= Html.DropDownListExFor(x=>x.Filter.Level, Model.Levels, new { @id="changeLevel", @class = "ssClass" })%>
                        </td>
                        <td class="labelsInside">
                            <%= Html.TableLabelFor(x => x.Filter.Organizer)%>
                        </td>
                        <td>
                            <%= Html.TextBoxExFor(x=>x.Filter.Organizer, new { @id="changeOrganizer", @class = "ssClass" })%>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelsInside">
                            <%= Html.TableLabelFor(x => x.Filter.Subject)%>
                        </td>
                        <td>
                            <%= Html.DropDownListExFor(x=>x.Filter.Subject, Model.Subjects, new { @id="changeSubject", @class = "ssClass" })%>
                        </td>
                        <td class="labelsInside">
                            <%= Html.TableLabelFor(x => x.Filter.Vosh)%>
                        </td>
                        <td>
                            <%= Html.DropDownListExFor(x=>x.Filter.Vosh, Model.Voshs, new { @id="changeVosh", @class = "ssClass" })%>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

    <div id="tablerows">
        <% Html.RenderPartial("Olympics/TableView", Model); %>
    </div>

    <div id="oncomplete"></div>

    <p class="left10 navigation" style="margin-bottom: 0px;"><a href="#" id="btnAddNewBottom" class="add">Добавить олимпиаду</a></p>
    <div class="navigation">
        <span class="back">
            <%=Url.GenerateLink<AdministrationController>(c=>c.CatalogsList(),"Назад") %>
        </span>
    </div>


    <script type="text/javascript">

        var filter = {
            Sort: 0, Page: 0,
            HasValue: false,
            Year: 0, Name: 0, Profile: 0, Level: 0, Subject: 0, Vosh: 0,
            Organizer: ''
        };

        $("#btnClear").live('click', function () {
            filter.HasValue = true;

            filter.Year = 0;
            filter.Name = 0;
            filter.Profile = 0;
            filter.Level = 0;
            filter.Organizer = '';
            filter.Subject = 0;
            filter.Vosh = 0;

            $("#changeYear").val(0);
            $("#changeName").val(0);
            $("#changeProfile").val(0);
            $("#changeLevel").val(0);
            $("#changeOrganizer").val('');
            $("#changeSubject").val(0);
            $("#changeVosh").val(0);

            OnChangeFilter();
        });


        $(document).ready(function () {

            $("#changeYear").change(function () {
                filter.HasValue = true;
                filter.Year = $("#changeYear").val();
                OnChangeFilter();
            });
            $("#changeName").change(function () {
                filter.HasValue = true;
                filter.Name = $("#changeName").val();
                OnChangeFilter();
            });
            $("#changeProfile").change(function () {
                filter.HasValue = true;
                filter.Profile = $("#changeProfile").val();
                OnChangeFilter();
            });
            $("#changeLevel").change(function () {
                filter.HasValue = true;
                filter.Level = $("#changeLevel").val();
                OnChangeFilter();
            });
            $("#changeOrganizer").keyup(function () {
                filter.HasValue = true;
                filter.Organizer = $("#changeOrganizer").val();
                OnChangeFilter();
            });
            $("#changeSubject").change(function () {
                filter.HasValue = true;
                filter.Subject = $("#changeSubject").val();
                OnChangeFilter();
            });
            $("#changeVosh").change(function () {
                filter.HasValue = true;
                filter.Vosh = $("#changeVosh").val();
                OnChangeFilter();
            });

            $('#btnAddNewTop,#btnAddNewBottom').click(function () {
                doAddEdit('<%= Url.Generate<AdministrationController>(x => x.OlympicsCatalogAdd()) %>', '',
				function () {
				    OnChangeFilter();
				})
            })

            $('.btnEdit').live('click', function () {
                var $tr = $(this).parents('tr:first')
                var itemID = $tr.attr('itemID')
                doAddEdit('<%= Url.Generate<AdministrationController>(x => x.OlympicsCatalogEdit(null)) %>', 'id=' + itemID,
				function () {
				    OnChangeFilter();
				});
            })

            $('.btnDelete').live('click', function () {
                var $tr = $(this).parents('tr:first');
                var itemID = $tr.attr('itemID')
                confirmDialog('Вы действительно хотите удалить олимпиаду?', function () {
                    doPostAjax('<%= Url.Generate<AdministrationController>(x => x.OlympicsCatalogDelete(null)) %>', 'id=' + itemID, function (data) {
                        if (data.IsError) $('<div>' + data.Message + '</div>').dialog(
						{ buttons: { OK: function () { $(this).dialog("close"); } } })
                        else {
                            OnChangeFilter();
                        }
                    }, "application/x-www-form-urlencoded")
                });

            })
        });

        function OnChangeFilter() {
            filter.Page = '<%=Model.PagedData.PageNumber%>';
            $.ajax({
                data: filter,
                type: "POST",
                url: '<%= (Url.Action("OlympicsCatalog","Administration", null, Request.Url.Scheme))%>',
                cache: false,
                success: function (data) {
                    $('#tablerows').html(data);
                    DrawSort();
                }
            });
        }

        function ToggleFilter() {
            if ($('#btnShowFilter').hasClass('filterDisplayed')) {
                $('#btnShowFilter').removeClass('filterDisplayed')
                $('#btnShowFilter').html('Отобразить фильтр')
                $('#btnShowFilter').parent().removeClass('nonHideTable')
                $('#btnShowFilter').parent().parent().parent().addClass('tableHeaderCollapsed')
                $('#divFilter').hide()
            }
            else {
                $('#btnShowFilter').addClass('filterDisplayed')
                $('#btnShowFilter').html('Скрыть фильтр')
                $('#btnShowFilter').parent().addClass('nonHideTable')
                $('#btnShowFilter').parent().parent().parent().removeClass('tableHeaderCollapsed')
                $('#divFilter').show()
            }
        };

        function DoSort(el, sortID) {
            var isSortedUp = $(el).hasClass('sortedUp')
            filter.Sort = isSortedUp ? -sortID : sortID;
            $(el).removeClass('sortedUp');
            OnChangeFilter();
        }

        function DrawSort() {
            $('.sortUp,.sortDown').remove().detach();

            var c = filter.Sort;
            if (filter.Sort < 0) {
                c = -filter.Sort;
                $('#col' + c).after('<span class="sortDown"></span>');
                $('#col' + c).addClass('sortedUp');
            }
            else
                if (filter.Sort > 0) {
                    $('#col' + c).after('<span class="sortUp"></span>');
                    $('#col' + c).addClass('sortedUp');
                }
        }

        function PagedOnComplete(obj) {
            //var $oncomplete = $('#oncomplete');
            //$oncomplete
            //    .text('Пэйджинг выполнен')
            //    .css('backgroundColor', 'yellow')
            //    .fadeOut({
            //        complete: function () {
            //            $oncomplete.css('backgroundColor', 'transparent').text('').show();
            //        }
            //    });
        }

        function PagedOnBegin(obj) {
        }

        var doAddEdit = function (navUrl, postData, callback) {

            createdItem = null
            doPostAjax(navUrl, postData, function (data) {
                $('#dialog').html(data);
                $('#dialog').dialog({
                    modal: true,
                    width: 900,
                    title: (postData == '' ? "Добавление" : "Редактирование") + " олимпиады",
                    buttons:
							{
							    "Сохранить": function () { $('#btnSubmit').click(); },
							    "Отмена": function () { $('#btnCancel').click(); }
							},
                    close: function () {
                        if (createdItem) callback()
                    }
                }).dialog('open');

                $(".chosen").chosen({ max_selected_options: 10, placeholder_text: "щелкните и выберите предметы..." });
                $(".chosen-deselect").chosen({ allow_single_deselect: true });
                $(".chosen").chosen().change();
                $(".chosen").trigger('liszt:updated');

                $("#editOrganizerName").autocomplete({
                    source: '<%= Url.Generate<AdministrationController>(x => x.GetInstitution(null)) %>',
                    select: function (event, ui) {
                        var id = 'id=' + ui.item.id;
                        $('#editOrganizerID').val(ui.item.id);
                        doPostAjax('<%= Url.Generate<AdministrationController>(x => x.GetAddressForInstitution(null)) %>', id, function (data) {
                            $('#editOrganizerAddress').val(data);
                        }, "application/x-www-form-urlencoded")
                        return true;
                    }
                });
                $("#editCoOrganizerName").autocomplete({
                    source: '<%= Url.Generate<AdministrationController>(x => x.GetInstitution(null)) %>',
                    select: function (event, ui) {
                        $('#editCoOrganizerID').val(ui.item.id);
                        return true;
                    }
                });

                var o1 = $('#editOrganizerID').val();
                var o2 = $('#editCoOrganizerID').val();
                var o3 = $('#editOrgOlympicEnterID').val();

                if (o3 != "") {
                    if (o3 == o1)
                        $("#y").attr("checked", true)
                    else
                        if (o3 == o2)
                            $("#z").attr("checked", true);
                }

            }, "application/x-www-form-urlencoded", "html")

        }
    </script>

</asp:Content>
