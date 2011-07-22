using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Studio
{
    public class CommandTargetProvider : ICommandTargetProvider
    {

        public CommandTargetProvider(System.Windows.IInputElement commandTarget)
        {
            this.CommandTarget = commandTarget;
        }

        #region ICommandTargetProvider Members

        public System.Windows.IInputElement CommandTarget
        {
            get;
            private set;
        }

        #endregion
    }
}
