package fantasy;

import java.util.*;

public final class UUIDUtils {
   
	private UUIDUtils()
	{
		
	}
	
	
	public static byte[] getBytes(UUID uuid)
	{
		if(uuid == null)
		{
			return null;
		}
		
		long mostSigBits = uuid.getMostSignificantBits();
		long leastSigBits = uuid.getLeastSignificantBits();
		
		byte[] rs = new byte[16];
		
		rs[0] = (byte)(mostSigBits >>> 56); 
	    rs[1] = (byte)(mostSigBits >>> 48); 
	    rs[2] = (byte)(mostSigBits >>> 40); 
	    rs[3] = (byte)(mostSigBits >>> 32); 
	    rs[4] = (byte)(mostSigBits >>> 24); 
	    rs[5] = (byte)(mostSigBits >>> 16); 
	    rs[6] = (byte)(mostSigBits >>>  8); 
	    rs[7] = (byte)(mostSigBits >>>  0); 
	    
	    
	    rs[8] = (byte)(leastSigBits >>> 56); 
	    rs[9] = (byte)(leastSigBits >>> 48); 
	    rs[10] = (byte)(leastSigBits >>> 40); 
	    rs[11] = (byte)(leastSigBits >>> 32); 
	    rs[12] = (byte)(leastSigBits >>> 24); 
	    rs[13] = (byte)(leastSigBits >>> 16); 
	    rs[14] = (byte)(leastSigBits >>>  8); 
	    rs[14] = (byte)(leastSigBits >>>  0); 
	    
	    return rs;

	}
	
	public static String toString(UUID uuid, String format)
	{
		
		if(uuid == null)
		{
			return null;
		}
		
		long mostSigBits = uuid.getMostSignificantBits();
		long leastSigBits = uuid.getLeastSignificantBits();
		
		switch(format)
		{
		case "N":
		case "n":
			return Long.toHexString(mostSigBits) + Long.toHexString(leastSigBits);
		default:
			return uuid.toString();
		
		}
		
	}
	
	@SuppressWarnings("unused")
	private static String digits(long val, int digits) {
        long hi = 1L << (digits * 4);
        return Long.toHexString(hi | (val & (hi - 1))).substring(1);
    }
	
	public static final UUID Empty  = new UUID(0,0);
}
