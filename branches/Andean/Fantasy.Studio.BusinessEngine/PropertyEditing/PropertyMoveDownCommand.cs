﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.PropertyEditing
{
    public class PropertyMoveDownCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            
            PropertyEditorModel model = (PropertyEditorModel)parameter;
            return model.Selected.Count == 1 && model.Class.Properties[model.Class.Properties.Count - 1] != model.Selected[0];
        }

        public event EventHandler CanExecuteChanged { add { } remove { } }

        public void Execute(object parameter)
        {
            PropertyEditorModel model = (PropertyEditorModel)parameter;

            BusinessProperty from = model.Selected[0];

            int index = model.Class.Properties.IndexOf(from);

            BusinessProperty to = model.Class.Properties[index + 1];

            long temp = from.DisplayOrder;
            from.DisplayOrder = to.DisplayOrder;
            to.DisplayOrder = temp;

            model.Class.Properties.Swap(index, index + 1);

        }

        #endregion
    }
}
