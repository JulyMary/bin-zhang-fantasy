package fantasy.jobs;

import org.joda.time.Duration;

import fantasy.xserialization.*;
import fantasy.jobs.properties.Resources;
import fantasy.servicemodel.*;

@Instruction
@XSerializable(name = "retry", namespaceUri = Consts.XNamespaceURI)
public class Retry extends Sequence
{
	public Retry()
	{
		this.Count = "1";
		this.Sleep = "00:00:00";
	}

	@Override
	public void Execute() throws Exception
	{
		IJob job = this.getSite().getRequiredService(IJob.class);
		IJobEngine engine = this.getSite().getRequiredService(IJobEngine.class);
		ILogger logger = this.getSite().getService(ILogger.class);
		boolean success = false;
		IStringParser parser = this.getSite().getRequiredService(IStringParser.class);
		int count = Integer.parseInt(parser.Parse(this.Count));
		int times = job.getRuntimeStatus().getLocal().GetValue("retry.times", 0);
		Duration sleep = Duration.parse(parser.Parse(this.Sleep));
		while (times < count && !success)
		{

			if(Thread.interrupted())
			{
				throw new InterruptedException();
			}
			try
			{
				this.ExecuteSequence();
				success = true;
			}
		    catch(InterruptedException error)
		    {
		    	throw error;
		    }
			catch(Exception error)
			{
				
					times++;
					job.getRuntimeStatus().getLocal().setItem("retry.times", times);
					if (times < count)
					{
						job.getRuntimeStatus().getLocal().setItem("sequence.current", 0);
						Log.SafeLogError(logger, LogCategories.getInstruction(), error, Resources.getRetryLaterMessage());
						
						if (sleep.isLongerThan(Duration.ZERO))
						{
						   engine.Sleep(sleep);
						}
					}
					else
					{
						Log.SafeLogError(logger, LogCategories.getInstruction(), error, Resources.getRetryExceedLimitMessage(), this.Count);
						
						throw error;
					}
				
			}


		}
	}

	@XAttribute(name = "count")
	public String Count = null;

	@XAttribute(name = "sleep")
	public String Sleep = null;

}