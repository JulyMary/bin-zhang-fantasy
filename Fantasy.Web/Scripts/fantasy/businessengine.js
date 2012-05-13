var be = new function () {

    //data to save entities;
    this.entities = new Array();

    //create or update a json object to ko observable object
    this.renew = function (entity) {
        var wrapped = this.entities[entity.Id];
        if (wrapped == undefined) {
            wrapped = new Object();
            this.entities[entity.Id] = wrapped;
        }

        for (var prop in entity) {
            if (entity.hasOwnProperty(prop)) {
                var val = entity[prop];
                if (wrapped[prop] == undefined) {
                    wrapped[prop] = ko.observable(val);
                }
                else {
                    wrapped[prop](val);
                }
            }
        }

        return wrapped;

    }

    //add a shortcut object from json; a shortcut object only contains Id and Name property.
    // It can be renewed by a full entity. 
    this.shortcut = function(entity)
    {
        var wrapped = this.entities[entity.Id];
        if (wrapped == undefined) {
            wrapped = new Object();
            for (var prop in entity) {
                if (entity.hasOwnProperty(prop)) {
                    var val = entity[prop];
                    wrapped[prop] = ko.observable(val);

                }
            }
            this.entities[entity.Id] = wrapped;
        }

        return wrapped;
    }

    this.getEntity = function (id) {
        return this.entities[id];
    }

    this.applyBindings = function (id, element) {

        var model = this.getEntity(id);
        $(element).each(function () {
            ko.applyBindings(model, this);
        });
    }
}