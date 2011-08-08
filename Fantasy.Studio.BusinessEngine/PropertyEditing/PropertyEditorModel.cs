using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using System.Collections.ObjectModel;
using Fantasy.Collection;

namespace Fantasy.Studio.BusinessEngine.PropertyEditing
{
    class PropertyEditorModel
    {
        public PropertyEditorModel(BusinessClass @class)
        {
            this.Class = @class;
        }

        public BusinessClass Class { get; private set; }

        private ObservableCollection<BusinessProperty> _selected = new ObservableCollection<BusinessProperty>();

        public IList<BusinessProperty> Selected 
        {
            get { return _selected; }
        }
    }
}
