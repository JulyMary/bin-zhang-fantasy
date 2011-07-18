using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.Adaption;

namespace Fantasy.AddIns
{
    public class RegisterAdaptionService : ServiceBase
    {
        public override void InitializeService()
        {
            IAdapterManager manager = this.Site.GetRequiredService<IAdapterManager>();
            foreach (Adapter codon in AddInTree.Tree.GetTreeNode("fantasy/adaption").BuildChildItems(this))
            {
                if (codon.Adaptees != null && codon.Factory != null)
                {
                    foreach (Type adaptee in codon.Adaptees)
                    {
                        manager.RegisterFactory(codon.Factory, adaptee);
                    }
                }
            }

            base.InitializeService();
        }
    }
}
