﻿// (c) Copyright 2002-2009 Telerik 
// This source is subject to the GNU General Public License, version 2
// See http://www.gnu.org/licenses/gpl-2.0.html. 
// All other rights reserved.

namespace Telerik.Web.Mvc.UI
{
    using System.Web.Mvc;
    using Telerik.Web.Mvc.Extensions;
    using Telerik.Web.Mvc.Infrastructure;

    public class AutoCompleteHtmlBuilder : IAutoCompleteHtmlBuilder
    {

        public AutoCompleteHtmlBuilder(AutoComplete component)
        {
            this.Component = component;
        }

        public AutoComplete Component
        {
            get;
            private set;
        }

        public IHtmlNode Build()
        {
            string value = Component.ViewContext.Controller.ValueOf<string>(Component.Name);
            if (string.IsNullOrEmpty(value))
            {
                object viewDataValue = Component.ViewContext.ViewData.Eval(Component.Name);
                value = viewDataValue != null ? viewDataValue.ToString() : "";
            }

            return new HtmlTag("input", TagRenderMode.SelfClosing)
                        .Attributes(new
                        {
                            id = Component.Id,
                            name = Component.Name
                        })
                        .ToggleAttribute("disabled", "disabled", !Component.Enabled)
                        .Attributes(Component.HtmlAttributes)
                        .PrependClass(UIPrimitives.Widget, "t-autocomplete", UIPrimitives.Input)
                        .ToggleAttribute("value", value, value.HasValue());
        }
    }
}