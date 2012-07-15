package Fantasy.ServiceModel;

import Microsoft.VisualBasic.*;
import Fantasy.Properties.*;

public interface IProgressMonitorContainer extends IProgressMonitor
{
	void AddMoniter(IProgressMonitor monitor);
	void RemoveMoniter(IProgressMonitor monitor);
	void Increment(int value);
	int getStep();
	void setStep(int value);

	void PerformStep();
}