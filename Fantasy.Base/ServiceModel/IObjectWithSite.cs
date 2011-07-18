using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.ServiceModel
{
    public interface IObjectWithSite
    {
        IServiceProvider Site { get; set; }
    }

    public class ObjectWithSite : IObjectWithSite
    {
        public IServiceProvider Site { get; set; }
    }
}
