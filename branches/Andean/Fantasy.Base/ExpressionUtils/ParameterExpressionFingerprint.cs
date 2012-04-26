namespace Fantasy.ExpressionUtil
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

    [SuppressMessage("Microsoft.Usage", "CA2218:OverrideGetHashCodeOnOverridingEquals", Justification="Overrides AddToHashCodeCombiner() instead.")]
    internal sealed class ParameterExpressionFingerprint : ExpressionFingerprint
    {
        public ParameterExpressionFingerprint(ExpressionType nodeType, Type type, int parameterIndex) : base(nodeType, type)
        {
            this.ParameterIndex = parameterIndex;
        }

        internal override void AddToHashCodeCombiner(HashCodeCombiner combiner)
        {
            combiner.AddInt32(this.ParameterIndex);
            base.AddToHashCodeCombiner(combiner);
        }

        public override bool Equals(object obj)
        {
            ParameterExpressionFingerprint fingerprint = obj as ParameterExpressionFingerprint;
            return (((fingerprint != null) && (this.ParameterIndex == fingerprint.ParameterIndex)) && base.Equals((ExpressionFingerprint) fingerprint));
        }

        public int ParameterIndex { get; private set; }
    }
}

