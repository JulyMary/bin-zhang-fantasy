package fantasy.jobs.scheduling;

import java.util.Arrays;
import java.util.Calendar;
import java.util.Date;

import org.apache.commons.lang3.time.DateUtils;

import fantasy.*;
import fantasy.xserialization.*;
import fantasy.jobs.management.*;
import fantasy.servicemodel.*;

@XSerializable(name = "schedule.data", namespaceUri = fantasy.jobs.Consts.ScheduleNamespaceURI)
class ScheduleData extends ObjectWithSite
{
	public ScheduleData()
	{
		this.setHistory(new java.util.ArrayList<ScheduleEvent>());
		this.ExecutedCount = 0;
		this.NextRunTime = null;
	}

	@XAttribute(name = "executedCount")
	public int ExecutedCount;


	@XAttribute(name = "name")
	public String Name = null;


	@XArray(name = "history", items= @XArrayItem(type = ScheduleEvent.class, name = "event"))
	private java.util.ArrayList<ScheduleEvent> privateHistory;
	public final java.util.ArrayList<ScheduleEvent> getHistory()
	{
		return privateHistory;
	}
	private void setHistory(java.util.ArrayList<ScheduleEvent> value)
	{
		privateHistory = value;
	}

    @XAttribute(name = "nextRunTime")
	public java.util.Date NextRunTime = null;


	private java.util.Date _lastRunTime;

	@XAttribute(name = "expired")
	public boolean Expired = false;

	private ScheduleItem privateScheduleItem;
	public final ScheduleItem getScheduleItem()
	{
		return privateScheduleItem;
	}
	public final void setScheduleItem(ScheduleItem value)
	{
		privateScheduleItem = value;
	}

	private long privateScheduleCookie;
	public final long getScheduleCookie()
	{
		return privateScheduleCookie;
	}
	public final void setScheduleCookie(long value)
	{
		privateScheduleCookie = value;
	}

	public final boolean OnLoad()
	{
		boolean rs = false;
		if (!this.Expired)
		{
			if (this.NextRunTime == null)
			{
				EvalNext();
				rs = true;
			}
			else if (this.getScheduleItem().getStartWhenAvailable() == false && this.NextRunTime.compareTo(new java.util.Date()) < 0)
			{
				EvalNext();
				rs = true;
			}
		}
		return rs;
	}

	public final void EvalNext()
	{

		_lastRunTime = this.NextRunTime;
		java.util.Date rs = null;
		Date now = new Date();

		if (this.getScheduleItem().getTrigger() != null)
		{
			if (getScheduleItem().getTrigger().getExecutionTimeLimit() != null && ExecutedCount >= getScheduleItem().getTrigger().getExecutionTimeLimit())
			{
				rs = null;
			}
			else
			{

				java.util.Date baseTime;

				if (getScheduleItem().getStartWhenAvailable())
				{
					baseTime = _lastRunTime != null ? DateUtils.addMilliseconds(_lastRunTime, 1) : getScheduleItem().getTrigger().getStartBoundary();
				}
				else
				{
					if (_lastRunTime == null || now.compareTo(_lastRunTime) > 0)
					{
						baseTime = now;
					}
					else
					{
						baseTime = DateUtils.addMilliseconds(_lastRunTime, 1);
					}
				}

				boolean success = false;
				do
				{
					baseTime = EvalNextDate((java.util.Date)baseTime);
					if (baseTime != null)
					{
						java.util.Date b = (java.util.Date)baseTime;
						RefObject<java.util.Date> tempRef_b = new RefObject<java.util.Date>(b);
						success = EvalNextTime(tempRef_b);
						b = tempRef_b.argvalue;
						baseTime = b;
					}
				} while (baseTime != null && !success);

				if (baseTime == null)
				{
					rs = null;
				}
				else
				{
					java.util.Date endBoundary = this.getScheduleItem().getTrigger().getEndBoundary();
					rs = endBoundary == null || endBoundary.compareTo(baseTime) >= 0 ? baseTime : null;
				}
			}

			this.ExecutedCount++;
			this.NextRunTime = rs;
			this.getScheduleItem().getTrigger().setNextRunTime(rs);
			this.getScheduleItem().getTrigger().setLastRunTime(_lastRunTime);
		}
		if (rs == null)
		{
			this.getScheduleItem().setExpired(this.Expired = true);
		}
	}

	private static TimeSpan MaxTimeOfDay = new TimeSpan(24, 0, 0);

	private boolean EvalNextTime(RefObject<java.util.Date> baseTime)
	{
		boolean rs = false;
		Trigger trigger = getScheduleItem().getTrigger();
		Repetition repetition = trigger.getRepetition();
		TimeSpan startTime = getScheduleItem().getTrigger().getStartTime();
		if (TimeSpan.timeOfDay(baseTime.argvalue).isLessThanOrEqual(startTime))
		{
			baseTime.argvalue = startTime.add(DateUtils2.getDate(baseTime.argvalue));
			rs = true;
		}
		else if (repetition != null && repetition.getInterval().isGreaterThan(TimeSpan.Zero))
		{
			TimeSpan boundary = new TimeSpan();
			if (repetition.getDuration() != null)
			{
				boundary = startTime.Add(repetition.getDuration());
				if (boundary.isGreaterThanOrEqual(MaxTimeOfDay))
				{
					boundary = MaxTimeOfDay;
				}
			}
			else
			{
				boundary = MaxTimeOfDay;
			}

			double currentTicks = TimeSpan.timeOfDay(baseTime.argvalue).getTicks();
			int count = (int)Math.floor((currentTicks - (double)startTime.getTicks()) / (double)repetition.getInterval().getTicks()) + 1;
			TimeSpan nextTime = new TimeSpan(repetition.getInterval().getTicks() * count + startTime.getTicks());
			if (nextTime.isLessThan(boundary))
			{
				baseTime.argvalue = nextTime.add(DateUtils2.getDate(baseTime.argvalue));
				rs = true;
			}
		}
		if (!rs)
		{
			baseTime.argvalue = DateUtils.addDays(DateUtils2.getDate(baseTime.argvalue), 1);
		}
		return rs;
	}

	private java.util.Date EvalNextDate(java.util.Date baseTime)
	{
		switch (getScheduleItem().getTrigger().getType())
		{
			case Time:
				return EvalOneTime(baseTime);

			case Daily:
				return EvalDaily(baseTime);

			case Weekly:
				return EvalWeekly(baseTime);

			case Monthly:
				return EvalMonthly(baseTime);

			case MonthlyDayOfWeek:
				return EvalMonthlyDOW(baseTime);
			default:
				throw new RuntimeException("Absurd!");

		}
	}

	private java.util.Date EvalMonthlyDOW(java.util.Date baseTime)
	{
		MonthlyDOWTrigger trigger = (MonthlyDOWTrigger)this.getScheduleItem().getTrigger();

		if (trigger.getMonthsOfYear().length == 0 || (trigger.getWeeksOfMonth().length == 0 && trigger.getRunOnLastWeekOfMonth() == false))
		{
			return null;
		}
		java.util.Date rs = null;
		do
		{
			java.util.Date b = EvalMonthOfYear(baseTime, trigger.getMonthsOfYear());
			if (b == null)
			{
				return null;
			}
			else
			{
				baseTime = (java.util.Date)b;
			}

			rs = EvalDayOfWeekOfMonth(baseTime, trigger);
			if (rs == null)
			{
				
				baseTime = DateUtils2.nextMonth(baseTime);
			}
		} while (rs == null);



		return rs;


	}

	private java.util.Date EvalDayOfWeekOfMonth(java.util.Date baseTime, MonthlyDOWTrigger trigger)
	{
		java.util.Date rs = null;
		
		Calendar cal = Calendar.getInstance();
		cal.setTime(DateUtils2.getDate(baseTime));
		int lastDOM = cal.getActualMaximum(Calendar.DAY_OF_MONTH);
		int day = -1;
		for (int i = cal.get(Calendar.DATE); i <= lastDOM; i++)
		{
			boolean inWeeks = false;
			WeekOfMonth wom = WeekOfMonth.forValue(i / 7);
			inWeeks = Arrays.asList(trigger.getWeeksOfMonth()).indexOf(wom) >= 0;
			if (!inWeeks && trigger.getRunOnLastWeekOfMonth() && i > lastDOM - 7)
			{
				inWeeks = true;
			}
			if (inWeeks)
			{
				Calendar cal2 = Calendar.getInstance();
				cal2.setTime(DateUtils2.getDate(baseTime));
				cal2.set(Calendar.DATE, i);
				
				
				if (Arrays.asList(trigger.getDaysOfWeek()).indexOf(cal2.get(Calendar.DAY_OF_WEEK)) >= 0)
				{
					day = i;
					break;
				}
			}

		}
		if (day != -1)
		{
			if (DateUtils2.getDay(baseTime) < day)
			{
				rs = DateUtils.addDays(DateUtils2.getDate(baseTime), day -  DateUtils2.getDay(baseTime));
			}
			else
			{
				rs = baseTime;
			}
		}
		return rs;
	}

	private java.util.Date EvalMonthly(java.util.Date baseTime)
	{
		MonthlyTrigger trigger = (MonthlyTrigger)this.getScheduleItem().getTrigger();
		if (trigger.getMonthsOfYear().length == 0 || (trigger.getDaysOfMonth().length == 0 && trigger.getRunOnLastDayOfMonth() == false))
		{
			return null;
		}


		java.util.Date rs;
		do
		{
			java.util.Date b = EvalMonthOfYear(baseTime, trigger.getMonthsOfYear());
			if (b == null)
			{
				return null;
			}
			else
			{
				baseTime = (java.util.Date)b;
			}
			rs = EvalDayOfMonth(baseTime, trigger);
			if (rs == null)
			{
				
				baseTime = DateUtils2.nextMonth(baseTime);
				
			}
		} while (rs == null);


		return rs;

	}

	private java.util.Date EvalDayOfMonth(java.util.Date baseTime, MonthlyTrigger trigger)
	{
		java.util.Date rs = null;

		Calendar cal = Calendar.getInstance();
		cal.setTime(DateUtils2.getDate(baseTime));
		int lastDOM = cal.getActualMaximum(Calendar.DAY_OF_MONTH);
		int day = -1;
		
		for (int i = DateUtils2.getDay(baseTime); i <= lastDOM; i++)
		{
			if (Arrays.asList(trigger.getDaysOfMonth()).indexOf(i) >= 0)
			{
				day = i;
				break;
			}
		}

		if (day == -1 && trigger.getRunOnLastDayOfMonth())
		{
			day = lastDOM;
		}
		if (day != -1)
		{
			if (day != DateUtils2.getDay(baseTime))
			{
				rs = DateUtils.addDays(DateUtils2.getDate(baseTime), day - DateUtils2.getDay(baseTime));
			}
			else
			{
				rs = baseTime;
			}
		}
		return rs;
	}

	

	private java.util.Date EvalMonthOfYear(java.util.Date baseTime, int[] monthsOfYear)
	{

		java.util.Date rs = null;

		for (int i = 0; i < 12; i++)
		{
			int m = (DateUtils2.getMonth(baseTime) + i) % 12;
			
			if (Arrays.asList(monthsOfYear).indexOf(m) >= 0)
			{
				if (i > 0)
				{
					rs = DateUtils.addMonths(DateUtils2.thisMonth(baseTime), i);
				}
				else
				{
					rs = baseTime;
				}
				break;
			}
		}
		return rs;
	}

	private java.util.Date EvalWeekly(java.util.Date baseTime)
	{
		WeeklyTrigger trigger = (WeeklyTrigger)this.getScheduleItem().getTrigger();
		if (trigger.getDaysOfWeek().length == 0)
		{
			return null;
		}

		java.util.Date l = (this._lastRunTime != null) ? this._lastRunTime : trigger.getStartBoundary();

		java.util.Date lastSunday = DateUtils2.thisSunday(l);
		java.util.Date baseSunday = DateUtils2.thisSunday(baseTime);

		int mod = ((int)(TimeSpan.substract(DateUtils2.getDate(baseSunday),  DateUtils2.getDate(lastSunday)).getTotalDays()) / 7) % trigger.getWeeksInterval();

		if (mod > 0)
		{
			baseTime = DateUtils.addDays(baseSunday, (trigger.getWeeksInterval() - mod) * 7);
		}

		java.util.Date rs;
		do
		{
			rs = EvalDayOfWeek(baseTime, trigger.getDaysOfWeek());
			if (rs == null)
			{
				baseSunday = DateUtils.addDays(baseSunday, 7 * trigger.getWeeksInterval());
				baseTime = baseSunday;
			}
		} while (rs == null);
		return rs;

	}

	private java.util.Date EvalDayOfWeek(java.util.Date baseTime, DayOfWeek[] daysOfWeek)
	{
		for (int i = DateUtils2.getDayOfWeek(baseTime).getValue(); i <= DayOfWeek.Saturday.getValue(); i++)
		{
			DayOfWeek day = DayOfWeek.forValue(i);
			if (Arrays.asList(daysOfWeek).indexOf(day) >= 0)
			{
				if (! DateUtils2.getDayOfWeek(baseTime).equals(day))
				{
					return DateUtils.addDays(DateUtils2.getDate(baseTime) , i - DateUtils2.getDayOfWeek(baseTime).getValue());
				}
				else
				{
					return baseTime;
				}
			}
		}

		return null;
	}



	private java.util.Date EvalDaily(java.util.Date baseTime)
	{
		DailyTrigger trigger = (DailyTrigger)this.getScheduleItem().getTrigger();
		java.util.Date rs = null;
		java.util.Date l = (this._lastRunTime != null) ? this._lastRunTime : trigger.getStartBoundary();
		l = DateUtils2.getDate(l);
		int lapsedDays = (int) TimeSpan.substract(DateUtils2.getDate(baseTime), l).getTotalDays();
		int mod = lapsedDays % trigger.getDaysInterval();
		if (mod > 0)
		{
			rs = DateUtils.addDays(DateUtils2.getDate(baseTime), trigger.getDaysInterval() - mod);
		}
		else
		{
			rs = baseTime;
		}
		return rs;
	}

	private java.util.Date EvalOneTime(java.util.Date baseTime)
	{
		TimeTrigger trigger = (TimeTrigger)this.getScheduleItem().getTrigger();
		if (DateUtils2.getDate(baseTime).compareTo(trigger.getDate()) < 0)
		{
			return trigger.getDate();
		}
		else if (trigger.getDate().equals(DateUtils2.getDate(baseTime)))
		{
			return baseTime;
		}
		else
		{
			return null;
		}
	}


	public final void RunAction()
	{
		IJobQueue queue = this.Site.<IJobQueue>GetRequiredService();
		ILogger logger = this.Site.<ILogger>GetService();
		String waitAll = "";
		if (this.getScheduleItem().getMultipleInstance() != InstancesPolicy.Parallel && this.getHistory().size() > 0)
		{
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: This type of object initializer has no direct Java equivalent:
			var query = from id in this.getHistory().Last().CreatedJobs select new { Id = id, IsTerminated = queue.IsTerminated(id) };
			switch (this.getScheduleItem().getMultipleInstance())
			{

				case Queue:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
					waitAll = DotNetToJavaStringHelper.join(";", query.Where(j => !j.IsTerminated).Select(j => j.Id));
					break;
				case IgnoreNew:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
					if (query.Any(j => !j.IsTerminated))
					{
						return;
					}
					break;
				case StopExisting:
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
					for (var job : query.Where(j => !j.IsTerminated))
					{
						queue.Cancel(job.Id);
						if (logger != null)
						{
							logger.LogMessage("Schedule", "Cancel job {0} because a new scheduled task will start", job.Id);
						}
					}
					break;
			}
		}

		ScheduleEvent tempVar = new ScheduleEvent();
		tempVar.setScheduleTime((java.util.Date)this.NextRunTime);
		tempVar.setExecutionTime(new java.util.Date());
		ScheduleEvent evt = tempVar;
		java.util.ArrayList<Guid> ids = new java.util.ArrayList<Guid>();
		for (String xml : this.CreateStartInfo(waitAll))
		{

			JobMetaData meta = queue.CreateJobMetaData();
			meta.LoadXml(xml);
			queue.ApplyChange(meta);
			ids.add(meta.getId());

		}
		evt.setCreatedJobs(ids.toArray(new Guid[]{}));

		this.getHistory().add(evt);
	}


	private Iterable<String> CreateStartInfo(String waitAll)
	{

		if (this.getScheduleItem().getAction() != null)
		{
			XNamespace ns = Consts.ScheduleNamespaceURI;
			String name = this.getScheduleItem().getName().split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).Last();
			XElement params = new XElement(ns + "params", new XElement(ns + "scheduleTime", this.NextRunTime.toString()), new XElement(ns + "author", this.getScheduleItem().getAuthor()), new XElement(ns + "runTime", new java.util.Date().toString()), new XElement(ns + "name", name), new XElement(ns + "fullname", this.getScheduleItem().getName()), new XElement(ns + "priority", this.getScheduleItem().getPriority()), new XElement(ns + "waitAll", waitAll));
			if (this.getScheduleItem().getCustomParams() != null)
			{
				XElement customParams = XElement.Parse(this.getScheduleItem().getCustomParams());
				params.Add(customParams);
			}


			switch (this.getScheduleItem().getAction().getType())
			{
				case Template:
					return this.TemplateCreateStartInfo(params);
				case Inline:
					return this.InlineCreateStartInfo(params);
				case Custom:
					return this.CustomCreateStartInfo(params);
				default:
					throw new RuntimeException("Absurd!");
			}
		}
		else
		{
			return new String[0];
		}
	}

	private Iterable<String> CustomCreateStartInfo(XElement params)
	{
		CustomAction action = (CustomAction)this.ScheduleItem.getAction();
		java.lang.Class t = java.lang.Class.forName(action.getCustomType(), true);
		ICustomJobBuilder builder = (ICustomJobBuilder)Activator.CreateInstance(t);
		if (builder instanceof IObjectWithSite)
		{
			((IObjectWithSite)builder).Site = this.Site;
		}
		return builder.Execute(params);
	}


	private Iterable<String> Trasform(String xsltString, XElement params)
	{
		XsltSettings tempVar = new XsltSettings();
		tempVar.EnableDocumentFunction = true;
		tempVar.EnableScript = true;
		XsltSettings xsltSettings = tempVar;
		XslCompiledTransform xslt = new XslCompiledTransform();
		XElement root = XElement.Parse(xsltString);
		xslt.Load(root.CreateReader(), xsltSettings, null);
		Stream stream = new MemoryStream();
		xslt.Transform(params.CreateReader(), null, stream);
		stream.Seek(0, SeekOrigin.Begin);
		XElement rs = XElement.Load(stream);
		XNamespace ns = Consts.XNamespaceURI;
		if (rs.getName() == ns + "jobStartList")
		{
			for (XElement si : rs.Elements(ns + "jobStart"))
			{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
				yield return si.ToString(SaveOptions.OmitDuplicateNamespaces);
			}
		}
		else if (rs.getName() == ns + "jobStart")
		{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
			yield return rs.ToString(SaveOptions.OmitDuplicateNamespaces);
		}

	}

	private Iterable<String> InlineCreateStartInfo(XElement params)
	{
		InlineAction action = (InlineAction)this.ScheduleItem.getAction();
		return this.Trasform(action.getXslt(), params);

	}

	private Iterable<String> TemplateCreateStartInfo(XElement params)
	{
		IScheduleLibraryService svc = this.Site.<IScheduleLibraryService>GetRequiredService();
		TemplateAction action = (TemplateAction)this.ScheduleItem.getAction();
		String xslt = svc.GetTemplate(action.getTemplate());
		return this.Trasform(xslt, params);
	}
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

}