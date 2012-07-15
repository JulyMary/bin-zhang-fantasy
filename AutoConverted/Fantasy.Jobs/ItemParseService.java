package Fantasy.Jobs;

import Fantasy.ServiceModel.*;

public class ItemParseService extends AbstractService implements IItemParser
{
	public final TaskItem[] GetItemByNames(String names)
	{
		IJob job = (IJob)this.Site.GetService(IJob.class);
		java.util.ArrayList<TaskItem> rs = new java.util.ArrayList<TaskItem>();
		for (String name : names.split("[;]", -1))
		{
			if (!DotNetToJavaStringHelper.isNullOrEmpty(name))
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
		}

		return rs.toArray(new TaskItem[]{});
	}

	public final TaskItem[] ParseItem(String text)
	{
		String str = this.ParseStringWithoutItems(text);
		Regex reg = new Regex("(?<sym>(#|@|%))\\((?<cate>[\\w]+)\\)");
		java.util.ArrayList<TaskItem> rs = new java.util.ArrayList<TaskItem>();
		if (!DotNetToJavaStringHelper.isNullOrEmpty(str))
		{
			int s = 0;
			while (s < str.length())
			{
				Match m = reg.Match(str, s);
				if (m.Success)
				{
					String itemNames = str.substring(s, m.Index);
					if (!DotNetToJavaStringHelper.isNullOrEmpty(itemNames))
					{
						rs.addAll(this.GetItemByNames(itemNames));
					}
					String sym = m.Groups["sym"].getValue();
					if (sym.equals("@") || sym.equals("%"))
					{
						String[] names = m.Groups["cate"].getValue().split(new char[] { '.' }, 2);
						if (names.length == 1)
						{
							TaskItem[] items = this.GetItemByCategory(m.Groups["cate"].getValue());
							if (items.length > 0)
							{
								if (sym.equals("@"))
								{
									rs.addAll(items);
								}
								else
								{
									rs.add(items.First());
								}
							}
						}
						else
						{
							IStringParser parser = this.Site.<IStringParser>GetRequiredService();
							String meta = parser.Parse(m.getValue());
							rs.addAll(this.GetItemByNames(meta));
						}

					}
					else
					{
						String[] names = m.Groups["cate"].getValue().split(new char[] { '.' }, 2);
						IJob job = this.Site.<IJob>GetRequiredService();

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
										rs.addAll(this.GetItemByNames(item.getItem(names[1])));
									}
									else
									{
										rs.add(item);
									}
								}
							}
							else if (var instanceof String)
							{
								rs.addAll(this.GetItemByNames((String)var));
							}
						}
					}
					s = m.Index + m.getLength();
				}
				else
				{
					String itemNames = str.substring(s);
					if (!DotNetToJavaStringHelper.isNullOrEmpty(itemNames))
					{
						rs.addAll(this.GetItemByNames(itemNames));
					}
					s = str.length();
				}
			}
		}

		return rs.toArray(new TaskItem[]{});
	}

	public final TaskItem[] GetItemByCategory(String category)
	{
		IJob job = (IJob)this.Site.GetService(IJob.class);
		TaskItem[] items = job.GetEvaluatedItemsByCatetory(category);
		return items;
	}

	private String ParseStringWithoutItems(String value)
	{
		java.util.HashMap<String, Object> context = new java.util.HashMap<String, Object>();
		context.put("EnableTaskItemReader", false);

		IStringParser parser = (IStringParser)this.Site.GetService(IStringParser.class);
		return parser.Parse(value, context);
	}


}