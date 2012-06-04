
var stdnav = {
    bindEntity: function (node, be) {
        var shortcut = $(node).data("entity");
        if (shortcut != undefined) {

            var entity = be.shortcut(shortcut);

            be.applyBindings(shortcut.Id, node);

            var handler = entity.entityState.subscribe(function (state) {
                if (state == "deleted") {

                    var tree = $.jstree._reference("#navigationTree");
                    var toSelect = tree._get_next(this, true);
                    if (!toSelect) {
                        toSelect = tree._get_prev(this, true);
                    }

                    if (!toSelect) {
                        toSelect = tree._get_parent(this);
                        if (toSelect != -1 && toSelect != false) {
                            //Skip FOLDER
                            toSelect = tree._get_parent(toSelect);
                        }
                    }


                    $("#navigationTree").jstree("remove", this);
                    this.data("entityStateHandler").dispose();

                    if (toSelect != -1 && toSelect != false) {
                        tree.select_node(toSelect, true);
                        toSelect.children("a:first").click();
                    }

                }
            }, $(node));
            $(node).data("entityStateHandler", handler);
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
    isConfirmed: false,
    showtree: function (root) {
        var be = $("#navigationTree").be();

        $("#navigationTree").bind("load_node.jstree", function (e, data) {

            var nodes = data.inst._get_children(data.rslt.obj);
            nodes.each(function () {
                stdnav.bindEntity(this, be);
            });
        })
    .bind("before.jstree", function (e, data) {
        if (data.func == "select_node") {
            var view = $("#contentpanel").children(":first");
            var tree = $.jstree._reference("#navigationTree");

            var newNode = tree._get_node(data.args[0]);
            if (!stdnav.isConfirmed && view.editview("isDirty")) {

                messageBox.show({
                    text: "Changes are not saved, do you want to save them?",
                    buttons: messageBox.buttons.YesNoCancel,
                    icon: messageBox.icons.Question,
                    defaultButtons: messageBox.defaultButtons.Button3,
                    onclose: function (dr) {

                        var op = undefined;
                        switch (dr) {
                            case messageBox.dialogResults.Yes:
                                op = "save";
                                break;
                            case messageBox.dialogResults.No:
                                op = "rollback";
                                break;
                        }
                        if (op) {
                            view.editview(op);
                            stdnav.isConfirmed = true;
                            try {
                                tree.select_node(newNode, true);
                                newNode.children("a:first").click();
                            }
                            finally {
                                stdnav.isConfirmed = false;
                            }
                        }
                    }
                });

                e.stopImmediatePropagation();
                return false;

            }
        }
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

            stdnav.isConfirmed = true;
            try {
                tree.select_node(node, true);
                var be = $("#navigationTree").be();
                stdnav.bindEntity(node[0], be);
            }
            finally {
                stdnav.isConfirmed = false;
            }

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


        return rs;
    },

    refreshNode: function (key, opt) {
        var tree = $.jstree._reference("#navigationTree");
        var node = tree._get_node(opt.$trigger);
        tree.refresh(node);
    }


};





