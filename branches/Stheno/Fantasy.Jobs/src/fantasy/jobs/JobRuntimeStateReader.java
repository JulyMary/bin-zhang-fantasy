package fantasy.jobs;

import fantasy.collections.*;
import fantasy.xserialization.*;
import fantasy.*;

public class JobRuntimeStateReader extends ObjectWithSite implements ITagValueProvider
{
	public final char getPrefix()
	{
		return '#';
	}

	public final String GetTagValue(String tag, java.util.Map<String, Object> context) throws Exception
	{
		String rs = null;
		RefObject<String> tempRef_rs = new RefObject<String>(rs);
		this.TryGetValue(tag, context, tempRef_rs);
		rs = tempRef_rs.argvalue;
		return rs;
	}


	private boolean TryGetValue(String name, java.util.Map<String, Object> context, RefObject<String> value) throws Exception
	{
		boolean rs = false;
		value.argvalue = null;
		if (this.getSite() != null)
		{

			String[] names = name.split("\\.", 2);
			name = names[0];

			String meta = names.length > 1 ? names[1] : null;

			IJob job = (Job)this.getSite().getService(IJob.class);

			Object o = null;

			RefObject<Object> tempRef_o = new RefObject<Object>(o);
			rs = job.getRuntimeStatus().TryGetValue(name, tempRef_o);
			o = tempRef_o.argvalue;

			if (rs)
			{
				if (o instanceof TaskItem)
				{
					TaskItem item = (TaskItem)o;
					value.argvalue = meta != null ? item.getItem(meta) : item.getName();
				}
				else if(meta != null)
				{
					TaskItem item = job.GetEvaluatedItemByName((String)o);
					if(item != null)
					{
						value.argvalue = item.getItem(meta);
					}
				}
				else if(o != null)
				{
					ITypeConverter cvt = XHelper.getDefault().CreateXConverter(o.getClass());
					value.argvalue = (String)cvt.convertFrom(o);
				}
			}

		}


		return rs;
	}

	public final boolean HasTag(String tag, java.util.Map<String, Object> context) throws Exception
	{

		String value = null;
		RefObject<String> tempRef_value = new RefObject<String>(value);
		boolean tempVar = this.TryGetValue(tag, context, tempRef_value);
		value = tempRef_value.argvalue;
		return tempVar;

	}



	public final boolean IsEnabled(java.util.Map<String, Object> context) throws Exception
	{
		return MapUtils.getValueOrDefault(context, "EnableTaskItemReader", true) && this.getSite() != null && this.getSite().getService(IJob.class) != null;
	}

}