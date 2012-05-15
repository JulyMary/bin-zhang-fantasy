﻿//common functions

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

function execute_external_script (src) {
    var exists = false;
    $("head > script").each(function () {
        if ($(this).attr("src").toLowerCase() == src.toLowerCase()) {
            exists = true;
        }
    })
    if (!exists) {
        $.getScript(src);
    }
}

function execute_ajax_scripts(scripts, startup) {
    var args = new Array();
    for(var i = 0; i < scripts.length; i ++)
    {
        var src = scripts[i];
        args[i] = function(){execute_external_script(src)};
    }

    $.when.apply($, args).then(start);

   
}

function append_ajax_script(html) {
    $(html).find("script").each(function () {
        $("head").append(this);
    })
}




