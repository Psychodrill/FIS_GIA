var f = function(num) {
    return num < 10 ? '0' + num : num;
};

/**
 *  ����������� ������ yyyy-MM-dd � dd.MM.yyyy
 */
var dateToRuFormat = function(date) {
    var dateRegex = /^(\d\d\d\d)\-(\d\d)\-(\d\d)/;
    var match = dateRegex.exec(date);
    return [match[3], match[2], match[1]].join('.');
};

/**
 *  ����������� ������ yyyy-MM-ddTHH:mm:SS+OFF � dd.MM.yyyy
 */
var dateTimeToRuDateFormat = function(date) {
    date = new Date(date);
    return [f(date.getDate()), f(date.getMonth() + 1), date.getFullYear()].join('.');
};

/**
 *  ����������� ������ yyyy-MM-ddTHH:mm:SSoffset � dd.MM.yyyy HH:mm:SS �� ��������� ���� ��������
 */
var dateTimeToRuFormat = function(date) {
    date = new Date(date);
    return [f(date.getDate()), f(date.getMonth() + 1), date.getFullYear()].join('.') + ' ' +
    [f(date.getHours()), f(date.getMinutes()), f(date.getSeconds())].join(':');
};

module.exports = {
    dateToRuFormat: dateToRuFormat,
    dateTimeToRuDateFormat: dateTimeToRuDateFormat,
    dateTimeToRuFormat: dateTimeToRuFormat,
};
