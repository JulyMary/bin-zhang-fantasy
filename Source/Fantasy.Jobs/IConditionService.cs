using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Expressions;
using System.Reflection;
using Fantasy.Jobs.Properties;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs
{
    public interface IConditionService
    {
        bool Evaluate(IConditionalObject obj);

        bool Evaluate(string expression);
    }

    public class ConditionService : AbstractService, IConditionService
    {

        #region IConditionService Members
        public bool Evaluate(string condition)
        {
            if (string.IsNullOrWhiteSpace(condition))
            {
                throw new ArgumentException(condition);
            }

            IStringParser parser = this.Site.GetRequiredService<IStringParser>();
            string parsed = condition;
            ILogger logger = (ILogger)this.Site.GetService(typeof(ILogger));
            Dictionary<string, object> ctx = new Dictionary<string, object>() { { "c#-style-string", true } };
            parsed = parser.Parse(parsed, ctx);

            if (!string.IsNullOrWhiteSpace(parsed))
            {
                Expression expr = new Expression(parsed);

                if (expr.Success)
                {
                    expr.InvokeFunction += new EventHandler<InvokeFunctionEventArgs>(ExpressionInvokeFunction);

                    object rs = expr.Eval();
                    if (rs is bool)
                    {
                        if (logger != null)
                        {
                            logger.LogMessage("condition", MessageImportance.Low, "Source: {0}, Parsed: {1}, Value: {2}", condition, parsed, rs);
                        }
                        return (bool)rs;
                    }
                    else
                    {
                        throw new InvalidConditionException(String.Format(Properties.Resources.ConditionNotBoolText, condition, parsed));
                    }
                }
                else
                {
                    throw new InvalidConditionException(String.Format(Properties.Resources.ParseConditionFailedText, condition, parsed));
                }


            }
            else
            {
                return true;
            }
        }



        public bool Evaluate(IConditionalObject obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            if (!string.IsNullOrEmpty(obj.Condition))
            {
                return this.Evaluate(obj.Condition);
            }
            else
            {
                return true;
            }

        }

        private void ExpressionInvokeFunction(object sender, InvokeFunctionEventArgs e)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.IgnoreCase;
            var query = from method in e.Type.GetMethods(flags) where StringComparer.OrdinalIgnoreCase.Compare(method.Name, e.FunctionName) == 0 select method;
            if (query.Any())
            {
                try
                {
                    object o = Activator.CreateInstance(e.Type);
                    if (o is IObjectWithSite)
                    {
                        ((IObjectWithSite)o).Site = this.Site;
                    }
                    e.Result = e.Type.InvokeMember(e.FunctionName, flags, null, o, e.Arguments);
                    e.Handled = true;
                }
                catch (MissingMethodException)
                {
                }
            }
        }

        #endregion
    }
}
