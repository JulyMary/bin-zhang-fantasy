using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;

namespace Fantasy.Studio.Controls
{
    public class DoubleClickToggleButton : ToggleButton
    {

        protected override void OnToggle()
        {
            if (!this._isSingleClick)
            {
                base.OnToggle();
            }
        }

        protected override void OnMouseDoubleClick(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            this.OnToggle();
        }

        private bool _isSingleClick = false;

        protected override void OnClick()
        {
            this._isSingleClick = true;
            try
            {
                base.OnClick();
            }
            finally
            {
                this._isSingleClick = false;
            }
            
        }
    }
}
