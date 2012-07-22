import java.io.*;

import java.nio.file.*;

import fantasy.JarUtils;

public class Main {
    public static void main(String[] args) throws Exception
    {
    	Path path = Paths.get("D:\\Fantasy\\Stheno\\Output\\test.txt");
    	
    	OutputStream os = Files.newOutputStream(path, StandardOpenOption.APPEND, StandardOpenOption.CREATE);
    	BufferedWriter writer = new BufferedWriter(new OutputStreamWriter(os));
    	
    	writer.write("hello");
    	writer.newLine();
    	writer.write("hello2");
    	writer.newLine();
    	
    	
    	Files.readAllLines(path)
    	
    	
    	File jar = new File("fantasy.base.jar");
    	
    	@SuppressWarnings("rawtypes")
		Class[] classes = JarUtils.GetAllClasses(jar);
    	for(Class<?> clazz : classes)
    	{
    		System.out.println(clazz.getName());
    	}
    	
 
    }
    
   
    
}
