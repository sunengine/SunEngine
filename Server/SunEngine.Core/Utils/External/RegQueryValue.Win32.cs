using System;
using System.Runtime.InteropServices;

namespace SunEngine.Core.Utils.External
{
  internal static class RegQueryValue
  {
    private const int ERROR_SUCCESS = 0;

    public const uint HKLM = 0x80000002;

    private const int KEY_READ = 0x20019;

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
    private static extern int RegOpenKeyEx(
            UIntPtr hKey,
            string subKey,
            int ulOptions,
            int samDesired,
            out UIntPtr hkResult
        );

    [DllImport("advapi32.dll")]
    private static extern int RegCloseKey(
        UIntPtr hKey
    );

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]

    private static extern int RegQueryValueEx(
        UIntPtr hKey,
        string lpValueName,
        int lpReserved,
        IntPtr type,
        IntPtr lpData,
        ref int lpcbData
    );

    private static void CheckErrorCode(int errorCode)
    {
      if (errorCode != ERROR_SUCCESS)
      {
        throw new Win32ExternalException(errorCode);
      }
    }

    public static string GetFromRegistryREG_SZ(uint rootKey, string subKey, string name)
    {
      CheckErrorCode(RegOpenKeyEx((UIntPtr)rootKey, subKey, 0, KEY_READ, out UIntPtr hkey));
      try
      {
        int cbData = 0;
        CheckErrorCode(RegQueryValueEx(hkey, name, 0, IntPtr.Zero, IntPtr.Zero, ref cbData));
        IntPtr pointer = Marshal.AllocHGlobal(cbData);
        try
        {
          CheckErrorCode(RegQueryValueEx(hkey, name, 0, IntPtr.Zero, pointer, ref cbData));
          return Marshal.PtrToStringUni(pointer,cbData/sizeof(char)).TrimEnd('\0');
        }
        finally
        {
          Marshal.FreeHGlobal(pointer);
        }
      }
      finally
      {
        CheckErrorCode(RegCloseKey(hkey));
      }
    }
  }
}
