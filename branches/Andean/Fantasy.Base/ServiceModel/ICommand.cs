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

    public abstract class CommandBase : ObjectWithSite, ICommand
    {
        public abstract object Execute(object caller);

    }
}
