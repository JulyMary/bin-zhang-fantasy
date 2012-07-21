package fantasy;

public interface IServiceProvider {
    public Object getService(@SuppressWarnings("rawtypes") Class serviceType);
    
    public <T> T getService2(Class<T> serviceType);
   
    public Object getRequiredService(@SuppressWarnings("rawtypes") Class serviceType);
    
    public <T> T getRequiredService2(Class<T> serviceType);
  
}
