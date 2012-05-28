using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fantasy.Web.Mvc.Html
{
    partial class TextEditor
    {

        protected override void PreExecute()
        {

            this.AddClass("text-editor");
            
           
            base.PreExecute();
        }

       
    }
}