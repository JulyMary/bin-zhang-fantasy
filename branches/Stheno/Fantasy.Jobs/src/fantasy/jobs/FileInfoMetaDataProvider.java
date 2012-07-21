package fantasy.jobs;

import fantasy.io.*;

public class FileInfoMetaDataProvider implements ITaskItemMetaDataProvider
{
	private static final String[] Names = new String[] { "Directory", "Exists", "Extension", "FullName", "FileName", "NameWithoutExtension" };

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITaskItemMetaDataProvider Members

	public final String[] GetNames(TaskItem item)
	{
		return FileInfoMetaDataProvider.Names;
	}

	public final String GetData(TaskItem item, String name)
	{
		Uri dir = new Uri(JobEngine.getCurrentEngine().getJobDirectory() + "\\");
		Uri full = new Uri(dir, item.getName());
		String path = full.IsFile ? full.LocalPath : full.toString();
//C# TO JAVA CONVERTER NOTE: The following 'switch' operated on a string member and was converted to Java 'if-else' logic:
//		switch (name.ToLower())
//ORIGINAL LINE: case "namewithoutextension" :
		if (name.toLowerCase().equals("namewithoutextension"))
		{
				return LongPath.GetFileNameWithoutExtension(path).toLowerCase();
		}
//ORIGINAL LINE: case "filename":
		else if (name.toLowerCase().equals("filename"))
		{
				return LongPath.GetFileName(path);
		}
//ORIGINAL LINE: case "exists":
		else if (name.toLowerCase().equals("exists"))
		{
				return LongPathFile.Exists(path).toString();
		}
//ORIGINAL LINE: case "extension":
		else if (name.toLowerCase().equals("extension"))
		{
				return LongPath.GetExtension(path);
		}
//ORIGINAL LINE: case "fullname":
		else if (name.toLowerCase().equals("fullname"))
		{
				return path;
		}
//ORIGINAL LINE: case "directory":
		else if (name.toLowerCase().equals("directory"))
		{
				return LongPath.GetDirectoryName(path);
		}
		else
		{
				throw new NotSupportedException();
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}