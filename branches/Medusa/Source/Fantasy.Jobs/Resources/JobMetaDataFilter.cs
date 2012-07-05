using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Management;
using Fantasy.Jobs;

namespace Fantasy.Jobs.Temporary
{
    public class JobMetaDataFilter : IJobMetaDataFilter
    {

        #region IJobMetaDataFilter Members

        public IEnumerable<JobMetaData> Filter(IQueryable<JobMetaData> source)
        {
            <%=vars%>

            var rs = source;

            
            <%=enableCondition%>rs = from job in rs where  <%=condition%> select job;

            

            <%=enableOrder%>rs = from job in rs orderby <%=order%> select job ; 
            
           
           return rs;
        }

        #endregion
    }
}
