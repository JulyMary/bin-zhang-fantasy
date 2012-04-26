namespace Fantasy.ExpressionUtil
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    [SuppressMessage("Microsoft.Usage", "CA2218:OverrideGetHashCodeOnOverridingEquals", Justification="Overrides AddToHashCodeCombiner() instead.")]
    internal sealed class IndexExpressionFingerprint : ExpressionFingerprint
    {
        public IndexExpressionFingerprint(ExpressionType nodeType, Type type, PropertyInfo indexer) : base(nodeType, type)
        {
            this.Indexer = indexer;
        }

        internal override void AddToHashCodeCombiner(HashCodeCombiner combiner)
        {
            combiner.AddObject(this.Indexer);
            base.AddToHashCodeCombiner(combiner);
        }

        public override bool Equals(object obj)
        {
            IndexExpressionFingerprint fingerprint = obj as IndexExpressionFingerprint;
            return (((fingerprint != null) && object.Equals(this.Indexer, fingerprint.Indexer)) && base.Equals((ExpressionFingerprint) fingerprint));
        }

        public PropertyInfo Indexer { get; private set; }
    }
}

