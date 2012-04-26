using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine.Security;
using Fantasy.AddIns;

namespace Fantasy.BusinessEngine.Services
{
    public class SecurityService : ServiceBase, ISecurityService
    {

        ISecurityProvider[] _providers;



        public override void InitializeService()
        {
            this._providers = AddInTree.Tree.GetTreeNode("fantasy/security/securityproviders").BuildChildItems<ISecurityProvider>(this, this.Site).ToArray();
            base.InitializeService();
        }



        #region ISecurityService Members

        public BusinessObjectSecurity GetObjectSecurity(ObjectSecurityArgs args)
        {
            BusinessClass @class = this.Site.GetRequiredService<IObjectModelService>().FindBusinessClass(args.Object.ClassId);
            BusinessObjectSecurity rs = BusinessObjectSecurity.Create(@class, null, null, null, null);
            foreach (ISecurityProvider provider in this._providers)
            {
                rs &= provider.GetObjectSecurity(args);

            }


            return rs;
        }

        public BusinessObjectSecurity GetClassSecurity(ClassSecurityArgs args)
        {
           
            BusinessObjectSecurity rs = BusinessObjectSecurity.Create(args.Class, null, null, null, null);
            foreach (ISecurityProvider provider in this._providers)
            {
                rs &= provider.GetClassSecurity(args);
            }


            return rs;
        }

        #endregion
    }
}
