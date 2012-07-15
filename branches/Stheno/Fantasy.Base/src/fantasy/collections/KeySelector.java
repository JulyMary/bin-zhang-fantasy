package fantasy.collections;

public interface KeySelector<T,TKey> {
    public TKey evaluate(T item);
}
