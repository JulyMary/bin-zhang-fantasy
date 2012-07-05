using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClickView.Jobs
{
    public class NotCondition : ICondition
    {

        #region ICondition Members

        public bool Evaluate(object owner)
        {
            return !ChildCondition.Evaluate(owner);  
        }

        #endregion

        public ICondition ChildCondition { get; set; }
    }


    public class AndCondition : ICondition
    {
        #region ICondition Members

        public bool Evaluate(object owner)
        {
            foreach (ICondition child in this.ChildConditions)
            {
                if (child.Evaluate(owner) == false)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion


        private List<ICondition> _childConditions = new List<ICondition>();
        public IList<ICondition> ChildConditions
        {
            get
            {
                return _childConditions;
            }
        }
    }

    public class OrCondition : ICondition
    {
        #region ICondition Members

        public bool Evaluate(object owner)
        {
            foreach (ICondition child in this.ChildConditions)
            {
                if (child.Evaluate(owner) == true)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion


        private List<ICondition> _childConditions = new List<ICondition>();
        public IList<ICondition> ChildConditions
        {
            get
            {
                return _childConditions;
            }
        }
    }

}
