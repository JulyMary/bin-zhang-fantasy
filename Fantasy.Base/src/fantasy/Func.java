package fantasy;

public interface Func<TResult> {

	TResult call() throws Exception;
}