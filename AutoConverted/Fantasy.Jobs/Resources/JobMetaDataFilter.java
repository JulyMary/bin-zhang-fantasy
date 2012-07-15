package Fantasy.Jobs.Temporary;

import Fantasy.Jobs.Management.*;
import Fantasy.Jobs.*;

public class JobMetaDataFilter implements IJobMetaDataFilter
{

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobMetaDataFilter Members

	public final Iterable<JobMetaData> Filter(IQueryable<JobMetaData> source)
	{
		<%=vars%> var rs = source;


//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		<%=enableCondition%>rs = from job in rs where <%=condition%> select job;



//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		<%=enableOrder%>rs = from job in rs orderby <%=order%> select job;


	   return rs;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}