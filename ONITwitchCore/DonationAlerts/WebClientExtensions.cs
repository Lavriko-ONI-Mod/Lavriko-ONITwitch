using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ONITwitchLib.Logger;

namespace ONITwitch.DonationAlerts;

public static class WebClientExtensions
{
    public static async Task<T> GetJsonAsync<T>(this WebClient client, string address)
        where T : class
    {
        var str = await client.DownloadStringTaskAsync(address);

        Log.Info($"Response: {str}");
        
        if (string.IsNullOrEmpty(str))
        {
            return null;
        }

        return JsonConvert.DeserializeObject<T>(str);
    }
}