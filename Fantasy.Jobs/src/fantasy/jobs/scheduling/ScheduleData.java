package fantasy.jobs.scheduling;

import java.util.*;


import org.apache.commons.lang3.time.DateUtils;
import org.jdom2.Namespace;

import fantasy.*;
import fantasy.xserialization.*;
import fantasy.collections.*;
import fantasy.jobs.management.*;
import fantasy.servicemodel.*;
import org.jdom2.*;
import org.jdom2.output.Format;
import org.jdom2.transform.*;

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
			
			if (Arrays.asList(monthsOfYear).indexOf(m + 1) >= 0)
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


	@SuppressWarnings("incomplete-switch")
	public final void RunAction() throws Exception
	{
		final IJobQueue queue = this.getSite().getRequiredService(IJobQueue.class);
		ILogger logger = this.getSite().getService(ILogger.class);
		String waitAll = "";
		if (this.getScheduleItem().getMultipleInstance() != InstancesPolicy.Parallel && this.getHistory().size() > 0)
		{
		Enumerable<UUID> query = new Enumerable<UUID>(this.getHistory().get(this.getHistory().size() - 1).getCreatedJobs()).where(new Predicate<UUID>(){

				@Override
				public boolean evaluate(UUID id) throws Exception {
					return ! queue.IsTerminated(id);
				}});     
			switch (this.getScheduleItem().getMultipleInstance())
			{

				case Queue:
					waitAll = StringUtils2.join(";", query);
					break;
				case IgnoreNew:
					if (query.any())
					{
						return;
					}
					break;
				case StopExisting:

					for (UUID id : query)
					{
						queue.Cancel(id);
						if (logger != null)
						{
							logger.LogMessage("Schedule", "Cancel job %1$s because a new scheduled task will start", id);
						}
					}
					break;
			}
		}

		ScheduleEvent tempVar = new ScheduleEvent();
		tempVar.setScheduleTime((java.util.Date)this.NextRunTime);
		tempVar.setExecutionTime(new java.util.Date());
		ScheduleEvent evt = tempVar;
		java.util.ArrayList<UUID> ids = new java.util.ArrayList<UUID>();
		for (String xml : this.CreateStartInfo(waitAll))
		{

			JobMetaData meta = queue.CreateJobMetaData();
			meta.LoadXml(xml);
			queue.Add(meta);
			ids.add(meta.getId());

		}
		evt.setCreatedJobs(ids.toArray(new UUID[]{}));

		this.getHistory().add(evt);
	}


	private Iterable<String> CreateStartInfo(String waitAll) throws Exception
	{

		if (this.getScheduleItem().getAction() != null)
		{
			Namespace ns =  Namespace.getNamespace(fantasy.jobs.Consts.ScheduleNamespaceURI);
			String name = new Enumerable<String>(StringUtils2.split(this.getScheduleItem().getName(), "\\", true)).last();
			Element params = new Element("params" , ns)
			    .addContent(new Element("scheduleTime", ns).setText(this.NextRunTime.toString()))
			    .addContent(new Element("author", ns).setText(this.getScheduleItem().getAuthor()))
			    .addContent(new Element("runTime", ns).setText(new java.util.Date().toString()))
			    .addContent(new Element("name", ns).setText(name))
			    .addContent(new Element("fullname", ns).setText(this.getScheduleItem().getName()))
			    .addContent(new Element("priority", ns).setText(Integer.toString(this.getScheduleItem().getPriority())))
			    .addContent(new Element("waitAll", ns).setText(waitAll));
			    
			if (this.getScheduleItem().getCustomParams() != null)
			{
				Element customParams = this.getScheduleItem().getCustomParams().clone();
				params.addContent(customParams);
			}

			Document doc = new Document().addContent(params);

			switch (this.getScheduleItem().getAction().getType())
			{
				case Template:
					return this.TemplateCreateStartInfo(doc);
				case Inline:
					return this.InlineCreateStartInfo(doc);
				case Custom:
					return this.CustomCreateStartInfo(doc);
				default:
					throw new RuntimeException("Absurd!");
			}
		}
		else
		{
			return new ArrayList<String>();
		}
	}

	@SuppressWarnings("rawtypes")
	private Iterable<String> CustomCreateStartInfo(Document params) throws Exception
	{
		CustomAction action = (CustomAction)this.getScheduleItem().getAction();
		java.lang.Class t = java.lang.Class.forName(action.getCustomType());
		ICustomJobBuilder builder = (ICustomJobBuilder)t.newInstance();
		if (builder instanceof IObjectWithSite)
		{
			((IObjectWithSite)builder).setSite(this.getSite());
		}
		return builder.Execute(params);
	}


	private Iterable<String> Trasform(String xsltString, Document params) throws Exception
	{
		
		ArrayList<String> rs = new ArrayList<String>();
		
		XSLTransformer xslt = new XSLTransformer(JDomUtils.parseDocument(xsltString));
		
		Element root = xslt.transform(params).getRootElement();
		
		Namespace ns = Namespace.getNamespace(fantasy.jobs.Consts.XNamespaceURI);
		
		if(root.getName() == "jobStartList" && root.getNamespaceURI() == fantasy.jobs.Consts.XNamespaceURI)
		{
			for(Element si : root.getChildren("jobStart", ns))
			{
				rs.add(JDomUtils.toString(si, Format.getCompactFormat()));
			}
		}
		else if (root.getName() == "jobStart" && root.getNamespaceURI() == fantasy.jobs.Consts.XNamespaceURI)
		{
			rs.add(JDomUtils.toString(root, Format.getCompactFormat()));
		}

		return rs;

	}

	private Iterable<String> InlineCreateStartInfo(Document params) throws Exception
	{
		InlineAction action = (InlineAction)this.getScheduleItem().getAction();
		return this.Trasform(action.getXslt(), params);

	}

	private Iterable<String> TemplateCreateStartInfo(Document params) throws Exception
	{
		IScheduleLibraryService svc = this.getSite().getRequiredService(IScheduleLibraryService.class);
		TemplateAction action = (TemplateAction)this.getScheduleItem().getAction();
		String xslt = svc.getTemplate(action.getTemplate());
		return this.Trasform(xslt, params);
	}


}