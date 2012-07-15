package Fantasy.Jobs;

import Fantasy.XSerialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Instruction, XSerializable("foreach", NamespaceUri = Consts.XNamespaceURI)]
public class Foreach extends Sequence
{
	@Override
	public void Execute()
	{
		IJob job = this.getSite().<IJob>GetRequiredService();
		IStringParser parser = this.getSite().<IStringParser>GetRequiredService();
		java.util.HashMap<String, Object> context = new java.util.HashMap<String, Object>();
		context.put("EnableTaskItemReader", false);
		String s = parser.Parse(this.In, context);
		String[] strings = s != null ? s.split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries) : new String[0];
		java.util.ArrayList<Object> values = new java.util.ArrayList<Object>();
		Regex reg = new Regex("^@\\((?<cate>[\\w]+)\\)$");

		for (String str : strings)
		{
			Match match = reg.Match(str);
			if (match.Success)
			{
				String cat = match.Groups["cate"].getValue();
				values.addAll(job.GetEvaluatedItemsByCatetory(cat));
			}
			else
			{
				values.add(str);
			}
		}

		int index = job.getRuntimeStatus().getLocal().GetValue("foreach.index", 0);
		while (index < values.size())
		{
			Object value = values.get(index);
			if (! (value instanceof String) || (!String.IsNullOrWhiteSpace((String)value) || !_skipEmptyString))
			{
				job.getRuntimeStatus().getLocal().setItem(Var, value);

				this.ExecuteSequence();
				this.ResetSequenceIndex();
			}
			index++;
			job.getRuntimeStatus().getLocal().setItem("foreach.index", index);
		}


	}


//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("var")]
	public String Var = null;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("in")]
	public String In = null;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("skipEmptyString")]
	private boolean _skipEmptyString = true;

}