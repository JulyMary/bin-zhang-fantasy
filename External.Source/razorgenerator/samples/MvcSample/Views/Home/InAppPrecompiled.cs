﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcSample.Views.Home
{
    using System.Web.Mvc.Html;


#line 2 "\Views\Home\InAppPrecompiled.cshtml"


#line default
#line hidden

    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Home/InAppPrecompiled.cshtml")]
    public class InAppPrecompiled : System.Web.Mvc.WebViewPage<dynamic>
    {
#line hidden

        public InAppPrecompiled()
        {
        }
        public override void Execute()
        {


            WriteLiteral("\r\n");


            WriteLiteral("\r\n<h3>This is executing from ");



#line 4 "\Views\Home\InAppPrecompiled.cshtml"
            Write(GetType().Assembly.Location);


#line default
#line hidden
            WriteLiteral(".</h3>\r\n\r\n");



#line 6 "\Views\Home\InAppPrecompiled.cshtml"
            using (Html.BeginForm())
            {



#line default
#line hidden
                WriteLiteral("    <label>");



#line 8 "\Views\Home\InAppPrecompiled.cshtml"
                Write(Html.CheckBox("executePrecompiledEngineFirst"));


#line default
#line hidden
                WriteLiteral(" <strong>Precompiled view engine executes first</strong>&nbsp;</label>\r\n");



#line 9 "\Views\Home\InAppPrecompiled.cshtml"



#line default
#line hidden
                WriteLiteral("    <input type=\"submit\" value=\"Submit\" />\r\n");



#line 11 "\Views\Home\InAppPrecompiled.cshtml"

            }


#line default
#line hidden
            WriteLiteral("\r\n");



#line 14 "\Views\Home\InAppPrecompiled.cshtml"
            Write(RenderPage("~/Views/Shared/_ViewTypes.cshtml"));


#line default
#line hidden

        }
    }
}
#pragma warning restore 1591
