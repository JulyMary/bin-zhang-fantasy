using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine;
using NHibernate;
using Fantasy.BusinessEngine.Services;
using Fantasy.Studio.Controls;

namespace Fantasy.Studio.BusinessEngine
{
    class DocumentPadModel : ObjectWithSite
    {

        public ExtendableTreeViewModel TreeViewModel { get; private set; }

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
                    this.TreeViewModel = new ExtendableTreeViewModel("fantasy/studio/businessengine/documentpad/treeview", this, this.Site);
                    BusinessPackage rootPackage = this.Site.GetRequiredService<IEntityService>().DefaultSession.Get<BusinessPackage>(BusinessPackage.RootPackageId);
                    this.TreeViewModel.Items.Add(rootPackage);
                }
            }
        }
    }
}
