//for sort array
//reverse = 'asc' || 'desc'
function _sortByKey(array, key, reverse) {
    return array.sort(function (a, b) {
        var x = a[key] == null ? '' : a[key].toString().toLowerCase();
        var y = b[key] == null ? '' : b[key].toString().toLowerCase();
        if (!reverse || reverse === 'asc') {
            return ((x < y) ? -1 : ((x > y) ? 1 : 0));
        }
        if (reverse === 'desc') {
            return ((x > y) ? -1 : ((x < y) ? 1 : 0));
        }
    });
};

//filter array
function _filterByKey(array, key, value) {
    if (!key || !value) {
        return array;
    }
    var _array = [];
    for (var i = 0; i < array.length; i++) {
        var item = array[i];
        var e = item[key] == null ? '' : item[key] && item[key].toString().toLowerCase();
        var v = value == null ? '' : value && value.toString().toLowerCase();
        if (e.indexOf(v) > -1) {
            _array.push(item);
        }
    }
    return _array;
};

//merge two arrays
//function arrayUnique(array) {
//    var a = array.concat();
//    for (var i = 0; i < a.length; ++i) {
//        for (var j = i + 1; j < a.length; ++j) {
//            if (a[i] === a[j])
//                a.splice(j--, 1);
//        }
//    }
//    return a;
//}

function _findByKey(array, key, value) {
    var item = {};
    for (var i = 0; i < array.length; i++) {
        var e = array[i];
        if (e[key] === value) {
            item = e;
            break;
        }
    }
    return item;
};

//
// message based on vue-toast.js
//

//error
function _errorToast(text) {
    Vue.toast(
    text + '&nbsp&nbsp',
    {
        className: ['toast-info'],
        duration: 10000,
        horizontalPosition: 'center',
        mode: 'override',
        closeable: 'true'
    })
}
//success
function _successToast(text) {
    Vue.toast(
    text,
    {
        className: ['toast-info'],
        duration: 3000,
        horizontalPosition: 'center',
        mode: 'override'
    })
}

//template for modal dialog
var modalTemplate =
'<transition name="modal">'
+ '<div class="modal-mask">'
    + ' <div class="modal-wrapper">'
    + '  <div class="modal-container">'
        + '  <div class="modal-header">'
        + '   <slot name="header">'
        + '   </slot>'
        + ' </div>'
        + '<div class="modal-body">'
        + '  <slot name="body">'
        + ' </slot>'
        + '</div>'
        + '<div class="modal-footer">'
        + ' <slot name="footer">'
            + '  default footer'
            + '  <button class="modal-default-button" @click="$emit("close")">'
                + 'close'
            + ' </button>'
            + '</slot>'
        + ' </div>'
    + ' </div>'
    + ' </div>'
    + '</div>'
+ '</transition>'


var datepicker = {
    props: ['value'],
    template: '<input type="text" class="shortInput datePicker" autocomplete="off" maxlength="10"\
            ref="input" \
            v-model="value" \
            v-bind:value="value | formatDate" />',

    mounted: function () {
        // activate the plugin when the component is mounted.
        $(this.$el).datepicker({
            changeMonth: true,
            changeYear: true,
            showOn: "both",
            buttonImage: '/Resources/Images/calendar.jpg',
            buttonImageOnly: true,
            yearRange: '-40:+0',

            //dateFormat: 'yy-mm-dd',
            //minDate: new Date(),
            //maxDate: "+89d",
            onClose: this.onClose
        })
    },
    beforeDestroy: function() {
        $(this.$el).datepicker('hide').datepicker('destroy');
    },

    methods: {
        // callback for when the selector popup is closed.
        onClose: function (date) {
            this.$emit('input', date);
        }
    },
    watch: {
        // when the value fo the input is changed from the parent,
        // the value prop will update, and we pass that updated value to the plugin.
        value: function (newVal) {
            $(this.el).datepicker('setDate', newVal);
        }
    }
};
