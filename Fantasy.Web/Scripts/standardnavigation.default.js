
$(document).ready(function () {

    $("#navigationTree").jstree({
        "json_data": {
            "data": [
				initNavigationTreeNodes
			]
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
