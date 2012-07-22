package fantasy.jobs;

import java.io.OutputStream;
import java.io.OutputStreamWriter;

import fantasy.servicemodel.*;
import Fantasy.IO.*;

public class JobLogFileService extends LogFileService
{


	@Override
	public void initializeService()
	{
		IJobEngine engine = (IJobEngine)((IServiceProvider)this.Site).GetService(IJobEngine.class);
		
		String filePath = String.format("%1$s\\%2$s.xlog", engine.getJobDirectory(), engine.getJobId());
		
		OutputStream stream = 
		FileStream fs = LongPathFile.Open(filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
		_writer = new StreamWriter(fs, Encoding.UTF8);
		WriteStart();

		super.initializeService();
	}

	@Override
	public void uninitializeService()
	{
		super.uninitializeService();
		this._writer.Close();
	}

	public static String GetLogFilePath(String jobsDirectory, Guid jobId)
	{
		return String.format("%1$s\\%2$s\\%2$s.xlog", jobsDirectory, jobId);
	}

	private OutputStreamWriter _writer;

	@Override
	protected OutputStreamWriter getWriter()
	{
		
	}
}