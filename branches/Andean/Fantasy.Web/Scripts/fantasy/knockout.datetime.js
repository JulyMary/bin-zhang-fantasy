ko.bindingHandlers['datetimeValue'] = {


    init: function (element, valueAccessor, allBindingsAccessor) {
        var settings = new Object();
        if (!String.isNullOrEmpty(allBindingsAccessor["datetimeFormat"])) {
            settings.datetimeFormat = allBindingsAccessor["datetimeFormat"];
        }
        $(element).datetimeEntry(settings);
        ko.utils.registerEventHandler(element, "change", function () {
            var value = valueAccessor();
            var oldValue = ko.utils.unwrapObservable(valueAccessor());

            var newValue = $(element).datetimeEntry('getDatetime');
            
            if (oldValue != newValue) {
                ko.jsonExpressionRewriting.writeValueToProperty(value, allBindingsAccessor, 'value', newValue);
            }
        });
    },

    update: function (element, valueAccessor) {
        if (ko.utils.tagNameLower(element) != "input")
            throw new Error("values binding applies only to input elements");

        var newValue = ko.utils.unwrapObservable(valueAccessor());

        $(element).datetimeEntry('setDatetime', newValue)

    }
};

ko.bindingHandlers['datetimeText'] = {
    update: function (element, valueAccessor, allBindingsAccessor) {

        function formatNumber(value, length) {
            value = '' + value;
            length = length || 2;
            while (value.length < length) {
                value = '0' + value;
            }
            return value;
        }
        function formatDatetime(value, format) {
            var currentDatetime = '';
            var ampm = format.indexOf('a') > -1;
            var defaults = $.datetimeEntry._defaults;
            for (var i = 0; i < format.length; i++) {
                var field = format.charAt(i);
                switch (field) {
                    case 'y':
                        currentDatetime += formatNumber(value.getFullYear() % 100);
                        break;
                    case 'Y':
                        currentDatetime += formatNumber(value.getFullYear(), 4);
                        break;
                    case 'o': case 'O':
                        currentDatetime += formatNumber(value.getMonth() + 1, field == 'o' ? 1 : 2);
                        break;
                    case 'n': case 'N':
                        currentDatetime += defaults[field == 'N' ? 'monthNames' : 'monthNamesShort'][value.getMonth()];

                        break;
                    case 'd': case 'D':
                        currentDatetime += formatNumber(value.getDate(), field == 'd' ? 1 : 2);
                        break;
                    case 'w': case 'W':
                        currentDatetime += defaults[field == 'W' ? 'dayNames' : 'dayNamesShort'][value.getDay()];
                        break;
                    case 'h': case 'H':
                        var h = value.getHours();
                        currentDatetime += formatNumber(!ampm ? h :
						h % 12 || 12, field == 'h' ? 1 : 2);
                        break;
                    case 'm': case 'M':
                        currentDatetime += formatNumber(value.getMinutes(), field == 'm' ? 1 : 2);
                        break;
                    case 's': case 'S':
                        currentDatetime += formatNumber(value.getSeconds(), field == 's' ? 1 : 2);
                        break;
                    case 'a':
                        currentDatetime += defaults['ampmNames'][value.getHours() < 12 ? 0 : 1];
                        break;
                    default:
                        currentDatetime += field;
                        break;
                }
            }
            return currentDatetime;
        };

        var datetimeFormat = allBindingsAccessor["datetimeFormat"];
        if (String.isNullOrEmpty(datetimeFormat)) {
            datetimeFormat = $.datetimeEntry._defaults["datetimeFormat"];
        }
        var value = ko.utils.unwrapObservable(valueAccessor());

        var text = value != undefined ? formatDatetime(value, datetimeFormat) : undefined;
        $(element).text(text);


    }


};