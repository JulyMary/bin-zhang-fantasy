package fantasy.jobs.management;

import java.sql.*;
import java.util.*;

import fantasy.servicemodel.*;
import fantasy.*;



class FantasyJobsEntities implements IDisposable
{
/**
	 * 
	 */
	private static final long serialVersionUID = -305858857547134842L;

	public FantasyJobsEntities() throws Exception
	{
	   
		 Class.forName("org.apache.derby.jdbc.EmbeddedDriver");
	}


	@Override
	public void dispose() {
		
		synchronized(_poolSync)
		{
			for(Connection cnnt : _poolingConnections)
			{
				try {
					cnnt.close();
				} catch (SQLException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
		}
	}
    
    private Connection getConnection() throws Exception
    {
    	Connection rs = null;
    	synchronized(_poolSync)
    	{
    		int index = _poolingConnections.size() - 1;
    		if(index >= 0)
    		{
    			rs = _poolingConnections.remove(index);
    			
    		}
    		else
    		{
    			rs = DriverManager.getConnection(JobManagerSettings.getJobsDbConnectionString());
    			
    		}
    		
    		_busyConnections.add(rs);
    	}
    	
    	return rs;
    }
    
    private void revokeConnection(Connection connection)
    {
    	synchronized(_poolSync)
    	{
    		_busyConnections.remove(connection);
    		
    		_poolingConnections.add(connection);
    	}
    }
    
    
    private Object _poolSync = new Object();
    
    private ArrayList<Connection> _poolingConnections = new ArrayList<Connection>();
    
    private ArrayList<Connection> _busyConnections = new ArrayList<Connection>();
	
	
	

	public List<JobMetaData> getUnterminated() throws Exception
	{
		ArrayList<JobMetaData> rs = new ArrayList<JobMetaData>();
		
		String sql = "select * from APP.CV_JOB_JOBS";
		Connection cnnt = this.getConnection();
		Statement stmt = null;
		try
		{
			stmt = cnnt.createStatement();
			ResultSet set = stmt.executeQuery(sql);
			while(set.next())
			{
				rs.add(this.readMetadata(set));
			}
			set.close();
			
		}
		finally
		{
			if(stmt != null)
			{
				stmt.close();
			}
			this.revokeConnection(cnnt);
		}
		
		return rs;
	}


	
	private JobMetaData readMetadata(ResultSet set) {
		JobMetaData rs = new JobMetaData();
		
		rs.setId(value)
		
		return rs;
	}


	public final void AddToUnterminates(JobMetaData jobMetaData)
	{
		super.AddObject("Unterminates", jobMetaData);
	}

	/** 
	 Deprecated Method for adding a new object to the Terminates EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
	 
	*/
	public final void AddToTerminates(JobMetaData jobMetaData)
	{
		super.AddObject("Terminates", jobMetaData);
	}




	

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}