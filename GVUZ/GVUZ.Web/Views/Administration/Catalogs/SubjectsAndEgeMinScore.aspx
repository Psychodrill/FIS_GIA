<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.Administration.Catalogs.SubjectsAndEgeMinScoreViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    Администрирование - Справочники системы
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PageTitle" runat="server">
	Минимальное количество баллов по результатам ЕГЭ
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div class="divstatement">
		
		<div id="dialog">
		</div>

		<div id="content">		
			<div class="navigation">
				<span class="back">
					<%=Url.GenerateLink<AdministrationController>(c=>c.CatalogsList(),"Назад") %>
				</span>
			</div>
			
			<input class="button3 editButton" type="button" value="Редактировать"/>
			<input class="button3 saveButton" type="button" value="Сохранить" style="display:none;"/>
			<input class="button3 cancelButton" type="button" value="Отмена" style="display:none;"/>
			<div id="errorPlaceTop"></div>
			<input type="hidden" id="Error" />
			<table class="gvuzDataGrid">
				<thead>
					<tr>
						<th>
							<span class="linkSumulator" onclick="doSort(this, 2)"><%= Html.LabelFor(x => x.ScoreDescr.SubjectName) %></span>
						</th>
						<th style="width:50px;">
							<span class="linkSumulator" onclick="doSort(this, 3)"><%= Html.LabelFor(x => x.ScoreDescr.MinScore) %></span>
						</th>						
					</tr>					
				</thead>
				<tbody>
					<tr id="firstRow" style="display:none">									
					</tr>
				</tbody>
			</table>

			<div id="errorPlaceBottom"></div>
			<input class="button3 editButton" type="button" value="Редактировать"/>
			<input class="button3 saveButton" type="button" value="Сохранить" style="display:none;"/>
			<input class="button3 cancelButton" type="button" value="Отмена" style="display:none;"/>

			<div class="navigation">
				<span class="back">
					<%=Url.GenerateLink<AdministrationController>(c=>c.CatalogsList(),"Назад") %>
				</span>
			</div>
		</div>
	</div>

	<script language="javascript" type="text/javascript">

		var gridItems = JSON.parse('<%= Html.Serialize(Model.SubjectsAndScores) %>')
		var currentSorting = null

		var switcher = new ModeSwitcher({
			editBtnSelector: '.editButton',
			saveBtnSelector: '.saveButton',
			cancelBtnSelector: '.cancelButton',
			targetSelector: '.editable'
		})

		function fillModel() {
			var model = {
				SubjectsAndScores : []
			}

			jQuery('.gvuzDataGrid tbody tr:not(#firstRow)').each(function () {				
				model.SubjectsAndScores.push(
				{
					SubjectID: jQuery(this).attr('itemID'),
					MinValue: jQuery(this).find(switcher.getEditContainerSelector()).val()
				})
			})

			return model
		}

		function addItem($trBefore, item) {			

			var className = $trBefore.prev().attr('class')

			if (className == 'trline2') className = 'trline1'; else className = 'trline2';

			$trBefore.before('<tr itemID="' + item.SubjectID + '" class="' + className + '">'
								+ '<td align="left">' + item.SubjectName + '</td>'
								+ '<td align="left" class="editable">' + item.MinScore + '</td></tr>'								
							)
		}

		function fillGrid() {
			jQuery('.gvuzDataGrid tbody tr:not(#firstRow)').remove().detach()

			jQuery.each(gridItems, function (index, object) {
				addItem(jQuery('#firstRow'), object)
			})
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

		function updateData() {

			var model =
			{
				SortID: currentSorting,
				PageNumber: pageNumber
			}

			doPostAjax('<%= Url.Generate<AdministrationController>(x => x.GetSubjectsAndEgeMinScore(null)) %>', JSON.stringify(model), function (data) {
				if (!addValidationErrorsFromServerResponse(data, false)) {
					gridItems = data.Data.SubjectsAndScores
					fillGrid()
					fillPager(data.Data.TotalPageCount, pageNumber)
				}
			})
		}

		jQuery(document).ready(function () {

			jQuery('.editButton').click(function () {
				switcher.setEditMode()
			})

			jQuery('.saveButton').click(function () {
				clearErrorNotifications()
				if (isValidValues()) {					
					var model = fillModel()

					doPostAjax('<%= Url.Generate<AdministrationController>(x => x.UpdateSubjectsMinEgeScores(null)) %>', JSON.stringify(model), function (data) {
						if (!addValidationErrorsFromServerResponse(data, false)) {
							switcher.setReadOnlyMode(false)
						}
					})
				}
			})

			jQuery('.cancelButton').click(function () {
				clearErrorNotifications()
				switcher.setReadOnlyMode(true)
			})

			updateData()
		})

		function clearErrorNotifications() {
			clearValidationErrors(jQuery('.gvuzDataGrid'))
			jQuery('#errorPlaceTop,#errorPlaceBottom').empty()			
		}

		function isValidValues() {			
			var errorMessage = '<span class="field-validation-error">Введено неверное значение. Ошибочные поля выделены красным.</span>'
			var isValid = true
			
			jQuery(switcher.getEditContainerSelector()).each(function () {								
				var value = jQuery(this).val()				
				if (value=='' || isNaN(value) || value < 0 || value > 100) {
					addValidationError(jQuery(this))
					jQuery('#errorPlaceTop,#errorPlaceBottom')
						.html(errorMessage)
					isValid = false
				}
			})

			return isValid
		}

	</script>

</asp:Content>
