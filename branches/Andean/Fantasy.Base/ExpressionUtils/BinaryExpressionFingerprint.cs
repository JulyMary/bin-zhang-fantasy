namespace Fantasy.ExpressionUtil
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    [SuppressMessage("Microsoft.Usage", "CA2218:OverrideGetHashCodeOnOverridingEquals", Justification="Overrides AddToHashCodeCombiner() instead.")]
    internal sealed class BinaryExpressionFingerprint : ExpressionFingerprint
    {
        public BinaryExpressionFingerprint(ExpressionType nodeType, Type type, MethodInfo method) : base(nodeType, type)
        {
            this.Method = method;
        }

        internal override void AddToHashCodeCombiner(HashCodeCombiner combiner)
        {
            combiner.AddObject(this.Method);
            base.AddToHashCodeCombiner(combiner);
        }

        public override bool Equals(object obj)
        {
            BinaryExpressionFingerprint fingerprint = obj as BinaryExpressionFingerprint;
            return (((fingerprint != null) && object.Equals(this.Method, fingerprint.Method)) && base.Equals((ExpressionFingerprint) fingerprint));
        }

        public MethodInfo Method { get; private set; }
    }
}

