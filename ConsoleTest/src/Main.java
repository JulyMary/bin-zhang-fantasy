import java.io.File;
import fantasy.JarUtils;

public class Main {
    public static void main(String[] args) throws Exception
    {
    	
    	File jar = new File("D:\\Fantasy\\Stheno\\Output\\fantasy.base.jar");
    	
    	@SuppressWarnings("rawtypes")
		Class[] classes = JarUtils.GetAllClasses(jar);
    	for(Class<?> clazz : classes)
    	{
    		System.out.println(clazz.getName());
    	}
    	
 
    }
    
   
    
}
