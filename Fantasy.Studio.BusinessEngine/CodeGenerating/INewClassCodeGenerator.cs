using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{
    public interface INewClassCodeGenerator
    {

        string ProcessTemplate(string template, IDictionary<string, object> parameters);

       
    }
}
