package Fantasy.Windows;

public class NotifyPropertyChangedObject implements INotifyPropertyChanged
{
//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event PropertyChangedEventHandler PropertyChanged;

	protected void OnPropertyChanged(String propertyName)
	{
		if (PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}