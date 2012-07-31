package fantasy.jobs.management;

import java.lang.reflect.*;
import java.rmi.RemoteException;

import fantasy.servicemodel.*;
import fantasy.collections.*;

public class JobManagerSettingsReaderService extends AbstractService implements IJobManagerSettingsReader
{

	

	/**
	 * 
	 */
	private static final long serialVersionUID = -1113729878924921686L;


	public JobManagerSettingsReaderService() throws RemoteException {
		super();
		
	}

	
	
	
    private Enumerable<Field> _fields = new  Enumerable<Field>(JobManagerSettings.class.getDeclaredFields());
    private Enumerable<Method> _methods = new Enumerable<Method>(JobManagerSettings.class.getDeclaredMethods());
	

	@SuppressWarnings("unchecked")
	@Override
	public <T> T getSetting(Class<T> type, final String name) throws Exception{
		
		
		Method method = _methods.firstOrDefault(new Predicate<Method>(){

			@Override
			public boolean evaluate(Method method) throws Exception {
				return method.getName() == "get" + name;
			}});
		
		
		if(method != null)
		{
			method.setAccessible(true);
			return (T)method.invoke(JobManagerSettings.getDefault());
		}
		else
		{
			Field field = _fields.singleOrDefault(new Predicate<Field>(){

				@Override
				public boolean evaluate(Field field) throws Exception {
					return field.getName() == name;
				}});
			if(field != null)
			{
				field.setAccessible(true);
				return (T)field.get(JobManagerSettings.getDefault());
			}
		}
		
		
		throw new NoSuchFieldException(name);
		
	}

}