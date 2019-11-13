using System.Linq;

namespace SunEngine.Core.Errors.Exceptions
{
    public class SunListException : SunException
    {
        public ErrorList ErrorList { get; }

        public SunListException(ErrorList errorList)
            : base(string.Join(",", errorList.Errors.Select(x =>
                $"{x.Description} {x.Message}")))
        {
            ErrorList = errorList;
        }
    }
}
