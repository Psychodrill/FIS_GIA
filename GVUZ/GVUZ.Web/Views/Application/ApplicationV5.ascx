<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<int>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<div id="content">	
    <table class="gvuzDataGrid navigation" style="width: 400px">
        <thead>
            <tr><th class="caption" align="center">Шаблоны печатных форм</th></tr>
        </thead>
        <tbody>
            <tr class="trline2">
                <td class="caption">
                    <span	class="btnEdit linkSumulator"
                         	onclick=" window.open('<%= Url.Generate<ApplicationController>(c => c.EducationDocumentCopyReplacement(Model)) %>',
    '', 'width=800,height=600,toolbar=0,scrollbars=1'); " >
                        Замена копии документа об образовании на подлинник
                    </span>					
                </td>					
            </tr>				
            <tr class="trline1">
                <td class="caption">
                    <span	class="btnEdit linkSumulator"
                         	onclick=" window.open('<%= Url.Generate<ApplicationController>(c => c.DocumentsIssuanceReceipt(Model)) %>',
        '', 'width=800,height=600,toolbar=0,scrollbars=1'); " >
                        Расписка в выдаче документов
                    </span>
                </td>					
            </tr>				
            <tr class="trline2">
                <td class="caption">
                    <span	class="btnEdit linkSumulator"
                         	onclick=" window.open('<%= Url.Generate<ApplicationController>(c => c.DocumentsAdmissionReceipt(Model)) %>',
            '', 'width=800,height=600,toolbar=0,scrollbars=1'); " >
                        Расписка в приеме документов
                    </span>
                </td>					
            </tr>				
            <tr class="trline1">
                <td class="caption">
                    <span	class="btnEdit linkSumulator"
                         	onclick=" window.open('<%= Url.Generate<ApplicationController>(c => c.ApplicationDocumentsList(Model)) %>',
                '', 'width=800,height=600,toolbar=0,scrollbars=1'); " >
                        Перечень документов, приложенных к заявлению
                    </span>
                </td>					
            </tr>
            <%--<tr class="trline2">
                <td class="caption">
                    <span	class="btnEdit linkSumulator"
                         	onclick=" window.open('<%= Url.Generate<ApplicationController>(c => c.ExaminationResultReference(Model)) %>',
                '', 'width=800,height=600,toolbar=0,scrollbars=1'); " >
                        Справка о результатах ЕГЭ
                    </span>
                </td>					
            </tr>	--%>		
        </tbody>
    </table>	
	
</div>