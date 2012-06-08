﻿(function ($) {

    var methods = {
        init: function (options) {

            var settings = $.extend({
                isDirty: false
            }, options);

            this.addClass("editview");
            return this.each(function () {
               
                $.data(this, "editviewOptions", settings);
              
            });
        },

        save: function () {
            return this.each(function () {
               
                var options = $.data(this, "editviewOptions");
                if (options.save) {
                    options.save.apply($(this));
                }
            });
        },

        rollback: function () {
            return this.each(function () {
                
                var options = $.data(this, "editviewOptions");
                if (options.rollback) {
                    options.rollback.apply($(this));
                }
            });
        },


        isDirty: function (val) {

            var rs = false;

            this.each(function () {
               
                var options = $.data(this, "editviewOptions");
                if (options) {
                    if (val != undefined) {

                        if (options.isDirty != val) {
                            options.isDirty = val;
                            $(this).trigger("isDirtyChanged.editview", val);
                        }
                    }
                    rs = rs || options.isDirty;
                }
            });

            return rs;

        }
    };


    $.fn.editview = function (method) {

        // Method calling logic
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.tooltip');
        }

    };


})(jQuery);