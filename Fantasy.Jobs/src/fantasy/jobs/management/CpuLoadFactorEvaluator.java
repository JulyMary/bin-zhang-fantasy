package fantasy.jobs.management;

import java.util.concurrent.*;
import fantasy.*;

import org.hyperic.sigar.*;

public class CpuLoadFactorEvaluator extends ObjectWithSite implements IComputerLoadFactorEvaluator
{

	
	private Sigar _sigar = new Sigar();
	ScheduledExecutorService _scheduler;
	public CpuLoadFactorEvaluator() throws SigarException
	{
		CpuPerc cpuPerc = this._sigar.getCpuPerc();
		
		
		double idle = cpuPerc.getIdle();
		for (int i = 0; i < 10; i++)
		{
			_idle[i] = idle;
		}
		
		
		_scheduler = Executors.newScheduledThreadPool(1);
		_scheduler.scheduleAtFixedRate(new Runnable(){

			@Override
			public void run() {
				CpuLoadFactorEvaluator.this.refresh();
			}}, 1, 1, TimeUnit.SECONDS);
		
		_scheduler.shutdown();

	}

	private double[] _idle = new double[10];
	private int _index = 0;

	private void refresh()
	{


	
	
			try {
				_idle[_index] = this._sigar.getCpuPerc().getIdle();
				_index++;
				_index %= 10;
			} catch (SigarException e) {
				
				e.printStackTrace();
			}

	}


	public final double Evaluate() throws Exception
	{
		IJobController controller = null;
	
		controller = this.getSite().getRequiredService(IJobController.class);
		
		double sum = 0;
		for(double d : _idle)
		{
			sum +=d;
		}
		
		return sum / 10 * controller.GetAvailableProcessCount();
	}

}