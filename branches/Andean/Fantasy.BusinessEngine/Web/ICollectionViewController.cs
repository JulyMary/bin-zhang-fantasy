using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Windows;
using System.Web.Mvc;
using Fantasy.BusinessEngine;

namespace Fantasy.Web
{
    public interface ICollectionViewController
    {
        ViewResult Default(IEnumerable<BusinessObject> collection);
    }
}