package Fantasy.Jobs;

public interface IJob
{
	TaskItem AddTaskItem(String name, String category);
	void RemoveTaskItem(TaskItem item);

	TaskItemGroup AddTaskItemGroup();
	void RemoveTaskItemGroup(TaskItemGroup group);


	TaskItem[] GetEvaluatedItemsByCatetory(String category);

	TaskItem GetEvaluatedItemByName(String name);

	TaskItem[] getItems();

	Guid getID();

	String getTemplateName();

	JobProperty[] getProperties();
	String GetProperty(String name);

	void SetProperty(String name, String value);

	boolean HasProperty(String name);

	void RemoveProperty(String name);
	String getStartupTarget();
	void setStartupTarget(String value);

	java.lang.Class ResolveInstructionType(XName name);

	RuntimeStatus getRuntimeStatus();

	void Execute();

	void ExecuteInstruction(IInstruction instruction);

	void ExecuteTarget(String targetName);



}