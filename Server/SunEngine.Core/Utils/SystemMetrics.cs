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
        return DateTime.UtcNow.Subtract(uptime);
      }
    }

    public static DateTime AppUptime
    {
      get
      {
        try
        {
          return Process.GetCurrentProcess().StartTime.ToUniversalTime();
        }
        catch (NotSupportedException)
        {
          return new DateTime();
        }
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

    public static double[] LoadAverage => !RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? throw new NotSupportedException("Operation system not is not unix") : File.ReadAllText("/proc/loadavg").Split().Take(3).Select(x => Convert.ToDouble(x)).ToArray();
  }
}
