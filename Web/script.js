$(function () {
    'use strict';

    var tts = TTS();

    var start = function (event) {
        tts.text($('#text').val());
        tts.position(0);
        tts.start();
    };

    var stop = function () {
        tts.stop();
    };

    var read = function () {
        if (tts.active()) {
            stop();
        } else {
            start();
        }
    };

    $('#read').on('click', read);
});
