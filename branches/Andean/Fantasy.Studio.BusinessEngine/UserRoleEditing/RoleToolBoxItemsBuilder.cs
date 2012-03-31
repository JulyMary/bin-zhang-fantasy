using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;
using Fantasy.Studio.Controls;
using System.Windows;

namespace Fantasy.Studio.BusinessEngine.UserRoleEditing
{
    public class RoleToolBoxItemsBuilder : ObjectWithSite, IToolBoxItemsBuilder
    {
        #region IToolBoxItemsBuilder Members

        public ToolBoxItemModel[] BuildItems(object owner)
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            BusinessPackage root = es.GetRootPackage();
            var query = from package in root.Flatten(p => p.ChildPackages)
                        from role in package.Roles
                        orderby role.Name
                        select new ToolBoxItemModel()
                        {
                            Category = "Roles",
                            Icon = "/Fantasy.Studio.BusinessEngine;component/images/role.png",
                            Text = role.Name,
                            DoDragDrop = new DoDragRoleEventHandler(role)
                        };
            return query.ToArray();

        }

        private class DoDragRoleEventHandler : IEventHandler<DoDragDropEventArgs>
        {
            public DoDragRoleEventHandler(BusinessRoleData role)
            {
                this._role = role;
            }

            private BusinessRoleData _role;

            #region IEventHandler<DoDragDropEventArgs> Members

            public void HandleEvent(object sender, DoDragDropEventArgs e)
            {
                e.Data = this._role;
                e.AllowedEffects = DragDropEffects.All;
            }

            #endregion
        }

        #endregion
    }
}
