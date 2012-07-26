package fantasy.jobs.management;




public class FantasyJobsEntities
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region Constructors

	/** 
	 Initializes a new FantasyJobsEntities object using the connection string found in the 'FantasyJobsEntities' section of the application configuration file.
	 
	*/
	public FantasyJobsEntities()
	{
		super("name=FantasyJobsEntities", "FantasyJobsEntities");
		this.ContextOptions.LazyLoadingEnabled = true;
		OnContextCreated();
	}

	/** 
	 Initialize a new FantasyJobsEntities object.
	 
	*/
	public FantasyJobsEntities(String connectionString)
	{
		super(connectionString, "FantasyJobsEntities");
		this.ContextOptions.LazyLoadingEnabled = true;
		OnContextCreated();
	}

	/** 
	 Initialize a new FantasyJobsEntities object.
	 
	*/
	public FantasyJobsEntities(EntityConnection connection)
	{
		super(connection, "FantasyJobsEntities");
		this.ContextOptions.LazyLoadingEnabled = true;
		OnContextCreated();
	}


	/** 
	 No Metadata Documentation available.
	 
	*/
	public final ObjectSet<JobMetaData> getUnterminates()
	{
		if ((_Unterminates == null))
		{
			_Unterminates = super.<JobMetaData>CreateObjectSet("Unterminates");
		}
		return _Unterminates;
	}
	private ObjectSet<JobMetaData> _Unterminates;

	/** 
	 No Metadata Documentation available.
	 
	*/
	public final ObjectSet<JobMetaData> getTerminates()
	{
		if ((_Terminates == null))
		{
			_Terminates = super.<JobMetaData>CreateObjectSet("Terminates");
		}
		return _Terminates;
	}
	private ObjectSet<JobMetaData> _Terminates;

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region AddTo Methods

	/** 
	 Deprecated Method for adding a new object to the Unterminates EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
	 
	*/
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