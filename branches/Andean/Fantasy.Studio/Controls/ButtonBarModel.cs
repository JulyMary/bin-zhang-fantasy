using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Collection;
using Fantasy.Studio.Codons;
using System.Collections;
using System.ComponentModel;
using Fantasy.AddIns;
using Fantasy.Windows;

namespace Fantasy.Studio.Controls
{
    public class ButtonBarModel : NotifyPropertyChangedObject, IUpdateStatus, IObjectWithSite
    {
        public ButtonBarModel(IEnumerable subItems)
        {
            List<object> childCollections = new List<object>();
            foreach (object o in subItems)
            {
                if (o is ButtonSourceCollections)
                {
                    ButtonSourceCollections bsc = (ButtonSourceCollections)o;
                    if (childCollections.Count > 0)
                    {
                        this.ChildItems.Union(childCollections);
                        childCollections = new List<object>();
                    }
                    this.ChildItems.Union(bsc.Items);
                }
                else
                {
                    childCollections.Add(o);
                }
                if (o is IUpdateStatus)
                {
                    this.ChildUpdateStatus.Add((IUpdateStatus)o);
                }
            }

            if (childCollections.Count > 0)
            {
                this.ChildItems.Union(childCollections);
            }
            
        }

        private UnionedObservableCollection<object> _childItems = new UnionedObservableCollection<object>();
        public UnionedObservableCollection<object> ChildItems { get { return _childItems; } }

        private List<IUpdateStatus> _childUpdateStatus = new List<IUpdateStatus>();
        public IList<IUpdateStatus> ChildUpdateStatus { get { return _childUpdateStatus; } }

        

        private bool _visible = true;
        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                if (_visible != value)
                {
                    _visible = value;
                    this.OnPropertyChanged("Visible");
                }
            }
        }


        public ConditionCollection Conditions { get; set; }

        public void Update(object caller)
        {
            if (this.Conditions != null)
            {
                this.Visible = this.Conditions.GetCurrentConditionFailedAction(caller) != ConditionFailedAction.Exclude;
            }
            if (this.Visible)
            {
                foreach (IUpdateStatus child in this.ChildUpdateStatus)
                {
                    child.Update(caller);
                }
            }
        }

        public IServiceProvider Site{ get; set;}

        public System.Windows.IInputElement CommandTarget
        {
            get
            {
                ICommandTargetProvider provider = this.Site.GetService<ICommandTargetProvider>();
                return provider != null ? provider.CommandTarget : null;
               
            }
        }
    }
}
