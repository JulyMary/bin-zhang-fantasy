using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Fantasy.AddIns;

namespace Fantasy.Studio
{
    class AddInCommandBinding : CommandBinding, IObjectWithSite
    {
        public AddInCommandBinding()
        {
            this.CanExecute += new CanExecuteRoutedEventHandler(AddInCommandBinding_CanExecute);
            this.Executed += new ExecutedRoutedEventHandler(AddInCommandBinding_Executed);
        }

        void AddInCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.Handler is System.Windows.Input.ICommand)
            {
                ((System.Windows.Input.ICommand)Handler).Execute(e.Parameter);
            }
            else
            {
                ((Fantasy.ServiceModel.ICommand)Handler).Execute(this.Owner);
            }
        }

        void AddInCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            bool can = false;
            if (this.Condition.GetCurrentConditionFailedAction(this.Owner) == ConditionFailedAction.Nothing)
            {
                if (Handler is System.Windows.Input.ICommand)
                {
                    can = ((System.Windows.Input.ICommand)Handler).CanExecute(e.Parameter);
                }
                else
                {
                    can = true;
                }
            }

            e.CanExecute = can;
        }

        public object Owner { get; set; }

        public ConditionCollection Condition { get; set; }

        public object Handler { get; set; }


        #region IObjectWithSite Members

        private IServiceProvider _site;
        public IServiceProvider Site
        {
            get
            {
                return _site;
            }
            set
            {
                _site = value;
                IObjectWithSite handler = this.Handler as IObjectWithSite;
                if (handler != null)
                {
                    handler.Site = value;
                }
            }
        }

        #endregion
    }
}
