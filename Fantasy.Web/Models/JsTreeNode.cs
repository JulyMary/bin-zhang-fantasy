using System.Collections.Generic;

namespace Fantasy.Web.Models
{

    /// <summary>
    /// Please reference <see cref="http://www.jstree.com/documentation/json_data"/>
    /// </summary>
    public class JsTreeNode
    {
       

        public JsTreeNode()
        {
            this.data = new JsTreeNodeData();
            this.children = new List<JsTreeNode>();
            this.state = Closed;
        }


        public JsTreeNodeData data { get; set; }

        public object metadata { get; set; }

        public List<JsTreeNode> children { get; private set; }

        public override string ToString()
        {
            return this.data.title;
        }


        public class JsTreeNodeData
        {
             public string title { get; set; }

             public object attr { get; set; }

             public string icon { get; set; }
        }

        public string state { get; set; }


        public const string Closed = "closed";

        public const string Open = "open";


      
    
    }


   
   


}