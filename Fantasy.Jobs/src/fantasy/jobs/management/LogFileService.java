package fantasy.jobs.management;

import java.io.*;
import java.net.InetAddress;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.nio.file.StandardOpenOption;
import java.rmi.RemoteException;
import org.joda.time.*;

import fantasy.io.*;

public class LogFileService extends fantasy.servicemodel.LogFileService {
	/**
	 * 
	 */
	private static final long serialVersionUID = -4009792413361081482L;

	public LogFileService() throws RemoteException {
		super();
		
	}

	private OutputStreamWriter _writer = null;
	private LocalDate _loggingDate = new LocalDate(0l);
	private String _directory;

	@Override
	public void initializeService() throws Exception {
		_directory = Path.combine(JobManagerSettings.getDefault()
				.getLogDirectoryFullPath(), InetAddress.getLocalHost().getHostName()
		 );
		if (!Directory.exists(_directory)) {
			Directory.create(_directory);
		}
		
		

		super.initializeService();
		this.writeStart();
	}

	@Override
	public void uninitializeService() throws Exception {

		super.uninitializeService();
		if (_writer != null) {
			_writer.close();

		}
	}

	@Override
	protected OutputStreamWriter getWriter() throws Exception {

		if (!_loggingDate.equals(new LocalDate()) && _writer != null) {
			_writer.close();
			_writer = null;
		}

		if (_writer == null) {
			this._loggingDate = new LocalDate();
			String filename = String.format("%1$s\\%2$s.xlog", _directory,
					this._loggingDate.toString("yyyy-MM-dd"));
			OutputStream os = Files.newOutputStream(Paths.get(filename), StandardOpenOption.APPEND, StandardOpenOption.CREATE);
			this._writer = new OutputStreamWriter(os, this.getCharset());
		}
		return _writer;

	}
}