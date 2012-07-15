package fantasy;

public interface ITypeConverter {
    public  Object convertFrom(Object value) throws Exception;
    public Object convertTo(Object value, @SuppressWarnings("rawtypes") Class destinationType ) throws Exception;
}
