namespace SunEngine.Core.Errors
{
    public class SunViewException : SunException
    {
        public ErrorView ErrorView { get; }

        public SunViewException(ErrorView errorView)
        {
            ErrorView = errorView;
        }
    }
}