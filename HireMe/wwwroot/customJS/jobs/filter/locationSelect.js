   $(document).ready(function () {
            $(".selectLocation").select2({
                placeholder: "Къде",
                allowClear: false,
                theme: "bootstrap",
                closeOnSelect: true,
                maximumSelectionLength: 1,
                minimumInputLength: 0,
                multiple: false,
                ajax: {
                    url: "/FeaturesApi/GetLocations",
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
$(".selectLocation").on("change", function () {
            var catId = $(this).val();
    $("#selectLocation_Value").val(catId);


    var textBoxValueData = $("#selectLocation_Value").val();
            $.ajax({
                url: '/Contestants/Index?LocationId=' + textBoxValueData,
                dataType: 'json',
                type: 'post',
            });
        });