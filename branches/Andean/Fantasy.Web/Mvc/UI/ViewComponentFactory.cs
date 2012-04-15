using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Web.Mvc;

namespace Fantasy.Web.Mvc.UI
{
    public class ViewComponentFactory
    {

        public ViewComponentFactory(HtmlHelper htmlHelper, IClientSideScriptFactory clientSideScriptFactory)
        {

        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public HtmlHelper HtmlHelper
        {
            get;
            set;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public IClientSideScriptFactory ClientSideScriptFactory
        {
            get;
            private set;
        }

        private ViewContext ViewContext
        {
            get
            {
                return HtmlHelper.ViewContext;
            }
        }
    }
}