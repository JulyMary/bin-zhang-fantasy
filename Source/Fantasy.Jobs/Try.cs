using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;

namespace Fantasy.Jobs
{
    [Instruction]
    [XSerializable("try", NamespaceUri = Consts.XNamespaceURI)]  
    internal class Try : Sequence, IConditionalObject 
    {
        public override void Execute()
        {
            base.ExecuteSequence();
        }


        [XAttribute("condition")]
        private string _condition = null;
        string IConditionalObject.Condition
        {
            get
            {
                return this._condition;
            }
            set
            {
                this._condition = value;
            }
        }
       
    }
}
