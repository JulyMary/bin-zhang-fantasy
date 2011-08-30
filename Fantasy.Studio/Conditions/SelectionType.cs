using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using Fantasy.Studio.Services;
using System.ComponentModel.Design;

namespace Fantasy.Studio.Conditions
{
    public class SelectionType : ConditionBase
    {

        public SelectionType()
        {
            this.Mode = SelectionTypeMode.Primary;
        }

        public override bool IsValid(object args, IServiceProvider services)
        {
            ISelectionService ss = services.GetService<ISelectionService>();

            if (ss == null)
            {
                IMonitorSelectionService mss = services.GetRequiredService<IMonitorSelectionService>();
                ss = mss.CurrentSelectionService;
            }

            if (ss.PrimarySelection == null)
            {
                return TargetType == null;
            }
            else if(TargetType == null)
            {
                return false;
            }
            else
            {
                switch (this.Mode)
                {
                    case SelectionTypeMode.Primary:
                        return TargetType.IsAssignableFrom(ss.PrimarySelection.GetType());
                        
                    case SelectionTypeMode.All:
                        foreach (object o in ss.GetSelectedComponents())
                        {
                            if (!TargetType.IsAssignableFrom(ss.PrimarySelection.GetType()))
                            {
                                return false;
                            }
                            
                        }
                        return true;
                    case SelectionTypeMode.Any:
                        foreach (object o in ss.GetSelectedComponents())
                        {
                            if (TargetType.IsAssignableFrom(ss.PrimarySelection.GetType()))
                            {
                                return true;
                            }
                          
                        }
                        return false;
                       
                    default:
                        throw new Exception("Absurd");
                }

                
            }
        }

        public Type TargetType { get; set; }

        public SelectionTypeMode Mode { get; set; }

    }

    public enum SelectionTypeMode { Primary, All, Any }
}
