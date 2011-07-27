using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using System.Windows.Markup;
using w = System.Windows;
using Fantasy.Studio.Properties;
using Fantasy.ServiceModel;
using Fantasy.Adaption;
using System.Reflection;
using System.ComponentModel;

namespace Fantasy.Studio.Codons
{
   

    public class Button : CodonBase
    {
        public Button()
        {
            this.Togglable = false;
            this.ParameterSource = ParameterSource.Parameter;
        }

        public override bool HandleCondition
        {
            get
            {
                return true;
            }
        }

        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition, IServiceProvider services)
        {

            System.Windows.Input.ICommand command = null;
            if (this._commandBuilder != null)
            {
                command = services.GetRequiredService<IAdapterManager>().GetAdapter<w.Input.ICommand>(this._commandBuilder.Build<object>());
            }
            

            ButtonModel rs = new ButtonModel() { Owner = owner, Icon = this.Icon, Text = this.Text, Conditions=condition, IsCheckable=this.Togglable, Command=command};
            rs.CommandParameter =  Invoker.Invoke(this.ParameterSource == ParameterSource.Owner ? owner : this.CommandParameter, this.ParameterMember);

            List<object> childCollections = new List<object>();
            foreach (object o in subItems)
            {
                if (o is ButtonSourceCollections)
                {
                    ButtonSourceCollections bsc = (ButtonSourceCollections)o;
                    if (childCollections.Count > 0)
                    {
                        rs.ChildItems.Union(childCollections);
                        childCollections = new List<object>();
                    }
                    rs.ChildItems.Union(bsc.Items);
                }
                else
                {
                    childCollections.Add(o);
                }
                if (o is IUpdateStatus)
                {
                    rs.ChildUpdateStatus.Add((IUpdateStatus)o);
                }
            }

            if (childCollections.Count > 0)
            {
                rs.ChildItems.Union(childCollections);
            }
            rs.Update(owner);
            return rs;
        }

        public object Icon { get; set; }

        public string Text { get; set; }

        private ObjectBuilder _commandBuilder = null;

        [Template("_commandBuilder")]
        public System.Windows.Input.ICommand Command { get; set; }
        
        public bool Togglable { get; set; }

        public object CommandParameter { get; set; }

        public ParameterSource ParameterSource { get; set; }

        public string ParameterMember { get; set; }
    }
}
