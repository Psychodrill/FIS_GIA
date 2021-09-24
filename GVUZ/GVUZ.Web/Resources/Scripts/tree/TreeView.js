/*
* Dependencies: 
*  - jQuery library
*  - utils.js - Function.prototype.bind
*  - BaseTreeView.js
*  - ClickableTreeView.js
*/

/*
* Функционал для работы с деревом.
* У контроллеров для добавления, редактирования, удаления на сервере должны быть переменные с именем structureItemID
*/

/*
Для работы с контролом дерева необходимо передать url'ы:
structureUrl - url к контроллеру выдающего структуру узла. На выходе: 
json, представляющее массив хэшей с полями ItemID, Name, IsLeaf, CanDelete, CanEdit, CanAdd.
addUrl - url к контроллеру для добавления узла, который должен вернуть html в открывающийся диалог.
editUrl - url к контроллеру для редактирования узла, который должен вернуть html в открывающийся диалог.
removeUrl - url к контроллеру для удаления узла. Дерево запрашивает подтверждение на удаление.
*/

function TreeView(container, structureUrl, addCallback, nodeClickCallback, deleteCallback,
 addImg, dltImg, strCpyImg, strCpyCallback, options)
{
	TreeView.superclass.constructor.call(this, container, options);	
	this.structureUrl = structureUrl;
	this.addCallback = addCallback;
	this.nodeClickCallback = nodeClickCallback;
	this.deleteCallback = deleteCallback;
	this.addImg = addImg;
	this.dltImg = dltImg;
	this.dialog = null;
	this.strCpyImg = strCpyImg;
	this.strCpyCallback = strCpyCallback;
}

extend(TreeView, BaseTreeView);

TreeView.prototype.init = function ()
{
	TreeView.superclass.init.apply(this, arguments)

	// init dialog area
	jQuery('<div id="' + this.container.get(0).id + 'Dialog"></div>').insertAfter(jQuery(this.container));
	this.dialog = jQuery('#' + this.container.get(0).id + 'Dialog');

	// add button handler
	if (this.addCallback)
	{
		jQuery('table.tree-icons img.addBtn').live('click', this, function (eventData)
		{
			var treeModel = eventData.data;
			var nodeContent = jQuery(this).parents('.NodeContent').first();
			var nodeEl = nodeContent.siblings('.Container').first();
			var addBtn = jQuery(this);
			var curLevelID = parseInt(nodeContent.attr('levelID'));

			treeModel.addCallback(treeModel.getStructItemID(this),
				function (item)
				{
					nodeEl.append(treeModel.createSimpleNode(item, curLevelID + 1));

					// if node was leaf
					if (nodeEl.parents('.Node').hasClass('ExpandLeaf'))
					{
						// set "-" for node
						nodeEl.parent('.Node').toggleClass('ExpandLeaf ExpandOpen');

						// hide cpyBtn if required
						if (treeModel.strCpyImg)
							addBtn.siblings('img.strCpyBtn').hide();
					}

					if (nodeEl.parent('.Node').hasClass('ExpandClosed'))
						treeModel.tree_toggle(nodeEl.parent('.Node').children('div.Expand').first(), false)

					/*
					this - это img.addBtn
					nodeEl - это <ul class="Container">
					nodeContent - это <div class="NodeContent">
					<ul class="Container">
						<li class="Node IsFirst ExpandClosed"> Ещё классы: ExpandOpen, ExpandLeaf, IsLast
							<div class="Expand"></div> - область для раскрытия дерева
							<div class="NodeContent" levelID="2">Node Name <table class="tree-icons" /> </div>
							<ul class="Container"> .... </ul>
						</li>
					</ul>*/
				});
		});
	}

	// strCpyImg button handler 
	if (this.strCpyCallback)
		jQuery('table.tree-icons img.strCpyBtn').live('click', this, function (eventData)
		{
			var treeModel = eventData.data;			
			var nodeContent = jQuery(this).parents('.NodeContent').first();
			var nodeEl = nodeContent.siblings('.Container').first();			
			var itemID = treeModel.getStructItemID(this);
			var cpyBtn = jQuery(this);

			treeModel.strCpyCallback(itemID,
				function callback()
				{
					// if node was leaf
					if (nodeEl.parent('.Node').hasClass('ExpandLeaf'))
					{
						// add to node "+"
						nodeEl.parent('.Node').toggleClass('ExpandLeaf');
						nodeEl.parent('.Node').addClass('ExpandClosed');
					}
					// hide copy structure icon
					cpyBtn.hide();
				});
		});

	// edit button handler
	if (this.nodeClickCallback)
		jQuery('.NodeContent a').live('click', this, function (eventData)
		{
			var treeModel = eventData.data;
			var nodeEl = jQuery(this);
			treeModel.nodeClickCallback(treeModel.getStructItemID(this),
				function (item)
				{
					if (item != null)
						nodeEl.html(item.Name);
				});
		});

	// delete button handler 
	if (this.deleteCallback)
		jQuery('table.tree-icons img.dltBtn').live('click', this, function (eventData)
		{
			var treeModel = eventData.data;
			var item = jQuery(this);
			var itemID = treeModel.getStructItemID(item);
			treeModel.deleteCallback(treeModel.getStructItemID(item),
				function callback(canCopyStr)
				{
					// скрываем узел после удаления
					jQuery(item.parents('li.Node').get(0)).hide();
					// если удален последний ребенок у родителя, то добавляем иконку листа родителю
					if (jQuery(item.parents('li.Node').get(0)).siblings(":visible").length == 0)
					{
						var parents = item.parents('li.Node');
						if (parents.length > 1)
						{
							var parentNode = jQuery(parents.get(1));
							parentNode.toggleClass('ExpandOpen');
							parentNode.addClass('ExpandLeaf');
							// remove attribute filled from Node
							parentNode.toggleClass('filled');

							// show structure copy button for parent
							if (treeModel.strCpyCallback && canCopyStr)
							{
								var parentContent = parentNode.children('div.NodeContent');
								if (parentContent.children('img.strCpyBtn').length == 0)
									parentContent.append('<img class="strCpyBtn" src="' + treeModel.strCpyImg + '" />');
								else
									jQuery(parentContent.children('img.strCpyBtn').get(0)).show();
							}
						}

						/*
						this - это img.addBtn
						nodeEl - это <ul class="Container">
						nodeContent - это <div class="NodeContent">
						<ul class="Container">
							<li class="Node IsFirst ExpandClosed"> Ещё классы: ExpandOpen, ExpandLeaf, IsLast
								<div class="Expand"></div> - область для раскрытия дерева
								<div class="NodeContent" levelID="2">Node Name <table class="tree-icons" /> </div>
							<ul class="Container"> .... </ul>
							</li>
						</ul>*/
					}
				});
		});


	// #146 bug with dialog 
	/*jQuery(window).bind('resize', function ()
	{
	console.log(jQuery(window).width() + ' ' + jQuery(window).height());
	jQuery('div.ui-widget-overlay').width(jQuery(window).width());
	jQuery('div.ui-widget-overlay').height(jQuery(window).height());
	//console.log(jQuery('div.ui-widget-overlay').width());
	//console.log(jQuery('div.ui-widget-overlay').height());
	});*/

	// #146 bug
	/*if (jQuery.browser.mozilla || jQuery.browser.webkit || jQuery.browser.msie)
	jQuery(window).bind('resize', function ()
	{
	//jQuery('#treeControlDialog').dialog('option', 'position', jQuery('#treeControlDialog').dialog('option', 'position'));
	jQuery('#treeControlDialog').dialog('option', 'position', [100, 200]);
	//jQuery('#treeControlDialog').dialog('option', 'position', 'center');
	});*/
}

TreeView.prototype.renderNodeContent = function (dataItem)
{
	var tags = [];
	if (dataItem.CanEdit || dataItem.CanClick)
		tags.push('<a href="#">' + dataItem.Name + '</a>');
	else
		tags.push(dataItem.Name);
	tags.push('<div style="float:right"><table class="tree-icons"><tr><td>');
	if (dataItem.StructCopy)
		tags.push('<img class="strCpyBtn" src="' + this.strCpyImg + '" />');
	tags.push('</td><td>');
	if (dataItem.CanDelete)
		tags.push('<img class="dltBtn" src="' + this.dltImg + '" />');
	tags.push('</td><td>');
	if (dataItem.CanAdd)
		tags.push('<img class="addBtn" src="' + this.addImg + '" />');
	tags.push('</td></tr></table></div>');

	return tags.join(' ');
}


/*	Пример дерева для структуры ОО
<ul class="Container">
	<li class="Node IsFirst ExpandClosed">
		<div class="Expand"></div>
		<div class="NodeContent">Факультет Экономики и управления</div>
		<ul class="Container"></ul>
	</li>
	<li class="Node IsLast ExpandOpen">
		<div class="Expand"></div>
		<div class="NodeContent">Факультет Управления информационными технологиями</div>
		<ul class="Container">
			<li class="Node ExpandLeaf IsLast">
				<div class="Expand"></div>
				<div class="NodeContent">Item 1</div>
			</li>
		</ul>
	</li>
</ul>
*/
