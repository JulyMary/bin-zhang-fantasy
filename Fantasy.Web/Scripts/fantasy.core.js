$.appendScript = function (href) {
    var exists = false;
    $("head>script").each(function () {
        if ($(this).attr("src").toLowerCase() == href.toLowerCase()) {
            exists = true;
        }
    })
    if (!exists) {
        $.getScript(href);
    }
}
