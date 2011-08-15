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
        public IEventHandler<DoDragDropEventArgs> DoDragDrop { get; set; }

        private ObjectBuilder _commandParameter = null;
        [Template("_commandParameter")]
        public object CommandParameter { get; set; }


        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition, IServiceProvider services)
        {
            ToolBoxItemModel rs = new ToolBoxItemModel()
            {
                 Category = this.Category,
                 Icon=this.Icon,
                 Text = this.Text,
                 DoubleClick = this.CreateTemplate(this._doubleClick, this.DoubleClick, services),
                 Selected = this.CreateTemplate(this._selected, this.Selected, services),
                 Unselected = this.CreateTemplate(this._unselected, this.Unselected, services),
                
                 
            };

            object cp = this._commandParameter != null ? this._commandParameter.Build<object>() : this.CommandParameter;
            if (cp is IObjectWithSite)
            {
                ((IObjectWithSite)cp).Site = services;
            }

            IEventHandler<DoDragDropEventArgs>  doDragDrop = this._doDragDrop != null ? services.GetRequiredService<IAdapterManager>().GetAdapter<IEventHandler<DoDragDropEventArgs>>(this._doDragDrop.Build<object>()) : this.DoDragDrop;

            if (doDragDrop is IObjectWithSite)
            {
                ((IObjectWithSite)doDragDrop).Site = services;
            }

            rs.DoDragDrop = doDragDrop;

            return rs;
        }

        private ICommand CreateTemplate(ObjectBuilder builder, ICommand command, IServiceProvider services)
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
