$(document).ready(function () {
    $(".selectWork").select2({
        placeholder: "Всички",
        allowClear: true,
        theme: "bootstrap",
        closeOnSelect: true,
        maximumSelectionLength: 7,
        minimumInputLength: 0,
        multiple: true,
        ajax: {
            url: "/FeaturesApi/GetWork",
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

    $(".selectWork").on("change", function () {
        var catId = $(this).val();
        $("#selectWork_Value").val(catId);
    });
});