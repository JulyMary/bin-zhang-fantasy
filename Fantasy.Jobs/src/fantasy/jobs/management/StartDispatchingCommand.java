﻿package fantasy.jobs.management;

import fantasy.*;
import fantasy.servicemodel.*;

public class StartDispatchingCommand implements ICommand, IObjectWithSite
{

	private IServiceProvider privateSite;
	public final IServiceProvider getSite()
	{
		return privateSite;
	}
	public final void setSite(IServiceProvider value)
	{
		privateSite = value;
	}


	public final Object Execute(Object args) throws Exception
	{
		final IJobDispatcher disp = (IJobDispatcher)this.getSite().getService(IJobDispatcher.class);
		if (disp != null)
		{
			
			ThreadFactory.queueUserWorkItem(new Runnable(){

				@Override
				public void run() {
					disp.start();
					
				}});
		}


		return null;

	}


}