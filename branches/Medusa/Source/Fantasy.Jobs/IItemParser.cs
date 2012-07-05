using System;
namespace Fantasy.Jobs
{
    public interface IItemParser
    {
        TaskItem[] GetItemByCategory(string category);
        TaskItem[] GetItemByNames(string names);
        TaskItem[] ParseItem(string text);
    }
}
