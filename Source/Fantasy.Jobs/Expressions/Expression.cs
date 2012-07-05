using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Expressions
{
    public class Expression
    {
        private ParseTree _tree;

        public Expression(string input)
        {
            Parser parser = new Parser(new Scanner());
            this._tree = parser.Parse(input);
            this._tree.InvokeFunction += new EventHandler<InvokeFunctionEventArgs>(Tree_InvokeFunction);
          
            
        }

        void Tree_ResolveType(object sender, ResolveTypeEventArgs e)
        {
            this.OnResolveType(e); 
        }

        public event EventHandler<ResolveTypeEventArgs> ResolveType;

        protected virtual void OnResolveType(ResolveTypeEventArgs e)
        {
            if (this.ResolveType != null)
            {
                this.ResolveType(this, e);
            }
        }

        public bool Success
        {
            get
            {
                return this._tree.Errors.Count == 0;
            }
        }

        public ParseError[] Errors
        {
            get
            {
                return this._tree.Errors.ToArray();
            }
        }

        void Tree_InvokeFunction(object sender, InvokeFunctionEventArgs e)
        {
            this.OnInvokeFunction(e);
        }


        public object Eval()
        {
            return this._tree.Eval();
        }

        public event EventHandler<InvokeFunctionEventArgs> InvokeFunction;
 
        protected virtual void OnInvokeFunction(InvokeFunctionEventArgs e) 
        {
            if(this.InvokeFunction != null)
            {
                this.InvokeFunction(this, e);
            }
        }

       




        
    }


    public class ResolveTypeEventArgs : EventArgs
    {
        public ResolveTypeEventArgs(string name)
        {
            this.TypeName = name;
        }

        public string TypeName { get; private set; }

        public Type Type { get; set; }
    }

    public class InvokeFunctionEventArgs : EventArgs
    {

        public InvokeFunctionEventArgs(Type t, string functionName, object[] arguments)
        {
            this.Type = t;
            this.FunctionName = functionName;
            this.Arguments = arguments;
            this.Handled = false;
            this.Result = null;
        }

        public bool Handled { get; set; }

        public Type Type { get; private set; }

        public string FunctionName { get; private set; }

        public object[] Arguments { get; private set; }

        public object Result { get; set; }


    }

    
}
