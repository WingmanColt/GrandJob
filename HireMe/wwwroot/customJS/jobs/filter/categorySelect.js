   $(document).ready(function () {
            $(".selectCategory").select2({
                placeholder: "Търси по категория",
                allowClear: true,
                theme: "bootstrap",
                closeOnSelect: true,
                maximumSelectionLength: 5,
                minimumInputLength: 0,
                multiple: false,
                ajax: {
                    url: "/FeaturesApi/GetCategories",
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
        $(".selectCategory").on("change", function () {
            var catId = $(this).val();
            $("#selectCategory_Value").val(catId);


            var textBoxValueData = $("#selectCategory_Value").val();
            $.ajax({
                url: '/Contestants/Index?CategoryId=' + textBoxValueData,
                dataType: 'json',
                type: 'post',
            });
        });