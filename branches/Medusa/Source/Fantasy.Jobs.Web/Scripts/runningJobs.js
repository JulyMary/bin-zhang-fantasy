$(document).ready(function ()
{
    
    CreateProgressBar();

    setInterval(Refresh, 2000);

})

function CreateProgressBar()
{
    $(".runningjobs-progress").each(function ()
    {
        var progress = $(this);
        var value = parseInt(progress.attr("initValue"));
        var bar = $(".runningjobs-progress-bar", progress);
        bar.progressbar(
        {
            value: value
        }
        )
    });
}

function GetJSONUrl(action)
{
    var format = $("#jsonUrl").val();
    var rs = String.format(format, action);
    return rs;
}

function Refresh()
{
    var version = $("#runingJobsVersion").val();

    $.ajax({
    type: "GET",
    url: GetJSONUrl("Refresh"),
    data: {version:version},
    dataType: "json",
    cache:false,
    success: RefreshCallback,
    error: RefreshError
    });

}

function RefreshCallback(data)
{
    if (data.Expired)
    {
        $.ajax({
            type: "GET",
            url: GetJSONUrl("List"),
            dataType: "html",
            cache: false,
            success: function (html)
            {
                $("#runningJobsContainer").html(html);
                setTimeout(CreateProgressBar, 100);
            },
            error: RefreshError
        });
    }
    else
    {
        for (var i = 0; i < data.Jobs.length; i++)
        {
            var model = data.Jobs[i];
            var meta = model.MetaData;
            var id = meta.Id;
            var progress = $("#progress" + id);
            $(".runningjobs-progress-text", progress).text(model.Progress + "%");
            $(".runningjobs-progress-bar", progress).progressbar("value", model.Progress);
            var status = $("#status" + id);
            status.html(model.Status);
        }
    }
}

function RefreshError(x, y, z)
{
    
}