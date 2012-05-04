using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Fantasy.XSerialization;

namespace Fantasy.Web.Controllers
{

    [XSerializable("standardNavigationView", NamespaceUri = Consts.Namespace)] 
    public class StandardNavigationViewSettings
    {

        public StandardNavigationViewSettings ()
	    {
            this.ClassSettings = new List<StandardNavigationViewClassSettings>();
            this.Customized = false;
            this.Deep = 1;
	    }

        [XAttribute("customized")]
        public bool Customized { get; set; }


        [XArray,
        XArrayItem(Name="class",Type=typeof(StandardNavigationViewClassSettings))]
        public List<StandardNavigationViewClassSettings> ClassSettings {get;private set;}


        [XAttribute("deep")]
        public int Deep { get; set; }

    }


    [XSerializable("standardNavigationView", NamespaceUri = Consts.Namespace)]  
    public class StandardNavigationViewClassSettings
    {
        public StandardNavigationViewClassSettings()
        {
            DisabledChildRoles = new List<string>();
        }

        [XAttribute("customized")]
        public bool Customized {get;set;}

        [XAttribute("class")]
        public Guid ClassId { get; set; }

        [XArray,
        XArrayItem(Name="role", Type = typeof(string))]
        public List<string> DisabledChildRoles { get; private set; }

    }


   
}