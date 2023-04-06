using Microsoft.Extensions.Localization;
using System.Resources;

namespace LogginServiceAPI.Exceptions
{
    public class CustomizedException : Exception
    {
        public CustomizedException(string messageKey) : base(GetExceptionMessage(messageKey)) { }
        public CustomizedException(string messageKey, Exception innerException)
            : base(GetExceptionMessage(messageKey), innerException)
        {
        }
        public static string GetExceptionMessage(string messageKey)
        {
            ResourceManager rm = new("LogginServiceAPI.Resources.Exceptions", typeof(CustomizedException).Assembly);

            return rm.GetString("ValidationException");
        }
    }
}
