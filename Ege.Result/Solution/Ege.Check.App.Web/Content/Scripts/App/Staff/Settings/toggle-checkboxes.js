var $ = require('jquery');

var toggle = function(target, check) {
    var table = target.parents('table'),
        row = target.parents('tr'),
        cell = target.parents('th'),
        index = row.find('th').index(cell);

    table.find('tbody tr').each(function() {
        $(this)
            .find('td')
            .eq(index)
            .find('input[type="checkbox"]')
            .prop('checked', check)
            .trigger('change');
    });
};

module.exports = toggle;