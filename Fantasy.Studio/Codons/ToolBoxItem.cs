using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using System.Windows.Input;
using Fantasy.Studio.Properties;
using Fantasy.Adaption;
using Fantasy.Studio.Controls;

namespace Fantasy.Studio.Codons
{
    public class ToolBoxItem : CodonBase
    {
        public ToolBoxItem()
        {
            this.Category = Resources.ToolBoxItemDefaultCategory;
        }

        public object Icon { get; set; }

        public string Text { get; set; }

        public string Category { get; set; }

        internal ObjectBuilder _doubleClick = null;
        [Template("_doubleClick")]
        public ICommand DoubleClick { get; set; }

        internal ObjectBuilder _selected = null;
        [Template("_selected")]
        public ICommand Selected { get; set; }

        internal ObjectBuilder _unselected = null;
        [Template("_unselected")]
        public ICommand Unselected { get; set; }

        private ObjectBuilder _doDragDrop = null;
        [Template("_doDragDrop")]
        public ICommand DoDragDrop { get; set; }

        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition, IServiceProvider services)
        {
            ToolBoxItemModel rs = new ToolBoxItemModel()
            {
                 Category = this.Category,
                 Icon=this.Icon,
                 Text = this.Text,
                 DoubleClick = this.CreateCommand(this._doubleClick, this.DoubleClick, services),
                 Selected = this.CreateCommand(this._selected, this.Selected, services),
                 Unselected = this.CreateCommand(this._unselected, this.Unselected, services),
                 DoDragDrop = this.CreateCommand(this._doDragDrop, this.DoDragDrop, services) 
            };
        }

        private ICommand CreateCommand(ObjectBuilder builder, ICommand command, IServiceProvider services)
        {
            ICommand rs = null;
            if (builder != null)
            {
                rs = services.GetRequiredService<IAdapterManager>().GetAdapter<ICommand>(builder.Build<object>());
            }
            else
            {
                rs = command;
            }
            return rs;
        }
    }
}
