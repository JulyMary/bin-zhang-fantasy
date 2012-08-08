package fantasy;

import java.math.BigInteger;
import java.security.*;

public final class MD5HashCodeHelper {
    private MD5HashCodeHelper()
    {
    	
    }
    
    public static String compute(String input) throws Exception
    {
    	byte[] data = input.getBytes(); 
    	MessageDigest m = MessageDigest.getInstance("MD5"); 
        
        m.update(data,0,data.length); 
        BigInteger i = new BigInteger(1,m.digest()); 
        return String.format("%1$032X", i); 

    }
}
