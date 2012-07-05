using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClickView.XSerialization;

namespace ClickView.Jobs
{
    [Instruction]
    [XSerializable("chance", NamespaceUri = Consts.XNamespaceURI)]  
    public class Chance : Sequence 
    {
        public override void Execute()
        {
            base.ExecuteSequence();
        }
    }
}
