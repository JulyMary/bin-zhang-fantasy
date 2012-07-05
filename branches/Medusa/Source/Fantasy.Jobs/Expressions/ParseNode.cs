using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using Fantasy.Jobs.Properties;
using System.Reflection;

namespace Fantasy.Jobs.Expressions
{
    partial class ParseNode
    {
        private int GetTokenCount(TokenType tokenType)
        {
            return (from node in this.Nodes where node.Token.Type == tokenType select node).Count();
        }


        private object EvalDECIMAL(ParseTree tree, object[] paramlist)
        {
            string s = this.Token.Text.TrimEnd('f', 'F');
            return Decimal.Parse(s); 
        }

        private object EvalHex(ParseTree tree, object[] paramlist)
        {
            string s = this.Token.Text.Substring(2);
            return Convert.ToInt32(s, 16);
    
        }

        private object EvalINTEGER(ParseTree tree, object[] paramlist)
        {
            return Int32.Parse(this.Token.Text);  
        }

        private object EvalSTRING(ParseTree tree, object[] paramlist)
        {
            Regex reg = new Regex(@"\\(0x(?<hex4>[0-9a-fA-F]{4})|0x(?<hex2>[0-9a-fA-F]{2})|(?<oct>[0-7]{3})|(?<char>.?))");

            string text = this.Token.Text;
            if (text.StartsWith("@"))
            {
                char quotation = text[1]; 
                string rs = text.Substring(2, text.Length - 3).Replace(new string(quotation,2), quotation.ToString());
                return rs;
            }
            else
            {
                text = text.Substring(1, text.Length - 2);
                StringBuilder rs = new StringBuilder();
                int s = 0;
                while (s < text.Length)
                {
                    Match m = reg.Match(text, s);
                    if (m.Success)
                    {
                        rs.Append(text.Substring(s, m.Index - s));

                        char value;
                        if (m.Groups["hex4"].Success)
                        {
                            value = Convert.ToChar(Convert.ToInt32(m.Groups["hex4"].Value, 16));
                        }
                        else if (m.Groups["hex2"].Success)
                        {
                            value = Convert.ToChar(Convert.ToInt32(m.Groups["hex2"].Value, 16));
                        }
                        else if (m.Groups["oct"].Success)
                        {
                            value = Convert.ToChar(Convert.ToInt32(m.Groups["oct"].Value, 8));
                        }
                        else
                        {
                            switch (m.Groups["char"].Value)
                            {
                                case "a":
                                    value = '\a';
                                    break;
                                case "b":
                                    value = '\b';
                                    break;
                                case "f":
                                    value = '\f';
                                    break;
                                case "n":
                                    value = '\n';
                                    break;
                                case "r":
                                    value = '\r';
                                    break;
                                case "t":
                                    value = '\t';
                                    break;
                                case "v":
                                    value = '\v';
                                    break;
                                case "'":
                                    value = '\'';
                                    break;
                                case "\\":
                                    value = '\\';
                                    break;
                                default:
                                    throw new InvalidOperationException(String.Format("Unrecognized escape sequences in string '{0}'.", text));

                            }
                        }
                        rs.Append(value);
                        s = m.Index + m.Length;
                    }
                    else
                    {
                        rs.Append(text.Substring(s));
                        s = text.Length;
                    }
                }

                return rs.ToString();
            }

            
            
        }

        protected virtual object EvalStart(ParseTree tree, params object[] paramlist)
        {
            if (this.GetTokenCount(TokenType.OrExpr) == 1)
            {
                return this.GetValue(tree, TokenType.OrExpr, 0);
            }
            else
            {
                return null; 
            }
 
        }

        protected virtual object EvalOrExpr(ParseTree tree, params object[] paramlist)
        {
            int count = this.GetTokenCount(TokenType.AndExpr);
            if (count == 1)
            {
                return this.GetValue(tree, TokenType.AndExpr, 0);
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    if ((bool)this.GetValue(tree, TokenType.AndExpr, i))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        protected virtual object EvalAndExpr(ParseTree tree, params object[] paramlist)
        {
            int count = this.GetTokenCount(TokenType.NotExpr);
            if (count == 1)
            {
                return this.GetValue(tree, TokenType.NotExpr, 0);
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    if (!(bool)this.GetValue(tree, TokenType.NotExpr, i))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        protected virtual object EvalNotExpr(ParseTree tree, params object[] paramlist)
        {
            object value = this.GetValue(tree, TokenType.CompareExpr, 0);
            if (this.GetTokenCount(TokenType.NOT) == 1)
            {
                return !(bool)value;
            }
            else
            {
                return value;
            }
        }

        protected virtual object EvalCompareExpr(ParseTree tree, params object[] paramlist)
        {
            int count = this.GetTokenCount(TokenType.AddExpr);
            
            object rs = this.GetValue(tree, TokenType.AddExpr, 0);
            for (int i = 1; i < count; i++)
            {
                object val = this.GetValue(tree, TokenType.AddExpr, 1);
                int cmp = Comparer.Default.Compare(rs, val);
                switch ((string)this.GetValue(tree, TokenType.COMPARER, i - 1))
                {
                    case ">=":
                        rs = cmp >= 0;
                        break;
                    case ">":
                        rs = cmp > 0;
                        break;
                    case "<=":
                        rs = cmp <= 0;
                        break;
                    case "<":
                        rs = cmp < 0;
                        break;
                    case "==":
                        rs = cmp == 0;
                        break;
                    case "!=":
                        rs = cmp != 0;
                        break;
                    default:
                        throw new Exception("Absurd");
                }
            }
            return rs;
           
        }

        private string EvalType(object value)
        {

            switch (Convert.GetTypeCode(value))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return "int";
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return "dec";
                case TypeCode.String:
                case TypeCode.Char:
                case TypeCode.Empty:
                    return "str";
                default:
                    return "un";

            }
        }

        protected virtual object EvalAddExpr(ParseTree tree, params object[] paramlist)
        {
            int count = this.GetTokenCount(TokenType.MultExpr);
            if (count > 1)
            {

                object rs = null;
                string oldType = "un";
                for (int i = 0; i < count; i++)
                {
                    object o = this.GetValue(tree, TokenType.MultExpr, i);
                    if (i == 0)
                    {
                        rs = o;
                        oldType = EvalType(rs);
                    }
                    else
                    {
                       
                        string newType = EvalType(o);
                        int op = (string)this.GetValue(tree, TokenType.PLUSMINUS, i - 1) == "+" ? 1 : -1;

                        if (oldType == "int" && newType == "int")
                        {
                            rs = (int)rs + op * (int)o;
                        }
                        else if (oldType == "int" && newType == "dec")
                        {
                            rs = (decimal)rs + op * (decimal)o;
                            oldType = "dec";
                        }
                        else if (oldType == "dec" && (newType == "dec" || newType == "int"))
                        {
                            rs = (decimal)rs + op * (decimal)o;
                        }
                        else if (oldType == "str" && newType == "str")
                        {
                            if (op == 1)
                            {
                                rs = (string)rs + (string)o;
                            }
                            else
                            {
                                throw new InvalidOperationException(Properties.Resources.DoNotSupportMinusStringText);  
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException(String.Format(Properties.Resources.DoNotSupportPlusText, rs ?? "null", o ?? "null"));  
                        }
                        
                    }
                }

                return rs;
            }
            else
            {
                return this.GetValue(tree, TokenType.MultExpr, 0);
            }

        }

        protected virtual object EvalMultExpr(ParseTree tree, params object[] paramlist)
        {
            int count = this.GetTokenCount(TokenType.CastExpr);
            if (count > 1)
            {
                object rs = null;
                string oldType = "un";
                for (int i = 0; i < count; i++)
                {
                    object o = this.GetValue(tree, TokenType.CastExpr, i);
                    if (i == 0)
                    {
                        rs = o;
                        oldType = EvalType(rs);
                    }
                    else
                    {

                        string newType = EvalType(o);
                        string op = (string)this.GetValue(tree, TokenType.MULTDIV, i - 1);

                        switch (op)
                        {
                            case "\\":
                                if (oldType == "int" && newType == "int")
                                {
                                    rs = (int)rs / (int)o;
                                }
                                else
                                {
                                    throw new InvalidOperationException(String.Format(Properties.Resources.DoNotSupportDivideExactlyText, rs ?? "null", o ?? "null"));   
                                }
                                break;
                            case "%":
                                if (oldType == "int" && newType == "int")
                                {
                                    rs = (int)rs % (int)o;
                                }
                                else
                                {
                                    throw new InvalidOperationException(String.Format(Properties.Resources.DoNotSupportModulusText, rs ?? "null", o ?? "null"));
                                }
                                break;
                            case "/":
                                if (oldType == "int" && newType == "int")
                                {
                                    rs = (int)rs / (int)o;
                                }
                                else if ((oldType == "int" || oldType == "dec") && (newType == "int" || newType == "dec"))
                                {
                                    rs = (decimal)rs / (decimal)o;
                                    oldType = "dec";
                                }
                                else
                                {
                                    throw new InvalidOperationException(String.Format(Properties.Resources.DoNotSupportMultDivText, rs ?? "null", o ?? "null"));
                                }
                                break;
                            case "*":
                                if (oldType == "int" && newType == "int")
                                {
                                    rs = (int)rs * (int)o;
                                }
                                else if ((oldType == "int" || oldType == "dec") && (newType == "int" || newType == "dec"))
                                {
                                    rs = (decimal)rs * (decimal)o;
                                    oldType = "dec";
                                }
                                else
                                {
                                    throw new InvalidOperationException(String.Format(Properties.Resources.DoNotSupportMultDivText, rs ?? "null", o ?? "null"));
                                }
                                break; 
                        }

                    }
                }

                return rs;
            }
            else
            {
                return this.GetValue(tree, TokenType.CastExpr, 0);
            }
        }

        protected virtual object EvalNegetiveExpr(ParseTree tree, params object[] paramlist)
        {
            object rs = this.GetValue(tree, TokenType.Atom, 0);
            if ((string)this.GetValue(tree, TokenType.NEGETIVE, 0) == "-")
            {
                string type = this.EvalType(rs);
                switch (type)
                {
                    case "int":
                        return -(int)rs;
                    case "dec":
                        return -(decimal)rs;
                    default:
                        throw new InvalidOperationException(String.Format(Properties.Resources.DoNotSupportNagetiveText, rs ?? "null"));
                }
            }
            else
            {
                return rs;
            }
        }

        protected virtual object EvalAtom(ParseTree tree, params object[] paramlist)
        {
            if (this.GetTokenCount(TokenType.STRING) == 1)
            {
                return this.GetValue(tree, TokenType.STRING, 0);
            }
            else if (this.GetTokenCount(TokenType.Number) == 1)
            {
                return this.GetValue(tree, TokenType.Number, 0);
            }
            else if (this.GetTokenCount(TokenType.Function) == 1)
            {
                return this.GetValue(tree, TokenType.Function, 0);
            }
            else if (this.GetTokenCount(TokenType.BOOL) == 1)
            {
                return this.GetValue(tree, TokenType.BOOL, 0);
            }
            else
            {
                return this.GetValue(tree, TokenType.OrExpr, 0);
            }
        }

        protected virtual object EvalNumber(ParseTree tree, params object[] paramlist)
        {
            if (this.GetTokenCount(TokenType.INTEGER) == 1)
            {
                return this.GetValue(tree, TokenType.INTEGER, 0);
            }
            if (this.GetTokenCount(TokenType.DECIMAL) == 1)
            {
                return this.GetValue(tree, TokenType.DECIMAL, 0);
            }
            else
            {
                return this.GetValue(tree, TokenType.HEX, 0);
            }
        }

        protected virtual object EvalFunction(ParseTree tree, params object[] paramlist)
        {
            int count = this.GetTokenCount(TokenType.IDENTITY);

            string method = (string)this.GetValue(tree, TokenType.IDENTITY, count - 1);

            StringBuilder typeName = new StringBuilder();
            for (int i = 0; i < count - 1; i++)
            {
                if (i > 0)
                {
                    typeName.Append(this.GetValue(tree, TokenType.DOT, i));
                }
                typeName.Append(this.GetValue(tree, TokenType.IDENTITY, i));
               
            }


            Type t = this.ResolveType(typeName.ToString());
            object[] args = (object[])this.GetValue(tree, TokenType.Arguments, 0);
            return tree.OnInvokeFunction(t, method, args);
    
        }

        protected virtual object EvalArguments(ParseTree tree, params object[] paramlist)
        {
            int count = this.GetTokenCount(TokenType.OrExpr);
            object[] rs = new object[count];
            for (int i = 0; i < count; i++)
            {
                rs[i] = this.GetValue(tree, TokenType.OrExpr, i); 
            }

            return rs;
        }

        protected virtual object EvalCastExpr(ParseTree tree, params object[] paramlist)
        {
            object value = this.GetValue(tree, TokenType.NegetiveExpr, 0);
            int count = this.GetTokenCount(TokenType.IDENTITY);
            if (count > 0)
            {
                StringBuilder typeName = new StringBuilder();
                for (int i = 0; i < count; i++)
                {
                    typeName.Append(this.GetValue(tree, TokenType.IDENTITY, i));
                    if (i > 0)
                    {
                        typeName.Append(this.GetValue(tree, TokenType.DOT, i-1));
                    }
                }

                Type t = ResolveType(typeName.ToString());

                value = Convert.ChangeType(value, t);
            }

            return value;
            
        }

        private object EvalBOOL(ParseTree tree, object[] paramlist)
        {
            return bool.Parse(this.Token.Text); 
        }

        protected Type ResolveType(string typeName)
        {
            switch (typeName.ToLower())
            {
                case "int":
                    return typeof(int);
                case "bool":
                    return typeof(bool);
                case "byte":
                    return typeof(byte);
                case "char":
                    return typeof(char);
                case "decimal":
                    return typeof(decimal);
                case "double":
                    return typeof(double);    
                case "float":
                    return typeof(float);
                case "long":
                    return typeof(long); 
                case "object":
                    return typeof(object);    
                case "sbyte":
                    return typeof(sbyte); 
                case "short":
                    return typeof(short);        
                case "string":
                    return typeof(string);          
                case "uint":
                    return typeof(uint);       
                case "ulong":
                    return typeof(ulong);       
                case "ushort":
                    return typeof(ushort);
                default:




                    Type rs = FindTypeInLoadedAssemblies(typeName);
                    if (rs == null)
                    {
                        rs = FindTypeInLoadedAssemblies("System." + typeName);
                    }
                    if (rs != null)
                    {
                        return rs;
                    }
                    else
                    {
                        return Type.GetType(typeName, true, true);
                    }
                    
            }
        }

        private Type FindTypeInLoadedAssemblies(string name)
        {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type t = asm.GetType(name, false);
                if (t != null)
                {
                    return t;
                }
            }

            return null;
        }
    }

   

    partial class ParseTree
    {
        public event EventHandler<InvokeFunctionEventArgs> InvokeFunction;

        public object OnInvokeFunction(Type t, string functionName, object[] arguments)
        {
            if (this.InvokeFunction != null)
            {
                InvokeFunctionEventArgs e = new InvokeFunctionEventArgs(t, functionName, arguments);
                this.InvokeFunction(this, e);
                if (e.Handled)
                {
                    return e.Result;
                }
               
            }
            
            return t.InvokeMember(functionName, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, arguments);
           
            
            
        }


       
        
    }
}
