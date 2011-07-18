using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using System.Windows;
using Fantasy.ServiceModel;

namespace Fantasy.Studio
{
    public class RunAppCommand : CommandBase
    {
        public override object Execute(object caller)
        {
            Application app = new Application();

            DefaultWorkbench w = this.Site.GetRequiredService<DefaultWorkbench>();

            app.Run(w);

            return null;
            
        }
    }
}
