using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using System.Collections.ObjectModel;

namespace Fantasy.Studio.BusinessEngine
{
    public class PropertyEditorModel
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
