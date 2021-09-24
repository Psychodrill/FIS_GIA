var $ = require('jquery'),
    _ = require('underscore'),
    chosen = require('chosen'),
    regionsJSON = require('./../../Config/regions.json');

var fillRegions = function(resp) {

    var select = $('#region');

    _.each(resp, function(item) {
        select.append($('<option>', {
            value: item.Id,
            text: _.findWhere(regionsJSON, { REGION: item.Id }).RegionName
        }));
    });

    select.trigger('chosen:updated');
};

module.exports = fillRegions;