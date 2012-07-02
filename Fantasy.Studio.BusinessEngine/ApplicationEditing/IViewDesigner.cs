using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine
{
    public interface IViewDesigner : IObjectWithSite
    {
        //string DesignTimeHtml { get; }

        //event EventHandler DesignTimeHtmlChanged;

        void LoadSettings(IBusinessEntity entity, XElement settings);

       
        UIElement UI { get; }


    }
}
