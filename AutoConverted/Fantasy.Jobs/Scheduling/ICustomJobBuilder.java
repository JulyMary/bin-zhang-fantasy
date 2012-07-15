package Fantasy.Jobs.Scheduling;

public interface ICustomJobBuilder extends IObjectWithSite
{
	Iterable<String> Execute(XElement args);
}