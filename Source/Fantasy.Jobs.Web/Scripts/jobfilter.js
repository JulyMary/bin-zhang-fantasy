function Search()
{
    var guidRegex =  /^\s*[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$\s*/i;
    var grid = $('#JobList').data('tGrid');
    //Display all records whose OrderID property is equal to 10248
    var filter = "";
    var name = $("#filter_name").val();
    if (name.length > 0)
    {
        if (guidRegex.test(name))
        {
            var url = $("#filter_name").attr("jobUrl");
            url = String.format(url, name);
            window.location = url;
            return; 
        }
        else
        {
            filter = AppendFilter(filter, CreateFilterString("Name", "substringof", name, true));
        }
    }

    if ($("#filter_state").data("tDropDownList").selectedIndex > 0)
    {
        var state = $("#filter_state").data("tDropDownList").value();
        filter = AppendFilter(filter, CreateFilterString("State", "eq", state));
        
    }

    if ($("#filter_template").data("tDropDownList").selectedIndex > 0)
    {
        var template = $("#filter_template").data("tDropDownList").value();

        filter = AppendFilter(filter, CreateFilterString("Template", "eq", template, true));
    }

    if ($("#filter_application").data("tDropDownList").selectedIndex > 0)
    {
        var app = $("#filter_application").data("tDropDownList").value();

        filter = AppendFilter(filter, CreateFilterString("Application", "eq", app, true));
    }

    if ($("#filter_user").data("tDropDownList").selectedIndex > 0)
    {
        var user = $("#filter_user").data("tDropDownList").value();

        filter = AppendFilter(filter, CreateFilterString("User", "eq", user, true));
    }

    var formatDate = function (date) 
    {
        return "datetime'" + $.telerik.formatString('{0:yyyy-MM-ddTHH-mm-ss}', date) + "'"
   } 

    var creationLow = $("#filter_creationTime_low").data("tDatePicker").value();
    if (creationLow != null)
    {
        filter = AppendFilter(filter, CreateFilterString("CreationTime", "ge", formatDate(creationLow)));
    }

    var creationHigh = $("#filter_creationTime_high").data("tDatePicker").value();
    if (creationHigh != null)
    {
        filter = AppendFilter(filter, CreateFilterString("CreationTime", "le", formatDate(creationHigh)));
    }

    var url = grid.filterUrl(filter);

    var ao = $("#filter_advance_container").is(':visible');

    url = setUrlEncodedKey("ao",ao, url);
    window.location = url;
}


function CreateFilterString(column, op, value, isString)
{
    if (isString)
    {
        var escapeQuoteRegExp = /'/ig;
        return column + "~" + op + "~" + "'" + value.replace(escapeQuoteRegExp, "''") + "'";
    }
    else
    {
        return column + "~" + op + "~" + value;
    }
}

function AppendFilter(source, filter)
{
    if (source.length > 0)
    {
        source += "~and~" + filter;
    }
    else
    {
        source = filter;
    }

    return source;
}

function advanceOptions()
{
    var img = $("#advanceOptionsButton");
    var oldSrc = img.attr("src");
    img.attr("src", img.attr("altSrc"));
    img.attr("altSrc", oldSrc);
    $("#filter_advance_container").toggle('slow');

}

