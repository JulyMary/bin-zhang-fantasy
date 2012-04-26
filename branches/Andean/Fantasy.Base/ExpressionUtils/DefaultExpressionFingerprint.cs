namespace Fantasy.ExpressionUtil
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    [SuppressMessage("Microsoft.Usage", "CA2218:OverrideGetHashCodeOnOverridingEquals", Justification="Overrides AddToHashCodeCombiner() instead.")]
    internal sealed class DefaultExpressionFingerprint : ExpressionFingerprint
    {
        public DefaultExpressionFingerprint(ExpressionType nodeType, Type type) : base(nodeType, type)
        {
        }

        public override bool Equals(object obj)
        {
            DefaultExpressionFingerprint fingerprint = obj as DefaultExpressionFingerprint;
            return ((fingerprint != null) && base.Equals((ExpressionFingerprint) fingerprint));
        }
    }
}

