using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;

namespace Fantasy.BusinessEngine.Services
{
    public class BusinessMenuService : ServiceBase, IBusinessMenuService
    {


        public override void InitializeService()
        {
            IImageListService imageList = this.Site.GetRequiredService<IImageListService>();
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            List<BusinessMenuItem> items = es.Query<BusinessMenuItem>().ToList();
            Root = items.Single(i=>i.Id == BusinessMenuItem.RootId);
            foreach (BusinessMenuItem item in Root.Flatten(i => i.ChildItems))
            {
                if (item.Icon != null)
                {
                    imageList.Register("__menuitem__" + item.Id.ToString(), item.Icon); 
                }
            }
           

            

            base.InitializeService();
        }

        #region IBusinessMenuService Members

        public BusinessMenuItem Root { get; private set; }
        

        #endregion

        #region IBusinessMenuService Members


        public string GetIconKey(BusinessMenuItem item)
        {
            return item.Icon != null ? "__menuitem__" + item.Id.ToString() : string.Empty;
        }

        #endregion
    }
}
