ko.bindingHandlers['visibility'] = {
    update: function (element, valueAccessor) {
        var newValue = ko.utils.unwrapObservable(valueAccessor());
        switch (newValue) {
            case "collapse":
                element.style.visibility = "collpase";
                break;
            case "hidden":
                element.style.visibility = "hidden";
                break;
            default:
                element.style.visibility = "";
        }
    }
}