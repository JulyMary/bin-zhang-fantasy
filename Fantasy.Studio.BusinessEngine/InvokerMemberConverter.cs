using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using Fantasy.Adaption;

namespace Fantasy.Studio.BusinessEngine
{
    public class InvokerMemberConverter : TypeConverter, IObjectWithSite
    {

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }


        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            object rs = Invoker.Invoke(value, this.Member);

            if (rs != null)
            {
                if (this.Site != null)
                {
                    
                    IAdapterManager am = this.Site.GetService<IAdapterManager>();
                    if (am != null)
                    {
                        rs = am.GetAdapter<string>(rs);
                    }
                }

                if (!(rs is string))
                {
                    rs = rs.ToString();
                }
                
            }
            return rs;
           
        }


        public string Member { get; set; }

        public IServiceProvider Site
        {
            get;
            set;
        }

        
    }
}
