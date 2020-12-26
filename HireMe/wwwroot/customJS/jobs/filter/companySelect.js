   $(document).ready(function () {
            $(".selectCompany").select2({
                placeholder: "Всички",
                allowClear: true,
                theme: "bootstrap",
                closeOnSelect: true,
                maximumSelectionLength: 1,
                minimumInputLength: 2,
                multiple: false,
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

            $('#form').change(function () {
                $(".selectCompany").submit();
            });

    var textBoxValueData = $("#selectCompany_Value").val();
            $.ajax({
                url: '/Jobs/Index?CompanyId=' + textBoxValueData,
                dataType: 'json',
                type: 'post',
            });
        });