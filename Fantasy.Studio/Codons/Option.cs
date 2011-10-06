using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using Fantasy.Studio.Controls;

namespace Fantasy.Studio.Codons
{
    public class Option : CodonBase
    {
        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition, IServiceProvider services)
        {
            IOptionPanel panel = this._panelBuilder != null ? this._panelBuilder.Build<IOptionPanel>() : this.Panel;
            OptionNode rs = new OptionNode() { Caption = Caption, Panel = panel };
            foreach (object o in subItems)
            {
                if (o is OptionNode)
                {
                    rs.ChildNodes.Add((OptionNode)o);
                }
                else if(o is IEnumerable<OptionNode>)
                {
                    foreach (OptionNode childNode in (IEnumerable<OptionNode>)o)
                    {
                        rs.ChildNodes.Add(childNode);
                    }
                }
            }

            return rs;
        }

        private ObjectBuilder _panelBuilder = null;
        [Template("_panelBuilder")]
        public IOptionPanel Panel { get; set; }

        public string Caption { get; set; }

    }
}
