package fantasy.io;

import java.io.*;
import java.nio.file.*;
import org.apache.commons.io.FileUtils;

import fantasy.servicemodel.IProgressMonitor;

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

	public static boolean delete(String path) {
	
		return new java.io.File(path).delete();
		
	}
	
	
	public static void copy(String source, String dest) throws Exception
	{
		java.io.File inputFile = new java.io.File(source);
		java.io.File outputFile = new java.io.File(dest);
		StreamUtils.CopyFile(inputFile, outputFile, null);
        
	}
	
	public static void copy(String source, String dest, IProgressMonitor progress) throws Exception
	{
		java.io.File inputFile = new java.io.File(source);
		java.io.File outputFile = new java.io.File(dest);
		StreamUtils.CopyFile(inputFile, outputFile, progress);
        
	}

	
	public static void move(String source, String dest) throws Exception
	{
		move(source, dest, null);
	}
	
	public static void move(String source, String dest,
			IProgressMonitor progress) throws Exception {
		java.io.File inputFile = new java.io.File(source);
		java.io.File outputFile = new java.io.File(dest);
		if(outputFile.exists())
		{
			outputFile.delete();
		}
		if(inputFile.renameTo(outputFile))
		{
			if(progress != null)
			{
				progress.setMinimum(0);
				progress.setMaximum(100);
				progress.setValue(100);
			}
		}
		else
		{
			copy(source, dest, progress);
			inputFile.delete();
		}
		
	}

	public static void appendAllText(String path, String text, String encoding) throws Exception {
		FileUtils.write(new java.io.File(path), text, encoding, true);
		
	}

	public static void writeAllText(String path, String text, String encoding) throws Exception {
		FileUtils.write(new java.io.File(path), text, encoding, false);
		
	}
}

