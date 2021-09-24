<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Portlets" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Controllers.Admission" %>
<%@ Register TagPrefix="gv" TagName="TabControl" Src="~/Views/Shared/Common/CommonTabControl.ascx" %>

<gv:TabControl ID="TabControl1" runat="server" />

<script type="text/javascript">


  var menuItems = [
		{ name: '����� ����������', link: '<%= Url.Generate<InstitutionController>(c => c.View(null)) %>', enable: true },
		{ name: '��������� ��', link: '<%= Url.Generate<StructureController>(c => c.Index()) %>', enable: true, noWrap: true },

<%
    if (HttpContext.Current.Request.RequestContext.HttpContext.InsidePortlet())
  {%>
		<%--{ name: '���������������� �����', link: '<%= Url.Generate<PreparatoryCourseController>(c => c.Index()) %>', enable: true },--%>
<%} 
  else
  {
%>

<%--
	    { name: '��������� ������', link: '<%= Url.Generate<AdmissionController>(c => c.Index()) %>', enable: true,
			noWrap: true },
		{ name: '������������� ���������', link: '<%= Url.Generate<EntranceTestController>(c => c.Index()) %>', enable: true },
--%>
<%-- FIS-1313
      
		{ name: '���������������� �����', link: '<%= Url.Generate<PreparatoryCourseController>(c => c.Index()) %>', enable: true },
--%>

<%--
		{ name: '���������', link: '<%= Url.Generate<BenefitController>(c => c.Index()) %>', enable: true,
			noWrap: true },
--%>
		{ name: '�����&nbsp;�&nbsp;��������� ������', link: '<%= Url.Generate<AdmissionController>(c => c.VolumeView(null, null)) %>', enable: true,
			noWrap: false },
      { name: '��������������&nbsp;����������', link: '<%= Url.Generate<InstitutionAchievementsController>(c => c.Index()) %>', enable: true,
			noWrap: false },
		{ name: '��������', link: '<%= Url.Generate<CompetitiveGroupController>(c => c.CompetitiveGroupList()) %>', enable: true,
			noWrap: false },
		{ name: '������� �����������', link: '<%= Url.Generate<TargetOrganizationController>(c => c.Index()) %>', enable: true,
			noWrap: false },
        { name: '��������������� ���������', link: '<%= Url.Generate<InstitutionProgramController>(c => c.Index()) %>', enable: true,
			noWrap: false }	
			<%--,
		{ name: '����� �������� ��������', link: '<%= Url.Generate<InstitutionController>(c => c.View(null)) %>', enable: false }--%>
<%}%>
	]		

    

</script>