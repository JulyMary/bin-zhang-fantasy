package fantasy.jobs;

import java.io.*;
import java.nio.file.*;

import java.rmi.RemoteException;

import org.apache.commons.io.*;

import fantasy.servicemodel.*;

public class JobStatusStorageService extends AbstractService implements IJobStatusStorageService
{

	/**
	 * 
	 */
	private static final long serialVersionUID = 5680865010716980001L;

	public JobStatusStorageService() throws RemoteException {
		super();
		
	}

	public final boolean getExists() throws Exception
	{
		return new File(this.GetFileName()).exists();
	}

	public final void Save(InputStream input) throws Exception
	{
		Path path = Paths.get(this.GetFileName());
		if (Files.exists(path))
		{
			Files.copy(path, Paths.get(this.GetFileName() + ".bak"), StandardCopyOption.REPLACE_EXISTING);
		}

		OutputStream output = Files.newOutputStream(path, StandardOpenOption.APPEND, StandardOpenOption.CREATE);
		
		
		

		try
		{
			IOUtils.copy(input, output);
		}
		finally
		{
			output.close();
		}

	}

	private String GetFileName() throws Exception
	{
			String rs;
			IJobEngine engine = this.getSite().getService(IJobEngine.class);
			rs = String.format("%1$s\\%2$s.job", engine.getJobDirectory(), engine.getJobId());
			return rs;
	}



	public final InputStream Load() throws Exception
	{
		String name = this.GetFileName();
		return InnerLoad(name);
	}

	private static InputStream InnerLoad(String name) throws Exception
	{

		Path path = Paths.get(name);
		
		BufferedInputStream rs = null; 
		
		
		if (Files.exists(path))
		{
			InputStream fs = Files.newInputStream(path, StandardOpenOption.READ);
			
			try
			{
				rs = new BufferedInputStream(fs);
			}
			finally
			{
				fs.close();
			}
		}
		

		return rs;
	}

	public final InputStream LoadBackup() throws Exception
	{
		String name = this.GetFileName() + ".bak";
		return InnerLoad(name);
	}

	

}