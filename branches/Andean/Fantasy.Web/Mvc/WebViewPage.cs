using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fantasy.Web.Mvc
{
    public class WebViewPage : System.Web.Mvc.WebViewPage
    {
        public override void Execute()
        {
            this.Render();
        }

        public virtual void Render()
        {

        }
    }


    public class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        public override void Execute()
        {
            this.Render();
        }

        public virtual void Render()
        {

        }
    }
}