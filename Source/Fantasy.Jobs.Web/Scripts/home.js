$(document).ready(function ()
{
    $(".icon-image").hover(function ()
    {
        var src = $(this).attr("src");
        $(this).attr("src", $(this).attr("toggleSrc"));
        $(this).attr("toggleSrc", src);
    });
});