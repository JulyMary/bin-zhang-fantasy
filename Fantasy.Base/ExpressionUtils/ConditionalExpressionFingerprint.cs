namespace Fantasy.ExpressionUtil
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    [SuppressMessage("Microsoft.Usage", "CA2218:OverrideGetHashCodeOnOverridingEquals", Justification="Overrides AddToHashCodeCombiner() instead.")]
    internal sealed class ConditionalExpressionFingerprint : ExpressionFingerprint
    {
        public ConditionalExpressionFingerprint(ExpressionType nodeType, Type type) : base(nodeType, type)
        {
        }

        public override bool Equals(object obj)
        {
            ConditionalExpressionFingerprint fingerprint = obj as ConditionalExpressionFingerprint;
            return ((fingerprint != null) && base.Equals((ExpressionFingerprint) fingerprint));
        }
    }
}

