package fantasy.jobs;

public interface IItemParser
{
	TaskItem[] GetItemByCategory(String category);
	TaskItem[] GetItemByNames(String names);
	TaskItem[] ParseItem(String text);
}