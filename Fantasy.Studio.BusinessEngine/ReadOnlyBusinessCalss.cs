using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.BusinessEngine;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using Fantasy.ServiceModel;
using Fantasy.Studio.Services;
using System.Windows.Input;
using Fantasy.Studio.Descriptor;

namespace Fantasy.Studio.BusinessEngine
{
    internal class ReadOnlyBusinessClass : IReadOnlyAdapter
    {
        public ReadOnlyBusinessClass(BusinessClass @class)
        {
            this.Class = @class;
        }

        public BusinessClass Class { get; private set; }

        public string Name
        {
            get { return this.Class.Name; }
        }

        public Guid Id
        {
            get
            {
                return this.Class.Id;
            }
        }

        public string FullName
        {
            get
            {
                return this.Class.FullName;
            }
        }

        public string CodeName
        {
            get
            {
                return this.Class.CodeName;
            }
        }

        public string FullCodeName
        {
            get
            {
                return this.Class.FullCodeName;
            }
        }
       
    }
}
