using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.IO
{
    public class DiskspaceInfo
    {
        public DiskspaceInfo(ulong freeBytesAvailable, ulong totalNumberOfBytes, ulong totalNumberOfFreeBytes)
        {
            this.FreeBytesAvailable = freeBytesAvailable;
            this.TotalNumberOfBytes = totalNumberOfBytes;
            this.TotalNumberOfFreeBytes = TotalNumberOfFreeBytes;
        }

        public ulong TotalNumberOfFreeBytes { get; private set; }

        public ulong TotalNumberOfBytes { get; private set; }

        public ulong FreeBytesAvailable { get; private set; }
    }
}
