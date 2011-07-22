using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.Adaption;
using Fantasy.AddIns;
using Fantasy.AddIns.Codons;

namespace Fantasy.AddIns.Services
{
    public class RegisterAdaptionService : ServiceBase
    {
        public override void InitializeService()
        {
            IAdapterManager manager = this.Site.GetRequiredService<IAdapterManager>();
            foreach (Adapter codon in AddInTree.Tree.GetTreeNode("fantasy/adaption").BuildChildItems(this, this.Site))
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
