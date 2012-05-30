
function showtree(root) {


    var be = $("#navigationTree").be();

    $("#navigationTree").bind("load_node.jstree", function (e, data) {


        function BindToEntity(node) {
            var shortcut = $(node).data("entity");
            if (shortcut != undefined) {

                be.shortcut(shortcut);

                be.applyBindings(shortcut.Id, node);
            }
            var children = data.inst._get_children(node);
            children.each(function () {
                BindToEntity(this);
            });

            var menuItems = $(node).data("contextmenu");
            if (menuItems != undefined) {
                var aId = $("a:first", $(node)).attr("id");
                $.contextMenu({selector:"#" + aId, items:menuItems});

            }

        }

        var nodes = data.inst._get_children(data.rslt.obj);
        nodes.each(function () {
            BindToEntity(this);
        });
    })
    .jstree({
        "json_data": {
            "data": [
				root
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
        "plugins": ["themes", "json_data", "ui"],

        "core": { "html_titles": true }
    });

    $("#pagelayout").layout(
    { applyDefaultStyles: true,
        initClosed: false
    }
   );


}
