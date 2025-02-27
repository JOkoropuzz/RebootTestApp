using System;
using System.Runtime.InteropServices;

namespace RebootTestApp
{
    public class Reboot
    {
        internal const int SE_PRIVILEGE_ENABLED = 2;
        internal const int TOKEN_QUERY = 8;
        internal const int TOKEN_ADJUST_PRIVILEGES = 32;
        internal const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";

        [DllImport("advapi32.dll", EntryPoint = "InitiateSystemShutdownEx")]
        private static extern int InitiateSystemShutdown(
          string lpMachineName,
          string lpMessage,
          int dwTimeout,
          bool bForceAppsClosed,
          bool bRebootAfterShutdown);

        [DllImport("kernel32.dll")]
        private static extern int GetLastError();


        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool AdjustTokenPrivileges(
          IntPtr htok,
          bool disall,
          ref Reboot.TokPriv1Luid newst,
          int len,
          IntPtr prev,
          IntPtr relen);

        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool LookupPrivilegeValue(string host, string name, ref long pluid);

        private void SetPriv()
        {
            IntPtr zero = IntPtr.Zero;

            var currentProcess = Reboot.GetCurrentProcess();
            Console.WriteLine($"Reboot.GetCurrentProcess() возвращено {currentProcess}");
            
            var processToken = Reboot.OpenProcessToken(currentProcess, 40, ref zero);
            Console.WriteLine($"Reboot.OpenProcessToken(currentProcess, 40, ref zero) возвращено {processToken}");
            

            if (!processToken)
            {
                var errorCode = GetLastError();
                Console.WriteLine($"Т.к. processToken = false, выполнение SetPriv() остановлено ErrorCode = {errorCode}");
                return;
            }
                

            Reboot.TokPriv1Luid newst;
            newst.Count = 1;
            newst.Attr = 2;
            newst.Luid = 0L;
            var lookupPrivilegeValue = Reboot.LookupPrivilegeValue((string)null, "SeShutdownPrivilege", ref newst.Luid);
            Console.WriteLine($"Reboot.LookupPrivilegeValue((string)null, \"SeShutdownPrivilege\", ref newst.Luid) возвращено {lookupPrivilegeValue}");
            if (!lookupPrivilegeValue)
            {
                var errorCode = GetLastError();
                Console.WriteLine($"Ошибка в LookupPrivilegeValue() ErrorCode = {errorCode}");
            }

            var adjustTokenPrivileges = Reboot.AdjustTokenPrivileges(zero, false, ref newst, 0, IntPtr.Zero, IntPtr.Zero);
            Console.WriteLine($"Reboot.AdjustTokenPrivileges(zero, false, ref newst, 0, IntPtr.Zero, IntPtr.Zero) возвращено {adjustTokenPrivileges}");
            if (!adjustTokenPrivileges)
            {
                var errorCode = GetLastError();
                Console.WriteLine($"Ошибка в AdjustTokenPrivileges() ErrorCode = {errorCode}");
            }
        }

        public int halt(bool RSh, bool Force)
        {
            SetPriv();
            //return InitiateSystemShutdown(null, null, 0, Force, RSh);
            return 0;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct TokPriv1Luid
        {
            public int Count;
            public long Luid;
            public int Attr;
        }
    }
}