using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy
{
    public delegate void MethodInvoker();

    public delegate void MethodInvoker<T>(T arg1);

    public delegate void MethodInvoker<T1, T2>(T1 arg1, T2 arg2);

    public delegate void MethodInvoker<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);

    public delegate void MethodInvoker<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

    public delegate void MethodInvoker<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);


}
