﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows;

namespace Fantasy.AddIns.Codons
{
    [System.Windows.Markup.ContentProperty("Template")]
    public class Object : CodonBase, ICodon
    {
        private ObjectBuilder _builder = null;

        [Template("_builder")]
        public object Template { get; set; }

        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition, IServiceProvider services)
        {
            return this._builder != null ? _builder.Build<object>() : this.Template;
        }
    }
}
