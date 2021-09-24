<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.PreparatoryCourseViewModel>" %>

<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/InstitutionsTabControl.ascx" %>
<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
	Подготовительные курсы
</asp:Content>
<asp:Content ID="header" ContentPlaceHolderID="PageTitle" runat="server">Сведения об образовательной организации</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="divstatement">
		<gv:TabControl runat="server" ID="tabControl" />
		<div id="treeControlDialog">
		</div>
		<div id="content">
			<table class="gvuzDataGrid" cellpadding="3">
				<thead>
					<tr>
						<th>
							<%= Html.LabelFor(x => x.CourseName) %>
						</th>
						<th>
							<%= Html.LabelFor(x => x.Subjects) %>
						</th>
						<th>
							<%= Html.LabelFor(x => x.Information) %>
						</th>
						<th>
							<%= Html.LabelFor(x => x.AdditionalInfo) %>
						</th>
						<th style="width: 40px">
						</th>
					</tr>
					<tr id="trAddNew">
						<td colspan="5">
							<a href="#" id="btnAddNew" class="add">Добавить подготовительный курс</a>
						</td>
					</tr>
				</thead>
				<tbody>
				</tbody>
			</table>
		</div>
	</div>
	<script type="text/javascript">

		menuItems[2].selected = true;

		var existingItems = JSON.parse('<%= Html.Serialize(Model.Courses) %>')
		var UserReadonly = <%= GVUZ.Helper.UrlUtils.IsReadOnly(FBDUserSubroles.InstitutionDataDirection) ? "true" : "false" %>
		var createdItem
		var addNewCourse = function ($trBefore, item)
		{
			var className = $trBefore.prev().attr('class')
			if(className == 'trline1') className = 'trline2'; else className = 'trline1';
		
			$trBefore.before('<tr itemID="' + item.CourseID + '" class="' + className + '"><td>'
					+ (UserReadonly ? escapeHtml(item.Name) : '<span class="btnEdit linkSumulator">' + escapeHtml(item.Name) + '</a>')
					+ '</td><td>' + escapeHtml(item.Subjects) + '</td><td>'
					+ (item.Information == null ? '' : (item.Information)) + '</td><td>' + 
					(item.FileID != null ? '<a href="<%= Url.Generate<PreparatoryCourseController>(x => x.GetFile1(null)) %>?fileID=' + item.FileID + '">' + item.FileName + '</a>' : '')
					+ '</td>'
					+ '<td align="center">'
					+ (UserReadonly ? '' : '<a href="#" class="btnEdit">&nbsp;</a>&nbsp;<a href="#" class="btnDelete">&nbsp;</a>')
					+ '</td></tr>')
		}

		var doAddEdit = function (navUrl, postData, callback)
		{
			createdItem = null
			doPostAjax(navUrl, postData, function (data)
			{
				jQuery('#treeControlDialog').html(data);
				jQuery('#treeControlDialog').dialog({
					modal: true,
					width: 800,
					title: (postData == '' ? "Добавление" : "Редактирование") + " подготовительного курса",
					buttons:
							{
								"Сохранить": function () { jQuery('#btnSubmit').click(); },
								"Отмена": function () { jQuery('#btnCancel').click(); }
							},
					close: function ()
					{
						if (createdItem) callback()
					}
				}).dialog('open');
			}, "application/x-www-form-urlencoded", "html")

		}

		jQuery(document).ready(function ()
		{
			if(UserReadonly) jQuery('#trAddNew').hide()
			jQuery('#btnAddNew').click(function ()
			{
				doAddEdit('<%= Url.Generate<PreparatoryCourseController>(x => x.AddPreparatoryCourse()) %>', '',
				function () { addNewCourse(jQuery('#trAddNew'), createdItem) })
				return false
			})

			jQuery('.btnEdit').live('click', function ()
			{
				var $tr = jQuery(this).parents('tr:first')
				var itemID = $tr.attr('itemID')
				doAddEdit('<%= Url.Generate<PreparatoryCourseController>(x => x.EditPreparatoryCourse(null)) %>', 'courseID=' + itemID,
				function () { addNewCourse($tr, createdItem); $tr.remove().detach() })
				return false
			})


			jQuery('.btnDelete').live('click', function ()
			{
				var $tr = jQuery(this).parents('tr:first');
				var itemID = $tr.attr('itemID')
				confirmDialog('Вы действительно хотите удалить подготовительный курс?', function ()
				{
					doPostAjax('<%= Url.Generate<PreparatoryCourseController>(x => x.DeletePreparatoryCourse(null)) %>', 'preparatoryCourseID=' + itemID, function (data)
					{
						if (data.IsError) jQuery('<div>' + data.Message + '</div>').dialog(
						{ buttons: { OK: function () { jQuery(this).dialog("close"); } } })
						else
							$tr.remove().detach()
					}, "application/x-www-form-urlencoded")
				});
				return false
			})

			for (var i = 0; i < existingItems.length; i++)
			{
				addNewCourse(jQuery('#trAddNew'), existingItems[i])
			}
		})

	</script>
</asp:Content>
