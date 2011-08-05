using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy
{
    public interface IObjectWithSite
    {
        IServiceProvider Site { get; set; }
    }

    public class ObjectWithSite : IObjectWithSite
    {
        public virtual IServiceProvider Site { get; set; }
    }
}
