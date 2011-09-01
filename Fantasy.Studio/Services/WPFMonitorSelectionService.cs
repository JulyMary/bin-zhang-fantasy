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
                    ISelectionServiceEx ex; 

                    if (_currentSelectionService != null)
                    {
                        this._currentSelectionService.SelectionChanged -= new EventHandler(CurrentSelectionService_SelectionChanged);
                        ex = this._currentSelectionService as ISelectionServiceEx;
                        if (ex != null)
                        {
                            ex.IsReadOnlyChanged -= new EventHandler(CurrentSelectionService_IsReadOnlyChanged);
                        }
                    }
                    _currentSelectionService = value;
                    if (_currentSelectionService != null)
                    {
                        this._currentSelectionService.SelectionChanged += new EventHandler(CurrentSelectionService_SelectionChanged);
                        ex = this._currentSelectionService as ISelectionServiceEx;
                        if (ex != null)
                        {
                            ex.IsReadOnlyChanged += new EventHandler(CurrentSelectionService_IsReadOnlyChanged);
                            this.IsReadOnly = ex.IsReadOnly;

                        }
                        else
                        {
                            this.IsReadOnly = false;
                        }
                    }

                    this.OnSelectionChanged(EventArgs.Empty);
                }
            }
	    }

        void CurrentSelectionService_IsReadOnlyChanged(object sender, EventArgs e)
        {
            this.IsReadOnly = ((ISelectionServiceEx)this._currentSelectionService).IsReadOnly;  
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


        private bool _isReadOnly = false;

        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            private set
            {
                if (_isReadOnly != value)
                {

                    _isReadOnly = value;
                    this.OnIsReadOnlyChanged(EventArgs.Empty);
                }
            }
        }


        public event EventHandler IsReadOnlyChanged;

        protected virtual void OnIsReadOnlyChanged(EventArgs e)
        {
            if (this.IsReadOnlyChanged != null)
            {
                this.IsReadOnlyChanged(this, e);
            }
        }

    }
}
