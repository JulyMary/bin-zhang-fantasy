package fantasy.jobs.scheduling;

import fantasy.*;
import org.jdom2.*;
public interface ICustomJobBuilder extends IObjectWithSite
{
	Iterable<String> Execute(Element args);
}