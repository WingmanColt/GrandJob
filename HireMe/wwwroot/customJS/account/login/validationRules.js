$(document).ready(function () {
    $("form#loginForm").validate({

        validClass: "success",
        rules: {
            passwordlogin: {
                required: true
            },
            emaillogin: {
                required: true,
                email: true
               /* remote: {
                    url: '/UsersApi/isEmailAvaliableLogin',
                    type: "POST",
                    data:
                    {
                        term: function () {
                            return $('form#loginForm :input[name="emaillogin"]').val();
                        }
                    }
                }*/
            }
        },
        highlight: function (element) {
            $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
        },
        success: function (element) {
            $(element).closest('.form-group').removeClass('has-error').addClass('has-success');
        },
    });

});