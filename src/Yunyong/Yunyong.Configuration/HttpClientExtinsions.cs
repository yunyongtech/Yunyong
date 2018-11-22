using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microsoft.Extensions.Configuration
{
    public static class HttpClientExtinsions
    {
        public static async Task<T> GetAsync<T>(this HttpClient target, string url)
        {
            return JsonConvert.DeserializeObject<T>(await target.GetStringAsync(url));
        }
    }
}