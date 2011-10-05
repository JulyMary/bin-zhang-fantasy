using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Studio.Controls
{
    public class OptionNode : ObjectWithSite
    {
        public OptionNode()
        {
            this.ChildNodes = new List<OptionNode>();
        }

        public string Caption { get; set; }

        private IOptionPanel _panel;

        public IOptionPanel Panel
        {
            get { return _panel; }
            set 
            {
                _panel = value;
                IObjectWithSite ps = _panel as IObjectWithSite;
                if (ps != null && this.Site != null)
                {
                    ps.Site = this.Site;
                }
            }
        }


        public IList<OptionNode> ChildNodes { get; private set; }

        public override IServiceProvider Site
        {
            get
            {
                return base.Site;
            }
            set
            {
                base.Site = value;
                IObjectWithSite ps = Panel as IObjectWithSite; 
                if (ps != null)
                {
                    ps.Site = value;
                }
            }
        }
    }
}
