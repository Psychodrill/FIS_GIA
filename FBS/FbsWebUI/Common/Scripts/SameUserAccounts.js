// Скрипт поиска повторов пользователей

var SearchButtonId;
var ResultsPanelId;
var ResultsInfoId = 'sameUserAccountsResultInfo';

function InitSameUserAccounts(searchButtonId, resultsPanelId)
{
    SearchButtonId = searchButtonId;
    ResultsPanelId = resultsPanelId;

    // Покажу кнопку поиска повторов
    if (SearchButtonId)
        $get(SearchButtonId).style.display=''; 
    
    // Сообщу серверу что клиентский браузер умеет javascript
    $get('jsCheck').value='1';
}

function ShowSameUserAccountsResultPanel(canShow)
{
    if (canShow)
        $get(ResultsPanelId).style.display='';
    else
        $get(ResultsPanelId).style.display='none';
} 

function ToggleSameUserAccountsResultInfoState(me)
{
    var resultInfo = $get(ResultsInfoId); 
    if (resultInfo.style.display == 'none')
    {
        me.innerHTML = me.getAttribute('hideText');
        resultInfo.style.display = '';
    }
    else
    {
        me.innerHTML = me.getAttribute('showText');;
        resultInfo.style.display = 'none';
    }
}