//common functions

if (!Array.indexOf) {
    Array.prototype.indexOf = function (obj, start) {
        for (var i = (start || 0); i < this.length; i++) {
            if (this[i] == obj) {
                return i;
            }
        }
        return -1;
    }
}

function getUrlEncodedKey(key, query) {
    if (!query)
        query = window.location.search;
    var re = new RegExp("[?|&]" + key + "=(.*?)&");
    var matches = re.exec(query + "&");
    if (!matches || matches.length < 2)
        return "";
    return decodeURIComponent(matches[1].replace("+", " "));
}

function setUrlEncodedKey(key, value, query) {

    query = query || window.location.search;
    var q = query + "&";
    var re = new RegExp("[?|&]" + key + "=.*?&");
    if (!re.test(q))
        q += key + "=" + encodeURI(value);
    else
        q = q.replace(re, "&" + key + "=" + encodeURIComponent(value) + "&");
    q = q.trimStart("&").trimEnd("&");
    return q[0] == "?" ? q : q = "?" + q;
}

String.prototype.trimEnd = function (c) {
    if (c)
        return this.replace(new RegExp(c.escapeRegExp() + "*$"), '');
    return this.replace(/\s+$/, '');
}
String.prototype.trimStart = function (c) {
    if (c)
        return this.replace(new RegExp("^" + c.escapeRegExp() + "*"), '');
    return this.replace(/^\s+/, '');
}
String.prototype.escapeRegExp = function () {
    return this.replace(/[.*+?^${}()|[\]\/\\]/g, "\\$0");
};

//String format like c# style.
if (!String.format) {
    String.format = function (text) {

        //check if there are two arguments in the arguments list

        if (arguments.length <= 1) {
            return text;

        }

        //decrement to move to the second argument in the array

        var tokenCount = arguments.length - 2;

        for (var token = 0; token <= tokenCount; token++) {

            //iterate through the tokens and replace their placeholders from the original text in order

            text = text.replace(new RegExp("\\{" + token + "\\}", "gi"),

                                    arguments[token + 1]);

        }

        return text;

    };
}

if (!String.isNullOrEmpty) {
    String.isNullOrEmpty = function (text) {
        return text == undefined || text == "";
    }
}



function getFunction(code, argNames) {
    var fn = window, parts = (code || "").split(".");
    while (fn && parts.length) {
        fn = fn[parts.shift()];
    }
    if (typeof (fn) === "function") {
        return fn;
    }
    argNames.push(code);
    return Function.constructor.apply(null, argNames);
}



function appendStyleSheet (href) {
    var exists = false;
    $("head > link").each(function () {
        if ($(this).attr("href").toLowerCase() == href.toLowerCase()) {
            exists = true;
        }
    });

    if (!exists) {
        $("head").append(String.format('<link ref="stylesheet" href="{0}" type=text/css" />', href));
    }
}


function execute_ajax_scripts(scripts, startup) {
    var args = new Array();
    var c = 0;
    for(var i = 0; i < scripts.length; i ++)
    {
      
        var exists = false;
        var src = scripts[i];
        $("head > script").each(function () {
            var comparteTo = $(this).attr("src");
            if (comparteTo != undefined && comparteTo.toLowerCase() == src.toLowerCase()) {
                exists = true;

                return false;
            }
        })
        if (!exists) {
            args[c] = $.getScript(src);
            c++;
        }
              
        
    }
    $.when.apply(this, args).then(startup);
}


function Hashtable() {

    this.hashtable = new Array();
    this.clear = function () {
        this.hashtable = new Array();
    };
    this.containsKey = function (key) {
        for (var i in this.hashtable) {
            if (i == key) {
                return true;
            }
        }

        return false;
    };

    this.containsValue = function (value) {
        for (var i in this.hashtable) {
            if (this.hashtable[i] == value) {
                return true;
            }
        }
        return false;
    };

    this.get = function (key) {
        return this.hashtable[key];
    };

    this.isEmpty = function () {
        return this.size == 0 ? true : false;
    };

    this.keys = function () {
        var rs = new Array();
        for (var i in this.hashtable) {
            rs.push(i);
        }

        return rs;
    };

    this.put = function (key, value) {
        if (key == undefined) {
            throw "Key cannot be null.";
        }
        this.hashtable[key] = value;

    };

    this.remove = function (key) {
        var rs = this.hashtable[key];
        this.hashtable.splice(key, 1);
        return rs;
    };

    this.size = function () {
        return this.hashtable.length;
    };

    this.values = function () {
        var rs = new Array();
        for (var i in this.hashtable) {
            rs.push(this.hashtable[i]);
        }
        return rs;
    };

}

jQuery.ajaxSetup({cache:false});











