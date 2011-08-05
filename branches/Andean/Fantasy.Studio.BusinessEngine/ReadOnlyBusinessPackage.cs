using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.BusinessEngine;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Fantasy.Collection;
using System.Windows;
using Fantasy.AddIns;
using Fantasy.Studio.Services;
using Fantasy.ServiceModel;
using System.Windows.Input;
using System.Windows.Controls;
using Fantasy.Studio.Descriptor;

namespace Fantasy.Studio.BusinessEngine
{
    public class ReadOnlyBusinessPackage : IReadOnlyAdapter
    {
        public BusinessPackage Package {get;private set;}

       
        public ReadOnlyBusinessPackage(BusinessPackage package)
        {
            this.Package = package;
         }

        public string Name
        {
            get { return Package.Name; }
        }

        public Guid Id
        {
            get
            {
                return Package.Id;
            }
        }

        public string FullName
        {
            get
            {
                return Package.FullName;
            }
        }

        public string CodeName
        {
            get
            {
                return Package.CodeName;
            }
        }

        public string FullCodeName
        {
            get
            {
                return Package.FullCodeName;
            }
        }

       
    }
}
