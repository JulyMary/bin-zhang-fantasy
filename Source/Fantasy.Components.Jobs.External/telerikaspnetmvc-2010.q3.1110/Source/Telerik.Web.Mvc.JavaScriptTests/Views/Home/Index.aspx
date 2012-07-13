﻿<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Frameset//EN" "http://www.w3.org/TR/html4/frameset.dtd">
<html>

<head>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
<title>JsUnit Test Runner</title>
<script language="JavaScript" type="text/javascript" src="<%= Url.Content("~/Scripts/jsUnit/app/xbDebug.js")%>"></script>
<script language="JavaScript" type="text/javascript" src="<%= Url.Content("~/Scripts/jsUnit/app/jsUnitCore.js")%>"></script>
<script language="JavaScript" type="text/javascript">
    var DEFAULT_TEST_FRAME_HEIGHT = 250;

    function jsUnitParseParms(string) {
        var i;
        var searchString = unescape(string);
        var parameterHash = new Object();

        if (!searchString) {
            return parameterHash;
        }

        i = searchString.indexOf('?');
        if (i != -1) {
            searchString = searchString.substring(i + 1);
        }

        var parmList = searchString.split('&');
        var a;
        for (i = 0; i < parmList.length; i++) {
            a = parmList[i].split('=');
            a[0] = a[0].toLowerCase();
            if (a.length > 1) {
                parameterHash[a[0]] = a[1];
            }
            else {
                parameterHash[a[0]] = true;
            }
        }
        return parameterHash;
    }

    function jsUnitConstructTestParms() {
        var p;
        var parms = '';

        for (p in jsUnitParmHash) {
            var value = jsUnitParmHash[p];

            if (!value ||
                p == 'testpage' ||
                p == 'autorun' ||
                p == 'coverage' ||
                p == 'submitresults' ||
                p == 'showtestframe' ||
                p == 'resultid') {
                continue;
            }

            if (parms) {
                parms += '&';
            }

            parms += p;

            if (typeof(value) != 'boolean') {
                parms += '=' + value;
            }
        }
        return escape(parms);
    }

    var jsUnitParmHash = jsUnitParseParms(document.location.search);

    // set to true to turn debugging code on, false to turn it off.
    xbDEBUG.on = jsUnitGetParm('debug') ? true : false;
</script>

<script language="JavaScript" type="text/javascript" src="<%= Url.Content("~/Scripts/jsUnit/app/jsUnitTestManager.js")%>"></script>
<script language="JavaScript" type="text/javascript" src="<%= Url.Content("~/Scripts/jsUnit/app/jsUnitTracer.js")%>"></script>
<script language="JavaScript" type="text/javascript" src="<%= Url.Content("~/Scripts/jsUnit/app/jsUnitTestSuite.js")%>"></script>
<script language="JavaScript" type="text/javascript">

    var testManager;
    var utility;
    var tracer;


    if (!Array.prototype.push) {
        Array.prototype.push = function (anObject) {
            this[this.length] = anObject;
        }
    }

    if (!Array.prototype.pop) {
        Array.prototype.pop = function () {
            if (this.length > 0) {
                delete this[this.length - 1];
                this.length--;
            }
        }
    }

    function shouldKickOffTestsAutomatically() {
        return jsUnitGetParm('autorun') == "true";
    }

    function shouldShowTestFrame() {
        return jsUnitGetParm('showtestframe');
    }

    function shouldSubmitResults() {
        return jsUnitGetParm('submitresults');
    }

    function getResultId() {
        if (jsUnitGetParm('resultid'))
            return jsUnitGetParm('resultid');
        return "";
    }

    function submitResults() {
        window.mainFrame.mainData.document.testRunnerForm.runButton.disabled = true;
        window.mainFrame.mainResults.populateHeaderFields(getResultId(), navigator.userAgent, JSUNIT_VERSION, testManager.resolveUserEnteredTestFileName());
        window.mainFrame.mainResults.submitResults();
    }

    function wasResultUrlSpecified() {
        return shouldSubmitResults() && jsUnitGetParm('submitresults') != 'true';
    }

    function getSpecifiedResultUrl() {
        return jsUnitGetParm('submitresults');
    }

    function init() {
        var testRunnerFrameset = document.getElementById('testRunnerFrameset');
        if (shouldShowTestFrame() && testRunnerFrameset) {
            var testFrameHeight;
            if (jsUnitGetParm('showtestframe') == 'true')
                testFrameHeight = DEFAULT_TEST_FRAME_HEIGHT;
            else
                testFrameHeight = jsUnitGetParm('showtestframe');
            testRunnerFrameset.rows = '*,0,' + testFrameHeight;
        }
        testManager = new jsUnitTestManager();
        tracer = new JsUnitTracer(testManager);
        
        if (shouldKickOffTestsAutomatically()) {
            window.mainFrame.mainData.kickOffTests();
        }
    }


</script>
</head>

<frameset id="testRunnerFrameset" rows="*,0,0" border="0" onload="init()">
    <frame frameborder="0" name="mainFrame" src="<%= Url.Content("~/Scripts/jsUnit/app/main-frame.html")%>">
    <frame frameborder="0" name="documentLoader" src="<%= Url.Content("~/Scripts/jsUnit/app/main-loader.html")%>">
    <frame frameborder="0" name="testContainer" src="<%= Url.Content("~/Scripts/jsUnit/app/testContainer.html")%>">
</frameset>
</html>