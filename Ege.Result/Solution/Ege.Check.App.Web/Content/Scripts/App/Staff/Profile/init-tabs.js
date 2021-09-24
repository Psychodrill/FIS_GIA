var $ = require('jquery'),
    mustache = require('mustache'),
    regionsJSON = require('../../Config/regions.json');

var tabsContainer = $('#tabs'),
    tabs = tabsContainer.children(),
    activeClass = 'current',
    tabContent = $('#tab-content'),
    tabOptions;

var loadTab = function(index, url) {
    var timeout = setTimeout(function() {
        tabContent.addClass('loading').find('form').remove();
    }, 200);

    $.get(url)
        .done(function(resp) {
            clearTimeout(timeout);
            tabContent.removeClass('loading').find('form, button').remove();
            tabContent.append(mustache.render($('#tab-' + index).text(), resp));
        });
};

var setTab = function(index) {
    tabsContainer.find('.' + activeClass).removeClass(activeClass);
    tabs.eq(index).addClass(activeClass);

    var opt = tabOptions[index];
    if (opt.chooseRegion) {
        tabContent.html(mustache.render($('#region-tmpl').text(), {
            regions: regionsJSON
        }));
        tabContent.off().on('change', '#regions-select', function(e) {
            loadTab(index, opt.dest + '/' + $(e.target).val());
        });
    } else if (opt.source) {
        loadTab(index, opt.dest);
    } else {
        tabContent.html(mustache.render($('#tab-' + index).text(), {}));
    }
};

var initTabs = function(o) {
    tabOptions = o;
    tabs.on('click', function(e) {
        var target = $(e.target);
        if (!target.hasClass(activeClass)) {
            setTab(tabs.index(target));
        }
    });

    return {
        setTab: setTab
    };
};

module.exports = initTabs;