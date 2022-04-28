﻿   $(document).ready(function () {
       $(".selectLocation").select2({
                placeholder: "Местоположение",
                allowClear: true,
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
});

$('.selectLocation').val($('.selectLocation option:first-child').val()).trigger('change');