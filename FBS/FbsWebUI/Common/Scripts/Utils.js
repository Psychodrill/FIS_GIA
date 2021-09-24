isDOM=document.getElementById                       //DOM1 browser (MSIE 5+, Netscape 6, Opera 5+)
isOpera=isOpera5=window.opera && isDOM              //Opera 5+
isOpera6=isOpera && window.print                    //Opera 6+
isOpera7=isOpera && document.readyState             //Opera 7+
isMSIE4=document.all && document.all.item && !isOpera//Microsoft Internet Explorer 4+
isMSIE=isDOM && isMSIE4                             //MSIE 5+
isNetscape4=document.layers                         //Netscape 4.*
isMozilla=isDOM && navigator.appName=="Netscape"    //Mozilla or Netscape 6.*        

function addEvent(element, eventType, lamdaFunction) {
    if (element.addEventListener) {
        element.addEventListener(eventType, lamdaFunction, false);
        return true;
    } else if (element.attachEvent) {
        var r = element.attachEvent('on' + eventType, lamdaFunction);
        return r;
    } else {
        return false;
    }
}

function MoveControls(from, to){
    document.getElementById(to).innerHTML += document.getElementById(from).innerHTML;
    document.getElementById(from).innerHTML = "";
}

function InitTinyMCE(element) {
    tinyMCE.init({
        mode : "exact",
        elements : element,
        theme : "advanced",
        width : "100%",
        height: "350px",
        plugins : "safari,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template",

        // Theme options
        theme_advanced_buttons1 : "newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect,fontselect,fontsizeselect",
        theme_advanced_buttons2 : "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
        theme_advanced_buttons3 : "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
        theme_advanced_buttons4 : "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak",
        theme_advanced_toolbar_location : "top",
        theme_advanced_toolbar_align : "left"
    });
}

var calendar =  null;
function PickDate(field, anchor, container){
    if (calendar == null){
        InitCalendar(container);
    }
    calendar.select(document.getElementById(field), anchor, 'dd.MM.yyyy');
    return false;
}

function InitCalendar(container) {
    calendar = new CalendarPopup(container);
    calendar.setMonthNames('Январь','Февраль','Март','Апрель','Май','Июнь','Июль','Август','Снетябрь','Октябрь','Ноябрь','Декабрь');
    calendar.setDayHeaders('Вс','Пн','Вт','Ср','Чт','Пт','Сб');
    calendar.setWeekStartDay(1);
    calendar.setTodayText("сегодня");
    calendar.setCssPrefix("CALENDAR-");
}

function IntiInputWithDefaultValue(mainInputId, dependentInputId) {
    var main = document.getElementById(mainInputId);
    var dep = document.getElementById(dependentInputId);
    dep.mainInput = main;
    main.dependentInput = dep;
    
    if (main.value.length == 0){
        main.style.display = "none";
        dep.style.display = "";
    }
    
    addEvent(dep, 'focus', 
        function(evt){
            var obj = evt.srcElement == null ? evt.target : evt.srcElement;
            obj.style.display = "none";
            obj.mainInput.style.display = "";
            obj.mainInput.focus();
        }
    );
    
    addEvent(main, 'blur',  
        function(evt){
            var obj = evt.srcElement == null ? evt.target : evt.srcElement;
            if (obj.value.length == 0){
                obj.style.display = "none";
                obj.dependentInput.style.display = "";
            }
        }
    );
    
    // В ie значения из кэша (переход обратно по Back в браузере) как-то странно подгружаются. 
    // Исправляю таким фиксом.
    if (isMSIE)
        setTimeout("IntiInputIEFix('" + mainInputId + "','" + dependentInputId + "');", 1);
}

function IntiInputIEFix(mainInputId, dependentInputId) {
    var main = document.getElementById(mainInputId);
    var dep = document.getElementById(dependentInputId);
    
    if (main.value.length > 0){
        main.style.display = "";
        dep.style.display = "none";
    }    
}

function SwapElementVisibility(me, objId, messageHide, messageShow) {
    var obj = document.getElementById(objId);
    if (obj == null)
        return;
                
    if (obj.style.display == 'none')
    {
        obj.style.display = '';
        me.title = messageHide;
        me.innerHTML = messageHide;
    }
    else 
    {
        obj.style.display = 'none';
        me.title = messageShow;
        me.innerHTML = messageShow;
    }    
    
    return false;
}
