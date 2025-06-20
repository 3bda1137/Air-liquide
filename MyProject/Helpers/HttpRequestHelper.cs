using Microsoft.Extensions.Primitives;

namespace MyProject.Helpers
{
    public class HttpRequestHelper
    {
        public static HttpContext Current => new HttpContextAccessor().HttpContext;
        public static string DoIt()
        {
            string Protocol = Current.Request.Protocol;
            return Protocol;
        }
        public static bool IsHeaderContainsKey(string key)
        {
            return Current.Request?.Headers?.Any(header => header.Key.ToLower() == key.ToLower() && !string.IsNullOrEmpty(header.Value)) ?? false;
        }
        public static string GetHeaderValue(string key)
        {
            try
            {
                StringValues header;

                if (Current != null)
                {
                    Current.Request.Headers.TryGetValue(key.ToLower(), out header);
                    var value = header.ToString();
                    return value;
                }
                else
                    return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
