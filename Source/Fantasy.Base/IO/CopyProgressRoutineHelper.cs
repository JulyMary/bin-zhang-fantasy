using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.IO.Interop;

namespace Fantasy.IO
{
    class CopyProgressRoutineHelper
    {
        private IProgressMonitor _owner;

        private CopyProgressRoutineHelper(IProgressMonitor owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner"); 
            }
            this._owner = owner;
        }


        private CopyProgressResult CopyProgressRoutine(long TotalFileSize, long TotalBytesTransferred, long StreamSize, long StreamBytesTransferred,
            uint dwStreamNumber, CopyProgressCallbackReason dwCallbackReason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData)
        {
            if (dwCallbackReason == CopyProgressCallbackReason.CALLBACK_STREAM_SWITCH)
            {
                this._owner.Minimum = 0;
                this._owner.Maximum = 100;
                this._owner.Value = 0;
            }

            else
            {
                int value = (int)(Math.Round((double)TotalBytesTransferred / (double)TotalFileSize)) * 100; 
                this._owner.Value = value;
            }
            return CopyProgressResult.PROGRESS_CONTINUE; 
        }


        public static CopyProgressRoutine Create(IProgressMonitor monitor)
        {
            CopyProgressRoutineHelper helper = new CopyProgressRoutineHelper(monitor);
            CopyProgressRoutine rs = new CopyProgressRoutine(helper.CopyProgressRoutine);
            return rs;

        }


    }
}
