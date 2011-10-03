using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.PatternMatching;

namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{
    public static class NodeExtension
    {
        public static IEnumerable<INode> ChildNodes(this INode parent)
        {
            INode cur = parent.FirstChild;
            while (cur != null)
            {
                yield return cur;
                cur = cur.NextSibling;
            }
        }
    }
}
