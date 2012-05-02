
$(document).ready(function () {


    $("#navigationTree").jstree({
        "json_data": {
            "data": [
				initNavigationTreeNodes
			],
            "ajax":
            {
                "url": function (node) {

                    var url = $.data(node[0], "url");
                    return url;
                }

            }
        },

        "themes": {
            "theme": "default",
            "dots": false,
            "icons": true
        },
        "plugins": ["themes", "json_data", "ui"]
    });

    $("#pagelayout").layout(
    { applyDefaultStyles: true,
        initClosed: false
    }
   );


});
