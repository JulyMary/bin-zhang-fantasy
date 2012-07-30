package fantasy.jobs.management;

import java.net.InetAddress;
import java.util.Date;

import fantasy.io.*;

public class LogFileService extends fantasy.servicemodel.LogFileService {
	private StreamWriter _writer = null;
	private java.util.Date _loggingDate = new Date(Long.MIN_VALUE);
	private String _directory;

	@Override
	public void initializeService() {
		_directory = Path.combine(JobManagerSettings.getDefault()
				.getLogDirectoryFullPath(), InetAddress.getLocalHost().getHostName();
		 );
		if (!Directory.exists(_directory)) {
			Directory.create(_directory);
		}
		
		

		super.initializeService();
		this.WriteStart();
	}

	@Override
	public void uninitializeService() {

		super.uninitializeService();
		if (_writer != null) {
			_writer.Close();

		}
	}

	@Override
	protected System.IO.StreamWriter GetWriter() {

		if (!_loggingDate.equals(new java.util.Date().Date) && _writer != null) {
			_writer.Close();
			_writer = null;
		}

		if (_writer == null) {
			this._loggingDate = new java.util.Date().Date;
			String filename = String.format("%1$s\\%2$s.xlog", _directory,
					new java.util.Date().Date.ToString("yyyy-MM-dd"));
			FileStream fs = LongPathFile.Open(filename, FileMode.Append,
					FileAccess.Write, FileShare.Read);
			_writer = new StreamWriter(fs, Encoding.UTF8);
		}
		return _writer;

	}
}