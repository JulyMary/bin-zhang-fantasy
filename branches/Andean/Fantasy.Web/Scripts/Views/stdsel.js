var stdsel =
{
    bindEntity: function (node) {
        var shortcut = $(node).data("entity");
        if (shortcut != undefined) {
            var be = $(node).be();
            var entity = be.shortcut(shortcut);
            be.applyBindings(shortcut.Id, node);
        }
        var children = $.jstree._reference("#navigationTree")._get_children(node);
        children.each(function () {
            stdnav.bindEntity(this);
        });
    },
    show: function (treeId, data) {

        var tree = $("#treeId");
        tree.bind("load_node.jstree", function (e, data) {
            var nodes = data.inst._get_children(data.rslt.obj);
            nodes.each(function () {
                stdnav.bindEntity(this);
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
            "plugins": ["themes", "json_data", "ui", "crrm"],

            "core": { "html_titles": true },
            "ui": { select_limit: 1 }
        });

        tree.dialog({
            modal: true,
            buttons: [
                {
                    class: "stdsel-okbutton",
                    text: "OK",
                    click: function () {

                    }


                }
                ,
                {
                    text: "Cancle",
                    click: function () {
                        $(this).dialog("close");
                    }
                }
            ]
        });


    }

}