// Скрипт диалога фильтрации

// Инициализация фильтра
function initFilter()
{
    // Покажу статус фильтрации
    document.getElementById("filtr-layer").style.display = '';

    // Инициализирую диалог фильтрации
    var layer = document.getElementById("filtr2-layer");
    layer.setAttribute("class", "hid-filtr2");
    layer.setAttribute("className", "hid-filtr2");
    layer.style.display = 'none';
        
    document.getElementById("filtr2").style.position = 'absolute';

    // Покажу кнопку отмены    
    document.getElementById("btCancel").style.display = '';
}

// Отображение серого слоя
function showLayer(canShow) 
{
	var busyLayer = document.getElementById('busy-layer');
	if (canShow) 
	{
		busyLayer.style.visibility = 'hidden';
	} 
	else 
	{
	    // scrollHeight отдает неверное значение, если скролла нет.
	    // В этом случае возьму clientHeight. 
		if (document.body.clientHeight > document.body.scrollHeight)
            busyLayer.style.height = document.body.clientHeight+'px';		
        else    
            busyLayer.style.height = document.body.scrollHeight+'px';
		
		busyLayer.style.visibility = 'visible';
	}
}

// Отображение диалога фильтрации
function showFilter(canShow)
{
    var filter = document.getElementById('filtr2-layer');

    showLayer(!canShow);

    if (canShow)
        filter.style.display = '';
    else
        filter.style.display = 'none';
}