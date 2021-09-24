// bind used for later calling method in context of specified object and with specified arguments
Function.prototype.bind = function ()
{
	if (arguments.length < 1 && typeof arguments[0] != "undefined") return this;
	var __method = this, args = [];
	for (var i = 0; i < arguments.length; i++) { args.push(arguments[i]); }

	var object = args.shift();
	return function ()
	{
		var args_to_apply = []

		for (var i = 0; i < args.length; i++) { args_to_apply.push(args[i]); }
		for (var i = 0; i < arguments.length; i++) { args_to_apply.push(arguments[i]); }
		return __method.apply(object, args_to_apply);
	}
};

// use for oop 
function extend(Child, Parent)
{
	var F = function () { }
	F.prototype = Parent.prototype
	Child.prototype = new F()
	Child.prototype.constructor = Child
	Child.superclass = Parent.prototype
}

// navigation method
function navigateTo(path)
{
	window.location.href = path;
}

function isArrayNotEmpty(arr)
{
	return arr != null && arr instanceof Array && arr.length > 0;
}

function isStringNullOrEmpty(str)
{
	return str == null || (typeof(str) == 'string' && str.length == 0);
}

function isFunction(func)
{
	return func != null && func instanceof Function;
}

// add option to select
function addOption(selectID, text, value)
{	
	var selectObj = document.getElementById(selectID);
	var option = document.createElement("OPTION")	
	option.text = text
	option.value = value
	try
	{
		selectObj.add(option, null);
	}
	catch(ex)
	{
		selectObj.add(option); // IE only
	}
}

// remove option from select
function removeOption(selectID, index)
{
	var selectObj = document.getElementById(selectID);
	try
	{
		selectObj.options.remove(index);
	}
	catch (ex)
	{
		selectObj.removeChild(selectObj.options[index]); // FF only
	}
}

// get options array in <select>
function getOptionValues(selectID)
{
	var result = [];
	var selectObj = document.getElementById(selectID);

	for (var i = 0; i < selectObj.options.length; i++)		
		result.push(selectObj.options[i].value);

	return result;
}


// get selected options array in <select>
function getSelectedOptionValues(selectID)
{
	var result = [];
	var selectObj = document.getElementById(selectID);

	for (var i = 0; i < selectObj.options.length; i++)
		if (selectObj.options[i].selected)
			result.push(selectObj.options[i].value);

	return result;
}

// get selected options array in <select>
function getSelectedOptions(selectID)
{
	var result = [];
	var selectObj = document.getElementById(selectID);

	for (var i = 0; i < selectObj.options.length; i++)
		if (selectObj.options[i].selected)
			result.push(selectObj.options[i]);

	return result;
}


function moveSelectedOptions(srcSelectID, dstSelectID)
{
	var selValues = getSelectedOptions(srcSelectID);
	for (var i = 0; i < selValues.length; i++)
	{
		addOption(dstSelectID, selValues[i].innerHTML, selValues[i].value);
		removeOption(srcSelectID, selValues[i].index);
	}
}

function validateDate(value, min, max) {
    var curDate = new Date();
    min = new Date(curDate.getFullYear() - 100, curDate.getMonth(), curDate.getDate());
    max=curDate;
    try{
        var arrD = value.split(".");
        if (arrD.length != 3) { return false; }
        if (isNaN(arrD[0])) { return false; }
        if (isNaN(arrD[1])) { return false; }
        if (isNaN(arrD[2])) { return false; }
        arrD[1] -= 1;
        var d = new Date(arrD[2], arrD[1], arrD[0]);
        if ((d.getFullYear() == arrD[2]) && (d.getMonth() == arrD[1]) && (d.getDate() == arrD[0])) {
            if (d > max || d < min) { return false; }
            return d;
        } else {        
            return false; // alert("Введена некорректная дата!");
        }    
        
    }catch(e){
        return false;
    }
}