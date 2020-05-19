using SunEngine.Core.Utils.External;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace SunEngine.Core.Utils
{
  static class SystemMetrics
  {
    public static DateTime SystemUptime
    {
      get
      {
        var uptime = TimeSpan.FromMilliseconds(Environment.TickCount64);
        return new DateTime() + uptime;
      }
    }

    public static DateTime AppUptime
    {
      get
      {
        var uptime = DateTime.Now - Process.GetCurrentProcess().StartTime;
        return new DateTime() + uptime;
      }
    }

    public static string OSVersion
    {
      get
      {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
          return RegQueryValue.GetFromRegistryREG_SZ(RegQueryValue.HKLM, @"SOFTWARE\Microsoft\Windows NT\CurrentVersion", @"ProductName");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
          if (!File.Exists("/etc/os-release"))
            return string.Empty;
          return File.ReadAllLines("/etc/os-release").Where(x => x.StartsWith("PRETTY_NAME")).First().Split('"', StringSplitOptions.RemoveEmptyEntries).Skip(1).First();
        }
        else
          return string.Empty;
      }
    }

    public static string KernelVersion => RuntimeInformation.OSDescription;
  }
}
