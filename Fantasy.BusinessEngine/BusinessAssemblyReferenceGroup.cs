using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine
{
    public class BusinessAssemblyReferenceGroup : BusinessEntity
    {
        private IObservableList<BusinessAssemblyReference> _persistedReferences = new ObservableList<BusinessAssemblyReference>();
        protected internal virtual IObservableList<BusinessAssemblyReference> PersistedReferences
        {
            get { return _persistedReferences; }
            private set
            {
                if (_persistedReferences != value)
                {
                    _persistedReferences = value;
                    _references.Source = value;
                }
            }
        }

        private ObservableListView<BusinessAssemblyReference> _references;
        public virtual IObservableList<BusinessAssemblyReference> References
        {
            get
            {
                if (this._references == null)
                {
                    this._references = new ObservableListView<BusinessAssemblyReference>(this._persistedReferences);
                }
                return _references;
            }
        }


        public static readonly Guid RootId = new Guid("90390416-a147-4d0a-aa59-837bdb4a5228");
    }
}
