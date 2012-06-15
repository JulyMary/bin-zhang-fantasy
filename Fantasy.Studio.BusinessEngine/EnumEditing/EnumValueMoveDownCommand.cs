using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.EnumEditing
{
    public class EnumValueMoveDownCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public bool CanExecute(object parameter)
        {

            EnumValuesPanelModel model = (EnumValuesPanelModel)parameter;
            return model.Selected.Count == 1 && model.Enum.EnumValues[model.Enum.EnumValues.Count - 1] != model.Selected[0];
        }

        public event EventHandler CanExecuteChanged { add { } remove { } }

        public void Execute(object parameter)
        {
            EnumValuesPanelModel model = (EnumValuesPanelModel)parameter;

            BusinessEnumValue from = model.Selected[0];

            int index = model.Enum.EnumValues.IndexOf(from);

            BusinessEnumValue to = model.Enum.EnumValues[index + 1];

            long temp = from.DisplayOrder;
            from.DisplayOrder = to.DisplayOrder;
            to.DisplayOrder = temp;

            model.Enum.EnumValues.Swap(index, index + 1);

        }

        #endregion
    }
}
