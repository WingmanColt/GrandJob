"use strict";

$(document).ready(function () {
    var o = $(".message-notif-scroll");
    o.length > 0 && o.scrollbar();
    var n = $(".notif-scroll");
    n.length > 0 && n.scrollbar(), $(".scroll-bar");
    var r,
        g = 0,
        m = 0;

    m || ((r = $(".quick-sidebar-toggler")).on("click", function () {
        1 == g
            ? ($("html").removeClass("quick_sidebar_open"), $(".quick-sidebar-overlay").remove(), r.removeClass("toggled"), (m = 0))
            : ($("html").addClass("quick_sidebar_open"), r.addClass("toggled"), $('<div class="quick-sidebar-overlay"></div>').insertAfter(".quick-sidebar"), (m = 1));
    }),
        $(".app").mouseup(function (e) {
            var a = $(".quick-sidebar");
            e.target.className == a.attr("class") || a.has(e.target).length || ($("html").removeClass("quick_sidebar_open"), $(".quick-sidebar-toggler").removeClass("toggled"), $(".quick-sidebar-overlay").remove(), (m = 0));
        }),
        $(".close-quick-sidebar").on("click", function () {
            $("html").removeClass("quick_sidebar_open"), $(".quick-sidebar-toggler").removeClass("toggled"), $(".quick-sidebar-overlay").remove(), (m = 0);
        }),
        (m = !0));
});

/*
$(document).ready(function () {

    @{
        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        var json = serializer.Serialize(Model);
    }

    var model = @Html.Raw(json);
    if (model != null && @Html.Raw(json) != "undefined")
{
    var id = model.Id;
    var mainFloorPlanId = model.MainFloorPlanId;
    var imageDirectory = model.ImageDirectory;
    var iconsDirectory = model.IconsDirectory;
}*/

