// Скрипт управления состоянием информационного сообщения

var TITLE_SHOW_MESSAGE = 'Развернуть';
var TITLE_HIDE_MESSAGE = 'Свернуть';

// Инициализация скрипта
function InitNotice() 
{
	getElementsByDirName('c-in');
	
	if (typeof notices == 'undefined') 
	    return;
	
    for (var i=0; i<notices.length; i++) 
	{
		var notice = notices[i].parentNode.parentNode;
		var state = eval("sessvars."+notice.id);
		
	    if (typeof state == 'undefined' || state == 'show') 
		    notices[i].className = 'show';
		else
		    notices[i].className = 'hide';

        ToggleNoticeState(notices[i]);
	}
}

// Изменение состояния сообщения
function ToggleNoticeState(obj) {
	var notice = obj.parentNode.parentNode;
	if (obj.className == 'hide') 
	{
		eval("sessvars."+notice.id+"='hide'");
		notice.className = "notice notice-compact";
		obj.title = TITLE_SHOW_MESSAGE;
		obj.className = 'show';
		obj.innerHTML = '+<span></span>';
	}
	else
	{
		eval('sessvars.'+notice.id+"='show'");
		notice.className = 'notice';
		obj.title = TITLE_HIDE_MESSAGE;
		obj.className = 'hide';
		obj.innerHTML = 'x<span></span>';
	}
	
	return false;
}

function getElementsByDirName(parent) {
	var parentElement = document.getElementById(parent);
	if (parentElement ==null) return;

	var divs = parentElement.getElementsByTagName('div');
	if (typeof divs == "undefined") 
	    return; 

    var noticeIndex = 0;
	for (var divIndex = 0; divIndex < divs.length; divIndex++) {
		if (divs[divIndex].dir == "ltr") {
			notices[noticeIndex] = divs[divIndex];
			noticeIndex++;
		}
	}
}

notices = new Array();
