using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Fantasy.Web.Design
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PreviewImageAttribute : Attribute
    {
        public PreviewImageAttribute(Image image)
        {
            this.Image = image;
        }

        Image Image { get; set; }
    }
}
