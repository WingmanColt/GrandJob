$(function (id) {
    $(id).autocomplete({

        source: function (request, response) {

            $.ajax({
                url: '/MessageApi/SelectRecruiter',
                type: 'GET',
                cache: false,
                data: request,
                dataType: 'json',
                success: function (data) {
                    response($.map(data, function (item) {

                        return {
                            label: item.firstname + ' ' + item.lastname + ' ',
                            value: item.id,
                            image: item.picture
                        }
                    }))
                }
            });
        },


        focus: function (event, ui) {
            $(id).val(ui.item.label);

            return false;
        },

        select: function (event, ui) {
            $(id).val(ui.item.label);
            $(id + 'Hidden').val(ui.item.value);
            return false;
        }
    }).data("ui-autocomplete")._renderItem = function (ul, item) {

        var $div = $("<div></div>");
        $("<img style='height:76px;'>").attr("src", item.image).appendTo($div);
        $("<span></span>").text(item.label).appendTo($div);

        return $("<li></li>").append($div).appendTo(ul);
    };
});