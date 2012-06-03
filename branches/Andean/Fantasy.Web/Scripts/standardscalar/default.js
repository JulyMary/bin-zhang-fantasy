var stdscalar = {
    bindview: function (viewId, objId) {
        var be = $("#" + viewId).be();
        var entity = be.getEntity(objId);
        var isDirty = entity.entityState() != "clean";
        var view = $("#" + viewId).editview({ isDirty: isDirty, save: function () { view.submit(); } });
        entity.entityState.subscribe(function (state) {
            view.editview("isDirty", state != "clean");
        });

    },

    cancelCreation: function (viewId, objId) {

        var view = $("#" + viewId);
        view.editview("isDirty", false);
        var be = view.be();
        var entity = be.getEntity(objId);

        entity.entityState("deleted");
    }


}