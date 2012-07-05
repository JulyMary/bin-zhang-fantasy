
$(document).ready(function () {
    $(".trigger-type-radio").click(function () {
        var containerId;
        switch ($(this).val()) {
            case "Time":
                containerId = "TimeOptionsContainer";
                break;
            case "Daily":
                containerId = "DailyOptionsContainer";
                break;
            case "Weekly":
                containerId = "WeeklyOptionsContainer";
                break;
            case "Monthly":
                containerId = "MonthlyOptionsContainer";
                break;
            case "MonthlyDayOfWeek":
                containerId = "MonthlyDOWOptionsContainer";
        }

        $(".trigger-options-container").each(function (index, element) {
            var $this = $(element);
            $this.css("display", $this.attr("id") == containerId ? "" : "none");
        });
    });

    $(".action-type-radio").click(function () {
        var containerId;
        switch ($(this).val()) {
            case "Template":
                containerId = "actionTypeTemplateContainer";
                break;
            case "Inline":
                containerId = "actionTypeInlineContainer";
                break;
            case "Custom":
                containerId = "actionTypeCustomContainer";
                break;
        }

        $(".action-options-container").each(function (index, element) {
            var $this = $(element);
            $this.css("display", $this.attr("id") == containerId ? "" : "none");
        });
    });


    $("#repetitionContainer").EnableDisable({ enabler: "#enableRepetitionInput" });
    $("#expirecontainer").EnableDisable({ enabler: "#enableExpireInput" });
    $("#executeLimitContainer").EnableDisable({ enabler: "#enableExecuteLimitInput" });

    $("#saveChangeButton").click(function () {
        var triggerType = $(".trigger-type-radio:checked").val();
        switch (triggerType) {
            case "Weekly":
                {
                    $("#DaysOfWeekInput").val(JoinValue(".weeklyDOW:checked"));
                }
                break;
            case "Monthly":
                {
                    $("#DaysOfMonthInput").val(JoinValue(".monthlyDOM:checked"));
                    $("#MonthsOfYearInput").val(JoinValue(".monthlyMOY:checked"));
                }
                break;
            case "MonthlyDayOfWeek":
                {
                    $("#DaysOfWeekInput").val(JoinValue(".monthlyDOWDOW:checked"));
                    $("#WeeksOfMonthInput").val(JoinValue(".monthlyDOWWOM:checked"));
                    $("#MonthsOfYearInput").val(JoinValue(".monthlyDOWMOY:checked"));
                }
                break;

        }
    });

    $('#saveChangeButton').button();
});


function JoinValue(selector)
{
    var rs = "";
    $(selector).each(function (index, element)
    {
        if (rs.length > 0)
        {
            rs += " ";
        }
        rs += $(element).val();

    });
    return rs;
}
