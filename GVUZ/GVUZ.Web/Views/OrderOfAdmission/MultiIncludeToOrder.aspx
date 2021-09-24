<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.OrderOfAdmission.MultiIncludeToOrderViewModel>" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<asp:Content ID="Content4" ContentPlaceHolderID="PageTitle" runat="server">Включение в приказ о зачислении</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageHeaderContent" runat="server">
<style type="text/css">
  #ConInfo tr{ height:25px; }
  #ConInfo tr td{ text-align:center; padding:0px; margin:0px;}
  #ConInfo tr td:nth-child(1) { width: 300px; text-align:right;}
  #ConInfo tr td:nth-child(2) { text-align:left; padding-left: 20px;}  
  #ConInfo tr td:nth-child(3) { text-align:right;}
  #ConInfo tr td:nth-child(4) { text-align:left; padding-left: 20px;}
  #ConInfo tr td:nth-child(5) { text-align:right;}  
  #ConInfo tr td:nth-child(6) { text-align:left; padding-left: 20px;}    
  
  #OrdInfo { width: auto;}
  #OrdInfo tr td:nth-child(1) { width: 300px;text-align:right}  
  #OrdInfo tr td:nth-child(2) { width: 200px;text-align:left; padding-left: 20px; }    
  #OrdInfo tr td:nth-child(3) { width: 300px;text-align:right}
  #OrdInfo tr td:nth-child(4) { width: 200px;text-align:left; padding-left: 20px;}
  #btnInsertIntoOrder { width:200px;}
  #Apps{ margin-top:20px; margin-bottom:20px;}
  
  #tblApps tr th{text-align: center;}
  #tblApps tr td:nth-child(5) {width: 100px;text-align: center;}  
  #tblApps tr td:nth-child(6) {width: 500px;text-align: left;}    
</style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PageSubtitle" runat="server"></asp:Content>
<asp:Content ID="MainContentCPH" ContentPlaceHolderID="MainContent" runat="server">
<div id="content" v-cloak>
    <table id="ConInfo" class="tableApp2">
        <thead>
            <tr>
                <th colspan="6" style="text-align:left">Сведения о заявлениях</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td><%= Html.TableLabelFor(m => m.Condition.CompetitiveGroupName)%></td>
                <td>{{condition.CompetitiveGroupName}}</td>
                <td><%= Html.TableLabelFor(m => m.Condition.CGTotal)%></td>
                <td>{{condition.CGCount}}/{{condition.CGTotal}}</td>
                <td>Уровень бюджета <span class="required">(*)</span>:</td>
                <td>
                    <select id="BudgetLevels" v-model="IdLevelBudget" 
                        :disabled="!condition || (condition && condition.IdLevelBudget != 0) || educationSourceID==15 || isForeigner">
                        <option v-for="option in budgetLevels" v-bind:value="option.ID">
                            {{ option.Name }}
                        </option>
                    </select>
                </td>   
            </tr>
            <tr>
                <td><%= Html.TableLabelFor(m => m.Condition.DirectionName)%></td>
                <td>{{condition.DirectionName}}</td>
                <td>По уровням бюджета:</td>
                <td></td>
            </tr>
            <tr>
                <td><%= Html.TableLabelFor(m => m.Condition.EduLevelName)%></td>
                <td>{{condition.EduLevelName}}</td>
                <td><%= Html.TableLabelFor(m => m.Condition.CGBFedTotal)%></td>
                <td>{{condition.CGBFedCount}}/{{condition.CGBFedTotal}}</td>
                <td>Льгота <span class="required">(*)</span>:</td>
                <td>
                    <select id='BenefitID' v-model="benefitID">
                        <option v-for="option in benefits" v-bind:value="option.BenefitID" :selected="null">
                            {{ option.BenefitName }}
                        </option>
                    </select>
                </td>
            </tr>
            <tr>
                <td><%= Html.TableLabelFor(m => m.Condition.EducationFormName)%></td>
                <td>{{condition.EducationFormName}}</td>
                <td><%= Html.TableLabelFor(m => m.Condition.CGBRegTotal)%></td>
                <td>{{condition.CGBRegCount}}/{{condition.CGBRegTotal}}</td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td><%= Html.TableLabelFor(m => m.Condition.EducationSourceName)%></td>
                <td>{{condition.EducationSourceName}} <%=Html.Hidden("EducationSourceID",Model.Condition.EducationSourceID)%></td>
                <td><%= Html.TableLabelFor(m => m.Condition.CGBMunTotal)%></td>
                <td>{{condition.CGBMunCount}}/{{condition.CGBMunTotal}}</td>
                <td></td>
                <td></td>
            </tr>
        </tbody>
    </table>
    <div id="Apps">
      <table id="tblApps" class="gvuzDataGrid tableStatement2" style="width: 100%">
          <thead>
              <tr><th rowspan="2"><input type="checkbox" v-model="checkAll" @click="CheckAll()" /></th>
                  <th rowspan="2"><%= Html.DisplayNameFor(x => x.App.Number) %></th>
                  <th rowspan="2"><%= Html.DisplayNameFor(x => x.App.FIO)%></th>
                  <th rowspan="2"><%= Html.DisplayNameFor(x => x.App.Document)%></th>
                  <th rowspan="2"><%= Html.DisplayNameFor(x => x.App.Points)%></th>
                  <th rowspan="2">Результат включения в приказ</th>
              </tr>      
          </thead>      
          <tbody>
              <tr v-for="app in applications">
                  <td><input type="checkbox" v-model="app.checked"/></td>
                  <td>{{app.Number}} от {{app.RegistrationDateText}}</td>
                  <td>{{app.FIO}}</td>
                  <td>{{app.Document}}</td>
                  <td>{{app.PointsStr}}</td>
                  <td :class="[app.IsError ? 'input-validation-error' : '']">
                      {{app.Result}}
                      <div v-for="(error, id) in app.Errors">{{id + 1}}) {{error.Msg}}</div>
                  </td>
              </tr>
          </tbody>          
      </table>  
    </div>
    <%=Html.Hidden("OrderID",   Model.Ord.OrderID)%>
    <table id="OrdInfo" class="tableApp2">
        <thead>
            <tr>
                <th colspan="4" style="text-align:left">Сведения о приказе</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td><%= Html.TableLabelFor(m => m.Ord.OrderName)%></td>
                <td>{{order.OrderName}}</td>
                <td><%= Html.TableLabelFor(m => m.Ord.EducationLevel)%></td>
                <td>{{order.EducationLevel}}</td>
            </tr>
            <tr>
                <td><%= Html.TableLabelFor(m => m.Ord.OrderDetails)%></td>
                <td>{{order.OrderDetails}}</td>
                <td><%= Html.TableLabelFor(m => m.Ord.EducationForm)%></td>
                <td>{{order.EducationForm}}</td>
            </tr>
            <tr>
                <td><%= Html.TableLabelFor(m => m.Ord.IsForBeneficiary)%></td>
                <td>{{order.IsForBeneficiaryText}}</td>
                <td><%= Html.TableLabelFor(m => m.Ord.EducationSource)%></td>
                <td>{{order.EducationSource}}</td>
            </tr>
            <tr>
                <td><%= Html.TableLabelFor(m => m.Ord.IsForeigner)%></td>
                <td>{{order.IsForeignerText}}</td>
                <td><%= Html.TableLabelFor(m => m.Ord.Stage)%></td>
                <td>{{order.Stage}}</td>
            </tr>
        </tbody>
    </table>
    <div style="padding: 8px; padding-left: 0">            
      <input id="btnInsertIntoOrder" type="button" value="Включить в приказ" class="button3" @click="InsertIntoOrder();"  />
      <input id="btnCancel" type="button" value="Отмена" class="button3" @click="CancelInsertIntoOrder();" />
    </div>
</div>
<script type="text/javascript">
    var app = new Vue({
            el: '#content',
            data: function() {
                return {
                    conditionIncludes: null,
                    applications: null,
                    selectedCondition: null,
                    budgetLevels: null,
                    benefits: null,

                    //атрибуты для включения
                    IdLevelBudget: null,
                    benefitID : null,
                    educationSourceID: null, 
                    isForeigner: null,

                    checkAll: false
                };
            },
            created: function () {
                this.loadData();
                this.CheckAll();
            },
            methods: {
                loadData: function () {
                    this.applications = JSON.parse('<%= Html.Serialize(Model.Apps) %>');
                    this.condition = JSON.parse('<%= Html.Serialize(Model.Condition) %>');
                    this.order = JSON.parse('<%= Html.Serialize(Model.Ord) %>');

                    this.educationSourceID = this.condition.EducationSourceID;
                    this.isForeigner = this.condition.IsForeigner;

                    this.budgetLevels = JSON.parse('<%= Html.Serialize(Model.BudgetLevels) %>');
                    this.benefits = JSON.parse('<%= Html.Serialize(Model.Benefits) %>');
                    this.benefits.unshift({BenefitID: 0, BenefitName: 'Нет льгот'});
                    this.benefits.unshift({BenefitID: null, BenefitName: 'Нет выбрано'});

                    if (this.condition.IdLevelBudget && this.condition.IdLevelBudget != 0) {
                        this.IdLevelBudget = this.condition.IdLevelBudget;
                    }

                    if (this.benefits.length == 2) {
                        this.benefitID = 0;
                    }
                    else
                        this.benefitID = null;                                      
                },
                CheckAll: function () {
                    this.checkAll = !this.checkAll
                    for (var i = 0; i < this.applications.length; i++) {
                        this.applications[i].checked = this.checkAll;
                    }
                },
                InsertIntoOrder: function () {
                    var req = {};
                    req.OrderID = app.order.OrderID;
                    req.IdLevelBudget = app.IdLevelBudget;
                    req.BenefitID = app.benefitID

                    if (app.educationSourceID != 15 && !app.order.IsForeigner) {
                        if (!(parseInt(req.IdLevelBudget) > 0)) {
                            $('#BudgetLevels').addClass('input-validation-error');
                            infoDialogC("Выберите уровень бюджета", { title: "Ошибка" });
                            return;
                        } else {
                            $('#BudgetLevels').removeClass('input-validation-error');
                        }
                    }
                    
                    if (req.BenefitID == null) {
                        infoDialogC("Выберите льготу", { title: "Ошибка" });
                        $("#BenefitID").addClass('input-validation-error');
                        return;
                    } else {
                        $("#BenefitID").removeClass('input-validation-error');
                    }
                    if (req.BenefitID == 0) {
                        req.BenefitID = null;
                    }

                    // Начинаем обработку
                    var ls = [];

                    for (var i = 0; i < app.applications.length; i++) {
                        var item = app.applications[i];
                        if (item.checked) {
                            ls.push(item);
                        }
                    }

                    //$('td[data-field=Result]').removeClass('input-validation-error').removeClass('input-validation-error-fixed');
                    if (!ls.length || ls.length === 0) {
                        infoDialog('Выберите заявления для включения в приказ');
                        return;
                    }
                    blockUI();

                    for (var i = 0; i < ls.length; i++) {
                        var ito = ls[i];
                        ito.OrderID = req.OrderID;
                        ito.IdLevelBudget = req.IdLevelBudget == 0 ? null : req.IdLevelBudget;
                        ito.BenefitID = req.BenefitID;

                        doPostAjaxSync("<%= Url.Generate<OrderOfAdmissionController>(x => x.IncludeAppToOrder(null,null,null,null,null)) %>", JSON.stringify(ito), function (data) {
                            var resultApp = _findByKey(app.applications, 'ApplicationID', ito.ApplicationID);
                            var index = app.applications.indexOf(resultApp);

                            if (data.IsError) {
                                // Если проверка , то в Data список проверок
                                resultApp.Result = "Заявление № " + ito.Number + " не может быть включено в приказ";
                                resultApp.IsError = 1
                                if (data.Data) {
                                    // Список проверок                                
                                    resultApp.Errors = data.Data;
                                }
                            } else {
                                resultApp.Result = "Заявление № " + ito.Number + " включено в приказ";
                                if (data.Data) {
                                    resultApp.Errors = data.Data;
                                }
                            }
                            Vue.set(app.applications, index, resultApp);
                        }, null, null, null);
                    }
                    unblockUI();  
                    $('#btnInsertIntoOrder').attr("disabled", true);
                    $('#btnCancel').val('Закрыть');
                    return;
                },
                CancelInsertIntoOrder: function () {
                    window.location = "<%= Url.Generate<InstitutionApplicationController>(x => x.ApplicationList()) %>#tab3";
                }
            }
        })
</script>
</asp:Content>