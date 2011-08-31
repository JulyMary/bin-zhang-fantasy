using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Fantasy.AddIns.Conditions
{
    public class CollectionCount : ConditionBase
    {
        public override bool IsValid(object args, IServiceProvider services)
        {
            int c = 0;
            if (args is ICollection)
            {
                c = ((ICollection)args).Count;  
            }
            else if (args is IEnumerable)
            {
                c = ((IEnumerable)args).Cast<object>().Count();
            }

            switch (this.Operator)
            {
                case NumericalComparisonOperator.Equality:
                    return c == Count;
                case NumericalComparisonOperator.Inequality:
                    return c != Count;
                case NumericalComparisonOperator.LessThan:
                    return c < Count;
                case NumericalComparisonOperator.LessThanOrEqual:
                    return c <= Count;
                case NumericalComparisonOperator.GreaterThan:
                    return c > Count;
                case NumericalComparisonOperator.GreaterThanOrEqual:
                    return c >= Count;
                default:
                    throw new Exception("Absurd!");
            }


        }

        public int Count { get; set; }

        public NumericalComparisonOperator Operator { get; set; }
    }

    


}
