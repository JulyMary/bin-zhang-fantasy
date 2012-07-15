package fantasy;

public interface IServiceProvider {
    public Object GetService(@SuppressWarnings("rawtypes") Class serviceType);
    public <T> T GetService();
    public Object GetRequiredService(@SuppressWarnings("rawtypes") Class serviceType);
    public <T> T GetRequiredService();
}
