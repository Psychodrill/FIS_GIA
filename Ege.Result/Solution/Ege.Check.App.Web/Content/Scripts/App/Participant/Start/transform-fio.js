var simplify = function (s, n, p) {
    s = s || '';
    n = n || '';
    p = p || '';
    return (s + n + p)
        .toLowerCase()
        .replace(/[^a-zA-Zа-яА-ЯЁё]+/g, '')
        .replace(/ё/g, 'е')
        .replace(/й/g, 'и');
};

var getShortForm = function (s, n, p) {
    s = s || '';
    n = n || '';
    return s.charAt(0).toUpperCase() +
        s.slice(1) +
        ' ' +
        n.charAt(0).toUpperCase() +
        '.' +
        (p ? (p.charAt(0).toUpperCase() +
        '.') : '');
};

module.exports = {
    simplify: simplify,
    getShortForm: getShortForm
};