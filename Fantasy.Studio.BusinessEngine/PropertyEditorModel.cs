using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine
{
    class PropertyEditorModel
    {
        public BusinessClass Class { get; set; }

        private BusinessProperty _selected;

        public BusinessProperty Selected
        {
            get { return _selected; }
            set 
            {
                if (_selected != null)
                {
                    _selected = value;
                    this.OnSelectedChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler SelectedChanged;

        protected virtual void OnSelectedChanged(EventArgs e)
        {
            if (this.SelectedChanged != null)
            {
                this.SelectedChanged(this, e);
            }
        }


        
        
    }
}
