using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using Fantasy.ServiceModel;
using Fantasy.Studio.Services;
using System.Reflection;

namespace Fantasy.Studio
{
    [MarkupExtensionReturnTypeAttribute(typeof(System.Windows.Input.ICommand))]
    public class CommandExtension : MarkupExtension
    {

        public CommandExtension()
        {

        }


        public string Name { get; set; }

        public CommandExtension(string name)
        {
            this.Name = name;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            IRoutedCommandService svc = ServiceManager.Services.GetService<IRoutedCommandService>();
            if (svc != null)
            {
                return svc.FindCommand(this.Name);
            }
            else
            {
                IProvideValueTarget target = serviceProvider.GetRequiredService<IProvideValueTarget>();
                this._targetObject = target.TargetObject;
                this._targetProperty = (PropertyInfo)target.TargetProperty; 
                _deferred.Add(this);
                return null;
            }

            
        }

        private object _targetObject;
        private PropertyInfo _targetProperty;

        private static List<CommandExtension> _deferred = new List<CommandExtension>();


        public static void DefferSetValues(IRoutedCommandService service)
        {
            foreach (CommandExtension ext in _deferred)
            {
                System.Windows.Input.ICommand command = service.FindCommand(ext.Name);
                ext._targetProperty.SetValue(ext._targetObject, command, null);
            }

            _deferred.Clear();
        }
    }



}
