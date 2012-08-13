package fantasy.jobs.scheduling;

import java.rmi.Remote;

public interface IScheduleLibraryService extends Remote
{

	String[] getGroups(String path) throws Exception;

	void createGroup(String path) throws Exception;

	void deleteGroup(String path) throws Exception;

	void createSchedule(String path, ScheduleItem schedule, boolean overwrite) throws Exception;

	void deleteSchedule(String path) throws Exception;

	String[] getScheduleNames(String path) throws Exception;


	ScheduleItem getSchedule(String path) throws Exception;

	ScheduleEvent[] getScheduleHistory(String path) throws Exception;

	String[] getTemplateNames() throws Exception;

	String getTemplate(String name) throws Exception;

}