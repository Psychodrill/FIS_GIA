<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Model.Entrants" %>
<%@ Import Namespace="GVUZ.Web.ContextExtensions" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Models" %>
<script runat="server">
  public ApplicationStepType ApplicationStep { get; set; }
  private int StepInt { get { return (int)ApplicationStep; } }
  public bool IsTop { get; set; }
  public static readonly string[] TitleStrings=new[] { "","","Ввод личных данных", /*"Место жительства", "Сведения о родителях",*/ "Вступительные испытания", /*"Иностранные языки",*/ "Прилагаемые документы","Индивидуальные достижения", /*"Дополнительные сведения",*/ "Общие сведения" };

  private Application _app;

  private Application GetApplication() {
    if(_app==null) {
      int appID=(int)(ViewData["ApplicationID"]??0);
      if(appID>0)
        _app=ApplicationExtensions.GetApplication(appID);
    }
    return _app;
  }

  public string GetApplicationNumber() {
    return GetApplication()==null?"":_app.ApplicationNumber;
  }

  public int GetApplicationStatus() {
    return GetApplication()==null?ApplicationStatusType.Draft:_app.StatusID;
  }

  public bool IsEditApp {
    get { return GetApplicationStatus()!=ApplicationStatusType.Draft; }
  }

  public bool CanEditApp {
    get { return GetApplicationStatus()!=ApplicationStatusType.InOrder; }
  }

  public string ApplicationIDUrlPart {
    get {
      int appID=(int)(ViewData["ApplicationID"]??0);
      if(!Url.IsInsidePortlet())
        return "?applicationID="+appID;
      return "";
    }
  }

  public string GetTitle(int id) {
    //if (id == TitleStrings.Length - 1)
    //	return TitleStrings[id] + ViewData["Institution"];
    return TitleStrings[id];
  }

  public string GetNextButtonText(int id) {
    if(id==TitleStrings.Length-1)
      return "Сохранить";
    return "Далее";
  }

  public string UrlList {
    get {
      if(Url.IsInsidePortlet())
        return Url.Generate<EntrantController>(x => x.ApplicationList());
      else
        return Url.Generate<InstitutionApplicationController>(x => x.ApplicationList())+"?back=1";
    }
  }

  public string UrlLast {
    get {
      if(Url.IsInsidePortlet())
        return Url.Generate<EntrantController>(x => x.ApplicationList());
      else
        return Url.Generate<InstitutionApplicationController>(x => x.ApplicationList())+"#tab"+(Session["ApplicationReturnTab"]??"4");
    }
  }

  public string UrlGoBack {
    get {
      if(Url.IsInsidePortlet())
        return Url.Generate<EntrantController>(x => x.ApplicationGoBack());
      else {
        var tabID=(Session["ApplicationReturnTab"]??"1").ToString();
        if(tabID=="10")
          return Url.Generate<InstitutionApplicationController>(x => x.ExtendedApplicationList(null))+"?back=1";
        else if(tabID=="11")
          return Url.Generate<EntrantController>(x => x.EntrantsList())+"?back=1";
        else if(tabID.Length>0&&tabID.StartsWith("e"))
          return Url.Generate<EntrantController>(x => x.EntrantInfo(Convert.ToInt32(tabID.Remove(0,1))))+"&back=1";
        else
          return Url.Generate<InstitutionApplicationController>(x => x.ApplicationList())+"?back=1"+"#tab"+tabID;
      }
    }
  }

  public string GetSendText() {
    if(Url.IsInsidePortlet())
      return "Отправить";

    return "Сохранить";
  }
</script>
<% if(IsTop&&IsEditApp) { %>
<div class="divstatement">
  <div id="appEditTabPart" class="submenu">
  </div>
  <% } %>
  <% if(!IsEditApp) { %>
  <div class="navigation">
    <a id="btnAppBack<%= IsTop ? "Top" : "" %>" href="#" class="back">Назад</a> <a id="btnAppForward<%= IsTop ? "Top" : "" %>"
      href="#" class="forvard">
      <%= GetNextButtonText(StepInt) %></a> <a id="btnAppCancel<%= IsTop ? "Top" : "" %>"
        href="#" class="cancel">Отмена</a> <a id="btnAppSave<%= IsTop ? "Top" : "" %>" href="#"
          class="stop">Приостановить добавление заявления</a>
  </div>
  <% } %>
  <% if(IsEditApp&&CanEditApp) { %>
  <div class="navigation">
    <input type="button" id="btnAppSaveE<%= IsTop ? "Top" : "" %>" value="Сохранить"
      class="button3" />
    <input type="button" id="btnAppCancelE<%= IsTop ? "Top" : "" %>" value="Отмена" class="button3" />
  </div>
  <% } %>
  <% if(!IsTop&&IsEditApp) { %>
</div>
<% } %>
<% if(!IsTop) { %>
<script language="javascript" type="text/javascript">
	var navigateUrls = [
	 '<%= UrlGoBack %>',
	 '<%= UrlList %>',
	 '<%= Url.Generate<ApplicationController>(x => x.ApplicationPersonalData(null)) + ApplicationIDUrlPart %>',
	 <%--'<%= Url.Generate<ApplicationController>(x => x.ApplicationAddress()) + ApplicationIDUrlPart %>',
	 '<%= Url.Generate<ApplicationController>(x => x.ApplicationParentData()) + ApplicationIDUrlPart %>',--%>
	 '<%= Url.Generate<ApplicationController>(x => x.ApplicationEntranceTest()) + ApplicationIDUrlPart %>',
	 <%--'<%= Url.Generate<ApplicationController>(x => x.ApplicationLanguages()) + ApplicationIDUrlPart %>',--%>
	 '<%= Url.Generate<ApplicationController>(x => x.ApplicationDocuments()) + ApplicationIDUrlPart %>',
     '<%= Url.Generate<ApplicationController>(x => x.ApplicationIndividualAchivements()) + ApplicationIDUrlPart %>',
<%--	 '<%= Url.Generate<ApplicationController>(x => x.ApplicationAdditionalInfo())+ ApplicationIDUrlPart %>',--%>
	 '<%= Url.Generate<ApplicationController>(x => x.ApplicationSending()) + ApplicationIDUrlPart %>',
	 '<%= UrlLast %>',
	 '<%= Url.Generate<InstitutionApplicationDraftController>(c => c.PrepareNewApplication()) %>']
	var navDirection = 0
	jQuery(document).ready(function ()
		{
			<% if(GetApplicationStatus() == ApplicationStatusType.Draft) { %>
			jQuery('#pageTitleH2').text('Ввод заявления.');
			jQuery('#pageSubtitleH2').text('Шаг <%= StepInt %>: <%= GetTitle(StepInt)%>');
			<%} else { %>
			jQuery('#pageTitleH2').text('Редактирование заявления: ');
			jQuery('#pageSubtitleH2').text('№: <%= GetApplicationNumber().Replace("\\", "\\u005C") %>');
			generateTabControl()
			<%} %>
			jQuery('#btnAppBack,#btnAppBackTop').attr('href', navigateUrls[<%= StepInt - 1 %>])
			jQuery('#btnAppSave,#btnAppSaveTop').attr('href', navigateUrls[0])
			jQuery('#btnAppCancel,#btnAppCancelTop').attr('href', navigateUrls[0])
			jQuery('#btnAppBack,#btnAppBackTop').click(function() { navDirection = <%= StepInt - 1 %>;$outerClickedElement = null; return doSubmit('back') })
			jQuery('#btnAppForward,#btnAppForwardTop').click(function() { navDirection = <%= StepInt + 1 %>;$outerClickedElement = null; return doSubmit('next') })
			jQuery('#btnAppSendAndNew,#btnAppSendAndNewTop').click(function() { navDirection = navigateUrls.length - 1;$outerClickedElement = null; return doSubmit('next') })
			jQuery('#btnAppSave,#btnAppSaveTop').click(function() { navDirection = <%= 0 %>;$outerClickedElement = null;return doSubmit('save') })
			jQuery('#btnAppCancel,#btnAppCancelTop').click(function() { navDirection = <%= 0 %>; $outerClickedElement = null;doAppNavigate();return false; })

			jQuery('#btnAppSaveE,#btnAppSaveETop').click(function() { navDirection = <%= 0 %>;$outerClickedElement = null;return doSubmit('save') })
			jQuery('#btnAppCancelE,#btnAppCancelETop').click(function() { navDirection = <%= 0 %>;$outerClickedElement = null; confirmCancelClick() })
	        $('#DocumentTypeID').change(function() {
            switch ($(this).val()) {
             case '3':
                 $("#DocumentNumber").attr('maxlength','50');
                 $("#DocumentSeries").attr('maxlength','20');
                 break;
             case '9':
                 $("#DocumentNumber").attr('maxlength','50');
                 $("#DocumentSeries").attr('maxlength','20');
                 break;
             case '1':
                 $("#DocumentNumber").attr('maxlength','6');
                 $("#DocumentNumber").val($("#DocumentNumber").val().length > 6 ? $("#DocumentNumber").val().substr(0, 6) : $("#DocumentNumber").val());
                 $("#DocumentSeries").attr('maxlength','4');
                 $("#DocumentSeries").val($("#DocumentSeries").val().length > 4 ? $("#DocumentSeries").val().substr(0, 4) : $("#DocumentSeries").val());
                 break;
             default:
                 $("#DocumentNumber").attr('maxlength','10');
                 $("#DocumentNumber").val($("#DocumentNumber").val().length > 10 ? $("#DocumentNumber").val().substr(0, 10) : $("#DocumentNumber").val());
                 $("#DocumentSeries").attr('maxlength','6');
                 $("#DocumentSeries").val($("#DocumentSeries").val().length > 6 ? $("#DocumentSeries").val().substr(0, 6) : $("#DocumentSeries").val());
                 break;
             }
             });
	        $('#DocumentTypeID').change();
		})

	var $outerClickedElement = null
	function doAppNavigate()
	{
		if($outerClickedElement != null)
		{
			//$outerClickedElement.unbind('click', doClickOuterElement)
			//$outerClickedElement[0].click()
			window.location.href = $outerClickedElement.attr('href')
			return
		}
		window.location.href = navigateUrls[navDirection]
	}
	
	function doClickOuterElement()
	{
		var $el = jQuery(this)
		jQuery('<div>Вы хотите сохранить заявление перед уходом с данной страницы?</div>').dialog({
			width: '400px',
			modal: true,
			buttons: {
				"Сохранить": function() {
					$outerClickedElement = $el
					doSubmit('save')
					closeDialog(jQuery(this))
				 },
				"Не сохранять": function() {
					$outerClickedElement = $el
					doAppNavigate()
					closeDialog(jQuery(this))
				},
				"Отмена": function() { closeDialog(jQuery(this)) }
			}
		})
		/*if(confirm('Вы хотите сохранить заявление перед уходом с данной страницы?'))
		{
			$outerClickedElement = jQuery(this)
			doSubmit('save')
			return false
		}
		return true*/
		return false
	}

	function generateTabControl()
	{
		var tabControl = new TabControl(jQuery('#appEditTabPart'), [
		<% for(int i = 2; i < TitleStrings.Length - 1; i++) { %>
			{ name: '<%= GetTitle(i) %>', link: navigateUrls[<%= i %>], enable: true, selected: <%= i == StepInt ? "true" : "false" %>, noWrap: true },
		<%} %>
			{ name: '<%= GetTitle(TitleStrings.Length - 1) %>', link: navigateUrls[<%= TitleStrings.Length - 1  %>], enable: true, selected: <%= TitleStrings.Length - 1 == StepInt ? "true" : "false" %>, noWrap: true }
			])
		tabControl.init();
	}

	function confirmCancelClick()
	{
		confirmDialog('Вы действительно хотите отменить редактирование?', doAppNavigate)
	}

	jQuery(document).ready(function() {
		jQuery('.pageHeader a').click(doClickOuterElement)
	})
</script>
<%} %>
