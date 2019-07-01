using System.Linq;

namespace SunEngine.Core.Errors
{
    public class SunViewException : SunException
    {
        public ErrorView ErrorView { get; }

        public SunViewException(ErrorView errorView)
            : base(string.Join(",", errorView.Errors.Select(x =>
                $"{x.Description} {x.Message}")))
        {
            ErrorView = errorView;
        }
    }
}
