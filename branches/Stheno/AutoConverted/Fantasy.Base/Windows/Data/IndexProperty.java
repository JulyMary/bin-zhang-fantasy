package Fantasy.Windows.Data;

public final class IndexProperty
{
	public static IndexPropertyWrapper Item(INotifyIndexPropertyChanged owner, Object index)
	{
		return new IndexPropertyWrapper(owner, index);
	}
}