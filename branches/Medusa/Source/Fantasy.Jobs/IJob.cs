using System;
using System.Xml.Linq;
namespace Fantasy.Jobs
{
    public interface IJob
    {
        TaskItem AddTaskItem(string name, string category);
        void RemoveTaskItem(TaskItem item);

        TaskItemGroup AddTaskItemGroup();
        void RemoveTaskItemGroup(TaskItemGroup group);


        TaskItem[] GetEvaluatedItemsByCatetory(string category);

        TaskItem GetEvaluatedItemByName(string name);

        TaskItem[] Items { get; }

        Guid ID { get; }

        string TemplateName { get;}

        JobProperty[] Properties { get; }
        string GetProperty(string name);
        
        void SetProperty(string name, string value);

        bool HasProperty(string name);
        
        void RemoveProperty(string name);
        string StartupTarget { get; set; }

        Type ResolveInstructionType(XName name);

        RuntimeStatus RuntimeStatus { get; }

        void Execute();

        void ExecuteInstruction(IInstruction instruction);

        void ExecuteTarget(string targetName);

        

    }
}
