using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using NHibernate.Linq;

namespace Fantasy.BusinessEngine.Services
{
    public class DefaultBusinessDataTypeRepository : ServiceBase, IBusinessDataTypeRepository
    {

       
        BusinessDataType[] _all;

        public BusinessDataType[] All
        
        {
            get
            {
                if (_all == null)
                {
                    IEntityService es = this.Site.GetRequiredService<IEntityService>();
                    var query = from t in es.DefaultSession.Query<BusinessDataType>() select t;
                    _all = query.ToArray();

                }
                return _all;
            }
        }

        private BusinessDataType _enum;
        public BusinessDataType Enum
        {
            get 
            {
                if (_enum == null)
                {
                    _enum = this.All.Single(t => t.IsEnum);
                }
                return _enum;
            }
        }

        private BusinessDataType _class;
        public BusinessDataType Class
        {
            get
            {
                if (_class == null)
                {
                    _class = this.All.Single(t => t.IsBusinessClass);
                }
                return _class;
            }

        }
    }
}
