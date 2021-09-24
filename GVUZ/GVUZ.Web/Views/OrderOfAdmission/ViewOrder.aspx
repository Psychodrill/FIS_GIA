<%@ Page Title="Title" Language="C#" Inherits="System.Web.Mvc.ViewPage<GVUZ.Web.ViewModels.OrderOfAdmission.OrderOfAdmissionViewModel>" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="GVUZ.Helper" %>
<%@ Import Namespace="GVUZ.Web.Helpers" %>

<asp:Content runat="server" ContentPlaceHolderID="PageTitle">
    Просмотр приказа <%=Model.OrderTypeNamePrepositional %>  
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageHeaderContent">
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/libs/knockout-3.3.0.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/libs/knockout.mapping-latest.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/WebUtils.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/FilterViewModelBase.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/PagerViewModel.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Resources/Scripts/grid/ListViewModelBase.js") %>"></script>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="TitleContent">
    Просмотр приказа <%=Model.OrderTypeNamePrepositional %>  
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <div class="divstatement notabs" >

        <% Html.RenderPartial("ViewOrderFields", Model); %>
        <input id="cancelOrderForm" type="button" class="button3" value="Отмена" />
        <input type="button" class="button3" value="Проверка статуса приказа" id="checkOrderStatus" />

    </div>
    
    <h3>Заявления, включенные в приказ  <%=Model.OrderTypeNamePrepositional %> </h3>
    <% Html.RenderPartial("OrderOfAdmission/ApplicationOrderList", Model.ApplicationList); %>

    <script type="text/javascript">

        let currentDateTime = new Date();
        var orderKey = getOrderId();
        var fromLocalStorage = localStorage.getItem(orderKey);

        InitializeTimer();

        function InitializeTimer()
        {

            if (fromLocalStorage != null)
            {

                if (Date.parse(currentDateTime) < Date.parse(fromLocalStorage))
                {
                    Run();
                   
                }
                else
                {
                    delete localStorage.orderKey;
                }
            }

        }

        function Run()
        {
            var checkOrderStatus = document.getElementsByClassName("checkOrderStatus");
            checkOrderStatus[0].innerText = "Ожидается следующая проверка!";

            var timerLabel = document.getElementsByClassName("timerLabel");
            timerLabel[0].textContent = "Следующая проверка через:";

            initializeClock('countdown', fromLocalStorage);

        }

        function getTimeRemaining(endtime) {
            var t = Date.parse(endtime) - Date.parse(new Date());

            var seconds = Math.floor((t / 1000) % 60);
            var minutes = Math.floor((t / 1000 / 60) % 60);
            var hours = Math.floor((t / (1000 * 60 * 60)) % 24);
            /*var days = Math.floor(t / (1000 * 60 * 60 * 24));*/
            return {
                'total': t,
                /*'days': days,*/
                'hours': hours,
                'minutes': minutes,
                'seconds': seconds
            };
        }

        function initializeClock(id, endtime) {
            var clock = document.getElementById(id);
            /*var daysSpan = clock.querySelector('.days');*/
            var hoursSpan = clock.querySelector('.hours');
            var minutesSpan = clock.querySelector('.minutes');
            var secondsSpan = clock.querySelector('.seconds');

            function updateClock() {
                var t = getTimeRemaining(endtime);

                if (t.total < 0) {

                    clearInterval(timeinterval);
                    hoursSpan.innerHTML = '';
                    minutesSpan.innerHTML = '';
                    secondsSpan.innerHTML = '';
                    return true;
                }

                /*daysSpan.innerHTML = t.days;*/
                hoursSpan.innerHTML = ('0' + t.hours + ':').slice(-3);
                minutesSpan.innerHTML = ('0' + t.minutes + ':').slice(-3);
                secondsSpan.innerHTML = ('0' + t.seconds).slice(-2);


            }

            updateClock();
            var timeinterval = setInterval(updateClock, 1000);
        }


        $(function() {
            $('input#cancelOrderForm').click(function (e) {
                e.preventDefault();
                <% if (Model.OrderTypeId == GVUZ.Data.Model.OrderOfAdmissionType.OrderOfAdmission)
                   {
                       %>
                window.location = '<%= Url.Action("OrdersOfAdmissionList") %>';
                <% }
                   else if (Model.OrderTypeId == GVUZ.Data.Model.OrderOfAdmissionType.OrderOfAdmissionRefuse)
                   { %>
                window.location = '<%= Url.Action("OrdersOfAdmissionRefuseList") %>';
                <% }
                      %>
                return false;
            });

        });

        function getOrderId()
        {
            var result = <%=Model.OrderId%>;
            return result;
            
        }

        $(function(){
                $('input#checkOrderStatus').click(function (e) {
                    e.preventDefault();

                    let checkDeadLine = new Date(currentDateTime.getFullYear(), currentDateTime.getMonth(), currentDateTime.getDate() + 1,
                        currentDateTime.getHours(), currentDateTime.getMinutes(), currentDateTime.getSeconds());

                    //let testDate = new Date(currentDateTime.getFullYear(), currentDateTime.getMonth(), currentDateTime.getDate(),
                    //    17, 35, 10);

                    if (localStorage.getItem(orderKey) == null)
                    {
                        localStorage.setItem(orderKey, checkDeadLine);
                        
                    }
 
                    fromLocalStorage = localStorage.getItem(orderKey);

                    InitializeTimer();


                });

            function getTimeRemaining(endtime) {
                var t = Date.parse(endtime) - Date.parse(new Date());

                var seconds = Math.floor((t / 1000) % 60);
                var minutes = Math.floor((t / 1000 / 60) % 60);
                var hours = Math.floor((t / (1000 * 60 * 60)) % 24);
                /*var days = Math.floor(t / (1000 * 60 * 60 * 24));*/
                return {
                    'total': t,
                    /*'days': days,*/
                    'hours': hours,
                    'minutes': minutes,
                    'seconds': seconds
                };
            }

            function initializeClock(id, endtime) {
                var clock = document.getElementById(id);
                /*var daysSpan = clock.querySelector('.days');*/
                var hoursSpan = clock.querySelector('.hours');
                var minutesSpan = clock.querySelector('.minutes');
                var secondsSpan = clock.querySelector('.seconds');




                function updateClock() {
                    var t = getTimeRemaining(endtime);

                    if (t.total < 0) {

                        clearInterval(timeinterval);
                        hoursSpan.innerHTML = '';
                        minutesSpan.innerHTML = '';
                        secondsSpan.innerHTML = '';
                        return true;
                    }

                    /*daysSpan.innerHTML = t.days;*/
                    hoursSpan.innerHTML = ('0' + t.hours+':').slice(-3);
                    minutesSpan.innerHTML = ('0' + t.minutes + ':').slice(-3);
                    secondsSpan.innerHTML = ('0' + t.seconds).slice(-2);


                }

                updateClock();
                var timeinterval = setInterval(updateClock, 1000);
            }

                
        });

    </script>
</asp:Content>

