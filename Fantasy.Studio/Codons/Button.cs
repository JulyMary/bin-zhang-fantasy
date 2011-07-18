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

namespace Fantasy.Studio.Codons
{
   
    public class Button : CodonBase
    {
        public Button()
        {
            this.Togglable = false;
        }

        public override bool HandleCondition
        {
            get
            {
                return true;
            }
        }

        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition)
        {

            System.Windows.Input.ICommand command = null;
            
            if (this.Command != null)
            {
                command = this.Command;
            }
            else if (this.CommandType != null)
            {
                object cmd = Activator.CreateInstance(this.CommandType);
                command = ServiceManager.Services.GetRequiredService<IAdapterManager>().GetAdapter<System.Windows.Input.ICommand>(cmd);
            }

            ButtonModel rs = new ButtonModel() { Owner = owner, Icon = this.Icon, Text = this.Text, Conditions=condition, IsCheckable=this.Togglable, Command=command };
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

        private Type  _commandType;
        public Type CommandType
        {
            get { return _commandType; }
            set 
            {
                if (value == null || typeof(System.Windows.Input.ICommand).IsAssignableFrom(value) || typeof(ICommand).IsAssignableFrom(value))  
                {
                    _commandType = value;
                }
                else
                {
                    throw new AddInException(string.Format(Resources.ButtonCommandIsNotICommandText, typeof(System.Windows.Input.ICommand).FullName, typeof(ICommand).FullName));
                }
            }
        }

        public System.Windows.Input.ICommand Command { get; set; }
        
        public bool Togglable { get; set; }
    }
}
