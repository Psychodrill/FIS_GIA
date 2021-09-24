<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.RequestListViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Register TagPrefix="gv" TagName="AdminMenuControl" Src="~/Views/Shared/Controls/AdminMenuControl.ascx" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    Список ОО
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PageTitle" runat="server">
	Список ОО
</asp:Content>



<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
<style type="text/css">
	textarea{
	    resize:none;
	}
</style>

	<div class="divstatement">
	<% ViewData["MenuItemID"] = 6; %>
	<gv:AdminMenuControl ID="AdminMenuControl1" runat="server" />	
        <div id="deny">
        <b>Отказать в добавлении/удалении</b><br/>
         <textarea style="width: 640px; height: 150px;" id = "comment" cols="91" rows="9" placeholder="Введите коментарий отказа выполнения заявки..."></textarea>


        </div>
		<div id="dialog">
        	<table class="gvuzDataGrid tableStatement2">
			<thead>
            <tr>
                <th>
                    Направление
                </th>
                <th>
                    Комментарий
                </th>
                <th></th>
                <th> </th>
            </tr>
            </thead>
            <tbody id="directionsTab">
            </tbody>
            <tr>
            <td>

            </td>
            </tr>
            </table>
        
        </div>
		<div id="divPopupApplication"></div>
		
		<!--<div class="tableHeader" id="divFilterRegionF">
			<div class="hideTable nonHideTable" style="float:left;"><span style="cursor:default;">Быстрый поиск</span></div>
			<div id="spAppCountF" class="appCount">Количество записей: <span id="spAppCountFieldF"></span></div>

		</div>-->
		
		<div class="tableHeader5l" id="divFilterRegion" style="display:none">	
	<div id="divFilterPlace">
		<div class="hideTable nonHideTable" style="float:left"><span style="cursor:default;">Расширенный поиск</span></div>
		<div id="spAppCount" class="appCount">Записей:&nbsp;<span id="spAppCountField"></span></div>
	</div>

	</div>

		<div id="content">
			<table class="gvuzDataGrid tableStatement2">
				<thead>
					<tr>
						<th>
							<span class="linkSumulator" onclick="doSort(this, 1)"><%= Html.LabelFor(x => x.InstitutionDataNull.FullName) %></span>
						</th>
						<th>
							<span class="linkSumulator" onclick="doSort(this, 2)">Количество заявок</span>
						</th>
						<th>
							<span class="linkSumulator" onclick="doSort(this, 3)">Дата отправки запроса</span>
						</th>
					</tr>					
				</thead>
				<tbody>
					<tr id="firstRow" style="display:none">	
					</tr>
				</tbody>
			</table>
		</div>
	</div>

	<script language="javascript" type="text/javascript">

		var gridItems = null
		var currentSorting = 0		

        /*$(document).ready(function()
        {
            document.getElementById("Filter_ShortName").onkeypress=function(event) {entergo(event)};
            document.getElementById("Filter_RegionID").onkeypress=function(event) {entergo(event)};
            document.getElementById("Filter_RegionIDF").onkeypress=function(event) {entergo(event)};
            document.getElementById("Filter_Owner").onkeypress=function(event) {entergo(event)};
            document.getElementById("Filter_FullName").onkeypress=function(event) {entergo(event)};
            document.getElementById("Filter_INN").onkeypress=function(event) {entergo(event)}; 
            document.getElementById("Filter_OGRN").onkeypress=function(event) {entergo(event)};
            jQuery('#Filter_RegionID').val('')
            jQuery('#Filter_RegionIDF   ').val('')
	    });*/
		
        function entergo(evt) //Событие - нажатие на кнопку Enter для начала поиска.
        {
            evt = (evt) ? evt : window.event
            var charCode = (evt.which) ? evt.which : evt.keyCode
            if (charCode == 13)
            {
                applyFilter();
            }
        }

		function addItem($trBefore, item) {							

				var className = jQuery('[itemID]').last().attr('class')
				if (className == 'trline2') className = 'trline1'; else className = 'trline2';
				var resultTableContent = ''

				var selectedStyle = ''

				if(item.InstitutionID == <%= Model.CurrentInstitutionID %>)
					selectedStyle = 'style="background-color:#ffffe0;"'
                    
				resultTableContent += '<tr itemID="' + item.InstitutionID + '" class="' + className + '" ' + selectedStyle + '>'
										+ '<td width="75%" title="' + escapeHtml(item.FullName) + '">'
											+'<a href="javascript:void(0)" onclick="openRequest('+ item.InstitutionID +');">'+ escapeHtml(item.FullName) + '</a></td>'
										+ '<td width="8%" >' + item.RequestNumber + '</td>'
										+ '<td width="17%" >' + item.Date + '</td>'
										+'</tr>'
				
				$trBefore.before(resultTableContent)
		}	
        


        function addDirection(item) {							
        var resultDirections = '';
                resultDirections += '<tr id="' + item.ID + '">'
										+ '<td width="34%">' + (item.Code == null ? '' : (escapeHtml(item.Code) + '.' + escapeHtml(item.QualificationCode))) + '/' + (item.NewCode == null ? '' : escapeHtml(item.NewCode)) +' - ' + escapeHtml(item.Name) + '</td>'
										+ '<td id="'+ item.ID +'td" width="60%" >' + '</td>'
										+ '<td id="'+ item.ID +'tda"width="3%" >' + '<a href="javascript:void(0)" class="btnOk" onClick="doAddDirection('+ item.ID +')" title="Принять"></a>' + '</td>'
                                        + '<td id="'+ item.ID +'tdd"width="3%" >' + '<a href="javascript:void(0)" class="btnDelete" onClick="doDenyDirection(' + item.ID + ')" title=""Отклонить"></a>' + '</td>'
										+ '</tr>';

				$('#directionsTab').before(resultDirections);
				//$('#directionsTab').html(resultDirections);
		}	

        function addComment(item) {							
        var resulttd = '#' + item.DirectionID +'td';
               var action = item.Action;
               if (action == 'Add')
               action = 'На добавление';
               else action = 'На удаление';

               var admission1 = item.admissionType;
               var admission = '';

               if (admission1 == 2)
               admission = 'Бакалавриат';
               if (admission1 == 3)
               admission = 'Бакалавриат (сокращ.)';
               if (admission1 == 4)
               admission = 'Магистратура';
               if (admission1 == 5)
               admission = 'Специалитет';
               if (admission1 == 17)
               admission = 'СПО';
               				//$(resulttd).before(item.Comment);
				$(resulttd).html(action + ' - ' + admission + ' - ' + item.Comment);
		}	


		function fillGrid() {
			jQuery('.gvuzDataGrid tbody tr:not(#firstRow)').remove().detach()			
			
			if(gridItems.length > 0)
				jQuery.each(gridItems, function (index, object) { addItem(jQuery('#firstRow'), object) })
			else
				jQuery('#firstRow').before('<tr><td colspan="5" align="center">Не найдено ни одной заявки</td></tr>')
		}

		function doSort(el, sortID) {			
			var isSortedUp = jQuery(el).hasClass('sortedUp')
			jQuery('.sortUp,.sortDown').remove().detach()
			if (isSortedUp)
				jQuery(el).after('<span class="sortDown"></span>')
			else
				jQuery(el).after('<span class="sortUp"></span>')
			jQuery(el).removeClass('sortedUp')
			if (isSortedUp)
				sortID = -sortID;
			else
				jQuery(el).addClass('sortedUp')
			currentSorting = sortID
			updateData()
		}

		var pageNumber = 0

		function movePager(pageID) {
			pageNumber = pageID
			updateData()
		}

		function prepareModel()
		{
			var model =
			{
				SortID: currentSorting,
				PageNumber: pageNumber
			}
			if (filterModel != null)
				model.Filter = filterModel
			return model
		}

		function updateData()
		{
			doPostAjax('<%= Url.Generate<RequestHandlerController>(x => x.GetRequestList(null)) %>', JSON.stringify(prepareModel()), function (data) {
				if (!addValidationErrorsFromServerResponse(data, false)) {
					gridItems = data.Data.Institutions
					fillGrid()
					setFilterItemCount(data.Data.TotalItemFilteredCount, data.Data.TotalItemCount)
					fillPager(data.Data.TotalPageCount, pageNumber)
				}
			})
		}

		function setFilterItemCount(filteredCount, totalCount)
		{
			var res = totalCount;
			if (filteredCount < totalCount)
				res = filteredCount + ' из ' + res;
			jQuery('#spAppCountField,#spAppCountFieldF').html(res)
		}


		jQuery(document).ready(function () {			
			//jQuery(".datePicker").datepicker({ changeMonth: true, changeYear: true, showOn: "button", buttonImage: '<%= Url.Images("calendar.jpg") %>', buttonImageOnly: true, yearRange: '-40:+0' })
		
			/*var regionNamesS = jQuery.map(regions, function(n,i) {return n.Name})
			autocompleteDropdown(jQuery("#Filter_RegionID"), {
				source: regionNamesS,
				delay : 200
			})
			autocompleteDropdown(jQuery("#Filter_RegionIDF"), {
				source: regionNamesS,
				delay : 200
			})
			autocompleteDropdown(jQuery("#Filter_Owner"), {
				source: ownerDepartments,
				delay : 200
			})
			jQuery('#Filter_ShortNameF').change(function() { jQuery('#Filter_FullName').val(jQuery(this).val()) }) //DO NOT TOUCH!!! IT'S MAGICK!
			jQuery('#Filter_RegionIDF').blur(function() { jQuery('#Filter_RegionID').val(jQuery(this).val()) })
			jQuery('#Filter_InstitutionTypeIDF').change(function() { jQuery('#Filter_InstitutionTypeID').val(jQuery(this).val()) })

			restoreFilter()*/
			updateData()
		})



		function toggleFilter(v)
		{
			//divFilterRegionF
			if (v == 1)
			{
                clearFilter();
				jQuery('#divFilterRegionF').hide()
				jQuery('#divFilterRegion').show()
			}
			else
			{
				clearFilter();
				jQuery('#divFilterRegionF').show()
				jQuery('#divFilterRegion').hide()
			}
		}

		var filterModel = null

        var adType = [];
        var INID = 0;
        function openRequest(instId)
        {
            $("#dialog").dialog('open');
            
            INID = instId;

            var inst =
            {
                institutionid: instId
            }

            doPostAjax('<%= Url.Generate<RequestHandlerController>(x => x.GetRequest(null)) %>', JSON.stringify(inst), function (data)
            {
                if (data.IsError == true)
                    alert(data.Message);
                else
                {
                   $.each(data.Data.Request, function(i,e) 
                   {
                        addDirection(e);
                   }); 

                   $.each(data.Data.Comment, function(i,e) 
                   {
                        addComment(e);
                        adType[i] = 
                        {
                            adItem: e.DirectionID,
                            adValue: e.admissionType
                        }
                   });
                }
            });
        }

    var tmp ;
    var tmpacc ;
    var tmpden ;

    function doAddDirection(item)
    {
        var target;
        $.each(adType, function(i,e){
            if (e.adItem == item) {
                target = e.adValue;
            }
        });
        var request = {
        did: item,
        inid: INID,
        adtype: target
        }
        doPostAjax('<%= Url.Generate<RequestHandlerController>(x => x.AddDirection(null,null,null)) %>', JSON.stringify(request), function (data)
                            {
                                if (data.IsError)
                                     alert(data.Message);
                                else
                                {
                                    tmp = '#' + item + 'td';
                                    tmpacc = '#' + item + 'tda';
                                    tmpden = '#' + item + 'tdd';
                                    $(tmp).html('Заявка закрыта.');
                                    $(tmpacc).html('');
                                    $(tmpden).html('');
                                }
                            });
    }

    var aComment = '';
    var itemid = 0;
    function doDenyDirection(item)
    {
        itemid = item;
        tmp = '#' + item + 'td';
        tmpacc = '#' + item + 'tda';
        tmpden = '#' + item + 'tdd';
        $('#deny').dialog('open');
    }



        window.onload = function()
        {
                doSort(this, 1);

                $("#dialog").dialog({
                beforeclose: function() {
                    var inst =
                    {
                        institutionid: INID
                    }
                    $.ajax({
                      async: false,
                	  type: 'POST',
                	  url: '<%= Url.Generate<RequestHandlerController>(x => x.MakeEditable(null)) %>',
                	  data: 'institutionid='+INID,
                	  success: function(data){
                        if (data.IsError == true)
                        alert(data.Message);
                	  }
                	});
                    /*doPostAjax('<%= Url.Generate<RequestHandlerController>(x => x.MakeEditable(null)) %>', JSON.stringify(inst), function (data) {
                    if (data.IsError == true)
                        alert(data.Message);
                    });*/
                },
                autoOpen: false,
                width: 1000,
                modal: true
            });
             $('#dialog').bind('dialogclose', function(event) {

                //updateData();
                window.location.reload(1);
             });

                $("#deny").dialog({
                autoOpen: false,
                width: 1000,
                modal: true,
                buttons:
                    {
                        "Отказать": function () {
                        aComment = $('textarea#comment').val();
                        var request = 
                        {
                            did: itemid,
                            Comment: aComment,
                            inid: INID
                        }
                        doPostAjax('<%= Url.Generate<RequestHandlerController>(x => x.DenyRequest(null,null,null)) %>', JSON.stringify(request), function (data)
                            {
                                if (data.IsError) alert(data.Message);
                                else
                                {
                                    $(tmp).html('Заявка закрыта.');
                                    $(tmpacc).html('');
                                    $(tmpden).html('');
                                    $('#deny').dialog('close');
                                }
                            });
                         
                         }
                    }
            });
             
           
        }
	</script>

</asp:Content>

