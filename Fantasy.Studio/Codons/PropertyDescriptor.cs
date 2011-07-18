using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using System.Collections;
using Fantasy.Studio.Descriptor;
using System.ComponentModel;

namespace Fantasy.Studio.Codons
{
    public class PropertyDescriptor : CodonBase
    {
        public PropertyDescriptor()
        {
            this.Browsable = true;
            this.ReadOnly = false;

        }

        public override object BuildItem(object owner, IEnumerable subItems, ConditionCollection conditions)
        {
            _conditions = conditions;
            return this;
        }


        public CustomPropertyDescriptor CreateDescriptor(object obj)
        {
            ConditionFailedAction action = _conditions.GetCurrentConditionFailedAction(obj);

            bool browsable = this.Browsable && action != ConditionFailedAction.Exclude;

            bool readOnly = this.ReadOnly || action == ConditionFailedAction.Disable;

            string caption = this.Caption ?? this.Name;

            object editor = this.Editor != null ? Activator.CreateInstance(this.Editor) : null; ;

            return new CustomPropertyDescriptor(this.Name, caption, this.Category, this.Description,
                browsable, readOnly, this.Type, this.Get, this.Set, editor, this.Converter, this.CanResetValue, this.DefaultValueProvider);
        }

        private ConditionCollection _conditions;

        public override bool HandleCondition
        {
            get { return true; }
        }


        public TypeConverter Converter {get;set;}

        public Type Editor  {get;set;}

        public bool ReadOnly { get; set; }

        public string Category { get; set; }

        public bool Browsable { get; set; }

        public string Name { get; set; }

        public string Caption { get; set; }

        public Type Type { get; set; }

        public IGetAction Get { get; set; }

        public ISetAction Set { get; set; }

        public string Description { get; set; }

        public bool CanResetValue { get; set; }

        public IDefaultValueProvider DefaultValueProvider { get; set; }

       


    }
}
