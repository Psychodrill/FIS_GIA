///////////////////////////////////////////// Константы ////////////////////////////////////////////

// Интервал проверки статуса (в миллисекундах)
updatePeriod = 250;  

// Интервал времени, по истечении которого считается что закачка подвисла (в миллисекундах)
expirePeriod = 60000; // 2 минуты

uploadStateUrl = '/Profile/UploadState.ashx?PostID=' + postID;

// Url страницы с "простой" формой закачки документа
simpleDocumentUploadUrl = '/Profile/DocumentUpload.aspx?Simple=1';

////////////////////////////////////////////////////////////////////////////////////////////////////

// Текущее количество попыток
var retryCount = 0; 
// Максимально допустимое количество попыток
var maxRetryCount = Math.round(expirePeriod / updatePeriod); 
// Предыдущее значение переданного количества байт
var prevTransBytes = 0;

var timer = null;
var request = null;
var ignoreWarning = false;

function CheckForm(){
    if ($(fileUpload).value.length > 0){
        clearError();
        return true;
    }
    showError('Файл не задан');
    $(fileUpload).focus();
    return false;
}

function doPost(){
    if (CheckForm()) {
        request = new JsHttpRequest();
        
        request.open(null, $(formUpload).action + '?PostID=' + postID + 
            '&FileName=' + $(fileUpload).value + (ignoreWarning ? '&ignore=1': ''), true);
        request.send({file: $(fileUpload)});

        $(formUpload).hide();
        $('TransferInfo').show();
        
        getTransferInfo();
    }
}

function getTransferInfo(){
    clearTimeout(timer);
    new Ajax.Request(uploadStateUrl, {
        method: 'get',
        onSuccess: parseTransferInfo
    });
}

function parseTransferInfo(transport){
    var status = transport.responseText.evalJSON();
    if (status.errorLevel){
        request.abort();
        if (status.errorLevel == 2 && (ignoreWarning = confirm(status.errorMsg + 'Продолжить?'))){
            doPost();
        } else if (status.errorLevel == 4){
            terminateTransfer();
        } else {
            resetForm();
            showError(status.errorMsg);
        }
    } else {
        // Сравню текущее значение переданных байт с количеством переденных байт во время 
        // предыдущего опроса сервера
        if (!status.transBytes || status.transBytes == prevTransBytes){
            // Увеличу счетчик
            retryCount++; 
        } else {
            // Сброшу счетчик
            retryCount = 0;   
            prevTransBytes = status.transBytes;
        }
    
        // Если в течении maxRetryCount попыток данные не изменились, то считаем что произошла 
        // ошибка и перекинем пользователя на страницу "простой" загрузки документа.
        if (retryCount >= maxRetryCount)
            terminateTransfer();
    
        if (status.totalBytes > 0){
            var percentage = Math.round(status.transBytes / status.totalBytes * 100) + "%";
            $('TransferPercentage').innerHTML = percentage +
                ' (' + Math.round(status.transBytes / 1024) + 'kb' +
                ' / ' + Math.round(status.totalBytes / 1024) + 'kb)';
            $('ProgressBar').style.width = percentage; 
            
            if (status.transBytes == status.totalBytes){
                    clearTimeout(timer);
                    ignoreWarning = false;
                    window.location = redirUrl;
                    return;
            }
        }
        timer = setTimeout("getTransferInfo()", updatePeriod);
    }
}

// Отмена загрузки (по инициативе пользователя)
function cancelTransfer(){
    clearTimeout(timer);
    timer = null;
    request.abort();
    new Ajax.Request(uploadStateUrl + '&cancel=1', {
        method: 'get',
        onSuccess:  resetForm
    });
}

// Принудительное завершение загрузки
function terminateTransfer(){
    clearTimeout(timer);
    request.abort();
    request = timer = null;
    location.href = simpleDocumentUploadUrl;
}

function showError(message){
    $('ErrorMsg').innerHTML = message;
    $('ErrorsContainer').show();
}

function clearError(){
    $('ErrorsContainer').hide();
    $('ErrorMsg').innerHTML = '';
}

function resetForm(){
    $(formUpload).reset();
    $(formUpload).show();
    $('TransferInfo').hide();
    $('ProgressBar').style.width = "0";
    $('TransferPercentage').innerHTML = "0%";
}