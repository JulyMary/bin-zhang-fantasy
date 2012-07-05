var scheduleTree;
$(document).ready(function ()
{
    scheduleTree = $('#ScheduleTree').data('tTreeView');

    $("#newGroupDialog").dialog(
        {
            autoOpen: false,
            resizable: false,
            modal: true
        }
    );

    $('input[type="button"]', $("#newGroupDialog")).button();

    $("#toolbar").buttonset();
    var buttons = $(".ui-button-text", $("#toolbar"));
    buttons.css("padding-top", "4px");
    buttons.css("padding-bottom", "2px");
});


function GetJSONUrl(action) {
    var format = $("#jsonUrl").val();
    var rs = String.format(format, action);
    return rs;
}

function getCurrentGroupPath()
{
    var rs = scheduleTree.getItemValue(scheduleTree.selectedNode());
    return rs;
}

function ScheduleTreeSelected(e) {

    var group = scheduleTree.getItemValue(e.item);
    $.ajax({
        type: "GET",
        url: GetJSONUrl("ScheduleList"),
        data: { group: group },
        dataType: "HTML",
        cache: false,
        success: function (html) {
            $("#scheduleListContainer").html(html);
        },
        error: ajaxErrorCallback
    });

}

function addGroup()
{
    $("#newGroupDialog").dialog("open");
}

function doAddGroup()
{
    $("#newGroupDialog").dialog("close");
    var path = getCurrentGroupPath() + "\\" + $("#newGroupDialogName").val();
    path = encodeURI(path);
    var location = GetJSONUrl("CreateGroup") + "?path=" + path;
    window.location = location;
}



function removeGroup()
{
    var path = getCurrentGroupPath();
    if (path != ".")
    {
        path = encodeURI(path);
        var location = GetJSONUrl("RemoveGroup") + "?path=" + path;
        window.location = location;
    }
}

function addSchedule()
{
    var path = getCurrentGroupPath();
    
    var location = GetJSONUrl("NewSchedule");
    if (path != ".")
    {
        path = encodeURI(path);
        location += "?path=" + path;
    }
    window.location = location;
}


function removeSchedule(path)
{
    $.ajax({
        type: "GET",
        url: GetJSONUrl("RemoveSchedule"),
        data: { path: path },
        dataType: "HTML",
        cache: false,
        success: function (html)
        {
            $("#scheduleListContainer").html(html);
        },
        error: ajaxErrorCallback
    });
}

function closeDialog(id)
{
    $("#" + id).dialog("close");
}

function ajaxErrorCallback(x, y, z)
{
}



