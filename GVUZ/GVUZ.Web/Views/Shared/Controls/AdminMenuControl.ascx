<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Security" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<script type="text/javascript">
		jQuery(function ()
		{
		    //<%-- админское меню. Используется сложная индексация, чтобы проставить правильный выделенный пункт на конкретной странице (она проставляет выделение) --%>
			var menuItems = [
			<% var separator = "";
			   var menuDict = new Dictionary<int, int>();
			   var idx = 0; 
               var user_only = 0;
               var roles = Roles.GetRolesForUser();
               if ((roles.Length == 1) && (roles[0] == "fbd_^user"))
                    user_only = 1;
            %>                        

            <% if(user_only == 1) { %>
             {name: 'Управление приемной кампанией', link: '<%= Url.Generate<CampaignController>(c => c.CampaignList()) %>'}<% menuDict[0] = 2; %>
            <% } else { %>

			<% if(UserRole.CurrentUserInRole(UserRole.FbdAuthorizedStaff)) { %><%= separator %>{ name: 'Учетные записи пользователей', link: '<%= Url.Generate<AdministrationController>(c => c.EduList()) %>' }<% separator=",";idx++;} menuDict[0] = idx; %>			
            <% if(UserRole.CurrentUserInRole(UserRole.FbdAuthorizedStaff)) { %><%= separator %>{ name: 'Управление приемной кампанией', link: '<%= Url.Generate<CampaignController>(c => c.CampaignList()) %>'}<%separator=",";idx++;} menuDict[1] = idx; %>
			<% if(UserRole.CurrentUserInRole(UserRole.FBDAdmin + "," + UserRole.FbdRonUser)) { %><%= separator %>{ name: 'Список ОО', link: '<%= Url.Generate<InstitutionAdminController>(c => c.List()) %>'}<%separator=",";idx++;} menuDict[2] = idx; %>
			<% if(UserRole.CurrentUserInRole(UserRole.FBDAdmin + "," + UserRole.RonAdmin)) { %><%= separator %>{ name: 'Справочники системы', link: '<%= Url.Generate<AdministrationController>(c => c.CatalogsList()) %>' } <%separator=",";idx++;} menuDict[3] = idx; %>
			<% if(UserRole.CurrentUserInRole(UserRole.FbdAuthorizedStaff + "," + UserRole.FBDAdmin)) { %><%= separator %>{ name: 'Очередь запросов', link: '<%= Url.Generate<ImportController>(c => c.ImportPackageList()) %>'}<%separator=",";idx++;} menuDict[4] = idx; %>
			<% if(UserRole.CurrentUserInRole(UserRole.FBDAdmin)) { %><%= separator %>{ name: 'Журнал доступа к ПД', link: '<%= Url.Generate<AccessLogController>(c => c.AccessLogList()) %>'}<%separator=",";idx++;} menuDict[5] = idx; %>
			<% if(UserRole.CurrentUserInRole(UserRole.FBDAdmin)) { %><%= separator %>{ name: 'Заявки', link: '<%= Url.Generate<RequestHandlerController>(c => c.ReqList()) %>'}<%separator=",";idx++;} menuDict[6] = idx; %>
			<%--<% if(UserRole.CurrentUserInRole(UserRole.FbdAuthorizedStaff + "," + UserRole.FBDAdmin)) { %><%= separator %>{ name: 'Выгрузка для ГЗГУ', link: '<%= Url.Generate<ApplicationExportController>(c => c.Index()) %>'}<%separator=",";idx++;} menuDict[7] = idx; %>--%>
			<% if(UserRole.CurrentUserInRole(UserRole.FbdAuthorizedStaff + "," + UserRole.FBDAdmin)) { %><%= separator %>{ name: 'Выгрузка сведений о сочинениях', link: '<%= Url.Generate<CompositionResultsExportController>(c => c.Index()) %>'}<%separator=",";idx++;} menuDict[8] = idx; %>
			<% if(ConfigHelper.ShowOlympicFilesUpload() && UserRole.CurrentUserInRole(UserRole.FbdAuthorizedStaff + "," + UserRole.FBDAdmin)) { %><%= separator %>{ name: 'Представление сведений по ОШ', link: '<%= Url.Generate<OlympicFilesController>(c => c.Index()) %>'}<%separator=",";idx++;} menuDict[9] = idx; %>
            <% } %>
			];
                        
			menuItems[<%= menuDict.ContainsKey((int) ViewData["MenuItemID"]) ? menuDict[(int) ViewData["MenuItemID"]] - 1 : 0 %>].selected = true;            
			new TabControl(jQuery('#gvuzMenu'), menuItems, { tabWidth: 140 }).init();
		});
</script>

<div id="gvuzMenu" class="submenu"></div>