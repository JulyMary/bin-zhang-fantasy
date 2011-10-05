using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using Fantasy.Studio.Controls;

namespace Fantasy.Studio.Codons
{
    public class OptionBuilder : CodonBase
    {
        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition, IServiceProvider services)
        {
            IOptionBuilder builder = this._builder != null ? this._builder.Build<IOptionBuilder>() : this.Builder;
            if (builder != null)
            {
                if (builder is IObjectWithSite)
                {
                    ((IObjectWithSite)builder).Site = services;
                }

                return builder.Build(owner);
            }
            else
            {
                return null;
            }
        }

        private ObjectBuilder _builder;
        [Template("_builder")]
        public IOptionBuilder Builder { get; set; }
    }
}
