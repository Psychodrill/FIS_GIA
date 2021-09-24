
// Таб контрол
// Параметры:
//	menuItems - массив объектов. Поля объекта:
//		name - название меню
//		link - ссылка
//    ajax - флаг, указывающий на загрузку данных в таб через ajax
//		disabled (boolean) - блокировка пункта меню
//		noWrap (boolean) - название таба в одну строку
//		selected (boolean) - выбор таба как активного
//	options - объект с настройками. Поля объекта:
//		tabWidth - размер таба, по умолчанию берется из стилей
//		prefix - префикс для ID menu item'ов
function TabControl(container, menuItems, options, contentArea)
{
	this.container = container;
	this.menuItems = menuItems;
	this.options = options;
	// area where server generated content displayed if ajax used
	this.contentArea = contentArea;
	this._isAjaxTab = false;
	// использовать для получения текущего выбранного пункта меню _getSelectedItemIndex()
	this._selectedItemIndex;
}
/*
	<a href="#" class="menuitemr select2">Общая<br>информация</a>
	<a href="#" class="menuitemr">Структура ОО</a>
	<a href="#" class="menuitemr">Структура<br>приема</a>
	<a href="#" class="menuitemr">Вступительные<br>испытания</a>
	<a href="#" class="menuitemr">Подготовительные<br>курсы</a>
	<a href="#" class="menuitemr">Олимпиады</a>
	<a href="#" class="menuitemr">Объем и структура<br>приема</a>
	<a href="#" class="menuitemr">Конкурсные<br>группы</a>
*/

TabControl.prototype._getSelectedItemIndex = function ()
{
	if (this._selectedItemIndex == null)
	{
		this._selectedItemIndex = 0;
		if (this.menuItems != null && this.menuItems.length > 0)
		{
			for (var i = 0; i < this.menuItems.length; i++)
				if (this.menuItems[i].selected)
				{
					this._selectedItemIndex = i;
					break;
				}
		}
	}

	return this._selectedItemIndex;
}

TabControl.prototype.renderItem = function (menuData, index)
{
	if (!menuData)
		return;
	
	if (menuData.ajax)
	{
		var itemArr = [];
		var pr = '';
		if (this.options != null)
			if (this.options.prefix != null)
				pr = this.options.prefix;
		var itemID = pr + "menuItem" + (index + 1);
		itemArr.push(sprintf('<input id="%1$s" type="radio" name="' + pr + 'menu" />', itemID));
		itemArr.push(sprintf('<label for="%1$s">%2$s</label>', itemID, menuData.name));
		this.container.append(itemArr.join(''));
		var item = jQuery(this.container.children(sprintf('#%1$s', itemID)));
		var label = jQuery(this.container.children(sprintf('label[for="%1$s"]', itemID)));
		if (menuData.disabled) label.addClass('buttonDisabled');
		if (menuData.selected)
		{
			item.attr('checked', 'checked');
			this._isItemSelected = true;
		}

		if (this.options != null)
		{
			if (this.options.tabWidth != null)
				label.width(this.options.tabWidth);
		}

		this.initItemNavigation(item, menuData);
	}
	else
	{		
		var selectedItemIndex = this._getSelectedItemIndex();
		var itemClass = index < selectedItemIndex ? "menuiteml" : "menuitemr";
		if (selectedItemIndex == index) itemClass += (menuData.name.indexOf("<br") > -1) ? " select2" : " select";		
		var itemHtml = sprintf('<a href="%1$s" class="%2$s">%3$s</a>', menuData.link, itemClass, menuData.name);
		this.container.append(itemHtml);
	}
}

TabControl.prototype.initItemNavigation = function (button, itemData)
{
	if(!itemData.ajax)
	{
		
		button.bind('click',
			function (eventData)
			{
				window.location.href = itemData.link;
			} .bind(this));
	} 
	else if(itemData.ajax)
	{
		this._isAjaxTab = true;

		// prepare content area for ajax content if it isn't specified by user
		if (this.contentArea == null)
		{
			var contentAreaID = this.container.attr('id') + '_content';
			this.container.after('<div id="' + contentAreaID + '"></div>');
			this.contentArea = jQuery('#' + contentAreaID);
		}

		button.bind('click',
			function (eventData)
			{
				doPostAjax(itemData.link, null, 
					function (eventData) {
							this.contentArea.html(eventData);
						}.bind(this), 'text/html', 'html');

					} .bind(this));

		// load content for selected tab
		if(itemData.selected)
			button.click();
	}
}

TabControl.prototype.init = function ()
{
	this._isItemSelected = false;
	this._selectedItemIndex = null;

	this.container.empty();
	this.container.hide();

	for (var i = 0; i < this.menuItems.length; i++)
		this.renderItem(this.menuItems[i], i);

	//activate first menu item for ajax tab if no one item selected
	if (this._isAjaxTab && !this._isItemSelected)
		this.container.children('input:first').click();

	this._isItemSelected = null;
	// применяем jQuery
	if (this._isAjaxTab)
		this.container.buttonset();

	this.container.find('label > span').each(
				function (index, elem)
				{
					if (this.menuItems[index] != null && this.menuItems[index].noWrap)
						jQuery(elem).addClass("menuNoWrap");
				} .bind(this));

	// показываем меню
	this.container.show();
}

//  Структура меню
//	<input id="item1" type="radio" name="menu" checked="checked" /><label for="item1">Общая информация</label>&nbsp;
//	<input id="item2" type="radio" name="menu" /><label for="item2">Структура ВУЗа</label>

