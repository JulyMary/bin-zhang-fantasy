using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using System.Windows.Input;
using Fantasy.Studio.Controls;

namespace Fantasy.Studio.Codons
{
    public class ToolBar : CodonBase
    {
        public override bool HandleCondition
        {
            get
            {
                return true;
            }
        }


        public string Text { get; set; }

        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition, IServiceProvider services)
        {
            DefaultTooBar rs = new DefaultTooBar() { Caption = this.Text };
            ToolBarModel model = new ToolBarModel(subItems, this.ID) { Conditions = condition, Text=Text };
            //model.Update(owner);
            
            rs.DataContext = model;

            CommandManager.RequerySuggested += delegate(object sender, EventArgs e)
            {
                model.Update(owner);
            };

            return rs;
        }
    }
}
