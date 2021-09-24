// Скрипт подтверждения изменений на странице

var CONFIRMATION_MESSAGE = 'Измененные данные не сохранены.\nОтменить изменения?';
var STATE_ELEMENT_NAME = 'state';
var isDataChanged = false; 

// Инициализация 
function InitConfirmation(containerId, stateValue)
{ try {
  var parent = document.getElementById(containerId);
  if (!parent)
    parent = document;

  if (stateValue)
  { 
    isDataChanged = stateValue;
    var state = document.getElementById(STATE_ELEMENT_NAME);
    if (state) 
      state.value = isDataChanged;  
  }  

  list = CollectVariationControls(parent);
  for (var i=0; i<list.length; i++)
    addHandler(list[i], 'change', changeHandler);
    
  list = CollectClickControls(parent);
  for (var i=0; i<list.length; i++)
    addHandler(list[i], 'click', clickHandler);

  list = CollectChangeControls(parent);
  for (var i=0; i<list.length; i++)
    addHandler(list[i], 'change', clickHandler);

} catch (ex) {alert(ex.message)} }

function CollectVariationControls(parent)
{
  var collection = new Array();

  // все селекты
  var list = parent.getElementsByTagName('SELECT');
  for (var i=0; i<list.length; i++)
    if (!list[i].getAttribute('handleVariation'))
      collection.push(list[i]);
  
  // все текстовые поля
  var list = parent.getElementsByTagName('INPUT');
  for (var i=0; i<list.length; i++)
    if (list[i].type == 'text' && !list[i].getAttribute('handleVariation'))
      collection.push(list[i]);

  // все чекбоксы
  var list = parent.getElementsByTagName('INPUT');
  for (var i=0; i<list.length; i++)
    if (list[i].type == 'checkbox' && !list[i].getAttribute('handleVariation'))
      collection.push(list[i]);

  // все текстовые области
  var list = parent.getElementsByTagName('TEXTAREA');
  for (var i=0; i<list.length; i++)
    if (!list[i].getAttribute('handleVariation'))
      collection.push(list[i]);

  return collection;
}

function CollectClickControls(parent)
{
  var collection = new Array();

  // все элементы ссылки
  var list = parent.getElementsByTagName('A');
  for (var i=0; i<list.length; i++)
    if (!list[i].getAttribute('handleClick'))
      collection.push(list[i]);

  // все элементы ссылки
  var list = parent.getElementsByTagName('BUTTON');
  for (var i=0; i<list.length; i++)
    if (!list[i].getAttribute('handleClick'))
      collection.push(list[i]);
      
  // все кнопки
  var list = parent.getElementsByTagName('INPUT');
  for (var i=0; i<list.length; i++)
    if ( ( list[i].type == 'button' || list[i].type == 'submit' ) 
        && !list[i].getAttribute('handleClick'))
      collection.push(list[i]);

  return collection;
}

function CollectChangeControls(parent)
{
  var collection = new Array();

  // все селекты с атрибутом handleClick="true"
  var list = parent.getElementsByTagName('SELECT');
  for (var i=0; i<list.length; i++)
    if (list[i].getAttribute('handleChange'))
    { // очищаем атрибут onchange. т.к. иначе это события выполняется раньше нашего.
      collection.push(list[i]);
      list[i].setAttribute('onchange', '');
    }
 
  return collection;
}

function addHandler(object, event, handler)
{
  if (typeof object.addEventListener != 'undefined')
    object.addEventListener(event, handler, false);
  else if (typeof object.attachEvent != 'undefined')
    object.attachEvent('on' + event, handler);
  else
    throw 'Incompatible browser';
}

function clickHandler(e)
{
  // значение менялось
  if (isDataChanged && !confirm(CONFIRMATION_MESSAGE))
  {
    if(e.preventDefault) e.preventDefault(); else e.returnValue = false;
    return false;
  }

  // Обработаем переопределенные события
  // ie и opera  
  if (e.srcElement && e.srcElement.getAttribute('handleChange'))
  {
    eval(e.srcElement.getAttribute('handleChange'));
  }
  // firefox 
  else if (e.target && e.target.getAttribute('handleChange'))
  {
    eval(e.target.getAttribute('handleChange'));
  }
}

function changeHandler(e)
{
  isDataChanged = true;

  var state = document.getElementById(STATE_ELEMENT_NAME);
  if (state) 
    state.value = isDataChanged;  
}