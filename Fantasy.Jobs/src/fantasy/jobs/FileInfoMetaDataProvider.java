package fantasy.jobs;

import fantasy.io.*;

public class FileInfoMetaDataProvider implements ITaskItemMetaDataProvider
{
	private static final String[] Names = new String[] { "Directory", "Exists", "Extension", "FullName", "FileName", "NameWithoutExtension" };

	public final String[] GetNames(TaskItem item)
	{
		return FileInfoMetaDataProvider.Names;
	}

	public final String GetData(TaskItem item, String name)
	{

		if (name.toLowerCase().equals("namewithoutextension"))
		{
				return Path.getFileNameWithoutExtension(name);
		}
		else if (name.toLowerCase().equals("filename"))
		{
				return Path.getFileName(name);
		}
		else if (name.toLowerCase().equals("exists"))
		{
				return File.exists(name) ? "true" : "false";
		}
		else if (name.toLowerCase().equals("extension"))
		{
				return Path.getExtension(name);
		}
		else if (name.toLowerCase().equals("fullname"))
		{
				return Path.getFullPath(name);
		}
		else if (name.toLowerCase().equals("directory"))
		{
				return Path.getDirectoryName(name);
		}
		else
		{
				throw new UnsupportedOperationException();
		}
	}

}