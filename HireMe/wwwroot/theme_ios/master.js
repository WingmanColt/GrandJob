
        $(document).ready(function () {
         $("div[href]").click(function () {
          window.location = $(this).attr("href");
            });

            $(window).keydown(function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                    return false;
                }
            });
        });
      
$(window).on('shown.bs.modal', function() { 
      $(".content-wrapper").animate({ scrollTop: 0}, "fast");
    });


        $(function () {
            $(".menu-link").click(function () {
                $(".menu-link").removeClass("is-active");
                $(this).addClass("is-active");
            });
        });

        $(function () {
            $(".main-header-link").click(function () {
                $(".main-header-link").removeClass("is-active");
                $(this).addClass("is-active");
            });
        });

        const dropdowns = document.querySelectorAll(".dropdown");
        dropdowns.forEach((dropdown) => {
            dropdown.addEventListener("click", (e) => {
                e.stopPropagation();
                dropdowns.forEach((c) => c.classList.remove("is-active"));
                dropdown.classList.add("is-active");
            });
        });

        $(".search-bar input")
            .focus(function () {
                $(".header").addClass("wide");
            })
            .blur(function () {
                $(".header").removeClass("wide");
            });

        $(document).click(function (e) {
            var container = $(".status-button");
            var dd = $(".dropdown");
            if (!container.is(e.target) && container.has(e.target).length === 0) {
                dd.removeClass("is-active");
            }
        });

        $(function () {
            $(".dropdown").on("click", function (e) {
                $(".content-wrapper").addClass("overlay");
                e.stopPropagation();
            });
            $(document).on("click", function (e) {
                if ($(e.target).is(".dropdown") === false) {
                    $(".content-wrapper").removeClass("overlay");
                }
            });
        });

        $(function () {
            $(".status-button:not(.open)").on("click", function (e) {
                $(".overlay-app").addClass("is-active");
            });
            $(".close").click(function () {
                $(".overlay-app").removeClass("is-active");
            });
        });


        const buttons = document.querySelectorAll(".menu-custom_item");
        let activeButton = document.querySelector(".menu-custom_item.active");

        buttons.forEach(item => {
            item.addEventListener("click", function () {

                if (this.classList.contains("active")) return;
                this.classList.add("active");

                if (activeButton) {
                    activeButton.classList.remove("active");
                }
                activeButton = this;
            });
        });

        (function () {
            $("#user-dropdown-button").on("click", function () {
                $(".dropdown").toggleClass('active');
            });

        })();

window.addEventListener('DOMContentLoaded', (event) => {
  setTimeout(removeLoader, 1000); //wait for page load PLUS two seconds.
});
function removeLoader(){
    $( "#overlay-app" ).fadeOut(200, function() {
      // fadeOut complete. Remove the loading div
      $( "#overlay-app" ).remove(); //makes page more lightweight 
  });  
}




$(document).ready(function () {
    const bodyItem = $("#apps-card");
    //const appcard = document.querySelector('.app-card');
    const gridViewBtn = document.querySelector('.grid-view');

   /* if (localStorage.getItem('GridView') === 'true') {
        bodyItem.removeClass('row');
        bodyItem.addClass('column')

        Array.from(document.querySelectorAll('.grid-view')).forEach(el => {
            el.classList.remove("row")
        });

       // appcard.forEach(el => el.classList.remove("row"));
    } else {
        bodyItem.removeClass('column');
        bodyItem.addClass('row')

        Array.from(document.querySelectorAll('.grid-view')).forEach(el => {
            el.classList.add("row")
        });
    }
    */
    gridViewBtn.addEventListener("click", () => {

        if (bodyItem.hasClass("apps-card-row")) {
            bodyItem.removeClass('apps-card-row');
            bodyItem.addClass('apps-card-column')

            Array.from(document.querySelectorAll('.app-card')).forEach(el => {
                el.classList.remove("app-card-row")
            });
        } else {
            bodyItem.removeClass('apps-card-column');
            bodyItem.addClass('apps-card-row')

            Array.from(document.querySelectorAll('.app-card')).forEach(el => {
                el.classList.add("app-card-row")
            });
        }

    });
});




function receiveImgData(isLight, ApiName, bodyItem) {
    var data = '';

    if (isLight === true && localStorage.hasOwnProperty("imgData-light")) {
        data = localStorage.getItem('imgData-light');
        bodyItem.css("background-image", "url('" + data + "')");
    }
    else if (isLight === false && localStorage.hasOwnProperty("imgData-dark")) {
        data = localStorage.getItem('imgData-dark');
        bodyItem.css("background-image", "url('" + data + "')");
    }
    else {
        $.ajax({
            url: ApiName,
            type: 'GET',
            cache: true,
            async: true,
            success: function (result) {
                if (isLight === true) {
                    localStorage.setItem('imgData-light', result);
                }
                else {
                    localStorage.setItem('imgData-dark', result);
                }
                data = result;
                bodyItem.css("background-image", "url('" + data + "')");
            }
        })
    }
};

/*
        const bodyItem = $("#theme");
        const adItem = $(".content-wrapper-img");
        const innnerButton = document.querySelector('.dark-light');

        $(document).ready(function () {
        var dataAdImage = localStorage.getItem('imgData-ad');
        adItem.attr("src", dataAdImage);

            if (localStorage.getItem('lightTheme') === 'true') {
                bodyItem.addClass('light-mode')
                
                receiveImgData(true, '/uploads/dayBG.txt', bodyItem);
            } else {
                bodyItem.removeClass("light-mode");

               receiveImgData(false, '/uploads/nightBG.txt', bodyItem);
            }
           
        });

        innnerButton.addEventListener("click", () => {

            if (bodyItem.hasClass("light-mode")) {
                localStorage.setItem('lightTheme', 'false');

                receiveImgData(false, '/uploads/nightBG.txt', bodyItem);
             document.body.classList.toggle('light-mode');
            } else {
                localStorage.setItem('lightTheme', 'true');
                receiveImgData(true, '/uploads/dayBG.txt', bodyItem);
                
             document.body.classList.toggle('light-mode');
            }

        });

function receiveImgData(isLight, ApiName, bodyItem) {
    var data = '';

    if (isLight === true && localStorage.hasOwnProperty("imgData-light")) {
        data = localStorage.getItem('imgData-light');
        bodyItem.css("background-image", "url('" + data + "')");
    }
    else if (isLight === false && localStorage.hasOwnProperty("imgData-dark")) {
        data = localStorage.getItem('imgData-dark');
        bodyItem.css("background-image", "url('" + data + "')");
    }
    else {
        $.ajax({
            url: ApiName,
            type: 'GET',
            cache: true,
            async: true,
            success: function (result) {
                if (isLight === true) {
                    localStorage.setItem('imgData-light', result);
                }
                else {
                    localStorage.setItem('imgData-dark', result);
                }
                data = result;
                bodyItem.css("background-image", "url('" + data + "')");
            }
        })
    }
};*/