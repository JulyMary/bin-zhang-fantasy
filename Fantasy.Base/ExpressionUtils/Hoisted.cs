namespace Fantasy.ExpressionUtil
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal delegate TValue Hoisted<TModel, TValue>(TModel model, List<object> capturedConstants);
}

