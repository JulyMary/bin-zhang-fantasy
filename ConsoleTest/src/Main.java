import java.io.*;

import java.nio.charset.*;
import java.nio.file.*;
import java.util.*;


public class Main {
    public static void main(String[] args) throws Exception
    {
    	Path path = Paths.get("D:\\Fantasy\\Stheno\\Output\\test.txt");
    	
    	OutputStream os = Files.newOutputStream(path, StandardOpenOption.APPEND, StandardOpenOption.CREATE);
    	BufferedWriter writer = new BufferedWriter(new OutputStreamWriter(os, "UTF-8"));
    	
    	writer.write("hello");
    	writer.newLine();
    	writer.write("hello2");
    	writer.newLine();
    	writer.flush();
    	
    	
    	List<String> list = Files.readAllLines(path, Charset.forName("UTF-8"));
    	for(String ln : list)
    	{
    		System.out.println(ln);
    	}
    	
    	writer.close();
    	
    	
    
 
    }
    
   
    
}
