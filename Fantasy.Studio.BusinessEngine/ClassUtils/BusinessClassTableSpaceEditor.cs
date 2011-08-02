using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Descriptor;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine
{
    class BusinessClassTableSpaceEditor : ListBoxDropDownTypeEditor, IObjectWithSite
    {


        public IServiceProvider Site { get; set; }
        protected override object[] Items
        {
            get
            {
                IDDLService ddl = this.Site.GetRequiredService<IDDLService>();
                return ddl.TableSpaces;
            }
        }
    }
}
