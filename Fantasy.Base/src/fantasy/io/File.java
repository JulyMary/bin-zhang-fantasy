package fantasy.io;

import java.io.*;
import java.nio.file.Files;
import java.nio.file.StandardOpenOption;

import org.apache.commons.io.FileUtils;

public final class File {
	
	
	private File()
	{
		
	}
	
	public static  OutputStream Create(String path) throws Exception
	{
		java.io.File f = new java.io.File(path);
		
		Directory.create(f.getParent());
		
		OutputStream rs =  Files.newOutputStream(f.toPath(), StandardOpenOption.CREATE);
		
		return rs;
	}
	
	public static String readAllText(String path, String encoding) throws Exception
	{
		return FileUtils.readFileToString(new java.io.File(path), encoding);
	}
	
	public static String readAllText(String path) throws Exception
	{
		
		return FileUtils.readFileToString(new java.io.File(path));
		
		
	}

	public static boolean exists(String path) {
		java.io.File f = new java.io.File(path);
		return f.exists() && f.isFile();
	}
}

