package fantasy.io;

import java.io.*;
import java.nio.channels.FileChannel;

import fantasy.servicemodel.*;

public final class StreamUtils
{

	public static void CopyFile(java.io.File inputFile, java.io.File outputFile)
	{
		CopyFile(inputFile, outputFile);
	}


	public static void CopyFile(java.io.File inputFile, java.io.File outputFile, IProgressMonitor progress) throws Exception
	{
		CopyFile(inputFile, outputFile, progress, 32768);
	}

	public static void CopyFile(java.io.File inputFile, java.io.File outputFile, IProgressMonitor progress, int bufferSize ) throws Exception
	{

		if (progress != null)
		{
			progress.setValue(0);
			progress.setMinimum(0);
			progress.setMaximum(100);

		}

		FileInputStream in=new FileInputStream(inputFile);
		try
		{
			FileOutputStream out=new FileOutputStream(outputFile);
			try
			{
				FileChannel inC=in.getChannel();
				FileChannel outC=out.getChannel();

				while(inC.position() < inC.size())
				{
					int length;
					if((inC.size()-inC.position())<bufferSize)
					{
						length=(int)(inC.size()-inC.position());
					}
					else
					{
						length=bufferSize;
					}
					inC.transferTo(inC.position(),length,outC);
					inC.position(inC.position()+length);

					if (progress != null)
					{
						progress.setValue((int)(((double)inC.position() / (double)inC.size()) * 100));
					}

				}
			}
			finally
			{
				out.close();
			}
		}
		finally
		{
			in.close();
		}

	}


	public static void CopyStream(InputStream input, OutputStream output) throws Exception
	{
		CopyStream(input, output, null);
	}

	public static void CopyStream(InputStream input, OutputStream output, IProgressMonitor progress) throws Exception
	{
		CopyStream(input, output, progress, 32768);
	}

	public static void CopyStream(InputStream input, OutputStream output, IProgressMonitor progress, int bufferSize) throws Exception
	{

		if (progress != null)
		{
			progress.setValue(0);
			progress.setMinimum(0);
			progress.setMaximum(100);

		}

		byte[] buffer = new byte[bufferSize];
		long readed = 0;
		long length = input.available();
		int count;
		do
		{
			count = input.read(buffer, 0, buffer.length);
			readed += count;
			if (count > 0)
			{
				output.write(buffer, 0, count);
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