package fantasy.jobs.management;

import java.rmi.*;
import java.util.*;


import fantasy.jobs.*;


public interface IJobService extends Remote
{

	String Version() throws Exception;

	JobMetaData StartJob(String startInfo) throws Exception;


	void Resume(UUID id) throws Exception;


	void Cancel(UUID id) throws Exception;

	void Pause(UUID id) throws Exception;

	void Resume(UUID[] ids) throws Exception;

	void Cancel(UUID[] ids) throws Exception;

	void Pause(UUID[] ids) throws Exception;

	void ResumeByFilter(String filter) throws Exception;

	void CancelByFilter(String filter) throws Exception;

	void PauseByFilter(String filter) throws Exception;

	JobMetaData FindJobById(UUID id) throws Exception;

	JobMetaData[] FindUnterminatedJob(String filter, String order, int skip, int take) throws Exception;

	JobMetaData[] FindTerminatedJob(String filter, String order, int skip, int take) throws Exception;

	int GetTerminatedCount() throws Exception;

	int GetUnterminatedCount() throws Exception;

	String GetJobLog(UUID id) throws Exception;

	String GetManagerLog(java.util.Date date) throws Exception;

	java.util.Date[] GetManagerLogAvaiableDates() throws Exception;


	JobTemplate[] GetJobTemplates() throws Exception;

	String GetJobScript(UUID id) throws Exception;


	String[] GetAllApplications() throws Exception;

	String[] GetAllUsers() throws Exception;


	String GetSettings(String typeName) throws Exception;

	void SetSettings(String typeName, String xml) throws Exception;

	String GetLocation() throws Exception;
	
	void addListener(UUID token, IJobServiceListener listener) throws Exception;
	void removeListener(UUID token) throws Exception;
	
	void echo() throws Exception;
}