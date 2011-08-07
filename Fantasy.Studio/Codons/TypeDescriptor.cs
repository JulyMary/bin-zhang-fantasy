﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using System.ComponentModel;
using Codons = Fantasy.Studio.Codons;
using Fantasy.Studio.Descriptor;

namespace Fantasy.Studio.Codons
{
    public class TypeDescriptor : CodonBase, IObjectWithSite
    {
        public Type TargetType { get; set; }

        public object Editor { get; set; }

        public IServiceProvider Site { get; set; }

        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition, IServiceProvider services)
        {
            this._conditions = condition;
            this._propertyCodons.AddRange(subItems.Cast<PropertyDescriptor>());
            return this;
        }

        public override bool HandleCondition
        {
            get
            {
                return true;
            }
        }

        public ICustomTypeDescriptor CreateDescriptor(object obj)
        {
            if (!TargetType.IsInstanceOfType(obj))
            {
                return null;
            }

            Fantasy.Studio.Descriptor.CustomTypeDescriptor rs = new Fantasy.Studio.Descriptor.CustomTypeDescriptor(obj) {Site = this.Site };
            foreach (Codons.PropertyDescriptor propCodon in _propertyCodons)
            {
                CustomPropertyDescriptor prop = propCodon.CreateDescriptor(obj);
                if (prop != null)
                {
                    prop.Owner = rs;
                    rs.Properties.Add(prop);
                }
            }

            if (Editor != null && _conditions.GetCurrentConditionFailedAction(obj) == ConditionFailedAction.Nothing)
            {
                rs.Editor = this.Editor;
                if (rs.Editor is IObjectWithSite)
                {
                    ((IObjectWithSite)rs.Editor).Site = this.Site;
                }
            }

            return rs;
        }

        private List<PropertyDescriptor> _propertyCodons = new List<PropertyDescriptor>();
        private ConditionCollection _conditions;
    }
}