$(document).ready(function () {

    var wizard = $("form#conForm"); // cache the form element selector
    var current_fs, next_fs;
    var left, opacity, scale; //fieldset properties which we will animate
    var animating; //flag to prevent quick multi-click glitches
    var count_fs = 0;

    $(".next").click(function ()
    {
        tinymce.triggerSave();

        var editorContent = tinymce.activeEditor.getContent();

        var body = tinymce.get("mytextarea").getBody();
        var contentCount = tinymce.trim(body.innerText || body.textContent).length;

        if (count_fs == 2 && editorContent == '' || editorContent == null) {
            if (!$('#tinyMsg3').length) {
                $('<span class="text-danger" id="tinyMsg3">Това поле е задължително.</span><br>').insertAfter("#tinyMsg");
            }
            wizard.valid() = false;
        }
        if (count_fs == 2 && contentCount < 20 || contentCount > 10000) {
            // Add error message if not already present
            if (!$('#tinyMsg2').length) {
                $('<span class="text-danger" id="tinyMsg2">Минималният брой символи е 20, а максималният 10000. </span><br>').insertAfter("#tinyMsg");
            }
            wizard.valid() = false;
        }
        if (!wizard.valid()) { // validate the form
            wizard.validate().focusInvalid(); //focus the invalid fields
        }

        else {

            if (animating) return false;
            animating = true;

            current_fs = $(this).parent();
            next_fs = $(this).parent().next();
            $(".step-indicator li").eq($("fieldset").index(current_fs)).addClass("complete");

            //activate next step on progressbar using the index of next_fs
            $(".step-indicator li").eq($("fieldset").index(next_fs)).addClass("active");

            //show the next fieldset
            next_fs.show();
            count_fs = count_fs + 1; // counter to detect tinymce fieldset
            window.scrollTo(0, 0); // scroll to top

            //hide the current fieldset with style
            current_fs.animate({ opacity: 0 }, {
                step: function (now, mx) {

                    //as the opacity of current_fs reduces to 0 - stored in "now"
                    //1. scale current_fs down to 80%
                    scale = 1 - (1 - now) * 0.2;
                    //2. bring next_fs from the right(50%)
                    left = (now * 50) + "%";
                    //3. increase opacity of next_fs to 1 as it moves in
                    opacity = 1 - now;
                    current_fs.css({
                        'transform': 'scale(' + scale + ')',
                        'position': 'absolute'
                    });
                    next_fs.css({ 'left': left, 'opacity': opacity });

                },
                duration: 1000,
                complete: function () {
                    current_fs.hide();
                    animating = false;
                }
            });
        };
    });


});