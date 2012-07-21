package fantasy.jobs;

import java.rmi.RemoteException;
import java.util.Arrays;
import java.util.regex.*;

import fantasy.servicemodel.*;
import fantasy.*;

public class ItemParseService extends AbstractService implements IItemParser
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 8979094706545956324L;

	public ItemParseService() throws RemoteException {
		super();
		
	}

	public final TaskItem[] GetItemByNames(String names)
	{
		IJob job = this.getSite().getService2(IJob.class);
		java.util.ArrayList<TaskItem> rs = new java.util.ArrayList<TaskItem>();
		for (String name : StringUtils2.split(names, ";", true))
		{
			
			TaskItem item = job.GetEvaluatedItemByName(name);
			if (item == null)
			{
				TaskItem tempVar = new TaskItem();
				tempVar.setName(name);
				item = tempVar;

			}
			rs.add(item);
			
		}

		return rs.toArray(new TaskItem[]{});
	}

	public final TaskItem[] ParseItem(String text)
	{
		String str = this.ParseStringWithoutItems(text);
		Pattern reg = Pattern.compile("(?<sym>(#|@|%))\\((?<cate>[\\w]+)\\)");
		java.util.ArrayList<TaskItem> rs = new java.util.ArrayList<TaskItem>();
		if (!StringUtils2.isNullOrEmpty(str))
		{
			int s = 0;
			while (s < str.length())
			{
				Matcher m = reg.matcher(str.substring(s));
				if (m.lookingAt())
				{
					String itemNames = str.substring(s, m.start());
					if (!StringUtils2.isNullOrEmpty(itemNames))
					{
						rs.addAll(Arrays.asList(this.GetItemByNames(itemNames)));
					}
					String sym = m.group("sym");
					if (sym.equals("@") || sym.equals("%"))
					{
						String[] names = m.group("cate").split("\\.", 2);
						if (names.length == 1)
						{
							TaskItem[] items = this.GetItemByCategory(m.group("cate"));
							if (items.length > 0)
							{
								if (sym.equals("@"))
								{
									rs.addAll(Arrays.asList(items));
								}
								else
								{
									rs.add(items[0]);
								}
							}
						}
						else
						{
							IStringParser parser = this.getSite().getRequiredService2(IStringParser.class);
							String meta = parser.Parse(m.group());
							rs.addAll(Arrays.asList(this.GetItemByNames(meta)));
						}

					}
					else
					{
						String[] names = m.group("cate").split("\\.", 2);
						IJob job = this.getSite().getRequiredService2(IJob.class);

						Object var = null;
						RefObject<Object> tempRef_var = new RefObject<Object>(var);
						boolean tempVar = job.getRuntimeStatus().TryGetValue(names[0], tempRef_var);
						var = tempRef_var.argvalue;
						if (tempVar)
						{
							if (var instanceof TaskItem)
							{
								TaskItem item = (TaskItem)var;
								if (item != null)
								{
									if (names.length > 1)
									{
										rs.addAll(Arrays.asList(this.GetItemByNames(item.getItem(names[1]))));
									}
									else
									{
										rs.add(item);
									}
								}
							}
							else if (var instanceof String)
							{
								rs.addAll(Arrays.asList(this.GetItemByNames((String)var)));
							}
						}
					}
					s = m.end();
				}
				else
				{
					String itemNames = str.substring(s);
					if (!StringUtils2.isNullOrEmpty(itemNames))
					{
						rs.addAll(Arrays.asList(this.GetItemByNames(itemNames)));
					}
					s = str.length();
				}
			}
		}

		return rs.toArray(new TaskItem[]{});
	}

	public final TaskItem[] GetItemByCategory(String category)
	{
		IJob job = (IJob)this.getSite().getService2(IJob.class);
		TaskItem[] items = job.GetEvaluatedItemsByCatetory(category);
		return items;
	}

	private String ParseStringWithoutItems(String value)
	{
		java.util.HashMap<String, Object> context = new java.util.HashMap<String, Object>();
		context.put("EnableTaskItemReader", false);

		IStringParser parser = (IStringParser)this.getSite().getService(IStringParser.class);
		return parser.Parse(value, context);
	}


}