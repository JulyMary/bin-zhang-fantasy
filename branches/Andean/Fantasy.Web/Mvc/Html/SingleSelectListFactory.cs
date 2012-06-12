using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;

namespace Fantasy.Web.Mvc.Html
{
    public class SingleSelectListFactory : UserControlFactory<SingleSelectList>
    {
        public SingleSelectListFactory(HtmlHelper htmlHelper)
            :base(htmlHelper)
        {

        }

        public SingleSelectListFactory OptionsText(string optionsText)
        {
            ((SingleSelectList)this.Control).OptionsText = optionsText;
            return this;
        }

        public SingleSelectListFactory OptionsCaption(string optionsCaption)
        {
            ((SingleSelectList)this.Control).OptionsCaption = optionsCaption;
            return this;
        }

        public SingleSelectListFactory OptionsValue(string optionsValue)
        {
            ((SingleSelectList)this.Control).OptionsValue = optionsValue;
            return this;
        }

        public SingleSelectListFactory Items(IEnumerable items)
        {
            ((SingleSelectList)this.Control).Items.AddRange(items.Cast<object>());
            return this;
        }
    }
}