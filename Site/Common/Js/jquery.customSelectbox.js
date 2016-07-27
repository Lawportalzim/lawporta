/* custom select boxes */
(function($){

    function customSelectHide(e)
    {                       
        var el = e.target;
        while (el)
        {
            if ($(el).hasClass('select-list') || $(el).hasClass('input-select')) return;
            el = el.parentNode;
        }
        $('ul.select-list').remove();        
    }

	$.fn.customSelect = function() {

	    $(window).resize(function(){ $('ul.select-list').remove(); });
	
		return this.each(function(){
        
            var $select = $(this);
            var $selected = $(this).find(':selected');
            
            // create replacement
            var $div = $('<div class="input-select" tabindex="0"><div>');
            $div.find('div').text($selected.text());
            
            $div
                .keydown(function(e){
                
                    // get selected item
                    var $list = $('ul.select-list');
                    $selected = $select.find(':selected');
                    
                    // select next/previous
                    if(e.keyCode == 40) 
                    {
                        // find next
                        var $next = $selected.next();
                        if ($next.length == 0) $next = $select.children().eq(0);
                        $select.val($next.val()).change();
                        $div.find('div').text($next.text());
                        $list.children().removeClass('selected').each(function(){
                            if ($(this).text() == $next.text()) $(this).addClass('selected');
                        });
                                            
                        // ignore
                        return false;
                    }
                    else if (e.keyCode == 38) 
                    {
                        // find previous
                        var $prev = $selected.prev();
                        if ($prev.length == 0) $prev = $select.children().eq($select.children().length - 1);
                        $select.val($prev.val()).change();
                        $div.find('div').text($prev.text());
                        $list.children().removeClass('selected').each(function(){
                            if ($(this).text() == $prev.text()) $(this).addClass('selected');
                        });
                        
                        // ignore
                        return false;
                    }       
                    else if (e.keyCode == 13) 
                    {
                        $list.remove();
                        return false;
                    }         
                })
                .click(function(){
                
                    // remove existing lists
                    var $list = $('ul.select-list');
                    if ($list.length > 0 && $list.data('select') == $select.attr('name'))
                    {
                        $list.remove();
                        return;                    
                    }
                    $list.remove();
                
                    // get elements
                    $list = $('<ul>').addClass('select-list').data('select', $select.attr('name')).addClass('select-list-' + $select.attr('class')).data('select', $select.attr('name'));
               
                    
                    // build list
                    $select.children().each(function(){  
                        var $item = $(this);            
                        $('<li>')
                            .addClass($item.val() == $select.val() ? 'selected' : '')
                            .text($item.text())
                            .appendTo($list)
                            .click(function(){                            
                                $select.val($item.val()).change();
                                $div.find('div').text($item.text());
                                $list.remove();                         
                            })
                            .hover(function(){ $(this).addClass('hover'); }, function(){ $(this).removeClass('hover'); });
                    });
                    
                    // get position
                    var scrolltop = jQuery(window).scrollTop();
                    var scrollleft = jQuery(window).scrollLeft();
                    var top = $div.offset().top + $div.outerHeight();
                    var left = $div.offset().left - scrollleft;
                    var width = $div.outerWidth() - 10;
                    
                    // add list
                    $('body')
                        .append($list.css({ top: 0, left: '-99999px', width: width }))
                        .unbind('click', customSelectHide)
                        .click(customSelectHide);
                    
                    // check position
                    var bottom = top + $list.outerHeight();
                    var bottom_bounds = jQuery(window).height() + scrolltop;
                    if (bottom > bottom_bounds)
                    {
                        top = $div.offset().top - $list.outerHeight();
                        $list.css({ top: top });
                    }
                    
                    // show list
                    $list.css({ top: top, left: left });
                    $list.scrollTop($list.find('.selected').position().top - 5);
                });
            
            $select.after($div).hide().bind('change', function(){
                $selected = $(this).find(':selected');
                $div.find('div').text($selected.text());
            });
			
		});
	  
	};

})(jQuery);