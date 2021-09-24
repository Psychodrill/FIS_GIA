<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<%@ Import Namespace="FogSoft.Helpers" %>

<%@ Import Namespace="FogSoft.WSRP" %>

<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Portlets" %>
<%@ Import Namespace="Microsoft.Practices.ServiceLocation" %>

<%-- �� ������ ���������� ���� - �������� ���. --%>
<div id="gvuzMenu" class="gvuzMenu">
<%--
  ���� ������ ����� ����� ���
	<input id="item1" type="radio" name="menu" checked="checked" /><label for="item1">����� ����������</label>&nbsp;
	<input id="item2" type="radio" name="menu" /><label for="item2">��������� ����</label>

	selected = true - � menuItems
--%>
</div>

<script type="text/javascript">

<%if (HttpContext.Current.Request.RequestContext.HttpContext.InsidePortlet())
  {%>		
  var menuItems = [
		{ name: '����� ����������', link: '<%= PortletLinkHelper.InstitutionTabLink(PortletType.InstitutionInfoTab, 0) %>', enable: true },
		{ name: '��������� ��', link: '<%= PortletLinkHelper.InstitutionTabLink(PortletType.InstitutionStructureTab, 1) %>', enable: true, noWrap: true },
		{ name: '�������� � ������', link: '<%= PortletLinkHelper.InstitutionTabLink(PortletType.InstitutionReceiveTab, 2) %>', enable: true	},
		{ name: '������ ��� �����������', link: '<%= PortletLinkHelper.InstitutionTabLink(PortletType.InstitutionFacilityTab, 3) %>', enable: true }

        <%--,{ name: '���������������� �����', link: '<%= PortletLinkHelper.InstitutionTabLink(PortletType.InstitutionCourcesTab, 4) %>', enable: true }--%>
	]
<%}
  else
  {
      %>
    var menuItems = [
<%--
		{ name: 'Ajax1', link: '<%= Url.Generate<TreeController>(c => c.SendPage1()) %>',	enable: true, ajax: true },
		{ name: 'Ajax2', link: '<%= Url.Generate<TreeController>(c => c.SendPage2()) %>',	enable: true, ajax: true },
--%>
		{ name: '�����<br />����������', link: '<%= Url.Generate<InstitutionController>(c => c.View(null)) %>', enable: true },
		{ name: '��������� ��', link: '<%= Url.Generate<StructureController>(c => c.Index()) %>', enable: true,
			noWrap: true },
		<%--{ name: '����������������<br />�����', link: '<%= Url.Generate<PreparatoryCourseController>(c => c.Index()) %>', enable: true }--%>
<%--,
		{ name: '����� �������� ��������', link: '<%= Url.Generate<InstitutionController>(c => c.View(null)) %>', enable: false }--%>

	]
      <% 
}%>

		jQuery(document).ready(function ()
		{

			<%if (Url.IsInsidePortlet())
			{
				string currentTab = "0";
				ISession session = ServiceLocator.Current.GetInstance<ISession>();
				if (session != null)
					currentTab = session.GetValue("currentTab", "0");
				%>
				menuItems[<%= currentTab%>].selected = true;
				<%
			}%>

			new TabControl(jQuery('#gvuzMenu'), menuItems, null, jQuery('#treeControl')).init();
		});

</script>