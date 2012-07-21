package fantasy.jobs;

import java.util.Arrays;
import java.util.regex.*;

import fantasy.xserialization.*;
import fantasy.*;

@Instruction
@XSerializable(name = "foreach",namespaceUri = Consts.XNamespaceURI)
public class Foreach extends Sequence
{
	@Override
	public void Execute() throws Exception
	{
		IJob job = this.getSite().getRequiredService2(IJob.class);
		IStringParser parser = this.getSite().getRequiredService2(IStringParser.class);
		java.util.HashMap<String, Object> context = new java.util.HashMap<String, Object>();
		context.put("EnableTaskItemReader", false);
		String s = parser.Parse(this.In, context);
		String[] strings = s != null ? StringUtils2.split(s, ";", true) : new String[0];
		java.util.ArrayList<Object> values = new java.util.ArrayList<Object>();
		Pattern reg = Pattern.compile("^@\\((?<cate>[\\w]+)\\)$");

		for (String str : strings)
		{
			Matcher match = reg.matcher(str);
			if (match.lookingAt())
			{
				String cat = match.group("cate");
				values.addAll(Arrays.asList(job.GetEvaluatedItemsByCatetory(cat)));
			}
			else
			{
				values.add(str);
			}
		}

		int index = job.getRuntimeStatus().getLocal().GetValue("foreach.index", 0);
		while (index < values.size())
		{
			if(Thread.interrupted())
			{
				throw new InterruptedException();
			}
			Object value = values.get(index);
			if (! (value instanceof String) || (!StringUtils2.isNullOrWhiteSpace((String)value) || !_skipEmptyString))
			{
				job.getRuntimeStatus().getLocal().setItem(Var, value);

				this.ExecuteSequence();
				this.ResetSequenceIndex();
			}
			index++;
			job.getRuntimeStatus().getLocal().setItem("foreach.index", index);
		}


	}

	@XAttribute(name = "var")
	public String Var = null;

    @XAttribute(name = "in")
	public String In = null;

	@XAttribute(name = "skipEmptyString")
	private boolean _skipEmptyString = true;

}