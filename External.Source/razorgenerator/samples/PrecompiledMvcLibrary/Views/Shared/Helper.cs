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

namespace PrecompiledMvcLibrary.Views.Shared
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "1.0.0.0")]
    public static class Helper
    {

        public static System.Web.WebPages.HelperResult ObjectInfo(this HtmlHelper helper, object instance)
        {
            return new System.Web.WebPages.HelperResult(__razor_helper_writer =>
            {



#line 1 "D:\forks\razorsinglefilegenerator\samples\PrecompiledMvcLibrary\Views\Shared\Helper.cshtml"


#line default
#line hidden

                WebViewPage.WriteLiteralTo(@__razor_helper_writer, "    <p>\r\n        ");



#line 3 "D:\forks\razorsinglefilegenerator\samples\PrecompiledMvcLibrary\Views\Shared\Helper.cshtml"
                WebViewPage.WriteTo(@__razor_helper_writer, instance.GetType().Assembly);

#line default
#line hidden

                WebViewPage.WriteLiteralTo(@__razor_helper_writer, " \r\n        <br />\r\n        ");



#line 5 "D:\forks\razorsinglefilegenerator\samples\PrecompiledMvcLibrary\Views\Shared\Helper.cshtml"
                WebViewPage.WriteTo(@__razor_helper_writer, instance.GetType().FullName);

#line default
#line hidden

                WebViewPage.WriteLiteralTo(@__razor_helper_writer, "\r\n    </p>\r\n");



#line 7 "D:\forks\razorsinglefilegenerator\samples\PrecompiledMvcLibrary\Views\Shared\Helper.cshtml"

#line default
#line hidden

            });

        }


        public static System.Web.WebPages.HelperResult WriteList(this HtmlHelper helper, IEnumerable<string> items)
        {
            return new System.Web.WebPages.HelperResult(__razor_helper_writer =>
            {



#line 9 "D:\forks\razorsinglefilegenerator\samples\PrecompiledMvcLibrary\Views\Shared\Helper.cshtml"


#line default
#line hidden

                WebViewPage.WriteLiteralTo(@__razor_helper_writer, "    <ul>\r\n");



#line 11 "D:\forks\razorsinglefilegenerator\samples\PrecompiledMvcLibrary\Views\Shared\Helper.cshtml"
                foreach (var s in items)
                {

#line default
#line hidden

                    WebViewPage.WriteLiteralTo(@__razor_helper_writer, "            <li>\r\n                ");



#line 13 "D:\forks\razorsinglefilegenerator\samples\PrecompiledMvcLibrary\Views\Shared\Helper.cshtml"
                    WebViewPage.WriteTo(@__razor_helper_writer, s);

#line default
#line hidden

                    WebViewPage.WriteLiteralTo(@__razor_helper_writer, " (From helper)\r\n            </li>\r\n");



#line 15 "D:\forks\razorsinglefilegenerator\samples\PrecompiledMvcLibrary\Views\Shared\Helper.cshtml"
                }

#line default
#line hidden

                WebViewPage.WriteLiteralTo(@__razor_helper_writer, "    </ul>\r\n");



#line 17 "D:\forks\razorsinglefilegenerator\samples\PrecompiledMvcLibrary\Views\Shared\Helper.cshtml"

#line default
#line hidden

            });

        }


    }
}
#pragma warning restore 1591
