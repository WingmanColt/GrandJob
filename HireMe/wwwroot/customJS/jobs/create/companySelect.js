   $(document).ready(function () {
            $(".selectCompany").select2({
                placeholder: "Свързани",
                theme: "bootstrap",
                ajax: {
                    url: "/FeaturesApi/GetCompanies",
                    dataType: 'json',
                    type: 'GET',
                    delay: 250,
                    data: function (params) {
                        return {
                            term: params.term
                        };
                    },
                    processResults: function (data, params) {
                        return {
                            results: data
                        };
                    }
                }
            });

   });

$(".selectCompany").on("change", function () {
    var catId = $(this).val();
    $("#selectCompany_Value").val(catId);
});