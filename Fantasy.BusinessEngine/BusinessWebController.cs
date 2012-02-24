using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine
{
    public class BusinessWebController : BusinessScriptBase
    {
        public virtual BusinessPackage Package
        {
            get
            {
                return (BusinessPackage)this.GetValue("Package", null);
            }
            set
            {
                this.SetValue("Package", value);
            }
        }

        private IObservableList<BusinessWebView> _persistedViews = new ObservableList<BusinessWebView>();
        protected internal virtual IObservableList<BusinessWebView> PersistedViews
        {
            get { return _persistedViews; }
            private set
            {
                if (_persistedViews != value)
                {
                    _persistedViews = value;
                    _views.Source = value;
                }
            }
        }

        private ObservableListView<BusinessWebView> _views;
        public virtual IObservableList<BusinessWebView> Views
        {
            get
            {
                if (this._views == null)
                {
                    this._views = new ObservableListView<BusinessWebView>(this._persistedViews);
                }
                return _views;
            }
        }
    }
}
