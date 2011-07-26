using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Fantasy.AddIns;
using System.Reflection;

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

           object args = Invoker.Invoke(this.ParameterSource == ParameterSource.Owner ? this.Owner : e.Parameter, this.ParameterMember);
           Handler.Execute(args);
          
        }

        void AddInCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            bool can = false;
            if (this.Condition.GetCurrentConditionFailedAction(this.Owner) == ConditionFailedAction.Nothing)
            {
                object args = Invoker.Invoke(this.ParameterSource == ParameterSource.Owner ? this.Owner : e.Parameter, this.ParameterMember);
                can = Handler.CanExecute(args);
            }

            e.CanExecute = can;
        }

        public object Owner { get; set; }

        public ConditionCollection Condition { get; set; }

        public System.Windows.Input.ICommand Handler { get; set; }

        public ParameterSource ParameterSource { get; set; }


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

        public string ParameterMember { get; set; }
    }
}
