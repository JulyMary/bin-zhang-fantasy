
public class Main {
    public static void main(String[] args)
    {
    	String s = Main.class.getProtectionDomain().getCodeSource().getLocation().getPath();
    	System.out.printf(s);
    	
 
    }
    
   
    
}
