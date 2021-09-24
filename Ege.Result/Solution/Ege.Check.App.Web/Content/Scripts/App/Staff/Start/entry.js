(function() {

    var $ = require('jquery'),
        login = require('./login.js'),
        keyboard = require('../../Config/keyboard.js');

    $('#submit').on('click', login);
    $('#entry-form').on('keypress', function(e) {
        if (e.keyCode == keyboard.ENTER) {
            login();
        }
    });

})();