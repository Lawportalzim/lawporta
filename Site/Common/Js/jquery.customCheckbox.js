/* custom select boxes */
(function ($)
{

    $.fn.customCheckbox = function ()
    {

        return this.each(function ()
        {

            var $checkbox = $(this);
            var $div = $('<div tabindex="0" class="input-checkbox">');

            $div
                .keydown(function (e)
                {

                    if (e.keyCode == 32)
                    {
                        $checkbox.attr('checked', !$checkbox.attr('checked')).change();
                        return false;
                    }

                })
                .click(function ()
                {

                    $checkbox.attr('checked', !$checkbox.attr('checked')).change();

                });

            $checkbox
                .after($div).hide().bind('change', function ()
                {

                    if ($checkbox.attr('checked'))
                    {
                        $div.addClass('input-checkbox-checked');

                        // radio
                        if ($checkbox.attr('type') == 'radio')
                        {
                            $('input[name="' + $checkbox.attr('name') + '"]').each(function ()
                            {
                                var $this = $(this);
                                if ($this.attr('id') != $checkbox.attr('id'))
                                {
                                    $this.next('div').removeClass('input-checkbox-checked');
                                }
                            })
                        }
                    }
                    else $div.removeClass('input-checkbox-checked');

                });

            if ($checkbox.attr('checked')) $div.addClass('input-checkbox-checked');

        });

    };

})(jQuery);