
/*
 * Иерархическая таблица.
 * dataObjects - это хэш с id объекта и его данными. Данные должны обязательно содержать поле Name с именем узла.
 * dataChildren - это хэш с id объекта и массивом id объектов детей.
 */
function TableTreeView(container, dataObjects, dataChildren, headersArray, nodeClickCallback, options)
{
	TableTreeView.superclass.constructor.call(this, container, null, nodeClickCallback, options);
	this.dataObjects = dataObjects;
	this.dataChildren = dataChildren;
	this.headersArray = headersArray;
	this.ColumnRenderer = options != null ? options.ColumnRenderer : null;
}

extend(TableTreeView, ClickableTreeView);

TableTreeView.prototype.init = function (options)
{
	TableTreeView.superclass.init.apply(this, arguments);
	GenerateHeader.call(this);

	function GenerateHeader()
	{
		if (this.headersArray instanceof Array && this.headersArray.length > 0)
		{
			var hdrs = this.headersArray;
			var tags = [];

			tags.push('<table class="gvuzHeaderTable">');
			tags.push('<col width="*" />');
			for (var i = 1; i < hdrs.length; i++)
				tags.push('<col width="85" />');
			tags.push(sprintf('<tr><td class="gvuzContentName">%1$s</td>', hdrs[0]));
			for (var i = 1; i < hdrs.length; i++)
				tags.push('<td class="gvuzContentDetails">' + hdrs[i] + '</td>');
			tags.push('</tr></table>');

			// add header after button controls in tree
			var buttons = this.container.children('input')
			if (buttons.size() > 0)
				buttons.last().after(tags.join(''));
			else
				this.container.before(tags.join(''));
		}
	}
}

TableTreeView.prototype.loadRootLevel = function ()
{
	var roots = this.dataChildren[0];
	for (var i = 0; i < roots.length; i++)
	{
		// prepare data for tree root level
		var rootItem = this.dataObjects[roots[i]];
		this.container.append(this.createRootNode(rootItem));
	}
}

TableTreeView.prototype.renderNodeContent = function (paramTreeData)
{
	var tags = [];
	var arrayData = new TreeData(paramTreeData);
	var displayArray = arrayData.formatArray();
	tags.push('<table class="gvuzContentTable">');
	tags.push('<col width="*" />');
	for (var i = 0; i < displayArray.length; i++)
		tags.push('<col width="85" />');
	tags.push(sprintf('<tr><td class="gvuzContentName">%1$s</td>',
		TableTreeView.superclass.renderNodeContent.call(this, paramTreeData)));

	for (var i = 0; i < displayArray.length; i++)
	{
		var tdValue = displayArray[i];
		if (this.ColumnRenderer != null && this.ColumnRenderer[i] != null)
			tdValue = this.ColumnRenderer[i](tdValue, paramTreeData);
		tags.push(sprintf('<td class="gvuzContentDetails">%1$s</td>', tdValue));
	}
	tags.push('</tr></table>');
	return tags.join('');
}