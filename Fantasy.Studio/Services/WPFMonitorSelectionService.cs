using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.ComponentModel.Design;

namespace Fantasy.Studio.Services
{
    public class WPFMonitorSelectionService : ServiceBase, IMonitorSelectionService
    {
        private ISelectionService _currentSelectionService;

	    public ISelectionService CurrentSelectionService
	    {
		    get { return _currentSelectionService;}
		    set 
            {
                if (value != _currentSelectionService)
                {

                    if (_currentSelectionService != null)
                    {
                        this._currentSelectionService.SelectionChanged -= new EventHandler(CurrentSelectionService_SelectionChanged);
                    }
                    _currentSelectionService = value;
                    if (_currentSelectionService != null)
                    {
                        this._currentSelectionService.SelectionChanged += new EventHandler(CurrentSelectionService_SelectionChanged);
                    }
                    this.OnSelectionChanged(EventArgs.Empty);
                }
            }
	    }

        void CurrentSelectionService_SelectionChanged(object sender, EventArgs e)
        {
            this.OnSelectionChanged(e);
        }

        public event EventHandler SelectionChanged;

        protected virtual void OnSelectionChanged(EventArgs e)
        {
            if (this.SelectionChanged != null)
            {
                this.SelectionChanged(this, e);
            }
        }


       
    }
}
