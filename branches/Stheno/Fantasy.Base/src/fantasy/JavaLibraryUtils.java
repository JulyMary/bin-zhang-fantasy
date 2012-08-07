package fantasy;

import java.io.*;
import java.util.*;
import java.util.jar.JarEntry;
import java.util.jar.JarInputStream;

import org.apache.commons.io.filefilter.DirectoryFileFilter;
import org.apache.commons.io.filefilter.SuffixFileFilter;

import fantasy.io.Path;

@SuppressWarnings("rawtypes") 
public final class JavaLibraryUtils {

	private JavaLibraryUtils()
	{
		
	}
	
	public static File getLibrary(Class type)
	{
		return new File(type.getProtectionDomain().getCodeSource().getLocation().getPath()); 
	}
	
	public static Class[] GetAllClasses(File jar) throws Exception
	{
		ArrayList<Class> rs = new ArrayList<Class>();
		
		if(jar.isFile())
		{
			loadClassesByJar(jar, rs);
		}
		else
		{
			loadClassesByDirectory(jar, "", rs);
		}
		
		return rs.toArray(new Class[0]);
	}
	
	private static void loadClassesByJar(File jar, ArrayList<Class> list) throws Exception
	{
		JarInputStream jarFile = new JarInputStream(new FileInputStream(jar));
		try
		{
			JarEntry entry = null;
			do
			{
				entry = jarFile.getNextJarEntry();
				if(entry != null)
				{
					if(entry.getName().endsWith(".class"))
					{
						
						String className = entry.getName().replace('/', '.');
						className = className.substring(0, className.length() - ".class".length());
						
						Class clazz = Class.forName(className);
						list.add(clazz);
					}
				}
				
			}while(entry != null);
		}
		finally
		{
			jarFile.close();
		}
		

	}
	
	private static void loadClassesByDirectory(File directory, String packageName, ArrayList<Class> list) throws Exception
	{
		
		for(File javaFile : directory.listFiles((FileFilter)new SuffixFileFilter(".class")))
		{
			String className = Path.getFileNameWithoutExtension(javaFile.getName());
			if( !StringUtils2.isNullOrEmpty(packageName ))
			{
				className = packageName + "." + className;
			}
			Class clazz = Class.forName(className);
			list.add(clazz);
			
		}
		
		for(File childDirectory : directory.listFiles((FileFilter)DirectoryFileFilter.INSTANCE ))
		{
			String childPackage = !StringUtils2.isNullOrEmpty(packageName) ? packageName + "." + childDirectory.getName() : childDirectory.getName();
			loadClassesByDirectory(childDirectory, childPackage, list);
		}
	}
	
	
	public static File getEntryLibrary() {
		return _entryLibrary;
	}

	public static void setEntryLibrary(File entryLibrary) {
		JavaLibraryUtils._entryLibrary = entryLibrary;
	}


	private static File _entryLibrary;
	
	
	public static String extractToFullPath(String path) throws Exception
	{
		
		File f = getEntryLibrary();
		if(f.isFile())
		{
			f = f.getParentFile();
		}
		
		
		return Path.combine(f.getAbsolutePath(), SystemUtils.expandEnvironmentVariables(path));
	}
	

}
