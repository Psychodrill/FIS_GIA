var getLastPart = function() {
    var parts = location.pathname.split('/'),
        count = parts.length;
    return parts[count - 1];
};

module.exports = {
    getLastPart: getLastPart
};