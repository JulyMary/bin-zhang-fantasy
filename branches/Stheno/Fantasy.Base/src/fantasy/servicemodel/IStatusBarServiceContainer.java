package fantasy.servicemodel;

public interface IStatusBarServiceContainer extends IStatusBarService
{
	void add(IStatusBarService service);
	void remove(IStatusBarService service);
}