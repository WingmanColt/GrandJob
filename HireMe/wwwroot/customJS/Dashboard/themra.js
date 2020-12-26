"use strict";
function layoutsColors() {
    $(".sidebar").is("[data-background-color]") ? $("html").addClass("sidebar-color") : $("html").removeClass("sidebar-color"),
        $(".sidebar").is("[data-image]")
            ? ($(".sidebar").append("<div class='sidebar-background'></div>"), $(".sidebar-background").css("background-image", 'url("' + $(".sidebar").attr("data-image") + '")'))
            : ($(this).remove(".sidebar-background"), $(".sidebar-background").css("background-image", ""));
}
function legendClickCallback(e) {
    for (var a = (e = e || window.event).target || e.srcElement; "LI" !== a.nodeName;) a = a.parentElement;
    var s = a.parentElement,
        i = parseInt(s.classList[0].split("-")[0], 10),
        o = Chart.instances[i],
        n = Array.prototype.slice.call(s.children).indexOf(a);
    o.legend.options.onClick.call(o, e, o.legend.legendItems[n]), o.isDatasetVisible(n) ? a.classList.remove("hidden") : a.classList.add("hidden");
}
function readURL(e) {
    if (e.files && e.files[0]) {
        var a = new FileReader();
        (a.onload = function (a) {
            $(e).parent(".input-file-image").find(".img-upload-preview").attr("src", a.target.result);
        }),
            a.readAsDataURL(e.files[0]);
    }
}
/*
function showPassword(e) {
    var a = $(e).parent().find("input");
    "password" === a.attr("type") ? a.attr("type", "text") : a.attr("type", "password");
}*/

$(".nav-search .input-group > input")
    .focus(function (e) {
        $(this).parent().addClass("focus");
    })

    .blur(function (e) {
        $(this).parent().removeClass("focus");
    }),

    $(function () {
        $('[data-toggle="tooltip"]').tooltip(), $('[data-toggle="popover"]').popover(), layoutsColors();
    }),

    $(document).ready(function () {
        $(".btn-refresh-card").on("click", function () {
            var e = $(this).parents(".card");
            e.length &&
                (e.addClass("is-loading"),
                    setTimeout(function () {
                        e.removeClass("is-loading");
                    }, 3e3));
        });

        var e = $(".sidebar .scrollbar-inner");
        e.length > 0 && e.scrollbar();
        var a = $(".messages-scroll.scrollbar-outer");
        a.length > 0 && a.scrollbar();
        var s = $(".tasks-scroll.scrollbar-outer");
        s.length > 0 && s.scrollbar();
        var i = $(".quick-scroll");
        i.length > 0 && i.scrollbar();
        var o = $(".message-notif-scroll");
        o.length > 0 && o.scrollbar();
        var n = $(".notif-scroll");
        n.length > 0 && n.scrollbar(), $(".scroll-bar").draggable();
        var r,
            l = !1,
            t = !1,
            c = !1,
            d = !1,
            g = 0,
            m = 0,
            u = 0,
            h = 0,
            p = 0;
        l ||
            ((r = $(".sidenav-toggler")).on("click", function () {
                1 == g ? ($("html").removeClass("nav_open"), r.removeClass("toggled"), (g = 0)) : ($("html").addClass("nav_open"), r.addClass("toggled"), (g = 1));
            }),
                (l = !0));
        m ||
            ((r = $(".quick-sidebar-toggler")).on("click", function () {
                1 == g
                    ? ($("html").removeClass("quick_sidebar_open"), $(".quick-sidebar-overlay").remove(), r.removeClass("toggled"), (m = 0))
                    : ($("html").addClass("quick_sidebar_open"), r.addClass("toggled"), $('<div class="quick-sidebar-overlay"></div>').insertAfter(".quick-sidebar"), (m = 1));
            }),
                $(".wrapper").mouseup(function (e) {
                    var a = $(".quick-sidebar");
                    e.target.className == a.attr("class") || a.has(e.target).length || ($("html").removeClass("quick_sidebar_open"), $(".quick-sidebar-toggler").removeClass("toggled"), $(".quick-sidebar-overlay").remove(), (m = 0));
                }),
                $(".close-quick-sidebar").on("click", function () {
                    $("html").removeClass("quick_sidebar_open"), $(".quick-sidebar-toggler").removeClass("toggled"), $(".quick-sidebar-overlay").remove(), (m = 0);
                }),
                (m = !0));
        /*if (!t) {
            var b = $(".topbar-toggler");
            b.on("click", function () {
                1 == u ? ($("html").removeClass("topbar_open"), b.removeClass("toggled"), (u = 0)) : ($("html").addClass("topbar_open"), b.addClass("toggled"), (u = 1));
            }),
                (t = !0);
        }*/
        if (!c) {
            var v = $(".btn-minimize");
            $("html").hasClass("sidebar_minimize") && ((h = 1), v.addClass("toggled"), v.html('<i class="fa fa-ellipsis-v"></i>')),
                v.on("click", function () {
                    1 == h
                        ? ($("html").removeClass("sidebar_minimize"), v.removeClass("toggled"), v.html('<i class="fa fa-bars"></i>'), (h = 0))
                        : ($("html").addClass("sidebar_minimize"), v.addClass("toggled"), v.html('<i class="fa fa-ellipsis-v"></i>'), (h = 1)),
                        $(window).resize();
                }),
                (c = !0);
        }
        if (!d) {
            var f = $(".page-sidebar-toggler");
            f.on("click", function () {
                1 == p ? ($("html").removeClass("pagesidebar_open"), f.removeClass("toggled"), (p = 0)) : ($("html").addClass("pagesidebar_open"), f.addClass("toggled"), (p = 1));
            }),
                $(".page-sidebar .back").on("click", function () {
                    $("html").removeClass("pagesidebar_open"), f.removeClass("toggled"), (p = 0);
                }),
                (d = !0);
        }
        $(".sidebar").hover(
            function () {
                $("html").hasClass("sidebar_minimize") && $("html").addClass("sidebar_minimize_hover");
            },
            function () {
                $("html").hasClass("sidebar_minimize") && $("html").removeClass("sidebar_minimize_hover");
            }
        ),
            $(".nav-item a").on("click", function () {
                $(this).parent().find(".collapse").hasClass("show") ? $(this).parent().removeClass("submenu") : $(this).parent().addClass("submenu");
            }),
            /*
            $(".messages-contact .user a").on("click", function () {
                $(".tab-chat").addClass("show-chat");
            }),
            $(".messages-wrapper .return").on("click", function () {
                $(".tab-chat").removeClass("show-chat");
            }),
            */
            $('[data-select="checkbox"]').change(function () {
                var e = $(this).attr("data-target");
                $(e).prop("checked", $(this).prop("checked"));
            }),
            $(".form-group-default .form-control")
                .focus(function () {
                    $(this).parent().addClass("active");
                })
                .blur(function () {
                    $(this).parent().removeClass("active");
                });
    }),
    $('.input-file-image input[type="file"').change(function () {
        readURL(this);
    }),
    $(".show-password").on("click", function () {
        showPassword(this);
    });
var containerSignIn = $(".container-login"),
    containerSignUp = $(".container-signup"),
    showSignIn = !0,
    showSignUp = !1;
function changeContainer() {
    1 == showSignIn ? containerSignIn.css("display", "block") : containerSignIn.css("display", "none"), 1 == showSignUp ? containerSignUp.css("display", "block") : containerSignUp.css("display", "none");
}
/*
$("#show-signup").on("click", function () {
    (showSignUp = !0), (showSignIn = !1), changeContainer();
}),
    $("#show-signin").on("click", function () {
        (showSignUp = !1), (showSignIn = !0), changeContainer();
    }),*/
    changeContainer(),
    $(".form-floating-label .form-control").keyup(function () {
        "" !== $(this).val() ? $(this).addClass("filled") : $(this).removeClass("filled");
    });
