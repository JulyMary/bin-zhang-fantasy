using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine;
using NHibernate;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine
{
    class ClassViewPadModel : ObjectWithSite
    {

        public TreeViewModel.ExtendableTreeViewModel TreeViewModel { get; private set; }

        public override IServiceProvider Site
        {
            get
            {
                return base.Site;
            }
            set
            {
                base.Site = value;
                if (this.Site != null)
                {
                    this.TreeViewModel = new TreeViewModel.ExtendableTreeViewModel("fantasy/studio/businessengine/classviewpad/treeview", this, this.Site);
                    BusinessClass rootClass = this.Site.GetRequiredService<IEntityService>().DefaultSession.Get<BusinessClass>(BusinessClass.RootClassId);
                    this.TreeViewModel.Items.Add(rootClass);
                }
            }
        }
    }
}
