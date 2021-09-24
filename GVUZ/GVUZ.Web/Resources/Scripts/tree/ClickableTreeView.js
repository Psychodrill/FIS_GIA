/*
* Dependencies: 
*  - jQuery library
*  - utils.js - Function.prototype.bind
*  - BaseTreeView.js
*/


function ClickableTreeView(container, structureUrl, nodeClickCallback, options)
{
	ClickableTreeView.superclass.constructor.call(this, container, options);
	this.container = jQuery(container);
	this.structureUrl = structureUrl;
	this.nodeClickCallback = nodeClickCallback;	
}

extend(ClickableTreeView, BaseTreeView);

ClickableTreeView.prototype.init = function ()
{	
	ClickableTreeView.superclass.init.apply(this, arguments)

	jQuery('.NodeContent a.nameLink').live('click', this, function (eventData)
	{
		var treeModel = eventData.data;
		var nodeEl = jQuery(this);
		treeModel.nodeClickCallback(treeModel.getStructItemID(this));
	});
}

ClickableTreeView.prototype.renderNodeContent = function (dataItem)
{	
	var tags = [];
	if (this.nodeClickCallback != null && dataItem.CanClick)
		tags.push('<a class="nameLink" href="javascript:void(0)">' + dataItem.Name + '</a>');
	else
		tags.push(dataItem.Name);
	return tags.join(' ');
}
