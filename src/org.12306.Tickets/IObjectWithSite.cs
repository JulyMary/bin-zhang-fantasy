using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org._12306.Tickets
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
