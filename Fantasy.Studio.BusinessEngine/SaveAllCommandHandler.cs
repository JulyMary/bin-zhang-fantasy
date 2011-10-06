using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine
{
    public class SaveAllCommandHandler : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged {add {} remove {}}

        public void Execute(object parameter)
        {

            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            es.BeginUpdate();
            try
            {

                IWorkbench wb = this.Site.GetRequiredService<IWorkbench>();
                foreach (IEditingViewContent ev in wb.Views.OfType<IEditingViewContent>())
                {
                    if (ev.DirtyState == EditingState.Dirty)
                    {
                        ev.Save();
                    }
                }

                //foreach (BusinessAssemblyReference reference in es.GetAssemblyReferenceGroup().References)
                //{
                //    es.SaveOrUpdate(reference);
                //}

               
                es.EndUpdate(true);
            }
            catch
            {
                es.EndUpdate(false);
                throw;
            }
        }

        #endregion
    }
}
