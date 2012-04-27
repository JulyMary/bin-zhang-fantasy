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

            this.NormalizeSecirty(rs);


            return rs;
        }


        private void NormalizeSecirty(BusinessObjectSecurity security)
        {
            if (security.CanCreate == null)
            {
                security.CanCreate = false;
            }

            if (security.CanDelete == null)
            {
                security.CanDelete = false;
            }
            foreach (BusinessObjectMemberSecurity prop in security.Properties)
            {
                if (prop.CanRead == null)
                {
                    prop.CanRead = false;
                }
                if (prop.CanWrite == null)
                {
                    prop.CanWrite = false;
                }
            }
        }

        public BusinessObjectSecurity GetClassSecurity(ClassSecurityArgs args)
        {
           
            BusinessObjectSecurity rs = BusinessObjectSecurity.Create(args.Class, null, null, null, null);
            foreach (ISecurityProvider provider in this._providers)
            {
                rs &= provider.GetClassSecurity(args);
            }

            this.NormalizeSecirty(rs);
            return rs;
        }

        #endregion
    }
}
