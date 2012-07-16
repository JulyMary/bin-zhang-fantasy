package fantasy.collections;

public interface Selector<T,TKey> {
    public TKey select(T item);
}
