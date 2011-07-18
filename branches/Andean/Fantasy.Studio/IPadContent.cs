using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Fantasy.Studio
{
    public interface IPadContent
    {
        string Name { get; }

        /// <summary>
        /// Returns the title of the pad.
        /// </summary>
        string Title
        {
            get;
        }

        /// <summary>
        /// Returns the m_icon bitmap resource name of the pad. May be null, if the pad has no
        /// m_icon defined.
        /// </summary>
        ImageSource Icon
        {
            get;
        }

        /// <summary>
        /// Returns the category (this is used for defining where the menu item to
        /// this pad goes)
        /// </summary>
        string Category
        {
            get;
            set;
        }

        void Initialize();

        /// <summary>
        /// Returns the menu m_shortcut for the view menu item.
        /// </summary>
        InputGestureCollection InputGestures { get; }


        /// <summary>
        /// Returns the Windows.Control for this pad.
        /// </summary>
        UIElement Content
        {
            get;
        }

        /// <summary>
        /// Tries to make the pad visible to the user.
        /// </summary>
        void BringPadToFront();
    }
}
