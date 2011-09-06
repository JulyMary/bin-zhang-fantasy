using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using System.Collections.ObjectModel;

namespace Fantasy.Studio.BusinessEngine.EnumEditing
{
    public class EnumValuesPanelModel
    {
        public EnumValuesPanelModel(BusinessEnum @enum)
        {
            this.Enum = @enum;
            this._enumValues = @enum.EnumValues.ToSorted("Value");
        }

        public BusinessEnum Enum { get; private set; }


        private IEnumerable<BusinessEnumValue> _enumValues = null;
        public IEnumerable<BusinessEnumValue> EnumValues
        {
            get
            {
                return _enumValues;
            }
        }


        private ObservableCollection<BusinessEnumValue> _selected = new ObservableCollection<BusinessEnumValue>();

        public IList<BusinessEnumValue> Selected
        {
            get { return _selected; }
        }
    }
}
