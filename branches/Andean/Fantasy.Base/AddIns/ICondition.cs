using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.AddIns
{
    public interface ICondition
    {
        ConditionFailedAction Action
        {
            get;
            set;
        }

        /// <summary>
        /// Returns true, when the condition is valid otherwise false.
        /// </summary>
        bool IsValid(object args, IServiceProvider services);

        string Member { get; set; }
    }
}
