(function ($) {
    $(function () {
        $('.sidenav').sidenav();

        $('.twitterLogin').click(function () {
            $('.loginForm').submit();
        });
    });
})(jQuery);
