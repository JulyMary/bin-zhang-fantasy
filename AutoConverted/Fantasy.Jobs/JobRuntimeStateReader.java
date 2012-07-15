package Fantasy.Jobs;

import Fantasy.XSerialization.*;

public class JobRuntimeStateReader extends ObjectWithSite implements ITagValueProvider
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITagValueProvider Members

	public final char getPrefix()
	{
		return '#';
	}

	public final String GetTagValue(String tag, java.util.Map<String, Object> context)
	{
		String rs = null;
		RefObject<String> tempRef_rs = new RefObject<String>(rs);
		this.TryGetValue(tag, context, tempRef_rs);
		rs = tempRef_rs.argvalue;
		return rs;
	}


	private boolean TryGetValue(String name, java.util.Map<String, Object> context, RefObject<String> value)
	{
		boolean rs = false;
		value.argvalue = null;
		if (this.Site != null)
		{

			String[] names = name.split(new char[] { '.' }, 2);
			name = names[0];

			String meta = names.length > 1 ? names[1] : null;

			IJob job = (Job)this.getSite().GetService(IJob.class);

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
					TypeConverter cvt = XHelper.Default.CreateXConverter(o.getClass());
					value.argvalue = cvt.ConvertToString(o);
				}
			}

		}


		return rs;
	}

	public final boolean HasTag(String tag, java.util.Map<String, Object> context)
	{

		String value = null;
		RefObject<String> tempRef_value = new RefObject<String>(value);
		boolean tempVar = this.TryGetValue(tag, context, tempRef_value);
		value = tempRef_value.argvalue;
		return tempVar;

	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITagValueProvider Members


	public final boolean IsEnabled(java.util.Map<String, Object> context)
	{
		return (boolean)context.GetValueOrDefault("EnableTaskItemReader", true) && this.Site != null && this.Site.GetService(IJob.class) != null;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}