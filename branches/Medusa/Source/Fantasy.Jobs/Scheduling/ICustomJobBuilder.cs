using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Fantasy.Jobs.Scheduling
{
    public interface  ICustomJobBuilder : IObjectWithSite
    {
        IEnumerable<string> Execute(XElement args);
    }
}
