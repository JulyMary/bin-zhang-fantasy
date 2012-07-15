package Fantasy.Jobs.Management;

import Fantasy.IO.*;

public class LogFileService extends Fantasy.ServiceModel.LogFileService
{
	private StreamWriter _writer = null;
	private java.util.Date _loggingDate = java.util.Date.getMinValue().Date;
	private String _directory;

	@Override
	public void InitializeService()
	{
		_directory = LongPath.Combine(JobManagerSettings.getDefault().getLogDirectoryFullPath(), Environment.MachineName);
		if (!Directory.Exists(_directory))
		{
			Directory.CreateDirectory(_directory);
		}

		super.InitializeService();
		this.WriteStart();
	}

	@Override
	public void UninitializeService()
	{

		super.UninitializeService();
		if (_writer != null)
		{
			_writer.Close();

		}
	}


	@Override
	protected System.IO.StreamWriter GetWriter()
	{

		if (!_loggingDate.equals(new java.util.Date().Date) && _writer != null)
		{
			_writer.Close();
			_writer = null;
		}

		if (_writer == null)
		{
			this._loggingDate = new java.util.Date().Date;
			String filename = String.format("%1$s\\%2$s.xlog", _directory, new java.util.Date().Date.ToString("yyyy-MM-dd"));
			FileStream fs = LongPathFile.Open(filename, FileMode.Append, FileAccess.Write, FileShare.Read);
			_writer = new StreamWriter(fs, Encoding.UTF8);
		}
		return _writer;

	}
}