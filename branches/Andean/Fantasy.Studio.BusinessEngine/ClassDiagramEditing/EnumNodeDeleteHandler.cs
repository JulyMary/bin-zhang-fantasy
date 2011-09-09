using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.ComponentModel.Design;
using Fantasy.BusinessEngine;
using System.Collections;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class EnumNodeDeleteHandler : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            ISelectionService selectionService = this.Site.GetRequiredService<ISelectionService>();

            bool rs = false;

            Model.EnumGlyph glyph = (Model.EnumGlyph)parameter;
            if (!glyph.IsShortCut)
            {
                ICollection selected = selectionService.GetSelectedComponents();

                if (selected.Count > 0)
                {
                    rs = true;
                    foreach (object o in selectionService.GetSelectedComponents())
                    {
                        BusinessEnumValue ev = o as BusinessEnumValue;
                        if (ev == null || ev.IsSystem || ev.Enum.IsExternal)
                        {
                            rs = false;
                            break;
                        }
                    }
                }
            }

            

            return rs;

        }

        event EventHandler ICommand.CanExecuteChanged { add { } remove { } }

        public void Execute(object parameter)
        {
            ISelectionService selectionService = this.Site.GetRequiredService<ISelectionService>();

            var selected = selectionService.GetSelectedComponents().Cast<object>();

            foreach (object o in selected)
            {
                if (o is BusinessEnumValue)
                {
                    BusinessEnumValue value = o as BusinessEnumValue;
                    value.Enum.EnumValues.Remove(value);
                    value.Enum = null;
                }
            }
        }

        #endregion
    }
}
