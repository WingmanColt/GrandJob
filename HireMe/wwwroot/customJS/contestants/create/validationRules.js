$(document).ready(function () {
    $("form#conForm").validate({

        validClass: "success",
        rules: {
            fullname: {
                required: true,
                rangelength: [10, 50]
            },
            about: {
                required: true,
                rangelength: [10, 100]
            },
            speciality: {
                required: true,
                rangelength: [3, 20]
            },
            age: {
                required: true,
                date: true
            },
            payrate: {
                required: true,
                max: 99999
            },
        },
        highlight: function (element) {
            $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
        },
        success: function (element) {
            $(element).closest('.form-group').removeClass('has-error').addClass('has-success');
        },
    });

});