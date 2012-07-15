package Fantasy.Jobs.Tasks;

import Fantasy.ServiceModel.*;

public final class StreamUtils
{
//C# TO JAVA CONVERTER TODO TASK: C# optional parameters are not converted to Java:
//ORIGINAL LINE: public static void CopyStream(Stream input, Stream output, IProgressMonitor progress = null, int bufferSize = 32768)
	public static void CopyStream(Stream input, Stream output, IProgressMonitor progress, int bufferSize)
	{

		if (progress != null)
		{
			progress.setValue(0);
			progress.Minimum = 0;
			progress.Maximum = 100;

		}

		byte[] buffer = new byte[bufferSize];
		long readed = 0;
		long length = input.getLength();
		int count;
		do
		{
			count = input.Read(buffer, 0, buffer.length);
			readed += count;
			if (count > 0)
			{
				output.Write(buffer, 0, count);
				if (progress != null)
				{
					progress.setValue((int)(((double)readed / (double)length) * 100));
				}
			}
		} while (count > 0);
		if (progress != null)
		{
			progress.setValue(100);
		}
	}
}