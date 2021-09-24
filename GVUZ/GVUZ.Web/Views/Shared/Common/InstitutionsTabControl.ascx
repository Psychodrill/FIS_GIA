<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Portlets" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Controllers.Admission" %>
<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/CommonTabControl.ascx" %>

<gv:TabControl ID="TabControl1" runat="server" />

<script type="text/javascript">


  var menuItems = [
		{ name: 'Общая информация', link: '<%= Url.Generate<InstitutionController>(c => c.View(null)) %>', enable: true },
		{ name: 'Структура ОО', link: '<%= Url.Generate<StructureController>(c => c.Index()) %>', enable: true, noWrap: true },

<%
    if (HttpContext.Current.Request.RequestContext.HttpContext.InsidePortlet())
  {%>
		<%--{ name: 'Подготовительные курсы', link: '<%= Url.Generate<PreparatoryCourseController>(c => c.Index()) %>', enable: true },--%>
<%} 
  else
  {
%>

<%--
	    { name: 'Структура приема', link: '<%= Url.Generate<AdmissionController>(c => c.Index()) %>', enable: true,
			noWrap: true },
		{ name: 'Вступительные испытания', link: '<%= Url.Generate<EntranceTestController>(c => c.Index()) %>', enable: true },
--%>
<%-- FIS-1313
      
		{ name: 'Подготовительные курсы', link: '<%= Url.Generate<PreparatoryCourseController>(c => c.Index()) %>', enable: true },
--%>

<%--
		{ name: 'Олимпиады', link: '<%= Url.Generate<BenefitController>(c => c.Index()) %>', enable: true,
			noWrap: true },
--%>
		{ name: 'Объем&nbsp;и&nbsp;структура приема', link: '<%= Url.Generate<AdmissionController>(c => c.VolumeView(null, null)) %>', enable: true,
			noWrap: false },
      { name: 'Индивидуальные&nbsp;достижения', link: '<%= Url.Generate<InstitutionAchievementsController>(c => c.Index()) %>', enable: true,
			noWrap: false },
		{ name: 'Конкурсы', link: '<%= Url.Generate<CompetitiveGroupController>(c => c.CompetitiveGroupList()) %>', enable: true,
			noWrap: false },
		{ name: 'Целевые организации', link: '<%= Url.Generate<TargetOrganizationController>(c => c.Index()) %>', enable: true,
			noWrap: false },
        { name: 'Образовательные программы', link: '<%= Url.Generate<InstitutionProgramController>(c => c.Index()) %>', enable: true,
			noWrap: false }	
			<%--,
		{ name: 'Архив приемных кампаний', link: '<%= Url.Generate<InstitutionController>(c => c.View(null)) %>', enable: false }--%>
<%}%>
	]		

    

</script>