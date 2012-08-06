package fantasy.io;

import java.util.*;

import org.apache.commons.io.FileUtils;
import org.apache.commons.io.filefilter.TrueFileFilter;
import org.apache.commons.io.filefilter.WildcardFileFilter;



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

	
	public static Iterable<String> enumerateFiles(String path, String pattern) {
		return enumerateFiles(path, pattern, true);
	}

	public static Iterable<String> enumerateFiles(String path, String pattern, boolean recursive) {
		
	    ArrayList<String> rs = new ArrayList<String>();
	    
		for(java.io.File f : FileUtils.listFiles(new java.io.File(path), new WildcardFileFilter(pattern), recursive ? TrueFileFilter.INSTANCE : null))
		{
			rs.add(f.getAbsolutePath());
		}
		return rs;
		
	}


	public static void delete(String path, boolean recursive) throws Exception {
		
		if(recursive)
		{
			FileUtils.deleteDirectory(new java.io.File(path));
		}
		else
		{
			new java.io.File(path).delete();
		}
	}
	
	
	

}
