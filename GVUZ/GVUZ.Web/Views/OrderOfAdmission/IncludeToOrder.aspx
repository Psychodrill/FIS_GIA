<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.OrderOfAdmission.IncludeToOrderViewModel>" %>

<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Helper.MVC" %>
<%@ Import Namespace="GVUZ.Web.Controllers" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>
<asp:Content ID="Content4" ContentPlaceHolderID="PageTitle" runat="server">
    Включение в приказ о зачислении</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageHeaderContent" runat="server">
    <style type="text/css">
        #AppInfo tr
        {
            height: 25px;
        }
        #AppInfo tr td
        {
            padding: 0px;
            margin: 0px;
        }
        #AppInfo tr td:nth-child(1)
        {
            width: 300px;
            text-align: right;
        }
        #AppInfo tr td:nth-child(2)
        {
            text-align: left;
            padding-left: 20px;
        }
        #AppInfo tr td:nth-child(3)
        {
            text-align: right;
        }
        #AppInfo tr td:nth-child(4)
        {
            text-align: left;
            padding-left: 20px;
        }
        
        #OrdInfo
        {
            width: 900px;
        }
        #OrdInfo tr td:nth-child(1)
        {
            width: 300px;
            text-align: right;
        }
        #OrdInfo tr td:nth-child(2)
        {
            text-align: left;
            padding-left: 20px;
        }
        #OrdInfo tr td:nth-child(3)
        {
            width: 300px;
            text-align: right;
        }
        #OrdInfo tr td:nth-child(4)
        {
            text-align: left;
            padding-left: 20px;
        }
        
        #tblCon tr th
        {
            text-align: center;
        }
        #tblCon tr td
        {
            text-align: center;
        }
        
        #btnInsertIntoOrder
        {
            width: 200px;
        }
        #Con
        {
            margin-top: 20px;
            margin-bottom: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PageSubtitle" runat="server">
</asp:Content>
<asp:Content ID="MainContentCPH" ContentPlaceHolderID="MainContent" runat="server">
    <div id="content" v-cloak>

        <%--Сведения о заявлении--%>
        <table id="AppInfo" class="tableApp2">
            <thead>
                <tr>
                    <th colspan="4" style="text-align: left">
                        Сведения о заявлении
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Номер заявления</td>
                    <td>{{application.Number}}</td>
                    <td>Уровень бюджета <span class="required">(*)</span>: </td>
                    <td>
                        <select id="BudgetLevels" v-model="IdLevelBudget" 
                            :disabled="!selectedCondition || (selectedCondition && selectedCondition.IdLevelBudget != 0) || educationSourceID==15 || isForeigner==1">
                            <option v-for="option in budgetLevels" v-bind:value="option.ID">
                                {{ option.Name }}
                            </option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>ФИО</td>
                    <td>{{application.FIO}}</td>
                    <td>Льгота <span class="required">(*)</span>:</td>
                    <td>
                        <select id='BenefitID' v-model="benefitID"  :disabled="!selectedCondition">
                            <option v-for="option in benefits" v-bind:value="option.BenefitID" :selected="null">
                                {{ option.BenefitName }}
                            </option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>Док-т, удостоверяющий личность:</td>
                    <td>{{application.Document}}</td>
                </tr>
            </tbody>
        </table>

        <%--конкурсы--%>
        <div id="Con">
            <table id="tblCon" class="gvuzDataGrid tableStatement2" style="width: 100%">
                <thead>
                    <tr>
                        <th rowspan="2"></th>
                        <th rowspan="2">Конкурс</th>
                        <th rowspan="2">Направление подготовки/специальность</th>
                        <th rowspan="2">Уровень образования</th>
                        <th rowspan="2">Форма обучения</th>
                        <th rowspan="2">Источник финансирования</th>
                        <th rowspan="2">Количество баллов</th>
                        <th rowspan="2">Объем приема</th>
                        <th colspan="3">По уровням бюджета</th>
                    </tr>
                    <tr>
                        <th>Фед</th>
                        <th>Рег</th>
                        <th>Мун</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(condition, index) in conditionIncludes">
                        <td><input type="radio" @click="ConSelect(condition)"/></td>
                        <td>{{condition.CompetitiveGroupName}}</td>
                        <td>{{condition.DirectionName}}</td>
                        <td>{{condition.EduLevelName}}</td>
                        <td>{{condition.EducationFormName}}</td>
                        <td>{{condition.EducationSourceName}}</td>
                        <td>{{condition.PointsStr}}</td>
                        <td>{{condition.CGCount}}/{{condition.CGTotal}}</td>
                        <td>{{condition.CGBFedCount}}/{{condition.CGBFedTotal}}</td>
                        <td>{{condition.CGBRegCount}}/{{condition.CGBRegTotal}}</td>
                        <td>{{condition.CGBMunCount}}/{{condition.CGBMunTotal}}</td>
                    </tr>
                </tbody>
            </table>
        </div>

        <%--приказ--%>
        <%=Html.Hidden("OrderID",   Model.Ord.OrderID)%>
        <table id="OrdInfo" class="tableApp2">
            <thead>
                <tr>
                    <th colspan="4" style="text-align: left">
                        Сведения о приказе
                    </th>
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
            <input id="btnInsertIntoOrder" type="button" value="Включить в приказ" class="button3"
                @click="InsertIntoOrder()" />
            <input type="button" value="Отмена" class="button3" @click="CancelInsertIntoOrder()" />
        </div>
    </div>
    <script type="text/javascript">
        var app = new Vue({
            el: '#content',
            data: function() {
                return {
                    conditionIncludes: null,
                    application: null,
                    selectedCondition: null,
                    budgetLevels: null,
                    benefits: null,

                    //атрибуты для включения
                    IdLevelBudget: null,
                    benefitID : null,
                    educationSourceID: null, 
                    isForeigner: null
                };
            },
            created: function () {
                this.loadData();
            },
            methods: {
                loadData: function () {
                    this.application = JSON.parse('<%= Html.Serialize(Model.App) %>');
                    this.conditionIncludes = JSON.parse('<%= Html.Serialize(Model.ConditionIncludes) %>');
                    this.order = JSON.parse('<%= Html.Serialize(Model.Ord) %>');

                    this.budgetLevels = JSON.parse('<%= Html.Serialize(Model.BudgetLevels) %>');
                    this.benefits = JSON.parse('<%= Html.Serialize(Model.Benefits) %>');
                    this.benefits.unshift({BenefitID: 0, BenefitName: 'Нет льгот'});
                    this.benefits.unshift({BenefitID: null, BenefitName: 'Нет выбрано'}); 
                    //console.log(this.conditionIncludes);
                },
                ConSelect: function (condition) {
                    app.selectedCondition = condition;
                    if (condition.IdLevelBudget) {
                        app.IdLevelBudget = condition.IdLevelBudget;
                    }

                    app.educationSourceID = condition.EducationSourceID
                    app.isForeigner = <%= Model.Ord.IsForeigner.Value?"1":"0" %>;

                    //$('#BenefitID').removeAttr("disabled");
                    if (app.benefits.length == 2) {
                        app.benefitID = 0;
                    }
                    else 
                        app.benefitID = null;
                },
                InsertIntoOrder: function () {
                    var req = {};

                    req.ApplicationID = app.application.ApplicationID;
                    req.OrderID = app.order.OrderID;
                    
                    if (!app.selectedCondition || !app.selectedCondition.AppCGItemId) {
                        infoDialogC("Выберите условие приема (конкурс)", { title: "Ошибка" });
                        return;
                    }

                    req.AppCGItemID = app.selectedCondition.AppCGItemId;
                    req.IdLevelBudget = app.IdLevelBudget == 0 ? null : app.IdLevelBudget;
                    req.BenefitID = app.benefitID

                    if (app.educationSourceID != 15 && app.isForeigner != 1) {
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
                    

                    doPostAjaxSync("<%= Url.Generate<OrderOfAdmissionController>(x => x.FuncCheckIncludeAppToOrder(null,null,null,null,null)) %>", JSON.stringify(req), function(data) {           
                        if (data.IsError) {
                            if (data.Data) {
                                if (data.Extra) {
                                    var msg = "Включить заявление № "+ app.application.Number +" от " + app.application.RegistrationDateText + " в приказ? <br/><br/>";
                                    for (var i = 0; i < data.Data.length; i++) {
                                        msg = msg + (i + 1) + ") " + data.Data[i].Msg + '<br/><br/>';
                                    }
                                    confirmDialog(msg, function() {
                                        doPostAjaxSync("<%= Url.Generate<OrderOfAdmissionController>(x => x.IncludeAppToOrder(null,null,null,null,null)) %>", JSON.stringify(req), function(data) {                               
                                            if(data.IsError)
                                            { 
                                                app.ShowIncludeAppToOrderError(data); 
                                            }
                                            else 
                                            {
                                                app.ShowIncludeAppToOrderSuccess();
                                            }
                                        });
                                    }, function() { return false });
                                } else {
                                    app.ShowIncludeAppToOrderError(data); 
                                }
                            }
                        } else {
                            doPostAjaxSync("<%= Url.Generate<OrderOfAdmissionController>(x => x.IncludeAppToOrder(null,null,null,null,null)) %>", JSON.stringify(req), function(data) {
                                if(data.IsError)
                                { 
                                    app.ShowIncludeAppToOrderError(data); 
                                }
                                else 
                                {
                                    app.ShowIncludeAppToOrderSuccess();
                                }
                            });
                        }
                    }); 
                },
                CancelInsertIntoOrder: function() {
                    window.location = "<%= Url.Generate<OrderOfAdmissionController>(x => x.ApplicationSelectOrder()) %>?ApplicationID=" + app.application.ApplicationID
                },
                ShowIncludeAppToOrderSuccess: function() {
                    infoDialog("Заявление № " + app.application.Number + " от " + app.application.RegistrationDateText + " включено в приказ ", function() {
                        window.location = "<%= Url.Generate<InstitutionApplicationController>(x => x.ApplicationList()) %>#tab3";
                    });
                },
                ShowIncludeAppToOrderError: function(data) {
                    var errMsg = "Произошла ошибка. Заявление № " + app.application.Number + " от " + app.application.RegistrationDateText + " не может быть включено в приказ <br/><br/>";
                    var errTitle = "Ошибка включения заявления в приказ";
                    if(data.Data) {
                        errMsg = "Заявление № " + app.application.Number + " от " + app.application.RegistrationDateText + " не может быть включено в приказ <br/><br/>";
                        for (var i = 0; i < data.Data.length; i++) {
                            errMsg = errMsg + (i + 1) + ") " + data.Data[i].Msg + '<br/><br/>';
                        }

                        errTitle = "Проверка условий зачисления";
                    }
                    infoDialogC(errMsg, { width: 640, height: 480, title: errTitle });
                }
            }
        })
    </script>
</asp:Content>
