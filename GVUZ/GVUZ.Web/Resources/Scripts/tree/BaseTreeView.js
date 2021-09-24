/*
* Dependencies: 
*  - jQuery library
*  - utils.js - Function.prototype.bind
*/

/*
* Базовый класс для дерева.
* options - параметры для работы с деревом. Поддерживает свойства
*   LoadExpandedRoot - загрузка дерева с развернутым корнем
*   ExpandAll (boolean) - поддержка "развернуть все",
*   CollapseAll (boolean) - поддержка "свернуть все",
*   ExpandUrl (string) - url к серверу для запроса. 
С сервера должны приходить два JSON объекта:
*			dataObjects - это хэш с id объекта и его данными. Данные должны обязательно содержать поле Name с именем узла.
*			dataChildren - это хэш с id объекта и массивом id объектов детей.
*/
function BaseTreeView(container, options)
{
	this.container = (container);

	// used for loading root and subroot elements
	this._rootLoad = false;
	this.options = options;

	// for expand all functionality
	this.dataObjects;
	this.dataChildren;
}

BaseTreeView.prototype.init = function ()
{
	this.container.addClass('Tree');
	this.container.bind('click',
		function (event)
		{
			event = event || window.event
			var clickedElem = event.target || event.srcElement;
			this.tree_toggle(jQuery(clickedElem));
		} .bind(this));

	if (this.isOptionTrue('LoadExpandedRoot'))
		this._rootLoad = true;

	this.loadRootLevel();

	if (this.options != null && (this.options.ExpandAll || this.options.CollapseAll))
		this.container.prepend(jQuery('<div class="spacer" style="margin-bottom: 20px"></div>'));
	if (this.options != null && this.options.ExpandAll)
	{
		this.container.prepend(jQuery('<input type="button" id="expandAllBtn" class="button3 w150px" value="Развернуть все" />'));
		var expandAllButton = jQuery('#expandAllBtn');		
		expandAllButton.bind('click', this,
			function (eventData)
			{
				var treeModel = eventData.data;
				if (treeModel.options != null && treeModel.options.ExpandAll)
					LoadExpandAllData.call(treeModel);
			});
	}

	if (this.options != null && this.options.CollapseAll)
	{
		this.container.prepend(jQuery('<input type="button" id="collapseAllBtn" class="button3 w150px" value="Свернуть все" />'));
		var collapseButton = jQuery('#collapseAllBtn')		
		collapseButton.bind('click', this,
			function (eventData)
			{
				var treeModel = eventData.data;
				// ищем все ноды и сворачиваем
				treeModel.container.find('li.ExpandOpen').each(
					function (index)
					{
						jQuery(this).removeClass('ExpandOpen');
						jQuery(this).addClass('ExpandClosed');
					});
			});
	}	

	// extract method to stand alone class with full structure loading support
	function LoadExpandAllData()
	{
		// load data if expandUrl specified and dataObjects still isn't loaded
		if (this.options != null && this.options.ExpandUrl != null && this.dataObjects == null)
		{
			doPostAjax(this.options.ExpandUrl, null,
				function (servData)
				{
					this.dataObjects = servData['Objects'];
					this.dataChildren = servData['Children'];
					this._expandNodes();
				} .bind(this));
		}
		else
			this._expandNodes();
	}
}

//
// private: expand all closed nodes
//
BaseTreeView.prototype._expandNodes = function ()
{
	var treeModel = this;
	this.container.find('li.ExpandClosed').each(
		function (index)
		{
			treeModel.tree_toggle(jQuery(this).children('.Expand'), true);
		});
}

BaseTreeView.prototype._setTreeItemLevel = function (levelID, $treeItem)
{ 
	
}

// render root level
// override required in descendant
BaseTreeView.prototype.loadRootLevel = function ()
{
	if (this.isOptionTrue('LoadExpandedRoot'))
	{
		blockUI();
		this._rootLoad = true;
	}

	// prepare data for tree root level
	doPostAjax(this.structureUrl, "structureItemID=0",
		function (data, status)
		{
			this.container.append(this.createRootNode(data));
			if (this.isOptionTrue('LoadExpandedRoot'))
			{
				if (data == null || data != null && data.IsLeaf)
				{
					this._rootLoad = false;
					unblockUI();
				}
				else
					this.container.find('li.Root .Expand').first().click();
			}			

		} .bind(this), "application/x-www-form-urlencoded", null, false);
}

// add children node with itemID into container with callback after action
// callback without arguments.
// override required in descendant. 
BaseTreeView.prototype.customLoadChildren = function (container, itemID, levelID, callback)
{
	// generate children for node with specified itemID
	doPostAjax(this.structureUrl, "structureItemID=" + itemID,
		function (data)
		{
			for (var i = 0; i < data.length; i++)
				container.append(this.createSimpleNode(data[i], levelID));

			if (this._rootLoad)
			{
				this._rootLoad = false;
				unblockUI();
			}

			callback();
		} .bind(this), "application/x-www-form-urlencoded", null, false);
}

BaseTreeView.prototype._internalLoadChildren = function (container, itemID, levelID, callback)
{
	// if exists this.dataObjects with full server structure than get children from it
	if (this.dataObjects != null)
	{
		var children = this.dataChildren[itemID];
		if (children != null && children instanceof Array && children.length > 0)
			for (var i = 0; i < children.length; i++)
			{
				var child = this.dataObjects[children[i]];
				container.append(this.createSimpleNode(child, levelID));
			}
		callback();
	}
	else
		this.customLoadChildren(container, itemID, levelID, callback);
}

// render node content. return html.
// override required in descendant
BaseTreeView.prototype.renderNodeContent = function (dataItem)
{
	var tags = [];
	tags.push('<a href="javascript:void(0)">')
	tags.push(dataItem.Name);
	tags.push('</a>')
	return tags.join('');
}

BaseTreeView.prototype.tree_toggle = function (clickedElem, doExpandChild)
{
	// клик не там
	if (!clickedElem.hasClass('Expand')) return;

	if (clickedElem.parent('.ExpandLeaf').length > 0)
	// клик на листе
		return;

	var parentElem = jQuery(clickedElem.parent());
	var contentElem = clickedElem.next();
	var nextLevelID = parseInt(contentElem.attr('levelID')) + 1;	

	// загрузить содержимое в случае необходимости	
	if (!parentElem.hasClass('filled'))
	{
		var container = jQuery(jQuery(parentElem).children('.Container'))
		container.empty();
		this._internalLoadChildren(container, parentElem.attr('itemID'), nextLevelID,
			function ()
			{
				// показываем содержимое узла после загрузки дерева
				parentElem.toggleClass('filled ExpandOpen ExpandClosed');

				// если doExpandChild, то разворачиваем дочерние узлы
				if (doExpandChild)
				{
					var expandElemArray = clickedElem.siblings('.Container').find('li > div.Expand');
					var treeModel = this;
					expandElemArray.each(function (index)
					{
						treeModel.tree_toggle(jQuery(this), true);
					});
				}
			} .bind(this));
	}
	else
		parentElem.toggleClass('ExpandOpen ExpandClosed');
}

// generate html for node
// mode = 0 - dont add any special classes
// mode = 1 - add class Root (show root in tree)
// item is a hash with properties ItemID, Name, IsLeaf, CanDelete, CanEdit, CanAdd
BaseTreeView.prototype.createNode = function (mode, item, levelID)
{
	var tags = [];
	if (mode == 1)
		tags.push('<ul class="Container ul0"><li class="Node');
	else
		tags.push('<li class="Node');
	if (mode == 1) tags.push('Root');
	if (item.IsLeaf) tags.push('ExpandLeaf');
	else tags.push('ExpandClosed');
	tags.push('" itemID="' + item.ItemID + '">');
	tags.push('<div class="Expand"></div>');
	tags.push(sprintf('<div class="NodeContent" levelID="%1$s">', levelID));	
	tags.push(this.renderNodeContent(item));
	tags.push('</div><ul class="Container"></ul>');
	if (mode == 1)
		tags.push("</li></ul>");
	else
		tags.push("</li>");
	return tags.join(' ');
}

BaseTreeView.prototype.getStructItemID = function (node)
{
	return jQuery(node).parents('li[itemID]').attr('itemID');
}

BaseTreeView.prototype.createRootNode = function (item)
{
	return this.createNode(1, item, 1);
}

BaseTreeView.prototype.createSimpleNode = function createSimpleNode(item, levelID)
{
	return this.createNode(0, item, levelID);
}

BaseTreeView.prototype.isOptionTrue = function (optionName)
{
	if (this.options != null && this.options[optionName] != null)
		return this.options[optionName] == true;
	else 
		return false;
}

/* 
Структура элемента дерева:
<ul class="Container">
<li class="Node IsFirst ExpandClosed"> Ещё классы: ExpandOpen, ExpandLeaf, IsLast
<div class="Expand"></div> - область для раскрытия дерева
<div class="NodeContent">Node Name</div>
<ul class="Container"> .... </ul>
</li>
</ul>

Как работает дерево:
1. Подгружаем верхний уровень дерева.
2. Делаем AJAX запрос при клике на узел дерева, получаем данные по ниже лежащему уровню через JSON 
3. Cтавим для только что раскрытого узла ставим класс filled.
*/

