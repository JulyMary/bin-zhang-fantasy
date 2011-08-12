using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Fantasy.AddIns;
using System.Windows.Input;

namespace Fantasy.Studio.Controls
{
    class ButtonSourceCollections : IUpdateStatus
    {


        public ConditionCollection Conditions { get; set; }

        
        public IButtonBuilder Builder { get; set; }

        public ButtonSourceCollections()
        {
            this.Items = new ObservableCollection<object>();
        }

        public ObservableCollection<object> Items { get; private set; }

        #region IUpdateStatus Members

        public void Update(object caller)
        {
            Items.Clear();
            if (Conditions.GetCurrentConditionFailedAction(caller) == ConditionFailedAction.Nothing)
            {
                foreach (ButtonModel bs  in this.Builder.Build(caller))
                {
                    bs.Conditions = this.Conditions;
                    Items.Add(bs);
                }
            }
            
        }

        #endregion
    }
}
