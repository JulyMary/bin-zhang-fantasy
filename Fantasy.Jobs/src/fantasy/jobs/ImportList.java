package fantasy.jobs;

import java.util.*;

class ImportList extends ArrayList<ImportAssembly> {

	
	/**
	 * 
	 */
	private static final long serialVersionUID = -7031698635967369666L;

	@Override
	public boolean add(ImportAssembly item)
	{
		boolean rs = super.add(item);
		ImportListAddedEvent e = new ImportListAddedEvent(this, item);
		
		for(IImportListListener listener : this._listeners)
		{
			listener.Added(e);
		}
		
		return rs;
	}
	
	private HashSet<IImportListListener> _listeners = new HashSet<IImportListListener>();
	
	public void addListener(IImportListListener listener)
	{
		this._listeners.add(listener);
	}
	
	public void removeListener(IImportListListener listener)
	{
		this._listeners.remove(listener);
	}
}
