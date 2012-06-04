using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Security
{
    class _SecurityLayer : ISecurityProvider
    {
        private IEnumerable<ISecurityProvider> _prividers;

        public _SecurityLayer(IEnumerable<ISecurityProvider> prividers)
        {
            this._prividers = prividers;
        }

        #region ISecurityProvider Members

        public BusinessObjectSecurity GetObjectSecurity(ObjectSecurityArgs args)
        {
            BusinessObjectSecurity rs = null;
            foreach (ISecurityProvider childProvider in _prividers)
            {
                if (rs == null)
                {
                    rs = childProvider.GetObjectSecurity(args);
                }
                else
                {
                    rs |= childProvider.GetObjectSecurity(args);
                }
            }
            return rs;
        }

        public BusinessObjectSecurity GetClassSecurity(ClassSecurityArgs args)
        {
            BusinessObjectSecurity rs = null;
            foreach (ISecurityProvider childProvider in _prividers)
            {
                if (rs == null)
                {
                    rs = childProvider.GetClassSecurity(args);
                }
                else
                {
                    rs |= childProvider.GetClassSecurity(args);
                }
            }
            return rs;
        }

        #endregion
    }
}
