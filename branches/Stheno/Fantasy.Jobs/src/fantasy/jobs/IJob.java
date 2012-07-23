package fantasy.jobs;

import java.util.*;

public interface IJob
{
	TaskItem AddTaskItem(String name, String category)throws Exception;
	void RemoveTaskItem(TaskItem item)throws Exception;

	TaskItemGroup AddTaskItemGroup()throws Exception;
	void RemoveTaskItemGroup(TaskItemGroup group)throws Exception;


	TaskItem[] GetEvaluatedItemsByCatetory(String category)throws Exception;

	TaskItem GetEvaluatedItemByName(String name)throws Exception;

	TaskItem[] getItems()throws Exception;

	UUID getID() throws Exception;

	String getTemplateName() throws Exception;

	JobProperty[] getProperties() throws Exception;
	String GetProperty(String name) throws Exception;

	void SetProperty(String name, String value) throws Exception;

	boolean HasProperty(String name) throws Exception;

	void RemoveProperty(String name) throws Exception;
	String getStartupTarget();
	void setStartupTarget(String value) throws Exception;

	@SuppressWarnings("rawtypes")
	Class ResolveInstructionType(String name, String namespace) throws Exception;

	RuntimeStatus getRuntimeStatus() throws Exception;

	void Execute() throws Exception;

	void ExecuteInstruction(IInstruction instruction) throws Exception;

	void ExecuteTarget(String targetName) throws Exception;



}