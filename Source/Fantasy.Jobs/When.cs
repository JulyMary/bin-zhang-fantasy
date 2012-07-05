using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;

namespace Fantasy.Jobs
{
    [Instruction]
    [XSerializable("when", NamespaceUri=Consts.XNamespaceURI )] 
    public class When : Sequence
    {
        [XAttribute("condition")]
        public string Condition { get; set; }

        public override void Execute()
        {
            this.ExecuteSequence(); 
        }
    }
}
