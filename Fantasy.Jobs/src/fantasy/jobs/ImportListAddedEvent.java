package fantasy.jobs;

import java.util.EventObject;

class ImportListAddedEvent extends EventObject {
	/**
	 * 
	 */
	private static final long serialVersionUID = 2259334333581790940L;

	public ImportListAddedEvent(Object source, ImportAssembly importedAssembly)
	{
		super(source);
		this._importedAssembly = importedAssembly;
	}
	
	private ImportAssembly _importedAssembly;
	
	public ImportAssembly getImportAssembly()
	{
		return this._importedAssembly;
	}
}
