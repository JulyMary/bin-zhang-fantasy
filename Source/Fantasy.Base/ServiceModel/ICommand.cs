using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.ServiceModel
{
    public interface ICommand
    {
        object Execute(object args);
    }
}
