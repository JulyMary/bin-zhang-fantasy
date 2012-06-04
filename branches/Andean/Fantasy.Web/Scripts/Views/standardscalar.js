var stdscalar = {
    bindEditView: function (viewId, objId) {
        var be = $("#" + viewId).be();
        var entity = be.getEntity(objId);
        var isDirty = entity.entityState() != "clean";
        var view = $("#" + viewId).editview({
            isDirty: isDirty,
            save: function () { view.submit(); },
            rollback: function () { $("#" + viewId + "_refresh").click(); }
        });
        entity.entityState.subscribe(function (state) {
            view.editview("isDirty", state != "clean");
        });

    },

    bindCreationView: function (viewId, objId) {
        var view = $("#" + viewId).editview({
            isDirty: true,
            save: function () { view.submit(); },
            rollback: function () { stdscalar.cancelCreation(viewId, objId); }
        });
    },

    cancelCreation: function (viewId, objId) {

        var view = $("#" + viewId);
        view.editview("isDirty", false);
        var be = view.be();
        var entity = be.getEntity(objId);

        entity.entityState("deleted");
        view.html("Operation cancelled");
    },

    entityDeleted: function (viewId, objId) {
        var view = $("#" + viewId);
        var be = view.be();
        var entity = be.getEntity(objId);
        entity.entityState("deleted");
    }




}