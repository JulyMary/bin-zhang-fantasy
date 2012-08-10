package fantasy.jobs.management;

import java.io.*;
import java.net.InetAddress;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.nio.file.StandardOpenOption;
import java.rmi.RemoteException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;

import org.apache.commons.lang3.time.DateUtils;


import fantasy.io.*;

public class LogFileService extends fantasy.servicemodel.LogFileService {
	/**
	 * 
	 */
	private static final long serialVersionUID = -4009792413361081482L;

	public LogFileService() throws RemoteException {
		super();
		
		this._loggingDate = DateUtils.truncate(new Date(), Calendar.DATE);
		
	}

	private OutputStreamWriter _writer = null;
	private Date _loggingDate;
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

		Date today = DateUtils.truncate(new Date(), Calendar.DATE);
		
		if (!_loggingDate.equals(today) && _writer != null) {
			_writer.close();
			_writer = null;
		}

		
		
		
		if (_writer == null) {
			this._loggingDate = today;
			
			SimpleDateFormat fmt = new SimpleDateFormat("yyyy-MM-dd");
			
			String filename = String.format("%1$s\\%2$s.xlog", _directory,
					fmt.format(this._loggingDate));
			OutputStream os = Files.newOutputStream(Paths.get(filename), StandardOpenOption.APPEND, StandardOpenOption.CREATE);
			this._writer = new OutputStreamWriter(os, this.getCharset());
		}
		return _writer;

	}
}