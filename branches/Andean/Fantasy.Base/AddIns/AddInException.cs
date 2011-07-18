using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.AddIns
{
    [Serializable]
    public class AddInException : Exception
    {
        public AddInException(string message) : base(message)
        {

        }
    }
}
