$(document).ready(function ()
{
    $(".untermianted-selection-checkbox").checkbox(
        {
            cls: 'jquery-safari-checkbox'

        });
    $(".untermianted-selection-checkbox-all").checkbox(
        {
            cls: 'jquery-safari-checkbox'
        })
        .click(function ()
        {
            var checked = !$(this).is(":checked");
            $(".untermianted-selection-checkbox").attr("checked", checked);
        });


        $(".toolbar-button-dropdown").button({ text: false, icons: {
            primary: "ui-icon-triangle-1-s"
        }
        })


});

function GetJSONUrl(action)
{
    var format = $("#jsonUrl").val();
    var rs = String.format(format, action);
    return rs;
}

function GetSelectedIds()
{
    

}

function ChangeJobState(action, data)
{
    $.ajax({
        type: "GET",
        url: GetJSONUrl(action),
        data: data,
        dataType: "json",
        cache: false,
        success: ChangedCallback,
        error: ChangedCallback
    });
}

function ChangedCallback(x, y, z)
{
    if (x === true)
    {
        window.location = window.location;
    }
}

function ChangeJobStateBySelected(action)
{
    var ids = "";
    $(".untermianted-selection-checkbox:checked").each(function ()
    {
        ids += ";"+$(this).val();
    });
    ChangeJobState(action, { ids: ids }) 
    
}

function ChangeJobStateByFilter(action)
{
    var filter = getUrlEncodedKey("JobList-filter");
    ChangeJobState(action, { filter: filter });    
}


