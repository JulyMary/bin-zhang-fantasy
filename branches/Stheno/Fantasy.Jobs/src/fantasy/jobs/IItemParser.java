package fantasy.jobs;

public interface IItemParser
{
	TaskItem[] GetItemByCategory(String category) throws Exception;
	TaskItem[] GetItemByNames(String names) throws Exception;
	TaskItem[] ParseItem(String text) throws Exception;
}