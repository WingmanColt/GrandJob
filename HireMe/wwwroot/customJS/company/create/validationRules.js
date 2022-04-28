$(document).ready(function () {
    var eikVar = $("#eik").val();
    $("form#companyForm").validate({

        validClass: "success",
        rules: {
            title: {
                required: true,
                rangelength: [10, 50]
            },
            about: {
                required: true,
                rangelength: [10, 100]
            },
            email: {
                required: true,
                email: true
            },
            website: {
                url: true
            },
            adress: {
                required: true,
                rangelength: [5, 50]
            },
            phonenumber: {
                required: true,
                digits: true
            },
            eik: {
                required: true,
                remote: {
                    url: 'isEIKValid',
                    type: "POST",
                    data:
                    {                      
                        eik: function () {
                            return $('form#companyForm :input[name="eik"]').val();
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