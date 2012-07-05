function onFilterChanged(e)
{
    var filter = "";
    if ($("#filter_type").data("tDropDownList").selectedIndex > 0)
    {
        filter = AppendFilter(filter, "Type", "eq", $("#filter_type").data("tDropDownList").value(), true);
    }

    if ($("#filter_category").data("tDropDownList").selectedIndex > 0)
    {
        filter = AppendFilter(filter, "Category", "eq", $("#filter_category").data("tDropDownList").value(), true);
    }

    if ($("#filter_importance").data("tDropDownList").selectedIndex > 0)
    {
        filter = AppendFilter(filter, "Importance", "eq", $("#filter_importance").data("tDropDownList").selectedIndex -1);
    }

    $("#XLogView").data('tGrid').filter(filter);
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

function AppendFilter(source, column, op, value, isString)
{
    filter = CreateFilterString(column, op, value, isString);
    
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