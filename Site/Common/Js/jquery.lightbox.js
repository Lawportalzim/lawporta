/*
Custom jQuery lightbox (videos and photos)
Bradley Searle - Kay Media
*/
;(function($)
{
    var lightbox =
	{
	    id: 0,
	    current: null,
	    working: false,
	    click: function(e)
	    {
	        // Remember
	        lightbox.current = $(e.currentTarget);

	        // Create and show overlay
	        $('<div id="lightbox-overlay"></div>').appendTo('body').css({ opacity: .8 }).hide().fadeIn('fast', lightbox.clickDone);

	        // Ignore click
	        return false;
	    },
	    clickDone: function()
	    {
	        // Create an image holder
	        $('<table id="lightbox-frame" cellspacing="0">' +
                    '<tr>' +
                        '<td>' +
                            '<table align="center" cellspacing="0">' +
                                '<tr>' +
                                    '<td colspan="2" id="lightbox-picture"><div></div></td>' +
                                '</tr>' +
                                '<tr>' +
                                    '<td id="lightbox-description">Loading...</td>' +
                                    '<td>' +
                                        '<ul>' +
	                                        '<li id="lightbox-previous"><a href="#">Previous</a></li>' +
	                                        '<li id="lightbox-next"><a href="#">Next</a></li>' +
	                                        '<li id="lightbox-close"><a href="#">Close</a></li>' +
	                                    '</ul>' +
                                    '</td>' +
                                '</tr>' +
                            '</table>' +
                        '</td>' +
                    '</tr>' +
                '</table>').appendTo('body').hide().fadeIn('fast', lightbox.show);

	        // Bind nav
	        $('#lightbox-previous a').bind('click', lightbox.previous);
	        $('#lightbox-next a').bind('click', lightbox.next);
	        $('#lightbox-close a').bind('click', lightbox.close);
	        $('body').bind('keydown', lightbox.keynav);
	    },
	    show: function()
	    {
	        // lock
	        lightbox.working = true;
	        
	        // Clear current media
	        $('#lightbox-picture-image').unbind('load').remove('');

	        // Load the description
	        $('#lightbox-description').html(lightbox.current.attr('title'));

	        // Load media
	        if (lightbox.current.attr('href').match(/.flv$/gi))
	        {
	            // Video
	            $('#lightbox-picture div').append('<div id="lightbox-video"></div>').animate({ width: 640, height: 480 }, 250, 'swing', function() { lightbox.video(); });
	        }
	        else
	        {
	            // Image
	            $('<img src="' + lightbox.current.attr('href') + '" id="lightbox-picture-image" />').appendTo('body').hide().load(function()
	            {
	                // reference
	                var $this = $(this);

	                // Check browser dimensions
	                var w = $this.width();
	                var h = $this.height();
	                var maxw = $(window).width() - 100;
	                var maxh = $(window).height() - 150;
	                if (w > maxw)
	                {
	                    h = (maxw / w) * h;
	                    w = maxw;
	                }
	                if (h > maxh)
	                {
	                    w = (maxh / h) * w;
	                    h = maxh;
	                }

	                // animate to new dimensions
	                $('#lightbox-picture div')
	                    .animate({ width: w, height: h }, 250, 'swing', function()
	                    {
	                        $this
	                            .css({ width: w, height: h })
	                            .hide()
	                            .appendTo('#lightbox-picture div')
	                            .fadeIn('slow')
	                            .click(lightbox.close);

	                        // unlock
	                        lightbox.working = false;
	                    });
	            });
	        }

	        // Next/previous
	        var id = parseInt(lightbox.current.attr('rel'));
	        var gallery = lightbox.current.data('gallery');
	        var max = $(gallery).length - 1;
	        if (id == 0)
	        {
	            $('#lightbox-previous a').addClass('disabled');
	        }
	        else
	        {
	            $('#lightbox-previous a').removeClass('disabled');
	        }
	        if (id == max)
	        {
	            $('#lightbox-next a').addClass('disabled');
	        }
	        else
	        {
	            $('#lightbox-next a').removeClass('disabled');
	        }
	    },
	    video: function()
	    {
	        var src = lightbox.current.attr('href').replace(/^(\/data\/)/gi, '');

	        $('#lightbox-video').flash(
			{
			    swf: '/web/images/videoplayer.swf',
			    width: 640,
			    height: 480,
			    hasVersion: 8,
			    bgColor: '#000000',
			    flashvars: {
			        flv: src,
			        autoplay: 'true'
			    }
			});
	    },
	    keynav: function(e)
	    {
	        switch (e.keyCode)
	        {
	            case 27: lightbox.close(); break;
	            case 37: lightbox.previous(); break;
	            case 39: lightbox.next(); break;
	        }
	    },
	    previous: function()
	    {
	        if (lightbox.working) return false;

	        var id = parseInt(lightbox.current.attr('rel'));
	        var gallery = lightbox.current.data('gallery');
	        if (id == 0) return false;

	        var el = $(gallery + '[rel=' + (id - 1) + ']');
	        if (el.length == 0) return;
	        lightbox.current = el;
	        lightbox.show();
	        return false;
	    },
	    next: function()
	    {
	        if (lightbox.working) return false;

	        var id = parseInt(lightbox.current.attr('rel'));
	        var gallery = lightbox.current.data('gallery');
	        var max = $(gallery).length - 1;
	        if (id == max) return false;

	        var el = $(gallery + '[rel=' + (id + 1) + ']');
	        if (el.length == 0) return;
	        lightbox.current = el;
	        lightbox.show();
	        return false;
	    },
	    close: function()
	    {
	        $('body').unbind('keydown');
	        $('#lightbox-frame').fadeOut('fast', function() { $(this).remove(); });
	        $('#lightbox-overlay').fadeOut('fast', function() { $(this).remove(); });
	        return false;
	    }
	};

    // Setup lightbox
    jQuery.fn.lightbox = function()
    {
        // reset
        lightbox.id = 0;

        var selector = $(this).selector;
        return this.each(function()
        {
            var $this = $(this);
            $(this)
                .unbind('click', lightbox.click).bind('click', lightbox.click)
                .data('gallery', selector)
                .attr('rel', lightbox.id++);
        });
    };

})(jQuery);