package fantasy.io;

import java.io.*;
import java.nio.file.Files;
import java.nio.file.StandardOpenOption;

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
}

