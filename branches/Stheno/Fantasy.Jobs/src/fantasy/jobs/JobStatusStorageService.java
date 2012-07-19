package fantasy.jobs;

import fantasy.servicemodel.*;

public class JobStatusStorageService extends AbstractService implements IJobStatusStorageService
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobStorageService Members


	public final boolean getExists()
	{
		return File.Exists(this.GetFileName());
	}

	public final void Save(Stream stream)
	{
		String name = this.GetFileName();
		if (File.Exists(name))
		{
			File.Copy(name, name + ".bak", true);
		}

		FileStream fs = new FileStream(name, FileMode.Create);

		try
		{
			int count = 0;
			byte[] buffer = new byte[1024];
			do
			{

				count = stream.Read(buffer, 0, buffer.length);
				fs.Write(buffer, 0, count);

			} while (count == buffer.length);
		}
		finally
		{
			fs.Close();
		}

	}

	private String GetFileName()
	{
			String rs;
			IJobEngine engine = (IJobEngine)this.Site.GetService(IJobEngine.class);
			rs = String.format("%1$s\\%2$s.job", engine.getJobDirectory(), engine.getJobId());
			return rs;
	}



	public final Stream Load()
	{
		String name = this.GetFileName();
		return InnerLoad(name);
	}

	private static Stream InnerLoad(String name)
	{

		MemoryStream rs = new MemoryStream();
		if (File.Exists(name))
		{
			FileStream fs = new FileStream(name, FileMode.Open);
			try
			{
				int count = 0;
				byte[] buffer = new byte[1024];
				do
				{

					count = fs.Read(buffer, 0, buffer.length);
					rs.Write(buffer, 0, count);

				} while (count == buffer.length);
			}
			finally
			{
				fs.Close();
			}
		}
		rs.Position = 0;

		return rs;
	}

	public final Stream LoadBackup()
	{
		String name = this.GetFileName() + ".bak";
		return InnerLoad(name);
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}