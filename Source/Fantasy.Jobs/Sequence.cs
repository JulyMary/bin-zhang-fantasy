using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using System.Collections;
using System.Xml.Linq;
using System.Xml;

namespace Fantasy.Jobs
{
    public abstract class Sequence : AbstractInstruction
    {
        [XArray(Serializer = typeof(InstructionsSerializer))]
        private List<IInstruction> _instructions = new List<IInstruction>();
        
        public IList<IInstruction> Instructions
        {
            get
            {
                return _instructions;
            }
        }


#pragma warning disable 169
        [XNamespace]
        protected XmlNamespaceManager _namespaces;
#pragma warning restore 169

        protected internal void ResetSequenceIndex()
        {
            IJob job = (IJob)this.Site.GetService(typeof(IJob));
            job.RuntimeStatus.Local["sequence.current"] = 0;
        }

        protected virtual void ExecuteSequence()
        {
            IJob job = (IJob)this.Site.GetService(typeof(IJob));
            int index = (int)job.RuntimeStatus.Local.GetValue("sequence.current", 0);
            while (index < this.Instructions.Count)
            {
                job.ExecuteInstruction(this.Instructions[index]);

                index++;
                job.RuntimeStatus.Local["sequence.current"] = index;
               
            }
        }

        class InstructionsSerializer : IXCollectionSerializer
        {
        #region IXCollectionSerializer Members

        public void Save(IServiceProvider context, XElement element, System.Collections.IEnumerable collection)
        {
            foreach (object inst in collection)
            {
                Type t = inst.GetType();
                XSerializer ser = new XSerializer(t) { Context = context };
                XElement childElement;
                if (t != typeof(ExecuteTaskInstruction))
                {
                    childElement = ser.Serialize(inst);
                }
                else
                {
                    ExecuteTaskInstruction taskInst = (ExecuteTaskInstruction)inst;
                    //string prefix = element.GetPrefixOfNamespace(taskInst.TaskNamespaceUri);
                    childElement = new XElement((XNamespace)taskInst.TaskNamespaceUri + taskInst.TaskName);
                    ser.Serialize(childElement, taskInst);
                }
                
                element.Add(childElement);
            }
        }

        public System.Collections.IEnumerable Load(IServiceProvider context, XElement element)
        {
            IJob job = (IJob)context.GetService(typeof(IJob));
            ArrayList rs = new ArrayList();
            foreach (XElement childElement in element.Elements())
            {
                Type t = job.ResolveInstructionType(childElement.Name);
                if (Array.IndexOf(t.GetInterfaces(), typeof(IInstruction)) < 0)
                {
                    t = typeof(ExecuteTaskInstruction);
                }
                XSerializer ser = new XSerializer(t) { Context = context };
                object inst = ser.Deserialize(childElement);
                rs.Add(inst);
            }

            return rs;
            
        }

        #endregion
    }
    }


    

}
