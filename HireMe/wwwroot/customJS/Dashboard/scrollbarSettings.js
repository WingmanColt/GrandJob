"use strict";

$(document).ready(function () {
    var o = $(".message-notif-scroll");
    o.length > 0 && o.scrollbar();
    var n = $(".notif-scroll");
    n.length > 0 && n.scrollbar(), $(".scroll-bar");
    var r,
        g = 0,
        m = 0,
        f = 0,
        t = 0;

    m || ((r = $(".quick-sidebar-toggler")).on("click", function () {
        1 == g
            ? ($("html").removeClass("quick_sidebar_open"), $(".quick-sidebar-overlay").remove(), r.removeClass("toggled"), (m = 0))
            : ($("html").addClass("quick_sidebar_open"), r.addClass("toggled"), $('<div class="quick-sidebar-overlay"></div>').insertAfter(".quick-sidebar"), (m = 1));
    }),
        f || ((r = $(".filter-sidebar-toggler")).on("click", function () {
            1 == t
                ? ($("html").removeClass("filter_sidebar_open"), $(".filter-sidebar-overlay").remove(), r.removeClass("toggled"), (f = 0))
                : ($("html").addClass("filter_sidebar_open"), r.addClass("toggled"), $('<div class="filter-sidebar-overlay"></div>').insertAfter(".filter-sidebar"), (f = 1));
        }),
        $(".app").mouseup(function (e) {
            var a = $(".quick-sidebar");
            e.target.className == a.attr("class") || a.has(e.target).length || ($("html").removeClass("quick_sidebar_open"), $(".quick-sidebar-toggler").removeClass("toggled"), $(".quick-sidebar-overlay").remove(), (m = 0));
            var b = $(".filter-sidebar");
            e.target.className == b.attr("class") || b.has(e.target).length || ($("html").removeClass("filter_sidebar_open"), $(".filter-sidebar-toggler").removeClass("toggled"), $(".filter-sidebar-overlay").remove(), (f = 0));
        }),
        $(".close-quick-sidebar").on("click", function () {
            $("html").removeClass("quick_sidebar_open"), $(".quick-sidebar-toggler").removeClass("toggled"), $(".quick-sidebar-overlay").remove(), (m = 0);
        }),
        $(".close-filter-sidebar").on("click", function () {
            $("html").removeClass("filter_sidebar_open"), $(".filter-sidebar-toggler").removeClass("toggled"), $(".filter-sidebar-overlay").remove(), (f = 0);
        }),
            (m = !0)),
            (f = !0));
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

