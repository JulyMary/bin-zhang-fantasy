namespace Fantasy.ExpressionUtil
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

    [SuppressMessage("Microsoft.Usage", "CA2218:OverrideGetHashCodeOnOverridingEquals", Justification="Overrides AddToHashCodeCombiner() instead.")]
    internal sealed class TypeBinaryExpressionFingerprint : ExpressionFingerprint
    {
        public TypeBinaryExpressionFingerprint(ExpressionType nodeType, Type type, Type typeOperand) : base(nodeType, type)
        {
            this.TypeOperand = typeOperand;
        }

        internal override void AddToHashCodeCombiner(HashCodeCombiner combiner)
        {
            combiner.AddObject(this.TypeOperand);
            base.AddToHashCodeCombiner(combiner);
        }

        public override bool Equals(object obj)
        {
            TypeBinaryExpressionFingerprint fingerprint = obj as TypeBinaryExpressionFingerprint;
            return (((fingerprint != null) && object.Equals(this.TypeOperand, fingerprint.TypeOperand)) && base.Equals((ExpressionFingerprint) fingerprint));
        }

        public Type TypeOperand { get; private set; }
    }
}

