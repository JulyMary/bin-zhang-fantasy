package Fantasy.ServiceModel;

import Microsoft.VisualBasic.*;
import Fantasy.Properties.*;

public interface IProgressMonitor
{
	int getValue();
	void setValue(int value);
	int getMaximum();
	void setMaximum(int value);
	int getMinimum();
	void setMinimum(int value);

	ProgressMonitorStyle getStyle();
	void setStyle(ProgressMonitorStyle value);
}