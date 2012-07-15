package Fantasy.Jobs;

import Fantasy.XSerialization.*;
import Fantasy.ServiceModel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Instruction, XSerializable("retry", NamespaceUri = Consts.XNamespaceURI)]
public class Retry extends Sequence
{
	public Retry()
	{
		this.size() = "1";
		this.Sleep = "00:00:00";
	}

	@Override
	public void Execute()
	{
		IJob job = this.getSite().<IJob>GetRequiredService();
		IJobEngine engine = this.getSite().<IJobEngine>GetRequiredService();
		ILogger logger = this.getSite().<ILogger>GetService();
		boolean success = false;
		IStringParser parser = this.getSite().<IStringParser>GetRequiredService();
		int count = Integer.parseInt(parser.Parse(this.size()));
		int times = job.getRuntimeStatus().getLocal().GetValue("retry.times", 0);
		TimeSpan sleep = TimeSpan.Parse(parser.Parse(this.Sleep));
		while (times < count && !success)
		{

			try
			{
				this.ExecuteSequence();
				success = true;
			}
			catch(RuntimeException error)
			{
				if (!(error instanceof ThreadAbortException))
				{
					times++;
					job.getRuntimeStatus().getLocal().setItem("retry.times", times);
					if (times < count)
					{
						job.getRuntimeStatus().getLocal().setItem("sequence.current", 0);
						logger.LogError(LogCategories.getInstruction(), error, "Retry instruction catchs a exception, will try again later.");
						if (sleep > TimeSpan.Zero)
						{
						   engine.Sleep(sleep);
						}
					}
					else
					{
						logger.LogError(LogCategories.getInstruction(), error, "Retry instruction catchs a exception and repeat times exceed maximum number ({0}).", this.size());
						throw error;
					}
				}
			}


		}
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("count")]
	public String Count = null;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("sleep")]
	public String Sleep = null;

}