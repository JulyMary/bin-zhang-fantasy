namespace Fantasy.ExpressionUtil
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    [SuppressMessage("Microsoft.Usage", "CA2218:OverrideGetHashCodeOnOverridingEquals", Justification="Overrides AddToHashCodeCombiner() instead.")]
    internal sealed class MethodCallExpressionFingerprint : ExpressionFingerprint
    {
        public MethodCallExpressionFingerprint(ExpressionType nodeType, Type type, MethodInfo method) : base(nodeType, type)
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
            MethodCallExpressionFingerprint fingerprint = obj as MethodCallExpressionFingerprint;
            return (((fingerprint != null) && object.Equals(this.Method, fingerprint.Method)) && base.Equals((ExpressionFingerprint) fingerprint));
        }

        public MethodInfo Method { get; private set; }
    }
}

