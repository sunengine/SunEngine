using System;

namespace SunEngine.Core.Utils.External
{
  public class ExternalException : Exception
  {
    protected ExternalException()
    {
    }

    protected ExternalException(string message) : base(message)
    {
    }

    protected ExternalException(string message, Exception innerException) : base(message, innerException)
    {
    }
  }

  public class Win32ExternalException : ExternalException
  {
    private readonly int errorCode;

    public Win32ExternalException(int Win32ErrorCode)
    {
      this.errorCode = Win32ErrorCode;
    }

    public Win32ExternalException(string message, int Win32ErrorCode) : this(Win32ErrorCode) { }

    public override string Message => $"Exception WIN32API was happened with error code: 0x{Convert.ToString(errorCode,16)}.";

    public override string HelpLink { get => "https://docs.microsoft.com/en-us/windows/win32/debug/system-error-codes#system-error-codes"; }
  }
}
