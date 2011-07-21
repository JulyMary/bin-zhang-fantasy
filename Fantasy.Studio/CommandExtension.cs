using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using Fantasy.ServiceModel;
using Fantasy.Studio.Services;

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
            return ServiceManager.Services.GetRequiredService<IRoutedCommandService>().FindCommand(this.Name);
        }
    }



}
