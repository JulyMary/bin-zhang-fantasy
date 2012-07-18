package fantasy;

public interface INotifyPropertyChanged {
    void AddPropertyChangedListener(IPropertyChangedListener listener);
    void RemovePropertyChangedListener(IPropertyChangedListener listener);
}
