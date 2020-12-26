   $(document).ready(function () {
            $(".selectLang").select2({
                placeholder: "Няма",
                allowClear: true,
                theme: "bootstrap",
                closeOnSelect: true,
                maximumSelectionLength: 10,
                minimumInputLength: 3,
                multiple: true,
                ajax: {
                    url: "/FeaturesApi/GetLanguages",
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
        $(".selectLang").on("change", function () {
            var catId = $(this).val();
            $("#selectLang_Value").val(catId);


            var textBoxValueData = $("#selectLang_Value").val();
            $.ajax({
                url: '/Contestants/Index?LanguageId=' + textBoxValueData,
                dataType: 'json',
                type: 'post',
            });
        });