using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Fantasy.Jobs
{
    public interface ITagValueProvider
    {
        char Prefix { get; }
        string GetTagValue(string tag, IDictionary<string, object> context);
        bool HasTag(string tag, IDictionary<string, object> context);
        bool IsEnabled(IDictionary<string, object> context);
    }


   

   



}
