namespace Fantasy.ExpressionUtil
{
    using System;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

    internal abstract class ExpressionFingerprint
    {
        protected ExpressionFingerprint(ExpressionType nodeType, System.Type type)
        {
            this.NodeType = nodeType;
            this.Type = type;
        }

        internal virtual void AddToHashCodeCombiner(HashCodeCombiner combiner)
        {
            combiner.AddInt32((int) this.NodeType);
            combiner.AddObject(this.Type);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ExpressionFingerprint);
        }

        protected bool Equals(ExpressionFingerprint other)
        {
            return (((other != null) && (this.NodeType == other.NodeType)) && object.Equals(this.Type, other.Type));
        }

        public override int GetHashCode()
        {
            HashCodeCombiner combiner = new HashCodeCombiner();
            this.AddToHashCodeCombiner(combiner);
            return combiner.CombinedHash;
        }

        public ExpressionType NodeType { get; private set; }

        public System.Type Type { get; private set; }
    }
}

