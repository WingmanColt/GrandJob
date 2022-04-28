(function($) {
	
	"use strict";
	
	//Hide Loading Box (Preloader)
	function handlePreloader() {
		if($('.preloader').length){
			$('.preloader').delay(200).fadeOut(500);
		}
	}


	function rellaxInit() {
		const target = document.querySelectorAll('.js-rellax')
		if (!target) return;
	
		var rellax = new Rellax('.js-rellax', {
			breakpoints: [576, 768, 1025]
		});
	}
	rellaxInit();


	function scrollToIdInit() {
		const targets = document.querySelectorAll('.js-scroll-to-id');
		if (!targets.length) return;
	
		targets.forEach(el => {
			el.addEventListener('click', (e) => {
				if (document.querySelector('.is-pin-active'))
					document.querySelector('.is-pin-active').classList.remove('is-pin-active')
					el.classList.add('is-pin-active')
			})
		});
	}
	scrollToIdInit();
	

	if($('.js-tab-menu').length) {
		const target = document.querySelector('.js-tab-menu');
		const SMcontroller = new ScrollMagic.Controller();
		let sections = document.querySelectorAll(".js-tab-menu-content");
		let sceneDuration = 0;
	
		sections.forEach(el => {
			sceneDuration += el.offsetHeight
		})

		const sceneOffset = document.querySelector('.main-header').offsetHeight;
		let trHook = "onLeave";

		const scene = new ScrollMagic.Scene({
			duration: sceneDuration,
			offset: '-' + sceneOffset + 'px',
			triggerElement: target,
			triggerHook: trHook,
		})
		.setPin(".js-tab-menu")
		.addTo(SMcontroller)
		
		window.addEventListener('resize', () => {
			sections.forEach(el => {
				sceneDuration += el.offsetHeight
			})

			const sceneOffset = document.querySelector('.main-header').offsetHeight;
			scene.duration(sceneDuration);
			scene.offset('-' + sceneOffset + 'px');
			scene.refresh();
		})

		window.addEventListener('scroll', () => {
			const sceneOffset = document.querySelector('.main-header').offsetHeight;
			scene.offset('-' + sceneOffset + 'px');
			scene.refresh();
		})
	}


	//Update Header Style and Scroll to Top
	function headerStyle() {
		if($('.main-header').length){
			var windowpos = $(window).scrollTop();
			var siteHeader = $('.main-header');
			var scrollLink = $('.scroll-to-top');
			var sticky_header = $('.main-header');
			if (windowpos > 1) {
				siteHeader.addClass('fixed-header animated slideInDown');
				scrollLink.fadeIn(300);
			} else{
				siteHeader.removeClass('fixed-header animated slideInDown');
				scrollLink.fadeOut(300);
			}
		}
	}
	headerStyle();


	//sticky-header Hide Show
	if($('.sticky-header').length){
		var stickyMenuContent = $('.main-header .main-box .nav-outer').html();
		$('.sticky-header .main-box').append(stickyMenuContent);
		//Sidebar Cart
		$('.main-header .cart-btn, .mobile-header .cart-btn').on('click', function() {
			$('body').addClass('sidebar-cart-active');
		});

		//Menu Toggle Btn
		$('.main-header .cart-back-drop, .main-header .close-cart').on('click', function() {
			$('body').removeClass('sidebar-cart-active');
		});
	}


	//Jquery Knob animation  // Pie Chart Animation
	if($('.dial').length){
          var elm = $('.dial');
          var color = elm.attr('data-fgColor');  
          var perc = elm.attr('value');  
          elm.knob({ 
               'value': 0, 
                'min':0,
                'max':100,
                'skin':'tron',
                'readOnly':true,
                'thickness':0.45,
				'dynamicDraw': true,
				'displayInput':false
          });

          $({value: 0}).animate({ value: perc }, {
			  duration: 2000,
              easing: 'swing',
              progress: function () { elm.val(Math.ceil(this.value)).trigger('change');
              }
          });


          var $t = $('.pie-graph .count-box'),
				n = $t.find(".count-text").attr("data-stop"),
				r = parseInt($t.find(".count-text").attr("data-speed"), 10);
				
			if (!$t.hasClass("counted")) {
				$t.addClass("counted");
				$({
					countNum: $t.find(".count-text").text()
				}).animate({
					countNum: n
				}, {
					duration: r,
					easing: "linear",
					step: function() {
						$t.find(".count-text").text(Math.floor(this.countNum));
					},
					complete: function() {
						$t.find(".count-text").text(this.countNum);
					}
				});
			}
    }



	// Mobile Navigation
	if($('#nav-mobile').length){
		jQuery(function ($) {
		  var $navbar = $('#navbar');
		  var $mobileNav = $('#nav-mobile');
		  
		  $navbar
		    .clone()
		    .removeClass('navbar')
		    .appendTo($mobileNav);
		  
		  $mobileNav.mmenu({
		  	"counters": false,
		  	extensions 	: [ "position-bottom", "fullscreen", "theme-black", ],
		    offCanvas: {
		      position: 'left',
		      zposition: 'front',
		    }
		  });
		});
	}

	//Banner Carousel 
	if ($('.banner-carousel').length) {
		$('.banner-carousel').owlCarousel({
			animateOut: 'fadeOut',
		    animateIn: 'fadeIn',
			loop:true,
			margin:0,
			items:1,
			nav:true,
			dots:false,
			smartSpeed: 500,
			autoplay: true,
			autoplayTimeout:5000,
			touchDrag:false,
			mouseDrag:false,
			navText: [ '<span class="fa fa-arrow-left"></span>', '<span class="fa fa-arrow-right"></span>' ],
		});    		
	}

	//Companies Carousel
	if ($('.companies-carousel').length) {
		$('.companies-carousel').owlCarousel({
			loop:true,
			nav:false,
			smartSpeed: 500,
			autoplay: true,
			autoplayTimeout:10000,
			navText: [ '<span class="flaticon-back"></span>', '<span class="flaticon-next"></span>' ],
			responsive:{
				0:{
					items:1
				},
				600:{
					items:2
				},
				768:{
					items:3
				},
				1024:{
					items:4
				}
			}
		});    		
	}

	// Product Carousel Slider
	if ($('.gallery-widget .image-carousel').length && $('.gallery-widget .thumbs-carousel').length) {

		var $sync1 = $(".gallery-widget .image-carousel"),
			$sync2 = $(".gallery-widget .thumbs-carousel"),
			flag = false,
			duration = 500;

			$sync1
				.owlCarousel({
					loop:false,
					items: 1,
					margin: 0,
					nav: true,
					navText: [ '<span class="icon flaticon-back"></span>', '<span class="icon flaticon-next"></span>' ],
					dots: false,
					autoplay: true,
					autoplayTimeout: 5000
				})
				.on('changed.owl.carousel', function (e) {
					if (!flag) {
						flag = false;
						$sync2.trigger('to.owl.carousel', [e.item.index, duration, true]);
						flag = false;
					}
				});

			$sync2
				.owlCarousel({
					loop:false,
					margin: 30,
					items: 1,
					nav: false,
					navText: [ '<span class="icon flaticon-back"></span>', '<span class="icon flaticon-next"></span>' ],
					dots: false,
					center: false,
					autoplay: true,
					autoplayTimeout: 5000,
					responsive: {
						0:{
				            items:2,
				            autoWidth: false
				        },
				        400:{
				            items:2,
				            autoWidth: false
				        },
				        600:{
				            items:3,
				            autoWidth: false
				        },
				        800:{
				            items:5,
				            autoWidth: false
				        },
				        1024:{
				            items:4,
				            autoWidth: false
				        }
				    },
				})
				
		.on('click', '.owl-item', function () {
			$sync1.trigger('to.owl.carousel', [$(this).index(), duration, true]);
		})
		.on('changed.owl.carousel', function (e) {
			if (!flag) {
				flag = true;		
				$sync1.trigger('to.owl.carousel', [e.item.index, duration, true]);
				flag = false;
			}
		});
	}


	//Add One Page nav
	if($('.scroll-nav').length) {
		$('.scroll-nav ul.navigation').onePageNav();
	}

	// Rating Review
	function ratingOverview(ratingElem) {
        $(ratingElem).each(function() {
            var dataRating = $(this).attr('data-rating');
            if (dataRating >= 4.0) {
                $(this).addClass('high');
                $(this).find('.rating-bars-rating-inner').css({
                    width: (dataRating / 5) * 100 + "%",
                });
            } else if (dataRating >= 3.0) {
                $(this).addClass('mid');
                $(this).find('.rating-bars-rating-inner').css({
                    width: (dataRating / 5) * 80 + "%",
                });
            } else if (dataRating < 3.0) {
                $(this).addClass('low');
                $(this).find('.rating-bars-rating-inner').css({
                    width: (dataRating / 5) * 60 + "%",
                });
            }
        });
    }

	// Rating Bars
	$('.rating-bars').appear(function(){
    	ratingOverview('.rating-bars-rating');
	});

	// Leave Rating
	$('.leave-rating input').change(function() {
        var $radio = $(this);
        $('.leave-rating .selected').removeClass('selected');
        $radio.closest('label').addClass('selected');
    });

	// Input Upload 
    var uploadButton = {
        $button: $('.uploadButton-input'),
        $nameField: $('.uploadButton-file-name')
    };
    uploadButton.$button.on('change', function() {
        _populateFileField($(this));
    });

    function _populateFileField($button) {
        var selectedFile = [];
        for (var i = 0; i < $button.get(0).files.length; ++i) {
            selectedFile.push($button.get(0).files[i].name + '<br>');
        }
        uploadButton.$nameField.html(selectedFile);
    }

	//Header Search
	if($('.mobile-search-btn').length) {
		$('.mobile-search-btn').on('click', function() {
			$('.main-header').addClass('moblie-search-active');
		});
		$('.close-search, .search-back-drop').on('click', function() {
			$('.main-header').removeClass('moblie-search-active');
		});
	}

	/*=== Header Search Active ===*/
	$(".header-search-form input").focus(function(){
	  $(this).parent().addClass("active");
	  $('body').addClass('search-active')
	});
	$(".header-search-form input").focusout(function(){
	  $(this).parent().removeClass("active");
	  $('.search-list').slideUp();
	  $('body').removeClass('search-active')
	});


	/*=== User Sidebar / On mobile view ===*/
	if($('#toggle-user-sidebar').length) {
		$('#toggle-user-sidebar, .dashboard-option a').on("click", function() {
		  $('body').toggleClass('user-sidebar-active');
		});

		$('.sidebar-backdrop').on("click", function() {
		  $('body').removeClass('user-sidebar-active');
		});
	}

	/*
	//Toggle More Options
	if ($('#more-options').length) {
	    $('#more-options').on('click', function(){
	    	$(this).parent().toggleClass('active');
	    });
	}
	*/
	//Toggle filters
	if ($('.toggle-filters').length) {
	    $('.toggle-filters').on('click', function(){
	    	$('body').toggleClass('active-filters');
	    });
	    $('.close-filters, .filters-backdrop').on('click', function(){
	    	$('body').removeClass('active-filters');
	    });
	    $('.hide-filters .toggle-filters').on('click', function(){
	    	$(this).html($(this).html() == '<span class="icon flaticon-plus-symbol"></span>Hide Filters' ? '<span class="icon flaticon-controls"></span>Show Filters' : '<span class="icon flaticon-plus-symbol"></span>Hide Filters');
	    });
	    $('.close-filters').on('click', function(){
	    	$('.hide-filters .toggle-filters').html($(this).html() == '<span class="icon flaticon-controls"></span>Hide Filters' ? '<span class="icon flaticon-plus-symbol"></span>Hide Filters' : '<span class="icon flaticon-controls"></span>Show Filters');
	    });
	}

	//Remove Filters On Mobile
	function removeFiltersOnMobile(){
		if ($(window).width() <= 1023) {
	    	$('body').removeClass('active-filters');
	    	$('.hide-filters .toggle-filters').html($(this).html() == '<span class="icon flaticon-controls"></span>Hide Filters' ? '<span class="icon flaticon-plus-symbol"></span>Hide Filters' : '<span class="icon flaticon-controls"></span>Show Filters');
	    }
	}
	removeFiltersOnMobile();


	//Custom Seclect Box
	if($('.custom-select-box').length){
		$('.custom-select-box').selectmenu().selectmenu('menuWidget').addClass('overflow');
	}

	//Chosen Seclect Box
	if($('.chosen-select').length){
		$(".chosen-select").chosen({
			disable_search_threshold: 10,
			width:'100%',
		});
	}

	//Chosen Search Select
	if($('.chosen-search-select').length){
		$(".chosen-search-select").chosen({
			width:'100%',
		});
	}

	// Custom Select Box
	if ($('.sortby-select').length) {
    	$('.sortby-select').select2();
	}

	// Tooltip
	$(function () {
	  $('[data-toggle="tooltip"]').tooltip()
	})
	/*
	// Open modal in AJAX callback
	$('.call-modal').on('click', function(event) {
	  event.preventDefault();
	  this.blur();
	  $.get(this.href, function(html) {
	    $(html).appendTo('body').modal({
	    	closeExisting: true,
			fadeDuration: 300,
			fadeDelay: 0.15
	    });
	  });
	});*/


	//Message Box
	if($('.message-box').length){
		$('.message-box .close-btn').on('click', function(e) {
			$(this).parent('.message-box').fadeOut();
		});
	}

	//Chat Contacts
	if($('.toggle-contact').length){
		$('.toggle-contact').on('click', function(e) {
			$('body').toggleClass('active-chat-contacts');
		});
		$('.contacts li').on('click', function(e) {
			$(this).addClass('active');
			$(this).siblings('li').removeClass('active');
			$('body').removeClass('active-chat-contacts');
		});
	}

	//Accordion Box
	if($('.accordion-box').length){
		$(".accordion-box").on('click', '.acc-btn', function() {
			
			var outerBox = $(this).parents('.accordion-box');
			var target = $(this).parents('.accordion');
			
			if($(this).hasClass('active')!==true){
				$(outerBox).find('.accordion .acc-btn').removeClass('active ');
			}
			
			if ($(this).next('.acc-content').is(':visible')){
				return false;
			}else{
				$(this).addClass('active');
				$(outerBox).children('.accordion').removeClass('active-block');
				$(outerBox).find('.accordion').children('.acc-content').slideUp(300);
				target.addClass('active-block');
				$(this).next('.acc-content').slideDown(300);	
			}
		});	
	}

	//Fact Counter + Text Count
	if($('.count-box').length){
		$('.count-box').appear(function(){
	
			var $t = $(this),
				n = $t.find(".count-text").attr("data-stop"),
				r = parseInt($t.find(".count-text").attr("data-speed"), 10);
				
			if (!$t.hasClass("counted")) {
				$t.addClass("counted");
				$({
					countNum: $t.find(".count-text").text()
				}).animate({
					countNum: n
				}, {
					duration: r,
					easing: "linear",
					step: function() {
						$t.find(".count-text").text(Math.floor(this.countNum));
					},
					complete: function() {
						$t.find(".count-text").text(this.countNum);
					}
				});
			}
			
		},{accY: 0});
	}

	//Progress Bar
	if($('.progress-line').length){
		$('.progress-line').appear(function(){
			var el = $(this);
			var percent = el.data('width');
			$(el).css('width',percent+'%');
		},{accY: 0});
	}

	//Tabs Box
	if($('.tabs-box').length){
		$('.tabs-box .tab-buttons .tab-btn').on('click', function(e) {
			e.preventDefault();
			var target = $($(this).attr('data-tab'));
			
			if ($(target).is(':visible')){
				return false;
			}else{
				target.parents('.tabs-box').find('.tab-buttons').find('.tab-btn').removeClass('active-btn');
				$(this).addClass('active-btn');
				target.parents('.tabs-box').find('.tabs-content').find('.tab').fadeOut(0);
				target.parents('.tabs-box').find('.tabs-content').find('.tab').removeClass('active-tab animated fadeIn');
				$(target).fadeIn(300);
				$(target).addClass('active-tab animated fadeIn');
			}
		});
	}


	//Salary Range Slider
	if ($('.salary-range-slider').length) {

		var minVal = $('#MinSalary').val();
		var maxVal = $('#MaxSalary').val();

		$( ".salary-range-slider" ).slider({
			range: true,
			min: 0,
			max: 100000,
			values: [minVal, maxVal],
			slide: function( event, ui ) {
				$( ".salary-amount .min" ).text( ui.values[0]);
				$(".salary-amount .max").text(ui.values[1]);

			}
		});
		
		$( ".salary-amount .min" ).text( $( ".salary-range-slider" ).slider( "values", 0 )); 
		$(".salary-amount .max").text($(".salary-range-slider").slider("values", 1));


		$('form').submit(function () {
		if($("#MinSalary").length) {
			$("#MinSalary").val($(".salary-range-slider").slider("values", 0));
		}
		if ($("#MaxSalary").length) {
			$("#MaxSalary").val($(".salary-range-slider").slider("values", 1));
		}
return true;
});

	}

	//LightBox / Fancybox
	if($('.lightbox-image').length) {
		$('.lightbox-image').fancybox({
			openEffect  : 'fade',
			closeEffect : 'fade',
			helpers : {
				media : {}
			}
		});
	}
	
	// Scroll to a Specific Div
	if($('.scroll-to-target').length){
		$(".scroll-to-target").on('click', function() {
			var target = $(this).attr('data-target');
		   // animate
		   $('html, body').animate({
			   scrollTop: $(target).offset().top
			 }, 1500);
	
		});
	}

	// Scroll to a Specific Div
	if($('.listing-nav li').length){
		$(".listing-nav li").on('click', function() {
			var target = $(this).attr('data-target');
			$(this).addClass('active').siblings('li').removeClass('active');
			$(target).appear(function(){
				$(this).addClass('active')
			});
		   // animate
		   $('html, body').animate({
			   scrollTop: $(target).offset().top + (-90)
			 }, 1000);
		});
	}

	//Make Content Sticky
	if ($('.sticky-sidebar').length) {
	    $('.sidebar-side').theiaStickySidebar({
	      // Settings
	      additionalMarginTop: 90,
	    });
	}

	
	// Elements Animation
	if($('.wow').length){
		var wow = new WOW(
		  {
			boxClass:     'wow',      // animated element css class (default is wow)
			animateClass: 'animated', // animation css class (default is animated)
			offset:       0,          // distance to the element when triggering the animation (default is 0)
			mobile:       false,       // trigger animations on mobile devices (default is true)
			live:         true       // act on asynchronously loaded content (default is true)
		  }
		);
		wow.init();
	}

	// Home Banners Animations / Mouse Move Animation
	if($('.anm').length){
		anm.on();
	}

	// Chosen touch support.
    if ($('.chosen-container').length > 0) {
      $('.chosen-container').on('touchstart', function(e){
        e.stopPropagation(); 
        e.preventDefault();
        // Trigger the mousedown event.
        $(this).trigger('mousedown');
      });
    }



/* ==========================================================================
   When document is Scrollig, do
   ========================================================================== */
	
	$(window).on('scroll', function() {
		headerStyle();
	});

/* ==========================================================================
   When document is loading, do
   ========================================================================== */
	
	$(window).on('load', function() {
		handlePreloader();
	});	

})(window.jQuery);



