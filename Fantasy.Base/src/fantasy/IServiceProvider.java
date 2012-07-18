package fantasy;

public interface IServiceProvider {
    public Object getService(@SuppressWarnings("rawtypes") Class serviceType);
   
    public Object getRequiredService(@SuppressWarnings("rawtypes") Class serviceType);
  
}
