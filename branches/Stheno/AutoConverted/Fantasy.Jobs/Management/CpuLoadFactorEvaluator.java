package Fantasy.Jobs.Management;

public class CpuLoadFactorEvaluator extends ObjectWithSite implements IComputerLoadFactorEvaluator
{

	private Thread _refreshThread;
	private PerformanceCounter _cpuCounter;
	public CpuLoadFactorEvaluator()
	{
		PerformanceCounter tempVar = new PerformanceCounter();
		tempVar.CategoryName = "Processor";
		tempVar.CounterName = "% Processor Time";
		tempVar.InstanceName = "_Total";
		_cpuCounter = tempVar;
		_cpuCounter.NextValue();
		double idle = (100.0 - _cpuCounter.NextValue()) / 100.0;
		for (int i = 0; i < 10; i++)
		{
			_idle[i] = idle;
		}
		_refreshThread = ThreadFactory.CreateThread(Refresh).WithStart();

	}

	private double[] _idle = new double[10];

	private void Refresh()
	{


		int n = 0;
		while (true)
		{
			Thread.sleep(1 * 1000);
			_idle[n] = (100 - _cpuCounter.NextValue()) / 100;
			n++;
			n %= 10;
		}
	}


//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IComputerLoadFactorEvaluator Members

	public final double Evaluate()
	{
		IJobController controller = this.Site.<IJobController>GetService();
		return _idle.Sum() / 10 * controller.GetAvailableProcessCount();
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}