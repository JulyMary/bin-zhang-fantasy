namespace Fantasy.ExpressionUtil
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    internal sealed class HoistingExpressionVisitor<TIn, TOut> : ExpressionVisitor
    {
        private static readonly ParameterExpression _hoistedConstantsParamExpr;
        private int _numConstantsProcessed;

        static HoistingExpressionVisitor()
        {
            HoistingExpressionVisitor<TIn, TOut>._hoistedConstantsParamExpr = Expression.Parameter(typeof(List<object>), "hoistedConstants");
        }

        private HoistingExpressionVisitor()
        {
        }

        public static Expression<Hoisted<TIn, TOut>> Hoist(Expression<Func<TIn, TOut>> expr)
        {
            HoistingExpressionVisitor<TIn, TOut> visitor = new HoistingExpressionVisitor<TIn, TOut>();
            return Expression.Lambda<Hoisted<TIn, TOut>>(visitor.Visit(expr.Body), new ParameterExpression[] { expr.Parameters[0], HoistingExpressionVisitor<TIn, TOut>._hoistedConstantsParamExpr });
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            return Expression.Convert(Expression.Property(HoistingExpressionVisitor<TIn, TOut>._hoistedConstantsParamExpr, "Item", new Expression[] { Expression.Constant(this._numConstantsProcessed++) }), node.Type);
        }
    }
}

