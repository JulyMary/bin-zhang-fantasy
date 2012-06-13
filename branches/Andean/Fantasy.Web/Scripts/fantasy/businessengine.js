
(function ($) {
    $.fn.be = function () {

        if (this.length > 0) {
            var appscope = this.filter(":first").closest(".fantasybuesinessengineappscope");
            if (appscope.length == 0) {
                appscope = $("body");
            }


            var rs = $.data(appscope[0], "fantasybuesinessengine");
            if (rs == undefined) {
                rs = new function () {

                    //data to save entities;
                    this.entities = new Array();

                    this.boundElements = new Hashtable();

                    //create or update a json object to ko observable object
                    this.renew = function (entity, rebind) {
                        var wrapped = this.entities[entity.Id];
                        if (wrapped == undefined) {
                            wrapped = new Object();
                            this.entities[entity.Id] = wrapped;
                        }

                        wrapped.isrenewing = true
                        if (wrapped.entityState == undefined) {
                            wrapped.entityState = ko.observable('clean');

                        }

                        for (var prop in entity) {
                            if (entity.hasOwnProperty(prop) && prop != "acl" && prop != "entityState") {
                                var val = entity[prop];
                                if (wrapped[prop] == undefined) {
                                    if ($.isArray(val)) {
                                        wrapped[prop] = ko.observableArray(val);
                                    }
                                    else if ($.isFunction(val)) {
                                        wrapped[prop] = ko.computed(val, wrapped);
                                    }
                                    else {
                                        wrapped[prop] = ko.observable(val);
                                        wrapped[prop].subscribe(function (x) {
                                            if (!this.isrenewing) {
                                                if (this.entityState() == "clean") {
                                                    this.entityState("dirty");
                                                }
                                            }
                                        }, wrapped);
                                    }

                                }
                                else {
                                    if ($.isArray(val)) {
                                        wrapped[prop].removeAll();
                                        for (var i = 0; i < val.length; i++) {
                                            wrapped[prop].push(val[i]);
                                        }

                                    }
                                    else if ($.isFunction(val)) {
                                        //We do not update computed functions;
                                    }
                                    else {
                                        wrapped[prop](val);
                                    }
                                }
                            }
                        }



                        var wrappedAcl = wrapped.acl;
                        if (wrappedAcl == undefined) {
                            wrappedAcl = wrapped.acl = new Object();
                        }

                        var acl = entity.acl;
                        if (acl != undefined) {
                            for (var prop in acl) {
                                if (acl.hasOwnProperty(prop)) {
                                    var val = acl[prop];
                                    if (wrappedAcl[prop] == undefined) {
                                        wrappedAcl[prop] = ko.observable(val);
                                    }
                                    else {
                                        wrappedAcl[prop](val);
                                    }
                                }
                            }
                        }
                        wrapped.isrenewing = false;
                        if (entity.entityState) {
                            wrapped.entityState(entity.entityState);
                        }


                        if (rebind !== false) {
                            var elements = this.boundElements.get(entity.Id);
                            if (elements != undefined) {
                                var newArray = new Array();
                                for (var i = 0; i < elements.length; i++) {
                                    var ele = elements[i];
                                    if ($.contains(document.documentElement, ele)) {
                                        newArray.push(ele);
                                        ko.applyBindings(wrapped, ele);
                                    }
                                }
                                this.boundElements.put(entity.Id, newArray);
                            }
                        }

                        return wrapped;

                    }

                    //add a shortcut object from json; a shortcut object only contains Id and Name property.
                    // It can be renewed by a full entity. 
                    this.shortcut = function (entity) {
                        var wrapped = this.entities[entity.Id];
                        if (wrapped == undefined) {
                            wrapped = this.renew(entity, false);
                        }

                        return wrapped;
                    }

                    this.getEntity = function (id) {
                        return this.entities[id];
                    }

                    this.applyBindings = function (id, element) {

                        var model = this.getEntity(id);
                        var elements = this.boundElements.get(id);
                        if (elements == undefined) {
                            elements = new Array();
                            this.boundElements.put(id, elements);
                        }
                        $(element).each(function () {
                            ko.applyBindings(model, this);
                            if (elements.indexOf(this) < 0) {
                                elements.push(this);
                            }

                        });
                    }


                };

                $.data(appscope[0], "fantasybuesinessengine", rs);



            }
            return rs;


        }
        else {
            $.error("requires at least one element to get current business engine context.");
        }

    };
})(jQuery);


