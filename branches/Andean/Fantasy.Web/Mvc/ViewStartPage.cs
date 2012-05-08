using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fantasy.Web.Mvc
{
    public class ViewStartPage : System.Web.Mvc.ViewStartPage
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