using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using Fantasy.AddIns;
using Fantasy.Studio.Codons;
using System.Windows.Input;
using Fantasy.Studio.Controls;

namespace Fantasy.Studio.Services
{
    public class DefaultMenuService : ServiceBase, IMenuService
    {
        #region IMenuService Members

        public UIElement CreateMainMenu(string path, object owner)
        {
            MainMenu rs = new MainMenu();
            IEnumerable<object> items = AddInTree.Tree.GetTreeNode(path).BuildChildItems<object>(owner);
            ButtonBarModel model = new ButtonBarModel(items);
            rs.DataContext = model;
            CommandManager.RequerySuggested += delegate(object sender, EventArgs e)
            {
                model.Update(owner);
            };
            return rs;
        }

        public ContextMenu CreateContextMenu(string path, object owner)
        {
            DefaultContextMenu rs = new DefaultContextMenu();
            IEnumerable<object> items = AddInTree.Tree.GetTreeNode(path).BuildChildItems<object>(owner);
            ButtonBarModel model = new ButtonBarModel(items);
            rs.DataContext = model;
            rs.Opened += delegate(object sender, RoutedEventArgs e)
            {



                model.Update(owner);

                Visibility v = Visibility.Collapsed; 
                foreach (ButtonModel button in model.ChildItems)
                {
                    MenuItem mi = (MenuItem)rs.ItemContainerGenerator.ContainerFromItem(button);

                    if (button.Visible)
                    {
                        v = Visibility.Visible;
                        break;
                    }
                }
                rs.Visibility = v;
            };
            return rs;

        }

        #endregion

        
    }
}
