var refreshInterval;
$(document).ready(function ()
{


    $("#operationbar").buttonset();
    var buttons = $(".ui-button-text", $(".jobdetails-operationbar-button"));
    buttons.css("padding-top", "0px");
    buttons.css("padding-bottom", "0px");

    $("#show_log_link").button();
    $(".ui-button-text", $("#show_log_link")).css("padding-top", "0px").css("padding-bottom", "0px");
    //hight light scripts
    SyntaxHighlighter.all();

    //create progress bar
    var bar = $(".runningjobs-progress-bar-large");
    if (bar.length > 0)
    {
        var value = parseInt(bar.attr("initValue"));
        bar.progressbar({ value: value });
    }


    if ($("#enableRefresh").val() == "True")
    {
        refreshInterval = setInterval(Refresh, 2000);
    }

});

function Refresh()
{
    var id = $("#jobId").val();
    $.ajax({
        type: "GET",
        url: GetJSONUrl("refresh"),
        data: { id: id },
        dataType: "json",
        cache: false,
        success: RefreshCallback,
        error: RefreshError
    });
}

function IsTerminated(state)
{
    return state < 0;
}

function GetJSONUrl(action)
{
    var format = $("#jsonUrl").val();
    var rs = String.format(format, action);
    return rs;
}

function RefreshCallback(data)
{
    var state = data.MetaData.State;
    $("#StateImage").attr("src", data.ImageUrl);
    if (!IsTerminated(data.MetaData.State))
    {
        $(".runningjobs-progress-text-large").html(data.Progress + "%");
        $(".runningjobs-progress-bar-large").progressbar("value", data.Progress);
        $(".runningjobs-status-large").text(data.Status);

        $("#button_resume").button(state == 0x10 ? "enable" : "disable");
        $("#button_pause").button(state != 0x10 ? "enable" : "disable");
        $("#button_stop").button("enable");

    }
    else
    {
        $(".runningjobs-progress-text-large").html("100%");
        $(".runningjobs-progress-bar-large").progressbar("value", 100);
        $(".runningjobs-status-large").text("");

        $(":button", $("#operationbar")).button("disable");

        clearInterval(refreshInterval);
    }

    var buttons = $(".ui-button-text", $(".jobdetails-operationbar-button"));
    buttons.css("padding-top", "0px");
    buttons.css("padding-bottom", "0px");
}

function RefreshError(x, y, z)
{
}

function changeJobState(action)
{
    $(":button", $("#operationbar")).attr("disabled", "disabled");
    var id = $("#jobId").val();
    $.ajax({
        type: "GET",
        url: GetJSONUrl(action),
        data: { id: id },
        dataType: "json",
        cache: false,
        error: RefreshError
    });
}