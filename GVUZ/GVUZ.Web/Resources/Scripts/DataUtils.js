// class wrapper for object which have standart fields ItemID, Name, IsLeaf and 
// parameter DisplayArray which contains values for display.
function TreeData(paramTreeData)
{
	this.object = paramTreeData;
	this.displayArray = paramTreeData.DisplayArray;
}

// use to format server json data as array for displaying in html
// replace null, undefined or empty string on &nbsp
// replace true and false on russian equivalent.
// return new array with formatted value properties
TreeData.prototype.formatArray = function ()
{
	var result = [];
	for (var i = 0; i < this.displayArray.length; i++)
	{
		var propValue = this.displayArray[i];
		if (typeof (propValue) == 'function') continue;
		if (propValue == 'undefined' || propValue == null || (propValue == '' && (typeof propValue != 'boolean')))
			result.push('&nbsp;');
		else if (typeof propValue == 'boolean') {
			if (propValue == true) result.push('Есть');
			else if (propValue == false) result.push('Нет');
			else result.push(propValue);
		} 
		else result.push(propValue);
	}

	return result;
}
