ko.bindingHandlers['flagsEnum'] = {
    getSelectedValuesFromSelectNode: function (selectNode) {

        function ToInt(val) {
            if (typeof val == 'string') {
                if (!String.isNullOrEmpty(val)) {
                    return parseInt(val);
                }
                else {
                    return undefined;
                }
            }
            else {
                return val;
            }
        };
        function XOr(v1, v2) {
            if (v1 == undefined) {
                return v2;
            }
            else if (v2 == undefined) {
                return v1;
            }
            else {
                return v1 | v2;
            }
        };
        var result = undefined;
        var nodes = selectNode.childNodes;
        for (var i = 0, j = nodes.length; i < j; i++) {
            var node = nodes[i], tagName = ko.utils.tagNameLower(node);
            if (tagName == "option" && node.selected) {
                var v = ToInt(ko.selectExtensions.readValue(node));
                result = XOr(result, v);
            }
            else if (tagName == "optgroup") {
                var v = ko.bindingHandlers['selectedOptions'].getSelectedValuesFromSelectNode(node);
                result = XOr(result, v);
            }
        }
        return result;
    },
    'init': function (element, valueAccessor, allBindingsAccessor) {
        ko.utils.registerEventHandler(element, "change", function () {
            var value = valueAccessor();
            var valueToWrite = ko.bindingHandlers['selectedOptions'].getSelectedValuesFromSelectNode(this);
            ko.jsonExpressionRewriting.writeValueToProperty(value, allBindingsAccessor, 'value', valueToWrite);
        });
    },
    'update': function (element, valueAccessor) {
        if (ko.utils.tagNameLower(element) != "select")
            throw new Error("values binding applies only to SELECT elements");

        var newValue = ko.utils.unwrapObservable(valueAccessor());

        var nodes = element.childNodes;
        for (var i = 0, j = nodes.length; i < j; i++) {
            var node = nodes[i];
            if (ko.utils.tagNameLower(node) === "option") {
                var selected = false;
                var ov = $(node).val();

                if (newValue == undefined) {
                    selected = String.isNullOrEmpty(ov);
                }
                else {
                    var n = parseInt(ov);
                    selected = (newValue & n) == n ? true : false;
                }
                ko.utils.setOptionNodeSelectionState(node, selected);
            }
        }

    }
};