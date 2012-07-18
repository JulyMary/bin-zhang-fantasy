package fantasy;

public interface MethodInvoker<T> {

	void invoke(T obj) throws Exception;
}
