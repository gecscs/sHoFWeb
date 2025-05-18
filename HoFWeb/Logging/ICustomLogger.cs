namespace HoFWeb.Logging
{
    public interface ICustomLogger
    {
        void CustomInfo(string message);
        void CustomInfo(string message, IDictionary<string, object> properties);
    }
}
