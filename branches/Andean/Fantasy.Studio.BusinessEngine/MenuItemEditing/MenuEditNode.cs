using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.BusinessEngine.Properties;

namespace Fantasy.Studio.BusinessEngine.MenuItemEditing
{
    public class MenuEditNode
    {

        static MenuEditNode()
        {
            Instance = new MenuEditNode();
        }


        private MenuEditNode()
        {

        }

        public string Name 
        {
            get
            {
                return Resources.MenuEditorName;
            }
        }

        public static MenuEditNode Instance { get; private set; }

    }
}
