package fantasy.jobs.management;

import java.sql.*;
import java.util.*;
import java.util.Date;

import fantasy.*;



class FantasyJobsEntities implements IDisposable
{


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
    			rs = DriverManager.getConnection(JobManagerSettings.getDefault().getJobsDbConnectionString());
    			
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
	
	
	

	public List<JobMetaData> getUnterminates() throws Exception
	{
		ArrayList<JobMetaData> rs = new ArrayList<JobMetaData>();
		
		String sql = "select * from APP.FTS_JOB_JOBS";
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


	
	private JobMetaData readMetadata(ResultSet record) throws Exception {
		JobMetaData rs = new JobMetaData();
		
          rs.setId(UUID.fromString(record.getString("ID")));
          String parentId = record.getString("PARENTID");
          if(parentId != null)
          {
        	  rs.setParentId(UUID.fromString(parentId));
          }
          rs.setTemplate(record.getString("TEMPLATE"));
          rs.setName(record.getString("Name"));
          rs.setState(record.getInt("STATE"));
          rs.setPriority(record.getInt("PRIORITY"));
          rs.setStartTime(toDate(record.getTimestamp("STARTTIME")));
          rs.setEndTime(toDate(record.getTimestamp("ENDTIME")));
          rs.setCreationTime(toDate(record.getTimestamp("CREATIONTIME")));
          rs.setApplication(record.getString("APPLICATION"));
          rs.setUser(record.getString("USER_"));
        
          rs.setStartInfo(record.getString("STARTINFO"));
          rs.setTag(record.getString("TAG"));
          
		
		return rs;
	}
	
	private static Date toDate(Timestamp time)
	
	{
		if(time == null)
		{
			return null;
		}
		else
		{
			return new Date(time.getTime());
		}
	}


	public final void addToUnterminates(JobMetaData job) throws Exception
	{
		
		String sql = "INSERT INTO APP.FTS_JOB_JOBS (ID, PARENTID, TEMPLATE, NAME, STATE, CREATIONTIME, APPLICATION, USER_, STARTINFO, TAG, PRIORITY)" +
				" VALUES (?, ?, ?, ?, ?, ?, ?, ? ,? ,?, ?)";
	    Connection cnnt = this.getConnection();
	    PreparedStatement stmt = null;
	    try
	    {
	    	stmt = cnnt.prepareStatement(sql);
	    	stmt.setString(1,job.getId().toString());
	    	stmt.setString(2,job.getParentId() != null ? job.getParentId().toString() : null);
	    	stmt.setString(3, job.getTemplate());
	    	stmt.setString(4,  job.getName());
	    	stmt.setInt(5, job.getState());
	    	stmt.setTimestamp(6, new Timestamp(job.getCreationTime().getTime()));
	    	stmt.setString(7, job.getApplication());
	    	stmt.setString(8, job.getUser());
	    	stmt.setString(9, job.getStartInfo());
	    	stmt.setString(10, job.getTag());
	    	stmt.setInt(11, job.getPriority());
	    	
	    	stmt.execute();
	    }
	    finally
	    {
	    	if(stmt != null)
	    	{
	    		stmt.close();
	    	}
	    	this.revokeConnection(cnnt);
	    	
	    }


	}

	
	public final void setState(JobMetaData job) throws Exception
	{
        String sql = "UPDATE APP.FTS_JOB_JOBS set STATE = ? where ID = ?";
		Connection cnnt = this.getConnection();
	    PreparedStatement stmt = null;
	    try
	    {
	    	stmt = cnnt.prepareStatement(sql);
	    	stmt.setInt(1, job.getState());
	    	stmt.setString(2,job.getId().toString());
	    	stmt.execute();
	    }
	    finally
	    {
	    	if(stmt != null)
	    	{
	    		stmt.close();
	    	}
	    	this.revokeConnection(cnnt);
	    	
	    }
	}
	
	public final void setStart(JobMetaData job) throws Exception
	{
		String sql = "UPDATE APP.FTS_JOB_JOBS set STATE = ?, STARTTIME = ? where ID = ?";
		
		Connection cnnt = this.getConnection();
	    PreparedStatement stmt = null;
	    try
	    {
	    	stmt = cnnt.prepareStatement(sql);
	    	stmt.setInt(1, job.getState());
	    	stmt.setTimestamp(2, new Timestamp(job.getStartTime().getTime()));
	    	stmt.setString(3,job.getId().toString());
	    	stmt.execute();
	    }
	    finally
	    {
	    	if(stmt != null)
	    	{
	    		stmt.close();
	    	}
	    	this.revokeConnection(cnnt);
	    	
	    }
		
	}
	
	public final void moveToTerminates(JobMetaData job) throws Exception
	{
		
		String dSql = "DELETE FROM APP.FTS_JOB_JOBS where ID = ?"; 
		String iSql = "INSERT INTO APP.FTS_JOB_ARCHIVEDJOBS (ID, PARENTID, TEMPLATE, NAME, STATE, CREATIONTIME, APPLICATION, USER_, STARTINFO, TAG, STARTTIME, ENDTIME, PRIORITY)" +
				"VALUES (?, ?, ?, ?, ?, ?, ?, ? ,? ,?, ?, ?, ?)";
		PreparedStatement dStatement = null;
		PreparedStatement iStatement = null;
		
	    Connection cnnt = this.getConnection();
	    try
	    {
	    	cnnt.setAutoCommit(false);
	    	cnnt.createStatement();
	    	dStatement = cnnt.prepareStatement(dSql);
	    	try
	    	{
	    		dStatement.setString(1, job.getId().toString());
	    	    dStatement.execute();
	    	}
	    	finally
	    	{
	    		dStatement.close();
	    	}
	    	iStatement = cnnt.prepareStatement(iSql);
	    	
	    	iStatement.setString(1,job.getId().toString());
	    	iStatement.setString(2,job.getParentId() != null ? job.getParentId().toString() : null);
	    	iStatement.setString(3, job.getTemplate());
	    	iStatement.setString(4,  job.getName());
	    	iStatement.setInt(5, job.getState());
	    	iStatement.setTimestamp(6, new Timestamp(job.getCreationTime().getTime()));
	    	iStatement.setString(7, job.getApplication());
	    	iStatement.setString(8, job.getUser());
	    	iStatement.setString(9, job.getStartInfo());
	    	iStatement.setString(10, job.getTag());
	    	iStatement.setTimestamp(11, new Timestamp(job.getStartTime().getTime()));
	    	iStatement.setTimestamp(12, new Timestamp(job.getEndTime().getTime()));
	    	iStatement.setInt(13, job.getPriority());
	    	try
	    	{
	    	    iStatement.execute();
	    	}
	    	finally
	    	{
	    		iStatement.close();
	    	}
	    	cnnt.commit();
	    }
	    finally
	    {
	    	cnnt.setAutoCommit(true);
	    	this.revokeConnection(cnnt);
	    }
	    
	    
	}
	
	
	public int getTerminatedCount() throws Exception
	{

		int rs;
		String sql = "select count(*) from APP.FTS_JOB_ARCHIVEDJOBS";
		Connection cnnt = this.getConnection();
		Statement stmt = null;
		try
		{
			stmt = cnnt.createStatement();
			ResultSet set = stmt.executeQuery(sql);
			set.next();
			
			rs = set.getInt(1);
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
	
	public List<JobMetaData> query(String table, String filter, String order, int take, int skip) throws Exception
	{
		StringBuilder sql = new StringBuilder();
		sql.append("select * from ").append(table);
		
		if(!StringUtils2.isNullOrEmpty(filter))
		{
			sql.append(" where ").append(filter);
		}
		
		if(!StringUtils2.isNullOrEmpty(order))
		{
			sql.append(" order by ").append(order);
		
		}
		if(skip > 0)
		{
			sql.append(" offset ").append(skip).append(" rows");
		}
		
		if(take < Integer.MAX_VALUE)
		{
			sql.append(" fetch ").append(take).append(" rows only");
		
		}
		
		
		
       ArrayList<JobMetaData> rs = new ArrayList<JobMetaData>();
		
		
		Connection cnnt = this.getConnection();
		Statement stmt = null;
		try
		{
			stmt = cnnt.createStatement();
			ResultSet set = stmt.executeQuery(sql.toString());
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


}