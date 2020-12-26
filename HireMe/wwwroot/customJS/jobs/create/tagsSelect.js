 $(document).ready(function () {
            $(".selectTags").select2({
                placeholder: "Ключови думи",
                allowClear: true,
                theme: "bootstrap",
                closeOnSelect: true,
                maximumSelectionLength: 5,
                minimumInputLength: 2,
                multiple: true,
                ajax: {
                    url: "/FeaturesApi/GetTags",
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
        $(".selectTags").on("change", function () {
            var catId = $(this).val();
            $("#selectTags_Value").val(catId);
        });
