package fantasy.io;



public final class Directory {

	private Directory()
	{
		
	}
	
	
	public static boolean exists(String path)
	{
		java.io.File f = new java.io.File(path);
		return f.exists() && f.isDirectory();
	}
	
	public static java.io.File create(String path)
	{
		java.io.File rs = new java.io.File(path);
		
		
		
		CreateRecursive(rs);
		
		return rs;
		
	}
	
	private static void CreateRecursive(java.io.File file)
	{
		if(!file.exists())
		{
			java.io.File parent = file.getParentFile();
			CreateRecursive(parent);
			
			file.mkdir();
		}
	}
	
	
	

}
