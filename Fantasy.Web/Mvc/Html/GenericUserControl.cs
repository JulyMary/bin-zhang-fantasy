using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fantasy.Web.Mvc.Html
{
    public class UserControl<TModel> : UserControl
    {
        private ViewDataDictionary<TModel> _viewData;

    // Methods
   

    public override void InitHelpers()
    {
        base.InitHelpers();
        this.Ajax = new AjaxHelper<TModel>(base.ViewContext, this);
        this.Html = new HtmlHelper<TModel>(base.ViewContext, this);
    }

    protected override void SetViewData(ViewDataDictionary viewData)
    {
        this._viewData = new ViewDataDictionary<TModel>(viewData);
        base.SetViewData(this._viewData);
    }


    protected new TModel Model2
    {
        get
        {
            return (TModel)base.Model2; 
        }
    }

    // Properties
    public new AjaxHelper<TModel> Ajax { get; private set; }


    public new HtmlHelper<TModel> Html { get; private set; }
    
    public new TModel Model
    {
        get
        {
            return this.ViewData.Model;
        }
    }

    public new ViewDataDictionary<TModel> ViewData
    {
        get
        {
            if (this._viewData == null)
            {
                this.SetViewData(new ViewDataDictionary<TModel>());
            }
            return this._viewData;
        }
        set
        {
            this.SetViewData(value);
        }
    }

    }
}