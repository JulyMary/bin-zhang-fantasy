using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy
{

    public interface IEventHandler
    {
        void HandleEvent(object sender, EventArgs e);
    }

    public interface IEventHandler<T> where T: EventArgs
    {
        void HandleEvent(object sender, T e);
    }
}
