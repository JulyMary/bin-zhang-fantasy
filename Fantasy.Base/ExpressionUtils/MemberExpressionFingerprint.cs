namespace Fantasy.ExpressionUtil
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    [SuppressMessage("Microsoft.Usage", "CA2218:OverrideGetHashCodeOnOverridingEquals", Justification="Overrides AddToHashCodeCombiner() instead.")]
    internal sealed class MemberExpressionFingerprint : ExpressionFingerprint
    {
        public MemberExpressionFingerprint(ExpressionType nodeType, Type type, MemberInfo member) : base(nodeType, type)
        {
            this.Member = member;
        }

        internal override void AddToHashCodeCombiner(HashCodeCombiner combiner)
        {
            combiner.AddObject(this.Member);
            base.AddToHashCodeCombiner(combiner);
        }

        public override bool Equals(object obj)
        {
            MemberExpressionFingerprint fingerprint = obj as MemberExpressionFingerprint;
            return (((fingerprint != null) && object.Equals(this.Member, fingerprint.Member)) && base.Equals((ExpressionFingerprint) fingerprint));
        }

        public MemberInfo Member { get; private set; }
    }
}

