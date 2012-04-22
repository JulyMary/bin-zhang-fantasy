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

        public ViewComponentFactory(HtmlHelper htmlHelper)
        {
            this.HtmlHelper = htmlHelper;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public HtmlHelper HtmlHelper
        {
            get;
            set;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public HtmlAssets HtmlAssets
        {
            get
            {
                return HtmlAssets.GetInstance(this.HtmlHelper); 
            }
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