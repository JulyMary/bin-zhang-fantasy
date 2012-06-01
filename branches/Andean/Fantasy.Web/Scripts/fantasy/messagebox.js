var messageBox = new function () {

    this.buttons = {
        OK: 0,
        OKCancel: 1,
        AbortRetryIgnore: 2,
        YesNoCancel: 3,
        YesNo: 4,
        RetryCancel: 5

    };

    this.icons = {
        Asterisk: 0x40,
        Error: 0x10,
        Exclamation: 0x30,
        Hand: 0x10,
        Information: 0x40,
        None: 0,
        Stop: 0x10,
        Warning: 0x30
    };

    this.defaultButtons = {
        Button1: 0,
        Button2: 1,
        Button3: 2
    };

    this.dialogResult = {
        None: 0,
        OK: 1,
        Cancel: 2,
        Abort: 3,
        Retry: 4,
        Ignore: 5,
        Yes: 6,
        No: 7
    };

    this.show = function (options) {


        if (typeof options == 'string') {
            options = { text: options };
        }

        options = $.extend({
            caption: document.title,
            buttons: this.buttons.OK,
            icon: this.icons.None,
            defaultButtons: this.defaultButtons.Button1
        }, options);

        var id = "#messageboxHiddenDiv";
        var div = $(id);
        if (div.length == 0) {
            div = $("<div id=\"messageboxHiddenDiv\" title=\"Message\" class=\"messagebox\"><ins></ins><p></p></div>");
            div.appendTo(document.body);
        }


        var icon=$("ins:first", div);
        icon.attr("class", "");
        switch(options.icon)
        {
            case 0 :
                icon.addClass("icon icon-none");
                break;
            case 0x10 :
                icon.addClass("icon icon-error");
                break;
            case 0x30 :
                icon.addClass("icon icon-warnning");
                break;
            case 0x40 :
                icon.addClass("icon icon-information");
                break;
        }

        $("p:first", div).html(options.text);


       


       
        var buttons;
        switch (options.buttons) {
            case 0: //OK
                buttons = {
                    "OK" : function(){
                        $(this).dialog("close");
                        if(options.callback)
                        {
                            options.callback.apply($(this), 1);
                        }
                    }
                };
                break;
            case 1: //OKCancel
                buttons = {
                    "OK" : function(){
                        $(this).dialog("close");
                        if(options.callback)
                        {
                            options.callback.apply($(this), 1);
                        }
                    },
                    "Cancel" : function(){
                        $(this).dialog("close");
                        if(options.callback)
                        {
                            options.callback.apply($(this), 2);
                        }
                    }
                };
                break;
            case 2: //AbortRetryIgnore
             buttons = {
                    "Abort" : function(){
                        $(this).dialog("close");
                        if(options.callback)
                        {
                            options.callback.apply($(this), 3);
                        }
                    },
                    "Retry" : function(){
                        $(this).dialog("close");
                        if(options.callback)
                        {
                            options.callback.apply($(this), 4);
                        }
                    },
                    "Ignore" : function(){
                        $(this).dialog("close");
                        if(options.callback)
                        {
                            options.callback.apply($(this), 5);
                        }
                    }
                };
                break;
            case 3: //YesNoCancel
             buttons = {
                    "Yes" : function(){
                        $(this).dialog("close");
                        if(options.callback)
                        {
                            options.callback.apply($(this), 6);
                        }
                    },
                    "No" : function(){
                        $(this).dialog("close");
                        if(options.callback)
                        {
                            options.callback.apply($(this), 7);
                        }
                    },
                    "Cancel" : function(){
                        $(this).dialog("close");
                        if(options.callback)
                        {
                            options.callback.apply($(this), 2);
                        }
                    }
                };
                break;
            case 4: //YesNo
                buttons = {
                    "Yes" : function(){
                        $(this).dialog("close");
                        if(options.callback)
                        {
                            options.callback.apply($(this), 6);
                        }
                    },
                    "No" : function(){
                        $(this).dialog("close");
                        if(options.callback)
                        {
                            options.callback.apply($(this), 7);
                        }
                    }
                };
                break;
            case 5: //RetryCancel;
                buttons = {
                    "Retry" : function(){
                        $(this).dialog("close");
                        if(options.callback)
                        {
                            options.callback.apply($(this), 4);
                        }
                    },
                    "Cancel" : function(){
                        $(this).dialog("close");
                        if(options.callback)
                        {
                            options.callback.apply($(this), 2);
                        }
                    }
                };
                break; 
        }

        div.dialog({
            resizable:false,
            modal:true,
            buttons:buttons,
            width:"auto",
            title:options.caption
        });

        var b = $("button", div.closest(".ui-dialog"));
        if(options.defaultButtons < b.length)
        {
            $(b[options.defaultButtons]).focus();
        }
    };

   
}