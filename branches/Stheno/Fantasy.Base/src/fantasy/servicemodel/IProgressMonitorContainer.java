package fantasy.servicemodel;


public interface IProgressMonitorContainer extends IProgressMonitor
{
	void addMoniter(IProgressMonitor monitor);
	void removeMoniter(IProgressMonitor monitor);
	void increment(int value);
	int getStep();
	void setStep(int value);

	void PerformStep();
}