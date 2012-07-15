package Fantasy.Jobs;

import Fantasy.ServiceModel.*;
import Fantasy.IO.*;

public class JobLogFileService extends LogFileService
{


	@Override
	public void InitializeService()
	{
		IJobEngine engine = (IJobEngine)((IServiceProvider)this.Site).GetService(IJobEngine.class);

		String filePath = String.format("%1$s\\%2$s.xlog", engine.getJobDirectory(), engine.getJobId());
		FileStream fs = LongPathFile.Open(filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
		_writer = new StreamWriter(fs, Encoding.UTF8);
		WriteStart();

		super.InitializeService();
	}

	@Override
	public void UninitializeService()
	{
		super.UninitializeService();
		this._writer.Close();
	}

	public static String GetLogFilePath(String jobsDirectory, Guid jobId)
	{
		return String.format("%1$s\\%2$s\\%2$s.xlog", jobsDirectory, jobId);
	}

	private StreamWriter _writer;

	@Override
	protected StreamWriter GetWriter()
	{
		return this._writer;
	}
}