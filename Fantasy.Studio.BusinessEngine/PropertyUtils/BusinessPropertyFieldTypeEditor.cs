﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Windows.Forms;
using Fantasy.BusinessEngine.Services;
using Fantasy.Studio.Descriptor;

namespace Fantasy.Studio.BusinessEngine
{
    class BusinessPropertyFieldTypeEditor : ListBoxDropDownTypeEditor, IObjectWithSite
    {

        public IServiceProvider Site { get; set; }
        protected override object[] Items
        {
            get
            {
                IDDLService ddl = this.Site.GetRequiredService<IDDLService>();
                return ddl.DataTypes;
            }
        }
    }
}
