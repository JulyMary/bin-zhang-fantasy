namespace Fantasy.ExpressionUtil
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    [SuppressMessage("Microsoft.Usage", "CA2218:OverrideGetHashCodeOnOverridingEquals", Justification="Overrides AddToHashCodeCombiner() instead.")]
    internal sealed class LambdaExpressionFingerprint : ExpressionFingerprint
    {
        public LambdaExpressionFingerprint(ExpressionType nodeType, Type type) : base(nodeType, type)
        {
        }

        public override bool Equals(object obj)
        {
            LambdaExpressionFingerprint fingerprint = obj as LambdaExpressionFingerprint;
            return ((fingerprint != null) && base.Equals((ExpressionFingerprint) fingerprint));
        }
    }
}

