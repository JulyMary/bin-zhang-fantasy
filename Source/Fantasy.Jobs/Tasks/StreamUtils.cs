using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Tasks
{
    public static  class StreamUtils
    {
        public static void CopyStream(Stream input, Stream output, IProgressMonitor progress = null, int bufferSize = 32768)
        {

            if (progress != null)
            {
                progress.Value = 0;
                progress.Minimum = 0;
                progress.Maximum = 100;

            }

            byte[] buffer = new byte[bufferSize];
            long readed = 0;
            long length = input.Length;
            int count;
            do
            {
                count = input.Read(buffer, 0, buffer.Length);
                readed += count;
                if (count > 0)
                {
                    output.Write(buffer, 0, count);
                    if (progress != null)
                    {
                        progress.Value = (int)(((double)readed / (double)length) * 100);
                    }
                }
            } while (count > 0);
            if (progress != null)
            {
                progress.Value = 100;
            }
        }
    }
}
