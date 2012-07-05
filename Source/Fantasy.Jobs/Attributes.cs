using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Properties;

namespace Fantasy.Jobs
{
    [Flags]
    public enum TaskMemberFlags { Input = 1, Output = 2, Required = 4, Inline = 8}

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TaskMemberAttribute : Attribute
    {
        public TaskMemberAttribute(string name)
        {
            this.Name = name;
            this.Flags = TaskMemberFlags.Input;
            this.ParseInline = true;
        }

        public string Name { get; private set; }

        public TaskMemberFlags Flags { get; set; }

        public bool ParseInline { get; set; }
    
        public string Description { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TaskAttribute : Attribute
    {
        public TaskAttribute(string name, string namespaceUri)
        {
            this.Name = name;
            this.NamespaceUri = namespaceUri; 
        }

        public string Name { get; private set; }
        public string NamespaceUri { get; private set; }

        public string Description { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class InstructionAttribute : Attribute
    {

    }

  






    

    






}
