$('document').ready(function () {
    //back button
    /* $('a.back').click(function () {
    history.go(-1);
    });*/

    //tabs
    $('ul.tabs li').click(function () {
        if ($(this).hasClass('selected')) {
            return;
        } else {
            $('ul.tabs li').removeClass('selected');
            $(this).addClass('selected');
            if ($(this).hasClass('bycat')) {
                $('div.details div.fullcase').slideUp();
                $('div.details div.byCategory').slideDown();
            }
            if ($(this).hasClass('byfull')) {
                $('div.details div.byCategory').slideUp();
                $('div.details div.fullcase').slideDown();
            }
        }
    });

    //subcats
    $('.list-tout > ul > li a.selected').parent().find('ul').show();
    $('.list-tout ul ul li a.selected').parent().parent().show();

    //no copy and paste
    $(document).bind("cut copy paste", function (e) {
        //e.preventDefault();
    });

    //useronline sync
    SyncOnlineStatuc();

    //toggle category summaries
    $('a.moreText').each(function () {
        var $_this = $(this);
        var $short = $_this.parent().find("div.synopsis");
        var $long = $_this.parent().find("div.fullText");
        $_this.click(function () {
            if ($_this.hasClass("expanded")) {
                $short.slideDown();
                $long.slideUp();
                $_this.removeClass("expanded");
                $_this.text("More");                
            } else {
                $short.slideUp();
                $long.slideDown();
                $_this.addClass("expanded");
                $_this.text("Less");
            }
        });
    });

});

function SyncOnlineStatuc() {
    var UserIdCookie = readCookie("Kay_auth");
    if (readCookie("Kay_auth") != null) UserIdCookie = readCookie("Kay_auth");

    var onlineInterval = setInterval(function () {
        if (readCookie("Kay_auth") == null || readCookie("Kay_auth") == "") {
            $.ajax("/onlineSync.aspx?userid=" + UserIdCookie + "&online=false");
            clearInterval(onlineInterval);
        }
    }, 50);

    var lastSeen = setInterval(function () {        
            $.ajax("/onlineSync.aspx?userid=" + UserIdCookie + "&online=true");
            clearInterval(onlineInterval);        
    }, 60000);
}

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}