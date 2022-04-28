$(document).ready(function () {
    $("form#registerForm").validate({

        validClass: "success",
        rules: {
            password: {
                required: true,
                rangelength: [10, 50]
            },
            confirmpassword: {
                required: true,
                equalTo: "#password"
            },
            email: {
                required: true,
                email: true,
                remote: {
                    url: '/UsersApi/isEmailAvaliable',
                    type: "POST",
                    data:
                    {
                        term: function () {
                            return $('form#registerForm :input[name="email"]').val();
                        }
                    }
                }
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