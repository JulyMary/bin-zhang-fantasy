using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;


using Fantasy.XSerialization;

namespace Fantasy.Jobs
{

    public interface IInstruction : IObjectWithSite
    {
        void Execute();
    }

    public abstract class AbstractInstruction : IInstruction, IObjectWithSite
    {

        #region IInstructment Members

        public abstract void Execute();

        public IServiceProvider Site
        {
            get;
            set;
        }

        #endregion

        #region IConditionalObject Members


        #endregion

        

    }
}
