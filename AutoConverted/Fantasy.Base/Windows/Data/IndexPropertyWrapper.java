package Fantasy.Windows.Data;

public class IndexPropertyWrapper implements INotifyPropertyChanged
{

	public IndexPropertyWrapper(INotifyIndexPropertyChanged owner, Object index)
	{
		_owner = owner;
		_index = index;
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		_owner.IndexPropertyChanged += new EventHandler<IndexPropertyChangedArgs>(OwnerIndexPropertyChanged);
	}

	private void OwnerIndexPropertyChanged(Object sender, IndexPropertyChangedArgs e)
	{
		if (e.getIndex() == this._index)
		{
			this.OnPropertyChanged("Value");
		}
	}

	private INotifyIndexPropertyChanged _owner;

	private Object _index;

	public final Object getValue()
	{
		return _owner.GetIndexValue(_index);
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;


	protected void OnPropertyChanged(String propertyName)
	{

		if (this.PropertyChanged != null)
		{
			PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
			this.PropertyChanged(this, e);
		}
	}



}