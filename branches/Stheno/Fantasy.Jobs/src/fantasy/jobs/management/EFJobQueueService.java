package fantasy.jobs.management;

import fantasy.jobs.properties.*;
import fantasy.servicemodel.*;

public class EFJobQueueService extends AbstractService implements IJobQueue
{

	private FantasyJobsEntities _entities;

	private java.util.ArrayList<JobMetaData> _unterminates = new java.util.ArrayList<JobMetaData>();

	@Override
	public void InitializeService()
	{
		_entities = new FantasyJobsEntities();
		_entities.Connection.Open();
		_unterminates.addAll(_entities.getUnterminates());
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		_unterminates.SortBy(m => m.Id);
		for (JobMetaData job : _unterminates)
		{
			if (job.getState() == JobState.Running)
			{
				job.setState(JobState.Suspended);
				this.ApplyChange(job);
			}
		}
		super.InitializeService();
	}

	@Override
	public void UninitializeService()
	{
		super.UninitializeService();
		_entities.Connection.Close();
		_entities.dispose();
	}


//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobQueue Members

	public final Iterable<JobMetaData> getUnterminates()
	{
		java.util.ArrayList<JobMetaData> uns;
		synchronized (_syncRoot)
		{
			uns = new java.util.ArrayList<JobMetaData>(_unterminates);
		}
		return uns;
	}

	public final Iterable<JobMetaData> getTerminates()
	{
		java.util.ArrayList<JobMetaData> rs;
		synchronized (_syncRoot)
		{
			rs = new java.util.ArrayList<JobMetaData>(this._entities.getTerminates());
		}
		return rs;
	}

	public final JobMetaData FindJobMetaDataById(UUID id)
	{
		synchronized (_syncRoot)
		{
			JobMetaData rs = null;
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			int pos = this._unterminates.BinarySearchBy(id, j => j.Id);
			if (pos >= 0)
			{
				rs = _unterminates.get(pos);
			}

			if (rs == null)
			{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
				rs = (from job in this.getTerminates() where job.Id == id select job).SingleOrDefault();
			}

			return rs;
		}
	}



	public final JobMetaData CreateJobMetaData()
	{
		return new JobMetaData();
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

	private Object _syncRoot = new Object();
	public final void ApplyChange(JobMetaData job)
	{
		int action = 0;
		boolean changed = false;
		boolean added = false;
		synchronized (_syncRoot)
		{
			JobMetaData old = this.FindJobMetaDataById(job.getId());
			if (old != null)
			{
				old.setState(job.getState());
				old.setStartTime(job.getStartTime());
				old.setEndTime(job.getEndTime());
				job = old;
			}
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//			using (IDbTransaction tx = this._entities.Connection.BeginTransaction())
			IDbTransaction tx = this._entities.Connection.BeginTransaction();
			try
			{

				switch (job.EntityState)
				{
					case System.Data.EntityState.Detached:
						{


							if (!job.getIsTerminated())
							{
								this._entities.getUnterminates().AddObject(job);
							}
							else
							{
								this._entities.getTerminates().AddObject(job);
							}
							added = true;
							action = 1;


						}
						break;

					case System.Data.EntityState.Added:
					case System.Data.EntityState.Modified:
						{

							if (!job.getIsTerminated() && job.EntityKey.EntitySetName.equals("Terminates"))
							{
								this._entities.getUnterminates().AddObject(job);
								this._entities.DeleteObject(job);
								this._entities.SaveChanges();

								action = 1;
							}
							else if (job.getIsTerminated() && job.EntityKey.EntitySetName.equals("Unterminates"))
							{
								this._entities.DeleteObject(job);
								this._entities.SaveChanges();
								this._entities.getTerminates().AddObject(job);
								action = -1;
							}
							changed = true;
						}
						break;
				}
				if (changed || added)
				{
					this._entities.SaveChanges();
				}
				if (action == 1)
				{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
					int pos = this._unterminates.BinarySearchBy(job.getId(), j => j.Id);

					this._unterminates.add(~pos, job);
				}
				else if (action == -1)
				{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
					int pos = this._unterminates.BinarySearchBy(job.getId(), j => j.Id);
					if (pos >= 0)
					{
						this._unterminates.remove(pos);
					}
				}
				tx.Commit();
			}
			finally
			{
				tx.dispose();
			}
		}
		if (changed)
		{
			this.OnChanged(new JobQueueEventArgs(job));
		}
		else if (added)
		{
			this.OnAdded(new JobQueueEventArgs(job));
		}

	}
	public final Iterable<JobMetaData> FindTerminated(RefObject<Integer> totalCount, String filter, String[] args, String order, int skip, int take)
	{
		java.util.ArrayList<JobMetaData> rs;
		synchronized (_syncRoot)
		{

			rs = new java.util.ArrayList<JobMetaData>(FindJobMetaDataByFilter(totalCount, this._entities.getTerminates(), filter, args, order, skip, take));
		}
		return rs;
	}
	public final Iterable<JobMetaData> FindUnterminated(RefObject<Integer> totalCount, String filter, String[] args, String order, int skip, int take)
	{
		java.util.ArrayList<JobMetaData> uns;
		synchronized (_syncRoot)
		{
			uns = new java.util.ArrayList<JobMetaData>(_unterminates);
		}

		return FindJobMetaDataByFilter(totalCount, uns.AsQueryable(), filter, args, order, skip, take);

	}

//C# TO JAVA CONVERTER WARNING: Java does not allow user-defined value types. The behavior of this class will differ from the original:
//ORIGINAL LINE: private struct Replacement
	private final static class Replacement
	{
		public String Pattern;
		public String Value;

		public Replacement clone()
		{
			Replacement varCopy = new Replacement();

			varCopy.Pattern = this.Pattern;
			varCopy.Value = this.Value;

			return varCopy;
		}
	}

	private Iterable<JobMetaData> FindJobMetaDataByFilter(RefObject<Integer> totalCount, IQueryable<JobMetaData> data, String filter, String[] args, String order, int skip, int take)
	{
		filter = (filter != null) ? filter : "";
		order = (order != null) ? order : "";

		CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

		String source = Properties.Resources.getJobMetaDataFilterTemplate();

		StringBuilder vars = new StringBuilder();
		if (args != null)
		{
			for (int i = 0; i < args.length; i++)
			{
				vars.append(String.format("var _%1$s = %2$s;", i, args[i]));
			}
		}

		Replacement tempVar = new Replacement();
		tempVar.Pattern="<%=vars%>";
		tempVar.Value= vars.toString();
		Replacement tempVar2 = new Replacement();
		tempVar2.Pattern="<%=condition%>";
		tempVar2.Value=filter;
		Replacement tempVar3 = new Replacement();
		tempVar3.Pattern="<%=enableCondition%>";
		tempVar3.Value= filter.equals("")? "//" : "";
		Replacement tempVar4 = new Replacement();
		tempVar4.Pattern="<%=order%>";
		tempVar4.Value=order;
		Replacement tempVar5 = new Replacement();
		tempVar5.Pattern="<%=enableOrder%>";
		tempVar5.Value= order.equals("")? "//" : "";
		Replacement[] replacements = new Replacement[] { tempVar, tempVar2, tempVar3, tempVar4, tempVar5 };
		for (Replacement item : replacements)
		{
			source = Regex.Replace(source, item.Pattern, item.getValue(), RegexOptions.IgnoreCase | RegexOptions.Multiline);
		}

		CompilerParameters cp = new CompilerParameters(new String[] { "System.dll", "System.Core.dll", "System.Data.Entity.dll" });
		cp.GenerateInMemory = true;
		cp.GenerateExecutable = false;
		cp.IncludeDebugInformation = false;
		cp.TreatWarningsAsErrors = false;
		cp.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().getLocation());
		CompilerResults cr = provider.CompileAssemblyFromSource(cp, source);
		if (!cr.Errors.HasErrors)
		{

			IJobMetaDataFilter f = (IJobMetaDataFilter)cr.CompiledAssembly.CreateInstance("Fantasy.Jobs.Temporary.JobMetaDataFilter");

			Iterable<JobMetaData> rs = f.Filter(data);
			totalCount.argvalue = rs.Count();
			if (skip > 0 || take < Integer.MAX_VALUE)
			{
				rs = new java.util.ArrayList<JobMetaData>(rs);
			}
			if (skip > 0)
			{
				rs = rs.Skip(skip);
			}
			if (take < Integer.MAX_VALUE)
			{
				rs = rs.Take(take);
			}
			return rs;
		}
		else
		{
			throw new JobException(String.format(Properties.Resources.getInvalidFilterText(), filter));
		}


	}

	public final Iterable<JobMetaData> FindAll()
	{
		java.util.ArrayList<JobMetaData> rs;

		synchronized (_syncRoot)
		{
			rs = new java.util.ArrayList<JobMetaData>(_unterminates);
			rs.addAll(this._entities.getTerminates());
		}
		return rs;
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<JobQueueEventArgs> Changed;

	protected void OnChanged(JobQueueEventArgs e)
	{
		if (this.Changed != null)
		{
			this.Changed(this, e);
		}
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<JobQueueEventArgs> Added;

	protected void OnAdded(JobQueueEventArgs e)
	{
		if (this.Added != null)
		{
			this.Added(this, e);
		}
	}

	public final void Resume(UUID id)
	{
		JobMetaData meta = this.FindJobMetaDataById(id);
		if (meta != null)
		{
			switch (meta.getState())
			{

				case JobState.UserPaused:
					meta.setState(meta.getStartTime() != null ? JobState.Suspended : JobState.Unstarted);
					this.ApplyChange(meta);
					break;
				default:
					throw new InvalidOperationException(String.format(Properties.Resources.getInvalidJobTransitionText(), id, JobState.ToString(meta.getState()), JobState.ToString(JobState.Suspended)));
			}
		}
	}

	public final void Cancel(UUID id)
	{
		JobMetaData meta = this.FindJobMetaDataById(id);
		if (meta != null)
		{
			if(! meta.getIsTerminated())
			{
				if(meta.getState() == JobState.Running)
				{
					if(this.RequestCancel != null)
					{
						this.RequestCancel(this, new JobQueueEventArgs(meta));
					}
				}
				else
				{
					meta.setState(JobState.Cancelled);
					meta.setEndTime(new java.util.Date());
					this.ApplyChange(meta);
				}
			}
			else

			{
				throw new InvalidOperationException(String.format(Properties.Resources.getInvalidJobTransitionText(), id, JobState.ToString(meta.getState()), JobState.ToString(JobState.Cancelled)));
			}
		}
	}

	public final void Suspend(UUID id)
	{
		JobMetaData meta = this.FindJobMetaDataById(id);
		if (meta != null)
		{
			if (!meta.getIsTerminated())
			{
				if (meta.getState() == JobState.Running)
				{
					if (this.RequestSuspend != null)
					{
						this.RequestSuspend(this, new JobQueueEventArgs(meta));
					}

				}
				else
				{
					meta.setState(JobState.Suspended);
					this.ApplyChange(meta);
				}
			}
			else
			{
				throw new InvalidOperationException(String.format(Properties.Resources.getInvalidJobTransitionText(), id, JobState.ToString(meta.getState()), JobState.ToString(JobState.Cancelled)));
			}
		}
	}

	public final void UserPause(UUID id)
	{
		JobMetaData meta = this.FindJobMetaDataById(id);
		if (meta != null)
		{
			if (!meta.getIsTerminated())
			{
				if (meta.getState() == JobState.Running)
				{
					if (this.RequestUserPause != null)
					{
						this.RequestUserPause(this, new JobQueueEventArgs(meta));
					}
				}
				else
				{
					meta.setState(JobState.UserPaused);
					this.ApplyChange(meta);
				}
			}
			else
			{
				throw new InvalidOperationException(String.format(Properties.Resources.getInvalidJobTransitionText(), id, JobState.ToString(meta.getState()), JobState.ToString(JobState.UserPaused)));
			}
		}
	}



//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<JobQueueEventArgs> RequestCancel;

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<JobQueueEventArgs> RequestSuspend;

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<JobQueueEventArgs> RequestUserPause;






	public final String[] GetAllApplications()
	{
		java.util.ArrayList<String> rs = new java.util.ArrayList<String>();

		synchronized (_syncRoot)
		{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			rs.addAll(_unterminates.Select(j => j.Application).Distinct());
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			rs.addAll(this._entities.getTerminates().Select(j => j.Application).Distinct());
		}


//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		return rs.Distinct().toArray();
	}

	public final String[] GetAllUsers()
	{
		java.util.ArrayList<String> rs = new java.util.ArrayList<String>();

		synchronized (_syncRoot)
		{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			rs.addAll(_unterminates.Select(j => j.User).Distinct());
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			rs.addAll(this._entities.getTerminates().Select(j => j.User).Distinct());
		}


//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		return rs.Distinct().toArray();
	}





	public final boolean IsTerminated(UUID id)
	{
		synchronized (_syncRoot)
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			return this._unterminates.BinarySearchBy(id, job => job.Id) < 0;
		}
	}


}