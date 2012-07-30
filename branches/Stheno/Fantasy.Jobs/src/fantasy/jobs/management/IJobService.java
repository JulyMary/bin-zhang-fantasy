package fantasy.jobs.management;

import java.rmi.*;
import java.util.*;

import fantasy.*;
import fantasy.jobs.*;


public interface IJobService extends Remote
{

	String Version();

	JobMetaData StartJob(String startInfo);


	void Resume(UUID id);


	void Cancel(UUID id);

	void Pause(UUID id);

	void Resume(UUID[] ids);

	void Cancel(UUID[] ids);

	void Pause(UUID[] ids);

	void ResumeByFilter(String filter);

	void CancelByFilter(String filter);

	void PauseByFilter(String filter);

	JobMetaData FindJobById(UUID id);

	JobMetaData[] FindUnterminatedJob(RefObject<Integer> totalCount, String filter, String order, int skip, int take);

	JobMetaData[] FindTerminatedJob(RefObject<Integer> totalCount, String filter, String order, int skip, int take);

	int GetTerminatedCount();

	int GetUnterminatedCount();

	String GetJobLog(UUID id);

	String GetManagerLog(java.util.Date date);

	java.util.Date[] GetManagerLogAvaiableDates();


	JobTemplate[] GetJobTemplates();

	String GetJobScript(UUID id);


	String[] GetAllApplications();

	String[] GetAllUsers();


	String GetSettings(String typeName);

	void SetSettings(String typeName, String xml);

	String GetLocation();
	
	void addListener(UUID token, IJobServiceListener listener);
	void removeListener(UUID token);
}