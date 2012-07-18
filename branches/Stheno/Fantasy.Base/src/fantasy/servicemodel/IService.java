package fantasy.servicemodel;

public interface IService
{
	void initializeService();
	void uninitializeService();
	
	void addServiceListener(IServiceListener listener);
	void removeServiceListener(IServiceListener listener);

}