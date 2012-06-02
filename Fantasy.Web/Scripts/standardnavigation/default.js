
var stdnav = {
    bindEntity: function (node, be) {
        var shortcut = $(node).data("entity");
        if (shortcut != undefined) {

            be.shortcut(shortcut);

            be.applyBindings(shortcut.Id, node);
        }
        var children = $.jstree._reference("#navigationTree")._get_children(node);
        children.each(function () {
            stdnav.bindEntity(this, be);
        });

        var menuItems = $(node).data("contextmenu");
        if (menuItems != undefined) {
            var aId = $("a:first", $(node)).attr("id");
            $.contextMenu({ selector: "#" + aId, items: menuItems });

        }
    },

    showtree: function (root) {
        var be = $("#navigationTree").be();

        $("#navigationTree").bind("load_node.jstree", function (e, data) {

            var nodes = data.inst._get_children(data.rslt.obj);
            nodes.each(function () {
                stdnav.bindEntity(this, be);
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

        $("#pagelayout").layout(
    { applyDefaultStyles: true,
        initClosed: false
    }
   );
    },
    addTreeNode: function (parent, property, childNode) {

        var tree = $.jstree._reference("#navigationTree");
        var pNode = $("#navigationTree" + parent + property).closest("li");
        var callback = function (node) {

            tree.select_node(node, true);
            var be = $("#navigationTree").be();
            stdnav.bindEntity(node[0], be);
        };


        if (tree.is_closed(pNode)) {
            tree.open_node(pNode, function () {
                $("#navigationTree").jstree("create", pNode, "last", childNode, callback, true);
            });

        }
        else {
            $("#navigationTree").jstree("create", pNode, "last", childNode, callback, true);
        }
    },

    confirmSaving: function () {

        var rs = true;
        var view = $("#contentpanel").children(":first");
        if (view.editview("isDirty")) {

            rs = confirm("Changes are not saved, contiune?") ? true:false;



        }

        return rs;
    }


};





