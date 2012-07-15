package Fantasy.ServiceModel;

import Microsoft.VisualBasic.*;

public interface IStatusBarServiceContainer extends IStatusBarService
{
	void Add(IStatusBarService service);
	void Remove(IStatusBarService service);
}